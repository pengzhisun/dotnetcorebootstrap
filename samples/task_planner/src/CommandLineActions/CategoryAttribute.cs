// -----------------------------------------------------------------------
// <copyright file="CategoryAttribute.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

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

        public string Category { get; private set; }

        public Type ActionTypeType { get; private set; }
    }
}