// -----------------------------------------------------------------------
// <copyright file="ActionParameterAttribute.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the action parameter attribute used on action argument property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class ActionParameterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionParameterAttribute"/>
        /// class with a default value and one or more aliases.
        /// </summary>
        /// <param name="defaultValue">
        /// The default parameter value if no action parameter found from
        /// command line argument.
        /// </param>
        /// <param name="aliases">
        /// The parameter aliases used for finding action parameter from command
        /// line argument, typically each parameter should have one shortcut
        /// alias with one dash symbol prefix like '-n' and one full
        /// descriptive alias with two dash symbols prefix and one dash
        /// delimiter like '--task-name'.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the given aliases is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the given aliases array is empty.
        /// </exception>
        public ActionParameterAttribute(
            object defaultValue,
            params string[] aliases)
        {
            if (aliases == null)
            {
                throw new ArgumentNullException(nameof(aliases));
            }

            if (aliases.Length == 0)
            {
                throw new ArgumentException(
                    ExceptionMessages.ActionParamNoAlias,
                    nameof(aliases));
            }

            this.DefaultValue = defaultValue;
            this.Aliases = Array.AsReadOnly(aliases);
        }

        /// <summary>
        /// Gets the default value for target action argument property.
        /// </summary>
        public object DefaultValue { get; private set; }

        /// <summary>
        /// Gets the aliases for target action argument property.
        /// </summary>
        public IReadOnlyCollection<string> Aliases { get; private set; }
    }
}