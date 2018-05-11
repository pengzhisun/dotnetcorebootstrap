// -----------------------------------------------------------------------
// <copyright file="ExceptionMessages.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    /// <summary>
    /// Defines the exception messages for command line actions.
    /// </summary>
    internal static class ExceptionMessages
    {
        /// <summary>
        /// The default command line exception message format, expect one
        /// <see cref="CommandLineErrorCode" /> arg.
        /// </summary>
        public const string DefaultCommandLineExceptionMessage =
            @"The command line exception message occurred with error code: '{0}'.";

        /// <summary>
        /// The invalid command line arguments exception message format, expect
        /// one invalid argument value and one full arguments string.
        /// </summary>
        public const string InvalidCommandLineArguments =
            @"The input arguments contains invalid value: '{0}', full arguments: '{1}'";

        /// <summary>
        /// The action value is not an enumeration value exception message format,
        /// expect one action value type name and one invalid action value.
        /// </summary>
        public const string ActionValueNotEnumValue =
            @"The action parameter value '[{0}]{1}' should be an enum value.";

        public const string ActionParamNoAlias =
            @"Action parameter should have at least one alias.";

        public const string CategoryNotEmptyNorWhitespace =
            @"The given category '{0}' shouldn't be empty or whitespace.";

        public const string AciontTypeTypeNotEnumType =
            @"The given actionTypeType '{0}' should be an enum type.";

        public const string PropMatchedMoreThanOneActionParams =
            @"Action parameter property '{0}' matched more than one action parameter.";

        public const string PropMatchedActionParamValueNotNull =
            @"The matched action parameter value for property '{0}' shouldn't be null.";

        public const string ActionMethodNotAcceptOneParam =
            @"The action method '{0}' should accept only one parameter.";

        public const string ActionMethodNotAcceptOneActionArgumentParam =
            @"The action method '{0}' should accept one ActionArgumentBase type parameter.";

        public const string ActionMethodNotFound =
            @"Can't find action definition for '{0}'.";

        public const string ActionMethodFoundDupDefinitions =
            @"Should have only one action definition for '{0}', declard types: {1}.";

        public const string CategoryNotFound =
            @"Can't find category definition for '{0}'.";

        public const string CategoryNotFoundDupDefinitions =
            @"Should have only one category definition for '{0}', declard types: {1}.";
    }
}