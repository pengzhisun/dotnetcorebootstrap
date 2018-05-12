// -----------------------------------------------------------------------
// <copyright file="ActionArgumentBase.cs" company="Pengzhi Sun">
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
    /// Defines the action argument base class.
    /// </summary>
    internal abstract class ActionArgumentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionArgumentBase"/>
        /// class from given command line argument.
        /// </summary>
        /// <param name="commandLineArgument">
        /// The given command line argument.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the given command line argument is null.
        /// </exception>
        /// <exception cref="CommandLineException">
        /// Thrown if the action property matched more than one action parameter,
        /// or the matched action parameter value for specific property is null.
        /// </exception>
        protected ActionArgumentBase(CommandLineArgument commandLineArgument)
        {
            if (commandLineArgument == null)
            {
                throw new ArgumentNullException(nameof(commandLineArgument));
            }

            this.InitNonPublicProperties(commandLineArgument);
        }

        /// <summary>
        /// Checks the action argument is valid or not, the derived class should
        /// override this method based on its own logic.
        /// </summary>
        /// <returns>
        /// Return true by default, the derived class should return based on its
        /// own logic.
        /// </returns>
        public virtual bool IsValid() => true;

        /// <summary>
        /// Gets the matched action parameter name from command line argument
        /// for specific property.
        /// </summary>
        /// <param name="propInfo">
        /// The <see cref="PropertyInfo"/> instance.
        /// </param>
        /// <param name="actionParamAttr">
        /// The <see cref="ActionParameterAttribute"/> instance from the given
        /// <see cref="PropertyInfo"/> instance.
        /// </param>
        /// <param name="commandLineArgument">The command line argument.</param>
        /// <returns>
        /// If found only one parameter name existed in the action parameters
        /// dictionary of the command line argument which either defined in the
        /// action parameter aliases or equals the property name (both in ignore
        /// case mode) then return the matched action parameter name,
        /// else return null.
        /// </returns>
        /// <exception cref="CommandLineException">
        /// Thrown if found more than one action parameter names matched.
        /// </exception>
        private static string GetMatchedActionParamName(
            PropertyInfo propInfo,
            ActionParameterAttribute actionParamAttr,
            CommandLineArgument commandLineArgument)
        {
            Debug.Assert(
                propInfo != null,
                @"The property information shouldn't be null.");
            Debug.Assert(
                actionParamAttr?.Aliases != null,
                @"The action parameter attribute and its aliases collection shouldn't be null.");
            Debug.Assert(
                commandLineArgument?.ActionParameters != null,
                @"Command line argument and its action parameters dictionary shouldn't be null");

            IEnumerable<string> matchedActionParams =
                actionParamAttr.Aliases.Append(propInfo.Name)
                    .Where(n =>
                        commandLineArgument.ActionParameters.Keys.Any(
                            k => k.Equals(n, StringComparison.OrdinalIgnoreCase)));

            if (matchedActionParams.Count() > 1)
            {
                throw new CommandLineException(
                    CommandLineErrorCode.ActionArgInitFailed,
                    ExceptionMessages.PropMatchedMoreThanOneActionParams,
                    propInfo.Name);
            }

            return matchedActionParams.SingleOrDefault();
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        /// <param name="propInfo">
        /// The <see cref="PropertyInfo"/> instance.
        /// </param>
        /// <param name="actionParamAttr">
        /// The <see cref="ActionParameterAttribute"/> instance from the given
        /// <see cref="PropertyInfo"/> instance.
        /// </param>
        /// <param name="commandLineArgument">The command line argument.</param>
        /// <param name="matchedActionParam">
        /// The matched action parameter name in the action parameters
        /// dictionary of the given command line argument.
        /// </param>
        /// <returns>
        /// If the matched action parameter name is null then return the default
        /// value defined in the action parameter attribute,
        /// else if the matched action parameter value is not null then convert
        /// the action parameter string value to property type and return the
        /// converted value,
        /// else if the matched action parameter value is null but the property
        /// type is bool? then return true.
        /// </returns>
        /// <exception cref="CommandLineException">
        /// Thrown if the matched action parameter is not null but the matched
        /// action parameter value is null and the property type is not bool?.
        /// </exception>
        private static object GetPropertyValue(
            PropertyInfo propInfo,
            ActionParameterAttribute actionParamAttr,
            CommandLineArgument commandLineArgument,
            string matchedActionParam)
        {
            Debug.Assert(
                propInfo != null,
                @"The property information shouldn't be null.");
            Debug.Assert(
                actionParamAttr?.Aliases != null,
                @"The action parameter attribute and its aliases collection shouldn't be null.");
            Debug.Assert(
                commandLineArgument?.ActionParameters != null,
                @"Command line argument and its action parameters dictionary shouldn't be null");

            if (matchedActionParam == null)
            {
                return actionParamAttr.DefaultValue;
            }

            string matchedActionParamValue =
                commandLineArgument.ActionParameters[matchedActionParam];

            if (matchedActionParamValue != null)
            {
                Type paramType =
                    Nullable.GetUnderlyingType(propInfo.PropertyType)
                    ?? propInfo.PropertyType;

                object paramValue =
                    Convert.ChangeType(
                        matchedActionParamValue,
                        paramType,
                        CultureInfo.InvariantCulture);

                return paramValue;
            }
            else if (propInfo.PropertyType == typeof(bool?))
            {
                return true;
            }
            else
            {
                throw new CommandLineException(
                    CommandLineErrorCode.ActionArgInitFailed,
                    ExceptionMessages.PropMatchedActionParamValueIsNull,
                    propInfo.Name);
            }
        }

        /// <summary>
        /// Initialize all non-public properties defined in derived class.
        /// </summary>
        /// <param name="commandLineArgument">The command line argument.</param>
        /// <exception cref="CommandLineException">
        /// Thrown if found invalid action parameter values from the command
        /// line argument.
        /// </exception>
        private void InitNonPublicProperties(
            CommandLineArgument commandLineArgument)
        {
            Debug.Assert(
                commandLineArgument != null,
                @"Command line argument shouldn't be null");

            foreach (PropertyInfo propInfo in
                this.GetType().GetProperties(
                    BindingFlags.NonPublic | BindingFlags.Instance))
            {
                this.InitActionParamProperty(propInfo, commandLineArgument);
            }
        }

        /// <summary>
        /// Initializes the action argument property by following rules:
        /// if the property doesn't have <see cref="ActionParameterAttribute" />
        /// then do nothing,
        /// else if the action parameter could find matched action parameter
        /// from command line argument then set the parameter value as property
        /// value,
        /// else if the action parameter couldn't be found, then set the default
        /// value of action parameter attribute as property value.
        /// </summary>
        /// <param name="propInfo">
        /// The <see cref="PropertyInfo"/> instance.
        /// </param>
        /// <param name="commandLineArgument">The command line argument.</param>
        /// <exception cref="CommandLineException">
        /// Thrown if found invalid action parameter values from the command
        /// line argument.
        /// </exception>
        private void InitActionParamProperty(
            PropertyInfo propInfo,
            CommandLineArgument commandLineArgument)
        {
            Debug.Assert(
                propInfo != null,
                @"The property information shouldn't be null.");
            Debug.Assert(
                propInfo.GetGetMethod() == null,
                @"The property shouldn't be public.");
            Debug.Assert(
                propInfo.CanWrite,
                @"The property shouldn't be read-only.");
            Debug.Assert(
                commandLineArgument != null,
                @"Command line argument shouldn't be null");

            ActionParameterAttribute actionParamAttr =
                propInfo.GetCustomAttribute<ActionParameterAttribute>();

            if (actionParamAttr == null)
            {
                return;
            }

            string matchedActionParam =
                GetMatchedActionParamName(
                    propInfo,
                    actionParamAttr,
                    commandLineArgument);

            object propertyValue =
                GetPropertyValue(
                    propInfo,
                    actionParamAttr,
                    commandLineArgument,
                    matchedActionParam);

            if (propertyValue != null)
            {
                propInfo.SetValue(this, propertyValue);
            }
        }
    }
}