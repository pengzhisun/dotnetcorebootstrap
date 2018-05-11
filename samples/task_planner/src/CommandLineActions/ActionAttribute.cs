// -----------------------------------------------------------------------
// <copyright file="ActionAttribute.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    /// <summary>
    /// Defines the action definition attribute used on action method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class ActionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineException"/>
        /// class with a specified action enumeration value.
        /// </summary>
        /// <param name="action">The action enumeration value.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the given action value is null.
        /// </exception>
        /// <exception cref="CommandLineException">
        /// Thrown if the given action value is not an enumeration value.
        /// </exception>
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

        /// <summary>
        /// Gets the action.
        /// </summary>
        public object Action { get; private set; }

        /// <summary>
        /// Creates and returns a string representation of the current attribute.
        /// </summary>
        /// <returns>A string representation of the current attribute.</returns>
        public override string ToString()
            => $"[{this.GetType().Name}] {nameof(this.Action)} = '{this.Action}'";
    }
}