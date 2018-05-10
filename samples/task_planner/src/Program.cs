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

    [ExcludeFromCoverage]
    internal static class Program
    {
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
                Console.WriteLine(excption.GetDetail());
            }
        }
    }
}
