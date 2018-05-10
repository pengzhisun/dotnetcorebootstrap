namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class ActionAttribute : Attribute
    {
        public ActionAttribute(object action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Type actionType = action.GetType();
            if (!actionType.IsEnum)
            {
                string messageFormat = ExceptionMessages.ActionValueNotEnumValue;
                throw new ArgumentException(
                    messageFormat.FormatInvariant(actionType.Name, action),
                    nameof(action));
            }

            this.Action = action;
        }

        public object Action { get; private set; }

        public override string ToString()
            => $"[{this.GetType().Name}] {nameof(this.Action)} = '{this.Action}'";
    }
}