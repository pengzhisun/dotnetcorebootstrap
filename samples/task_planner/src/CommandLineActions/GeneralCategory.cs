// -----------------------------------------------------------------------
// <copyright file="GeneralCategory.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Reflection;

    [Category(Constants.GeneralCategory, typeof(GeneralActionType))]
    internal sealed class GeneralCategory
    {
        [Action(GeneralActionType.Default)]
        public void DefaultAction(GeneralActionArg arg)
        {
            if (!arg.IsValid() || arg.HelpSwtichEnabled)
            {
                this.ShowHelp();
            }
            else if (arg.VersionSwtichEnabled)
            {
                this.ShowVersion();
            }
        }

        private void ShowHelp()
        {
            Console.WriteLine(Constants.HelpMessage);
        }

        private void ShowVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine(Constants.VersionMessageFormat, version);
        }
    }
}