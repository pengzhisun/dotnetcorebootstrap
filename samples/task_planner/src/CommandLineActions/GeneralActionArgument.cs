// -----------------------------------------------------------------------
// <copyright file="GeneralActionArgument.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    /// <summary>
    /// Defiens the general category action argument class.
    /// </summary>
    internal sealed class GeneralActionArgument : ActionArgumentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralActionArgument"/>
        /// class from given command line argument.
        /// </summary>
        /// <param name="commandLineArgument">
        /// The given command line argument.
        /// </param>
        /// <exception cref="CommandLineException">
        /// Thrown if the action property matched more than one action parameter,
        /// or the matched action parameter value for specific property is null.
        /// </exception>
        public GeneralActionArgument(CommandLineArgument commandLineArgument)
            : base(commandLineArgument)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the help switch is enabled or not.
        /// </summary>
        public bool HelpSwtichEnabled => this.HelpSwitch ?? false;

        /// <summary>
        /// Gets a value indicating whether the version switch is enabled or not.
        /// </summary>
        public bool VersionSwtichEnabled => this.VersionSwith ?? false;

        /// <summary>
        /// Gets or sets the help switch flag, this property is initialized from
        /// base class constructor.
        /// </summary>
        [ActionParameter(false, "-h", "--help")]
        private bool? HelpSwitch { get; set; }

        /// <summary>
        /// Gets or sets the version switch flag, this property is initialized
        /// from base class constructor.
        /// </summary>
        [ActionParameter(false, "-v", "--version")]
        private bool? VersionSwith { get; set; }

        /// <summary>
        /// Checks the action argument is valid or not, the derived should
        /// override this method based on its own logic.
        /// </summary>
        /// <returns>
        /// Return true if either only help switch or version switch enabled,
        /// otherwise false.
        /// </returns>
        public override bool IsValid() =>
            this.HelpSwtichEnabled ^ this.VersionSwtichEnabled;
    }
}