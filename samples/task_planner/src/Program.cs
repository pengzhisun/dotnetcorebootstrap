// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;
    using DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions;

    /// <summary>
    /// Defines the task planner console application.
    /// </summary>
    [ExcludeFromCoverage]
    internal static class Program
    {
        /// <summary>
        /// The main entry for task planner console application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            try
            {
                CommandLineArgument arg =
                    CommandLineArgumentParser.Parse(args);

                CommandLineEngine engine = new CommandLineEngine();

                engine.Process(arg);
            }
            catch (Exception excption)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(
                        $"Command: {AppDomain.CurrentDomain.FriendlyName} {string.Join(" ", args)}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(excption.GetDetail());
                }
                finally
                {
                    Console.ForegroundColor = previousColor;
                }
            }
        }
    }
}
