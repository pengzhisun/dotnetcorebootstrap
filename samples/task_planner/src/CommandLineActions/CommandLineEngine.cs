namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    internal static class CommandLineEngine
    {
        public static void Process(CommandLineArgument arg)
        {
            Type categoryType = GetCategoryType(arg.Category);

            object actionType = GetActionType(categoryType, arg.Action);

            MethodInfo actionMethod = GetActionMethod(categoryType, actionType);

            RunAction(categoryType, actionMethod, arg);
        }

        private static void RunAction(
            Type categoryType,
            MethodInfo actionMethod,
            CommandLineArgument arg)
        {
            ParameterInfo[] methodParams = actionMethod.GetParameters();

            if (methodParams.Count() != 1)
            {
                throw new InvalidOperationException(
                    $"The action method '{actionMethod}' should only accept one parameter.");
            }

            ParameterInfo methodParam = methodParams.Single();
            if (!methodParam.ParameterType.IsSubclassOf(typeof(CommandLineArgument)))
            {
                throw new InvalidOperationException(
                    $"The action method '{actionMethod}' should only accept one CommandLineArgument type parameter.");
            }

            object actionArg =
                Convert.ChangeType(
                    Activator.CreateInstance(methodParam.ParameterType, arg),
                    methodParam.ParameterType);

            object categoryInstance =
                Activator.CreateInstance(categoryType);
            actionMethod.Invoke(categoryInstance, new[]{ actionArg });
        }

        private static MethodInfo GetActionMethod(
            Type categoryType,
            object actionType)
        {
            IEnumerable<MethodInfo> methodInfos =
                categoryType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m =>
                {
                    ActionAttribute actionAttr =
                        m.GetCustomAttribute<ActionAttribute>();

                    return actionAttr != null && actionType.Equals(actionAttr.Action);
                });

            if (methodInfos.Count() == 0)
            {
                throw new InvalidOperationException(
                    $"Can't find action definition for '{actionType}'.");
            }
            else if (methodInfos.Count() > 1)
            {
                throw new InvalidOperationException(
                    $"Should have only one action definition for '{actionType}', declard types: {string.Join(",", methodInfos.Select(m => m.ToString()))}.");
            }

            return methodInfos.Single();
        }

        private static object GetActionType(Type categoryType, string action)
        {
            CategoryAttribute categoryAttr =
                categoryType.GetCustomAttribute<CategoryAttribute>();

            Enum.TryParse(
                enumType: categoryAttr.ActionTypeType,
                value: action,
                ignoreCase: true,
                result: out object actionType);

            return actionType;
        }

        private static Type GetCategoryType(string category)
        {
            IEnumerable<Type> types =
                Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t =>
                    {
                        CategoryAttribute categoryAttr =
                            t.GetCustomAttribute<CategoryAttribute>();
                        return categoryAttr != null
                            && category.Equals(
                                categoryAttr.Category,
                                StringComparison.InvariantCultureIgnoreCase);
                    });

            if (types.Count() == 0)
            {
                throw new InvalidOperationException(
                    $"Can't find category definition for '{category}'.");
            }
            else if (types.Count() > 1)
            {
                throw new InvalidOperationException(
                    $"Should have only one category definition for '{category}', declard types: {string.Join(",", types.Select(t => t.FullName))}.");
            }

            return types.Single();
        }
    }
}