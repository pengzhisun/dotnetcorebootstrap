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