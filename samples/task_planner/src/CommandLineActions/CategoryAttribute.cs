// -----------------------------------------------------------------------
// <copyright file="CategoryAttribute.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    /// <summary>
    /// Defines the category definition attribute used on category definition
    /// class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class CategoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAttribute"/>
        /// class with a specified action enumeration value.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="actionTypeType">The type of the action type.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the given category or actionTypeType is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the given category or actionTypeType is empty or
        /// whitespace.
        /// </exception>
        public CategoryAttribute(string category, Type actionTypeType)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                string messageFormat =
                    ExceptionMessages.CategoryIsEmptyOrWhitespace;
                throw new ArgumentException(
                    messageFormat.FormatInvariant(category),
                    nameof(category));
            }

            if (actionTypeType == null)
            {
                throw new ArgumentNullException(nameof(actionTypeType));
            }

            if (!actionTypeType.IsEnum)
            {
                string messageFormat =
                    ExceptionMessages.AciontTypeTypeNotEnumType;
                throw new ArgumentException(
                    messageFormat.FormatInvariant(actionTypeType.Name),
                    nameof(actionTypeType));
            }

            this.Category = category;
            this.ActionTypeType = actionTypeType;
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// Gets the type of the action type.
        /// </summary>
        public Type ActionTypeType { get; private set; }
    }
}