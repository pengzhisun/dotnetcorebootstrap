namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
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
                                $"Action parameter '{propInfo.Name}' found more than one alias.");
                        }

                        if (paramValueString != null)
                        {
                            Type paramType =
                                Nullable.GetUnderlyingType(propInfo.PropertyType)
                                ?? propInfo.PropertyType;

                            object paramValue =
                                Convert.ChangeType(paramValueString, paramType);

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

                        paramFound = true;
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