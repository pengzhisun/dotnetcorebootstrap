namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class ActionAttribute : Attribute
    {
        public ActionAttribute(object action)
        {
            this.Action = action;
        }

        public object Action { get; private set; }
    }
}