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
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines the command line engine class.
    /// </summary>
    internal sealed class CommandLineEngine
    {
        /// <summary>
        /// The assembly contains command line action definitions.
        /// </summary>
        private readonly Assembly actionsAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineEngine"/>
        /// class with a specified assembly.
        /// </summary>
        /// <param name="actionsAssembly">
        /// The assembly contains command line action definitions, use current
        /// executing assembly as default.
        /// </param>
        internal CommandLineEngine(Assembly actionsAssembly = null)
        {
            this.actionsAssembly =
                actionsAssembly ?? Assembly.GetExecutingAssembly();
        }

        /// <summary>
        /// Processes the specific command line argument.
        /// </summary>
        /// <param name="arg">The specifi command line argument instance.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the given command line argument is null.
        /// </exception>
        /// <exception cref="CommandLineException">
        /// Thrown if the category difinition is not found or invalid,
        /// or the action method is not found or invalid,
        /// or the action parameters are not found or invalid.
        /// </exception>
        public void Process(CommandLineArgument arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            Type categoryType = this.GetCategoryType(arg.Category);

            object actionTypeValue =
                GetActionTypeValue(categoryType, arg.Action);

            MethodInfo actionMethod =
                GetActionMethod(categoryType, actionTypeValue);

            RunAction(actionMethod, arg);
        }

        /// <summary>
        /// Gets the action type enumeration value for specific category
        /// definition type with the given action string.
        /// </summary>
        /// <param name="categoryType">The category definition type.</param>
        /// <param name="action">The action string.</param>
        /// <returns>
        /// The action type enumeration value, used the default enumeration
        /// value if the action string is not defined in the action type
        /// enumeration.
        /// </returns>
        private static object GetActionTypeValue(
            Type categoryType,
            string action)
        {
            CategoryAttribute categoryAttr =
                categoryType.GetCustomAttribute<CategoryAttribute>();

            Enum.TryParse(
                enumType: categoryAttr.ActionTypeType,
                value: action,
                ignoreCase: true,
                result: out object actionTypeValue);

            return actionTypeValue;
        }

        /// <summary>
        /// Gets the action method from specific category definition type and
        /// matched the given action type value.
        /// </summary>
        /// <param name="categoryType">The category definition type.</param>
        /// <param name="actionTypeValue">The action type value.</param>
        /// <returns>The action method for specific action type.</returns>
        /// <exception cref="CommandLineException">
        /// Thrown if not found only one matched action method from specific
        /// category definition type and matched the given action type value.
        /// </exception>
        private static MethodInfo GetActionMethod(
            Type categoryType,
            object actionTypeValue)
        {
            Debug.Assert(
                categoryType != null,
                @"The category difinition type shoudn't be null.");
            Debug.Assert(
                actionTypeValue != null,
                @"The action type value shouldn't be null");
            Debug.Assert(
                actionTypeValue.GetType().IsEnum,
                @"The action type value should be an enumeration value.");

            IEnumerable<MethodInfo> methodInfos =
                categoryType.GetMethods(
                    BindingFlags.Public | BindingFlags.Static)
                .Where(m =>
                {
                    ActionAttribute actionAttr =
                        m.GetCustomAttribute<ActionAttribute>();

                    return actionAttr != null
                        && actionTypeValue.Equals(actionAttr.Action);
                });

            if (methodInfos.Count() == 0)
            {
                throw new CommandLineException(
                    CommandLineErrorCode.InvalidActionMethodDefinition,
                    ExceptionMessages.ActionMethodNotFound,
                    actionTypeValue);
            }
            else if (methodInfos.Count() > 1)
            {
                throw new CommandLineException(
                    CommandLineErrorCode.InvalidActionMethodDefinition,
                    ExceptionMessages.ActionMethodFoundDupDefinitions,
                    actionTypeValue,
                    string.Join(",", methodInfos.Select(m => m.ToString())));
            }

            return methodInfos.Single();
        }

        /// <summary>
        /// Run the action method.
        /// </summary>
        /// <param name="actionMethod">The action method information.</param>
        /// <param name="arg">The command line argument.</param>
        /// <exception cref="CommandLineException">
        /// Thrown if the action method is not a static method
        /// or the action method not only accept one parameter which type is a
        /// derived class of the <see cref="ActionArgumentBase"/>.
        /// </exception>
        private static void RunAction(
            MethodInfo actionMethod,
            CommandLineArgument arg)
        {
            Debug.Assert(
                actionMethod != null,
                @"The action method shouldn't be null.");
            Debug.Assert(
                arg != null,
                @"The command line argument shouldn't be null.");

            if (!actionMethod.IsStatic)
            {
                throw new CommandLineException(
                    CommandLineErrorCode.InvalidActionMethodDefinition,
                    ExceptionMessages.ActionMethodNotStatic,
                    actionMethod);
            }

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

        /// <summary>
        /// Gets the category definition type for specific category from actions
        /// assembly.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>The category definition type.</returns>
        /// <exception cref="CommandLineException">
        /// Thrown if not found only one category definition type from actions
        /// assembly for specific category.
        /// </exception>
        private Type GetCategoryType(string category)
        {
            Debug.Assert(
                !string.IsNullOrWhiteSpace(category),
                @"The category shouldn't be null or empty or whitespace");

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