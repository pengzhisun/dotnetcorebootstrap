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
        /// <exception cref="CommandLineException">
        /// Thrown if the action property matched more than one action parameter,
        /// or the matched action parameter value for specific property is null.
        /// </exception>
        protected ActionArgumentBase(CommandLineArgument commandLineArgument)
        {
            Type argType = this.GetType();
            foreach (PropertyInfo propInfo in
                argType.GetProperties(
                    BindingFlags.NonPublic | BindingFlags.Instance))
            {
                this.InitActionParamProperty(propInfo, commandLineArgument);
            }
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
        /// Initializes the action argument property with action parameter from
        /// given command line argument.
        /// </summary>
        /// <param name="propInfo">
        /// The <see cref="PropertyInfo"/> instance.
        /// </param>
        /// <param name="commandLineArgument">
        /// The command line argument contains required action parameters.
        /// </param>
        private void InitActionParamProperty(
            PropertyInfo propInfo,
            CommandLineArgument commandLineArgument)
        {
            ActionParameterAttribute actionParamAttr =
                    propInfo.GetCustomAttribute<ActionParameterAttribute>();

            if (actionParamAttr == null)
            {
                return;
            }

            IEnumerable<string> matchedActionParams =
                actionParamAttr.Aliases.Append(propInfo.Name)
                    .Where(n =>
                        commandLineArgument.ActionParameters.Keys.Any(
                            k => k.Equals(n, StringComparison.OrdinalIgnoreCase)));

            if (matchedActionParams.Count() > 1)
            {
                throw new CommandLineException(
                    CommandLineErrorCode.CommandLineArgInitFailed,
                    ExceptionMessages.PropMatchedMoreThanOneActionParams,
                    propInfo.Name);
            }

            string matchedActionParam = matchedActionParams.FirstOrDefault();
            if (matchedActionParam == null)
            {
                if (actionParamAttr.DefaultValue != null)
                {
                    propInfo.SetValue(this, actionParamAttr.DefaultValue);
                }

                return;
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

                propInfo.SetValue(this, paramValue);
            }
            else if (propInfo.PropertyType == typeof(bool?))
            {
                propInfo.SetValue(this, true);
            }
            else
            {
                throw new CommandLineException(
                    CommandLineErrorCode.CommandLineArgInitFailed,
                    ExceptionMessages.PropMatchedActionParamValueIsNull,
                    propInfo.Name);
            }
        }
    }
}