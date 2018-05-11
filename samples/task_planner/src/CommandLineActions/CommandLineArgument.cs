// -----------------------------------------------------------------------
// <copyright file="CommandLineArgument.cs" company="Pengzhi Sun">
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
    using Newtonsoft.Json;

    internal class CommandLineArgument
    {
        public const string DefaultCategory = Constants.GeneralCategory;

        public static readonly string DefaultAction =
            GeneralActionType.Default.ToString();

        public CommandLineArgument(
            string category = null,
            string action = null,
            IDictionary<string, string> actionParams = null)
        {
            this.Category = category ?? DefaultCategory;
            this.Action = action ?? DefaultAction;
            this.ActionParameters =
                (actionParams ?? new Dictionary<string, string>())
                as IReadOnlyDictionary<string, string>;
        }

        public CommandLineArgument(CommandLineArgument commandLineArgument)
        {
            this.Category = commandLineArgument.Category;
            this.Action = commandLineArgument.Action;
            this.ActionParameters = commandLineArgument.ActionParameters;

            Type argType = this.GetType();
            foreach (PropertyInfo propInfo in
                argType.GetProperties(
                    BindingFlags.NonPublic | BindingFlags.Instance))
            {
                this.InitActionParamProperty(propInfo);
            }
        }

        public string Category { get; private set; }

        public string Action { get; private set; }

        public IReadOnlyDictionary<string, string> ActionParameters
        {
            get;
            private set;
        }

        public virtual bool IsValid() => true;

        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        private void InitActionParamProperty(PropertyInfo propInfo)
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
                        this.ActionParameters.Keys.Any(
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
                this.ActionParameters[matchedActionParam];

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
                    ExceptionMessages.PropMatchedActionParamValueNotNull,
                    propInfo.Name);
            }
        }
    }
}