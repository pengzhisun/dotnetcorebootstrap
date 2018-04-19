/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License.
 * See LICENSE file in the project root for full license information.  
 *
 * File Name:   Program.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core configuration demos.
 *****************************************************************************/

namespace DotNetCoreBootstrap.ConfigDemo
{
    using System;

    /// <summary>
    /// Defines the demo console application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point.
        /// </summary>
        /// <param name="args">The application command line arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine(new string('#', 80));
            Console.WriteLine($"# Begin .Net Core Configuartion Demos at {DateTime.Now}");
            Console.WriteLine(new string('#', 80));
            Console.WriteLine();

            RunDemo("JsonFileConfigDemo", JsonFileConfigDemo.Run);
            RunDemo("XmlFileConfigDemo", XmlFileConfigDemo.Run);

            Console.WriteLine(new string('#', 80));
            Console.WriteLine($"# End .Net Core Configuartion Demos at {DateTime.Now}");
            Console.WriteLine(new string('#', 80));
        }

        /// <summary>
        /// Run specific demo.
        /// </summary>
        /// <param name="demoName">The demo name.</param>
        /// <param name="demoAction">The demo action.</param>
        private static void RunDemo(string demoName, Action demoAction)
        {
            Console.WriteLine(new string('*', 80));
            Console.WriteLine($"* Run demo '{demoName}' at {DateTime.Now}");
            Console.WriteLine(new string('*', 80));
            Console.WriteLine();

            demoAction();

            Console.WriteLine();
        }
    }
}
