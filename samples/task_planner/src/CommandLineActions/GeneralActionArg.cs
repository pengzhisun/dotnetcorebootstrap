// -----------------------------------------------------------------------
// <copyright file="GeneralActionArg.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    internal sealed class GeneralActionArg : CommandLineArgument
    {
        public GeneralActionArg(CommandLineArgument arg)
            : base(arg)
        {
        }

        public bool HelpSwtichEnabled => this.HelpSwitch ?? false;

        public bool VersionSwtichEnabled => this.VersionSwith ?? false;

        [ActionParameter(false, "-h", "--help")]
        private bool? HelpSwitch { get; set; }

        [ActionParameter(false, "-v", "--version")]
        private bool? VersionSwith { get; set; }

        public override bool IsValid() =>
            this.HelpSwtichEnabled ^ this.VersionSwtichEnabled;
    }
}