// -----------------------------------------------------------------------
// <copyright file="CommandLineErrorCode.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    /// <summary>
    /// Defines the command line error codes enumeration.
    /// </summary>
    [Serializable]
    public enum CommandLineErrorCode
    {
        /// <summary>
        /// The unknown command line error.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The command line arguments parsing failed error.
        /// </summary>
        CommandLineArgsParseFailed = 1001,

        /// <summary>
        /// The command line argument initializing failed error.
        /// </summary>
        CommandLineArgInitFailed = 1002,

        /// <summary>
        /// The invalid action method definition error.
        /// </summary>
        InvalidActionMethodDefinition = 1003,

        /// <summary>
        /// The invalid category definition error.
        /// </summary>
        InvalidCategoryDefinition = 1004
    }
}