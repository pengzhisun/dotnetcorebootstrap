/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   CommandLineConfigDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core command line configuration demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.CommandLine
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder
 *****************************************************************************/

namespace DotNetCoreBootstrap.ConfigDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Defines the command line configuration demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages: Microsoft.Extensions.Configuration.CommandLine
    /// and Microsoft.Extensions.Configuration.Binder .
    /// </remarks>
    internal static class CommandLineConfigDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <examples>
        /// dotnet run str_setting_1=cmd_str_value_1 /int_setting_1=2 /section1:nested_setting_1=cmd_nested_value_1 -nested_setting_2 cmd_nested_value_2
        /// </examples>
        public static void Run(string[] args)
        {
            // default command line arguments:
            string[] cmdArgs = args.Any() ? args : new string[]
                {
                    "str_setting_1=def_str_value_1",
                    "/int_setting_1=1",
                    "/section1:nested_setting_1=def_nested_value_1",
                    "-nested_setting_2",
                    "def_nested_value_2"
                };

            // raw in-memory configurations
            Dictionary<string, string> switchMapping = new Dictionary<string, string>()
                {
                    { "-str_setting_1", "str_setting_1" },
                    { "-int_setting_1", "int_setting_1" },
                    { "-nested_setting_1", "section1:nested_setting_1" },
                    { "-nested_setting_2", "section1:nested_setting_2" },
                };

            // override in-memory configurations with command line configurations
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddCommandLine(cmdArgs, switchMapping);

            IConfiguration config = configBuilder.Build();

            Action<string, Func<object>> getValueAction =
                (path, getValueFunc) =>
                {
                    object value = getValueFunc();
                    Console.WriteLine($"path: '{path}', value: '{value}'");
                };

            // get string value demo
            getValueAction(
                "str_setting_1",
                () =>
                {
                    string value = config["str_setting_1"];
                    return value;
                });

            // get nested string value demo, using ':' delimiter
            getValueAction(
                "section1:nested_setting_1",
                () =>
                {
                    string value = config["section1:nested_setting_1"];
                    return value;
                });

            getValueAction(
                "section1:nested_setting_2",
                () =>
                {
                    string value = config["section1:nested_setting_2"];
                    return value;
                });

            // get int value demo, using GetValue<T> method
            getValueAction(
                "int_setting_1",
                () =>
                {
                    int value =
                        config.GetValue<int>("int_setting_1", defaultValue: 0);
                    return value;
                });
        }
    }
}