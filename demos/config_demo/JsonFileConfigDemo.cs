/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   JsonFileConfigDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core JSON format configuration file demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder
 *****************************************************************************/

namespace DotNetCoreBootstrap.ConfigDemo
{
    using System;
    using System.IO;
    using System.Text;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Defines the JSON format configuration file demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages: Microsoft.Extensions.Configuration.Xml
    /// and Microsoft.Extensions.Configuration.Binder .
    /// </remarks>
    internal static class JsonFileConfigDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json",
                optional: true,
                reloadOnChange: true);

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

            // print appsettings.xml
            Console.WriteLine();
            string appSettingsFilePath =
                Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            Console.WriteLine($"[Trace] config file path: {appSettingsFilePath}");
            string appSettingsFileContent =
                File.ReadAllText(appSettingsFilePath, Encoding.UTF8);
            Console.WriteLine(appSettingsFileContent);
        }
    }
}