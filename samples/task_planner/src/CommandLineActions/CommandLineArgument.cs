namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;

    internal class CommandLineArgument
    {
        private const string DefaultCategory = "General";

        private const string DefaultAction = "Default";

        public CommandLineArgument(
            string category = DefaultCategory,
            string action = DefaultAction,
            IDictionary<string, string> actionParams = null)
        {
            this.Category = category;
            this.Action = action;
            this.ActionParameters =
                (actionParams ?? new Dictionary<string, string>())
                as IReadOnlyDictionary<string, string>;
        }

        public CommandLineArgument(
            CommandLineArgument commandLineArgument)
        {
            this.Category = commandLineArgument.Category;
            this.Action = commandLineArgument.Action;
            this.ActionParameters = commandLineArgument.ActionParameters;

            Type argType = this.GetType();
            foreach (PropertyInfo propInfo in
                argType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                ActionParameterAttribute actionParamAttr =
                    propInfo.GetCustomAttribute<ActionParameterAttribute>();

                if (actionParamAttr == null)
                {
                    continue;
                }

                bool paramFound = false;
                foreach (string alias in actionParamAttr.Aliases)
                {
                    if (this.ActionParameters.TryGetValue(
                        alias, out string paramValueString))
                    {
                        if (paramFound)
                        {
                            throw new InvalidOperationException(
                                $"Action parameter '{propInfo.Name}' should only have one.");
                        }

                        if (paramValueString != null)
                        {
                            object paramValue =
                            Convert.ChangeType(
                                paramValueString,
                                propInfo.PropertyType);

                            propInfo.SetValue(this, paramValue);
                        }
                        else if (propInfo.PropertyType == typeof(bool?))
                        {
                            propInfo.SetValue(this, true);
                        }
                        else
                        {
                            throw new InvalidOperationException(
                                $"Action parameter '{propInfo.Name}' should have param value.");
                        }
                    }
                }

                if (!paramFound && actionParamAttr.DefaultValue != null)
                {
                    propInfo.SetValue(this, actionParamAttr.DefaultValue);
                }
            }
        }

        public string Category { get; private set; }

        public string Action { get; private set; }

        public IReadOnlyDictionary<string, string> ActionParameters { get; private set; }

        public virtual bool IsValid() => true;

        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}