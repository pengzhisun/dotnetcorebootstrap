/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   EnvironmentVariablesConfigDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core environment variables configuration demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.EnvironmentVariables
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder
 *****************************************************************************/

namespace DotNetCoreBootstrap.ConfigDemo
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Defines the environment variables configuration demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages: Microsoft.Extensions.Configuration.EnvironmentVariables
    /// and Microsoft.Extensions.Configuration.Binder .
    /// </remarks>
    internal static class EnvironmentVariablesConfigDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            Environment.SetEnvironmentVariable("str_setting_1", "str_value_1");
            Environment.SetEnvironmentVariable("int_setting_1", "1");
            Environment.SetEnvironmentVariable("section1:nested_setting_1", "nested_value_1");
            Environment.SetEnvironmentVariable("array_section:0:item_setting", "item_value_1");
            Environment.SetEnvironmentVariable("array_section:1:item_setting", "item_value_2");

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

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

            // get int value demo, using GetValue<T> method
            getValueAction(
                "int_setting_1",
                () =>
                {
                    int value =
                        config.GetValue<int>("int_setting_1", defaultValue: 0);
                    return value;
                });

            // get array item value demo, using ':' delimiter and indexer
            getValueAction(
                "array_section:0:item_setting",
                () =>
                {
                    string value = config["array_section:0:item_setting"];
                    return value;
                });
        }
    }
}