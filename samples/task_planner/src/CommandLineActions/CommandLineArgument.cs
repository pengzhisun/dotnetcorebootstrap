// -----------------------------------------------------------------------
// <copyright file="CommandLineArgument.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the common command line argument class.
    /// </summary>
    internal class CommandLineArgument
    {
        /// <summary>
        /// The default command line category.
        /// </summary>
        public const string DefaultCategory = Constants.GeneralCategory;

        /// <summary>
        /// The default command line action.
        /// </summary>
        public static readonly string DefaultAction =
            GeneralActionType.Default.ToString();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgument"/>
        /// class with specific category, action and action parameters.
        /// </summary>
        /// <param name="category">The specific category.</param>
        /// <param name="action">The specific action.</param>
        /// <param name="actionParams">
        /// The action parameters for specific action.
        /// </param>
        public CommandLineArgument(
            string category = null,
            string action = null,
            IReadOnlyDictionary<string, string> actionParams = null)
        {
            this.Category = category ?? DefaultCategory;
            this.Action = action ?? DefaultAction;
            this.ActionParameters =
                actionParams ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the category from the current command line argument.
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// Gets the action from the current command line argument.
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Gets the action parameters from the current command line argument.
        /// </summary>
        public IReadOnlyDictionary<string, string> ActionParameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and returns a string representation of the current command
        /// line argument.
        /// </summary>
        /// <returns>
        /// A string representation of the current command line argument.
        /// </returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}