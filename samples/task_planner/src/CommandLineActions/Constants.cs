// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    internal static class Constants
    {
        public const string GeneralCategory = @"General";

        public const string HelpMessage = @"tp command line arguments:
-----------------------------------------------------------
general arguments:
   -h, --help:      show tp command help message.
   -v, --version:   show tp command version.";

        public const string VersionMessageFormat = @"Version: {0}";
    }
}