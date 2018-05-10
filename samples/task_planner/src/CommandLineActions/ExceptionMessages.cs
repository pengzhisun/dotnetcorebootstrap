namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    internal static class ExceptionMessages
    {
        public const string DefaultCommandLineExceptionMessage =
            @"The command line exception message occurred with error code: '{0}'.";

        public const string InvalidCommandLineArguments =
            @"The input arguments contains invalid value: '{0}', full arguments: '{1}'";

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
    }
}