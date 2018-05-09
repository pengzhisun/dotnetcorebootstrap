namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;
    using DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions;

    [ExcludeFromCoverage]
    internal static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CommandLineArgument arg =
                    CommandLineArgumentParser.Parse(args);

                Console.WriteLine($"Command line argument received: {arg}");
            }
            catch (Exception excption)
            {
                Console.WriteLine(excption.Message);
                Console.WriteLine(excption.StackTrace);
            }
        }
    }


}
