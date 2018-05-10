// -----------------------------------------------------------------------
// <copyright file="CommandLineErrorCode.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    public enum CommandLineErrorCode
    {
        Unknown = 0,
        CommandLineArgsParseFailed = 1001,
        CommandLineArgInitFailed = 1002,
        InvalidActionMethodDefinition = 1003,
        InvalidCategoryDefinition = 1004
    }
}