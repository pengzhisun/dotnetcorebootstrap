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
                throw new ArgumentException(
                    $"The action parameter value '[{actionType.Name}]{action}' should be an enum value.",
                    nameof(action));
            }

            this.Action = action;
        }

        public object Action { get; private set; }

        public override string ToString()
            => $"[{this.GetType().Name}] {nameof(this.Action)} = '{this.Action}'";
    }
}