namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;
    using Xunit.Abstractions;

    internal static class TestOutputHelperExtensions
    {
        public static void PrintException(
            this ITestOutputHelper output,
            Exception exception)
            => output.WriteLine(exception.GetDetail());
    }
}