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
        /// argument for <see cref="CommandLineErrorCode" />.
        /// </summary>
        public const string DefaultCommandLineExceptionMessage =
            @"The command line exception message occurred with error code: '{0}'.";

        /// <summary>
        /// The invalid command line arguments exception message format, expect
        /// one argument for the invalid value and one for full arguments string.
        /// </summary>
        public const string InvalidCommandLineArguments =
            @"The input arguments contains invalid value: '{0}', full arguments: '{1}'";

        /// <summary>
        /// The action value is not an enumeration value exception message format,
        /// expect one argument for action value type name and one for invalid
        /// action value.
        /// </summary>
        public const string ActionValueNotEnumValue =
            @"The action parameter value '[{0}]{1}' should be an enum value.";

        /// <summary>
        /// The action paramaters contains no matched value for action argument
        /// property exception message.
        /// </summary>
        public const string ActionParamNoAlias =
            @"Action parameter should have at least one alias.";

        /// <summary>
        /// The given category is empty or whitespace exception message format,
        /// expect one argument for category value.
        /// </summary>
        public const string CategoryIsEmptyOrWhitespace =
            @"The given category '{0}' shouldn't be empty or whitespace.";

        /// <summary>
        /// The given type of the action type is not an enumeration type
        /// exception message format, expect one argument for given value.
        /// </summary>
        public const string AciontTypeTypeNotEnumType =
            @"The given actionTypeType '{0}' should be an enum type.";

        /// <summary>
        /// There are more than one matched value found from action parameters
        /// for specific action argument property exception message format,
        /// expect one argument for the property name.
        /// </summary>
        public const string PropMatchedMoreThanOneActionParams =
            @"Action parameter property '{0}' matched more than one action parameter.";

        /// <summary>
        /// The matched value found from action parameters for specific action
        /// argument property is null exception message format, expect one
        /// argument for the property name.
        /// </summary>
        public const string PropMatchedActionParamValueIsNull =
            @"The matched action parameter value for property '{0}' shouldn't be null.";

        /// <summary>
        /// The action method not accept one parameter exception message format,
        /// expect one argument for the method signature.
        /// </summary>
        public const string ActionMethodNotAcceptOneParam =
            @"The action method '{0}' should accept only one parameter.";

        /// <summary>
        /// The action method not accept one parameter which type is a derived
        /// class of the <see cref="ActionArgumentBase"/> exception message
        /// format, expect one argument for the method signature.
        /// </summary>
        public const string ActionMethodNotAcceptOneActionArgumentParam =
            @"The action method '{0}' should accept one ActionArgumentBase type parameter.";

        /// <summary>
        /// Action method for specific action type was not found exception
        /// message format, expect one argument for the action type.
        /// </summary>
        public const string ActionMethodNotFound =
            @"Can't find action definition for '{0}'.";

        /// <summary>
        /// Found more than one matched action methods for specific action type
        /// exception message format, expect one argument for the action type.
        /// </summary>
        public const string ActionMethodFoundDupDefinitions =
            @"Should have only one action definition for '{0}', declard types: {1}.";

        /// <summary>
        /// Category definition class for specific category was not found
        /// exception message format, expect one argument for the category.
        /// </summary>
        public const string CategoryNotFound =
            @"Can't find category definition for '{0}'.";

        /// <summary>
        /// Found more than one matched category definition classes for specific
        /// category exception message format, expect one argument for the
        /// category.
        /// </summary>
        public const string CategoryFoundDupDefinitions =
            @"Should have only one category definition for '{0}', declard types: {1}.";
    }
}