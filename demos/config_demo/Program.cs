﻿/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
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
            PrintMessageBlock("Begin .Net Core Configuartion Demos", '#');

            RunDemo("JsonFileConfigDemo", JsonFileConfigDemo.Run);
            RunDemo("XmlFileConfigDemo", XmlFileConfigDemo.Run);
            RunDemo("IniFileConfigDemo", IniFileConfigDemo.Run);
            RunDemo("InMemoryConfigDemo", InMemoryConfigDemo.Run);
            RunDemo("EnvironmentVariablesConfigDemo", EnvironmentVariablesConfigDemo.Run);
            RunDemo("CommandLineConfigDemo", () => CommandLineConfigDemo.Run(args));
            RunDemo("UserSecretsConfigDemo", UserSecretsConfigDemo.Run);
            RunDemo("AzureKeyVaultConfigDemo", AzureKeyVaultConfigDemo.Run);
            RunDemo("ConfigurationManagerDemo", ConfigurationManagerDemo.Run);

            PrintMessageBlock("End .Net Core Configuartion Demos", '#');
        }

        /// <summary>
        /// Run specific demo.
        /// </summary>
        /// <param name="demoName">The demo name.</param>
        /// <param name="demoAction">The demo action.</param>
        private static void RunDemo(string demoName, Action demoAction)
        {
            PrintMessageBlock($"Run '{demoName}'", '*');

            try
            {
                demoAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().FullName}: {ex.Message}\r\nStack Trace:\r\n{ex.StackTrace}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Print message to console.
        /// </summary>
        /// <param name="message">The message to be print.</param>
        /// <param name="blockChar">The message block char.</param>
        /// <param name="newLine">Print new line after message block.</param>
        private static void PrintMessageBlock(
            string message,
            char blockChar,
            bool newLine = true)
        {
            Console.WriteLine(new string(blockChar, 80));
            Console.WriteLine($"{blockChar} [{DateTime.Now:o}] {message}");
            Console.WriteLine(new string(blockChar, 80));

            if (newLine)
            {
                Console.WriteLine();
            }
        }
    }
}
