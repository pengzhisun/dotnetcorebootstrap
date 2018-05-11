// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    /// <summary>
    /// Defines the constant values used for command line actions.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The general category.
        /// </summary>
        public const string GeneralCategory = @"General";

        /// <summary>
        /// The help message.
        /// </summary>
        public const string HelpMessage = @"tp command line arguments:
-----------------------------------------------------------
general arguments:
   -h, --help:      show tp command help message.
   -v, --version:   show tp command version.";

        /// <summary>
        /// The version message format, expect one version arg.
        /// </summary>
        public const string VersionMessageFormat = @"Version: {0}";
    }
}