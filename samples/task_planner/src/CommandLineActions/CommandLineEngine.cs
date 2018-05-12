// -----------------------------------------------------------------------
// <copyright file="CommandLineEngine.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    internal sealed class CommandLineEngine
    {
        private readonly Assembly actionsAssembly;

        internal CommandLineEngine(Assembly actionsAssembly = null)
        {
            this.actionsAssembly =
                actionsAssembly ?? Assembly.GetExecutingAssembly();
        }

        public void Process(CommandLineArgument arg)
        {
            Type categoryType = this.GetCategoryType(arg.Category);

            object actionType = GetActionType(categoryType, arg.Action);

            MethodInfo actionMethod = GetActionMethod(categoryType, actionType);

            RunAction(actionMethod, arg);
        }

        private static void RunAction(
            MethodInfo actionMethod,
            CommandLineArgument arg)
        {
            ParameterInfo[] methodParams = actionMethod.GetParameters();

            if (methodParams.Count() != 1)
            {
                throw new CommandLineException(
                    CommandLineErrorCode.InvalidActionMethodDefinition,
                    ExceptionMessages.ActionMethodNotAcceptOneParam,
                    actionMethod);
            }

            ParameterInfo methodParam = methodParams.Single();
            if (!methodParam.ParameterType.IsSubclassOf(
                typeof(ActionArgumentBase)))
            {
                throw new CommandLineException(
                    CommandLineErrorCode.InvalidActionMethodDefinition,
                    ExceptionMessages.ActionMethodNotAcceptOneActionArgumentParam,
                    actionMethod);
            }

            object actionArg =
                Convert.ChangeType(
                    Activator.CreateInstance(methodParam.ParameterType, arg),
                    methodParam.ParameterType,
                    CultureInfo.InvariantCulture);

            actionMethod.Invoke(null, new[] { actionArg });
        }

        private static MethodInfo GetActionMethod(
            Type categoryType,
            object actionType)
        {
            IEnumerable<MethodInfo> methodInfos =
                categoryType.GetMethods(
                    BindingFlags.Public | BindingFlags.Static)
                .Where(m =>
                {
                    ActionAttribute actionAttr =
                        m.GetCustomAttribute<ActionAttribute>();

                    return actionAttr != null
                        && actionType.Equals(actionAttr.Action);
                });

            if (methodInfos.Count() == 0)
            {
                throw new CommandLineException(
                    CommandLineErrorCode.InvalidActionMethodDefinition,
                    ExceptionMessages.ActionMethodNotFound,
                    actionType);
            }
            else if (methodInfos.Count() > 1)
            {
                throw new CommandLineException(
                    CommandLineErrorCode.InvalidActionMethodDefinition,
                    ExceptionMessages.ActionMethodFoundDupDefinitions,
                    actionType,
                    string.Join(",", methodInfos.Select(m => m.ToString())));
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

        private Type GetCategoryType(string category)
        {
            IEnumerable<Type> types =
                this.actionsAssembly.GetTypes()
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
                throw new CommandLineException(
                    CommandLineErrorCode.InvalidCategoryDefinition,
                    ExceptionMessages.CategoryNotFound,
                    category);
            }
            else if (types.Count() > 1)
            {
                throw new CommandLineException(
                    CommandLineErrorCode.InvalidCategoryDefinition,
                    ExceptionMessages.CategoryFoundDupDefinitions,
                    category,
                    string.Join(",", types.Select(t => t.FullName)));
            }

            return types.Single();
        }
    }
}