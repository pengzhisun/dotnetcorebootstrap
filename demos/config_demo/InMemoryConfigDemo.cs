/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   InMemoryConfigDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core In-memory configuration demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder
 *****************************************************************************/

namespace DotNetCoreBootstrap.ConfigDemo
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Defines the In-memory configuration demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages: Microsoft.Extensions.Configuration
    /// and Microsoft.Extensions.Configuration.Binder .
    /// </remarks>
    internal static class InMemoryConfigDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            Dictionary<string, string> configDic = new Dictionary<string, string>()
                {
                    { "str_setting_1", "str_value_1" },
                    { "int_setting_1", "1" },
                    { "poco_section:poco_setting_1", "poco_value_1" },
                    { "array_section:0:item_setting", "item_value_1" },
                    { "array_section:1:item_setting", "item_value_2" },
                    { "graph_section:graph_sub_section:graph_setting_1", "graph_value_1" },
                };

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(configDic);

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

            // get int value demo, using GetValue<T> method
            getValueAction(
                "int_setting_1",
                () =>
                {
                    int value =
                        config.GetValue<int>("int_setting_1", defaultValue: 0);
                    return value;
                });

            // get POCO value demo, using GetSection and Bind methods
            getValueAction(
                "poco_section:poco_setting_1",
                () =>
                {
                    PocoSection pocoSection = new PocoSection();

                    config.GetSection("poco_section").Bind(pocoSection);
                    string value = pocoSection.poco_setting_1;
                    return value;
                });

            // get array item value demo, using GetSection and Bind methods
            getValueAction(
                "array_section:0:item_setting",
                () =>
                {
                    List<ArrayItem> array = new List<ArrayItem>();
                    config.GetSection("array_section").Bind(array);

                    string value = array[0].item_setting;
                    return value;
                });

            // get graph object value demo, using GetSection and Get<T> methods
            getValueAction(
                "graph_section:graph_sub_section:graph_setting_1",
                () =>
                {
                    GraphSection graphSection =
                        config.GetSection("graph_section").Get<GraphSection>();
                    string value = graphSection.graph_sub_section.graph_setting_1;
                    return value;
                });
        }

        /// <summary>
        /// Defiens the POCO section class, refer: https://en.wikipedia.org/wiki/Plain_old_CLR_object
        /// </summary>
        private sealed class PocoSection
        {
            /// <summary>
            /// The POCO setting.
            /// </summary>
            /// <returns>The POCO setting value.</returns>
            public string poco_setting_1 { get; set; }
        }

        /// <summary>
        /// Defiens the array item class.
        /// </summary>
        private sealed class ArrayItem
        {
            /// <summary>
            /// The array item setting.
            /// </summary>
            /// <returns>The array item setting value.</returns>
            public string item_setting { get; set; }
        }

        /// <summary>
        /// Defines the graph section class.
        /// </summary>
        private sealed class GraphSection
        {
            /// <summary>
            /// The graph sub section.
            /// </summary>
            /// <returns>The graph sub section.</returns>
            public GraphSubSection graph_sub_section { get; set; }
        }

        /// <summary>
        /// Defines the graph sub section class.
        /// </summary>
        private sealed class GraphSubSection
        {
            /// <summary>
            /// The graph setting.
            /// </summary>
            /// <returns>The graph setting value.</returns>
            public string graph_setting_1 { get; set; }
        }
    }
}