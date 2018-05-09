namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class CategoryAttribute : Attribute
    {
        public CategoryAttribute(string category, Type actionTypeType)
        {
            this.Category = category;
            this.ActionTypeType = actionTypeType;
        }

        public string Category { get; private set; }

        public Type ActionTypeType { get; private set; }
    }
}