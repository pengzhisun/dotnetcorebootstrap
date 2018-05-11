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

    /// <summary>
    /// Defines the general category class.
    /// </summary>
    [Category(Constants.GeneralCategory, typeof(GeneralActionType))]
    internal static class GeneralCategory
    {
        /// <summary>
        /// The default action in general category.
        /// If the help switch enabled or the arg is invalid then show the help
        /// message.
        /// If the version switch enabled then show the version message.
        /// </summary>
        /// <param name="arg">The general action argument.</param>
        [Action(GeneralActionType.Default)]
        public static void DefaultAction(GeneralActionArgument arg)
        {
            if (!arg.IsValid() || arg.HelpSwtichEnabled)
            {
                ShowHelp();
            }
            else if (arg.VersionSwtichEnabled)
            {
                ShowVersion();
            }
        }

        /// <summary>
        /// Show general help message.
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine(Constants.HelpMessage);
        }

        /// <summary>
        /// Show version message, the version number is from current executing
        /// assembly.
        /// </summary>
        private static void ShowVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine(Constants.VersionMessageFormat, version);
        }
    }
}