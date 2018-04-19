/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.  
 *
 * File Name:   Program.cs
 * Author:      Pengzhi Sun
 * Description: DotNetCore configuration demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/ 
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Xml
 *****************************************************************************/

namespace Winl.DncBootstrap.ConfigDemo
{
    using System;
    using Microsoft.Extensions.Configuration;

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
            RunDemo("JsonFileConfigDemo", JsonFileConfigDemo);
            RunDemo("XmlFileConfigDemo", XmlFileConfigDemo);
        }

        /// <summary>
        /// Run specific demo.
        /// </summary>
        /// <param name="demoName">The demo name.</param>
        /// <param name="demoAction">The demo action.</param>
        private static void RunDemo(string demoName, Action demoAction)
        {
            Console.WriteLine(new string('*', 80));
            Console.WriteLine($"* run demo {demoName} at {DateTime.Now}");
            Console.WriteLine(new string('*', 80));
            Console.WriteLine();

            demoAction();

            Console.WriteLine();
        }

        /// <summary>
        /// The JSON format configuration file demo.
        /// </summary>
        /// <remarks>
        /// Depends on nuget packages: Microsoft.Extensions.Configuration.Json
        /// and Microsoft.Extensions.Configuration.Binder .
        /// </remarks>
        private static void JsonFileConfigDemo()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", 
                optional: true, 
                reloadOnChange: true);
            
            IConfiguration config = configBuilder.Build();

            string path = "section1:nested_setting_1";
            Console.WriteLine($"path: '{path}', value: '{config[path]}'");

            path = "str_setting_1";
            Console.WriteLine($"path: '{path}', value: '{config[path]}'");

            path = "int_setting_1";
            Console.WriteLine($"path: '{path}', value: '{config.GetValue<int>(path, defaultValue: 0)}'");

            path = "array_section:0:item_setting";
            Console.WriteLine($"path: '{path}', value: '{config[path]}'");
        }

        /// <summary>
        /// The XML format configuration file demo.
        /// </summary>
        /// <remarks>
        /// Depends on nuget packages: Microsoft.Extensions.Configuration.Xml
        /// and Microsoft.Extensions.Configuration.Binder .
        /// </remarks>
        private static void XmlFileConfigDemo()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddXmlFile("appsettings.xml", 
                optional: true, 
                reloadOnChange: true);
                
            IConfiguration config = configBuilder.Build();

            string path = "section1:nested_setting_1";
            Console.WriteLine($"path: '{path}', value: '{config[path]}'");

            path = "str_setting_1";
            Console.WriteLine($"path: '{path}', value: '{config[path]}'");

            path = "int_setting_1";
            Console.WriteLine($"path: '{path}', value: '{config.GetValue<int>(path, defaultValue: 0)}'");

            path = "array_items:array_item:item_1:item_setting";
            Console.WriteLine($"path: '{path}', value: '{config[path]}'");
        }
    }
}
