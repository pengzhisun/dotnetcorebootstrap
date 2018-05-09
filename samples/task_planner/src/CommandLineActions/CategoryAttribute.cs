namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class CategoryAttribute : Attribute
    {
        public CategoryAttribute(string category, Type actionTypeType)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException(
                    @"The category parameter '{category}' shouldn't be empty or whitespace.",
                    nameof(category));
            }

            if (actionTypeType == null)
            {
                throw new ArgumentNullException(nameof(actionTypeType));
            }

            if (!actionTypeType.IsEnum)
            {
                throw new ArgumentException(
                    $"The actionTypeType parameter '{actionTypeType.Name}' should be an enum type.",
                    nameof(actionTypeType));
            }

            this.Category = category;
            this.ActionTypeType = actionTypeType;
        }

        public string Category { get; private set; }

        public Type ActionTypeType { get; private set; }
    }
}