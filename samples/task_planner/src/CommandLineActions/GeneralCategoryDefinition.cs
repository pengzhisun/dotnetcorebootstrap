// -----------------------------------------------------------------------
// <copyright file="GeneralCategoryDefinition.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Defines the general category actions.
    /// </summary>
    [Category(Constants.GeneralCategory, typeof(GeneralActionType))]
    internal static class GeneralCategoryDefinition
    {
        /// <summary>
        /// The default action in general category.
        /// If the help switch enabled or the arg is invalid then show the help
        /// message.
        /// If the version switch enabled then show the version message.
        /// </summary>
        /// <param name="actionArg">The general action argument.</param>
        [Action(GeneralActionType.Default)]
        public static void DefaultAction(GeneralActionArgument actionArg)
        {
            if (actionArg == null)
            {
                throw new ArgumentNullException(nameof(actionArg));
            }

            if (!actionArg.IsValid() || actionArg.HelpSwtichEnabled)
            {
                ShowHelp();
            }
            else if (actionArg.VersionSwtichEnabled)
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