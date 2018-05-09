namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;

    [ExcludeFromCoverage]
    [AttributeUsage(
        AttributeTargets.Class
        | AttributeTargets.Property
        | AttributeTargets.Method
        | AttributeTargets.Constructor)]
    public sealed class ExcludeFromCoverageAttribute
        : Attribute { }
}