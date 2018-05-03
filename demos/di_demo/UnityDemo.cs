/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   UnityDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core unity demos.
 * Reference:   https://www.nuget.org/packages/Unity/
 *              https://github.com/unitycontainer/unity
 *              https://unitycontainer.github.io/api/index.html
 *              https://www.nuget.org/packages/Unity.Configuration/
 *              https://github.com/unitycontainer/configuration
 *              https://www.nuget.org/packages/Unity.Microsoft.Logging/
 *              https://github.com/unitycontainer/microsoft-logging
 *****************************************************************************/

namespace DotNetCoreBootstrap.DIDemo
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    using Microsoft.Extensions.Logging;
    using Microsoft.Practices.Unity.Configuration;
    using Unity;
    using Unity.Microsoft.Logging;
    using Unity.Injection;
    using Unity.Resolution;

    /// <summary>
    /// Defines the unity demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// Unity
    /// Unity.Configuration
    /// Unity.Microsoft.Logging
    /// Microsoft.Extensions.Logging
    /// Microsoft.Extensions.Logging.TraceSource
    /// </remarks>
    internal sealed class UnityDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            // define a trace source logger with file listener.
            string logFilePath =
                Path.Combine(AppContext.BaseDirectory, "UnityDemo.log");
            Stream logStream = File.Create(logFilePath);
            TextWriterTraceListener traceListener =
                new TextWriterTraceListener(logStream);
            SourceSwitch verboseSwitch =
                new SourceSwitch("VerboseSwitch", "Verbose");
            ILoggerFactory loggerFactory =
                new LoggerFactory().AddTraceSource(verboseSwitch, traceListener);

            // define a local logger instance.
            ILogger logger = loggerFactory.CreateLogger<UnityDemo>();

            // create unity container instance.
            UnityContainer container = new UnityContainer();
            logger.LogDebug("Unity Container created.");

            // add logging extension to the unity container.
            container.AddExtension(new LoggingExtension(loggerFactory));
            logger.LogDebug("Logging extension added.");

            // load unity config file
            ExeConfigurationFileMap fileMap =
                new ExeConfigurationFileMap
                {
                    ExeConfigFilename = "UnityDemo.unity.config"
                };
            Configuration configuration =
                ConfigurationManager.OpenMappedExeConfiguration(
                    fileMap,
                    ConfigurationUserLevel.None);
            UnityConfigurationSection unitySection =
                (UnityConfigurationSection)configuration.GetSection("unity");

            // load design-time configuration to the unity container.
            container.LoadConfiguration(unitySection);
            logger.LogDebug("Design-time configuration loaded.");

            // add runtime registration to the unity container.
            container.RegisterType<IDemoClass, DemoClass>(
                "runtime_register",
                new InjectionConstructor(
                    new ResolvedParameter(typeof(ILogger<DemoClass>), "logger"),
                    new ResolvedParameter(typeof(string), "name"),
                    new ResolvedParameter(typeof(string), "value")));
            logger.LogDebug("Runtime type mapping registered.");

            // define parameter overrides for design time resolving.
            ParameterOverride designTimeNameOverride =
                new ParameterOverride("name", "design_time_demo_1");
            ParameterOverride designTimeValueOverride =
                new ParameterOverride("value", "demo_value_1");

            // resolve demo class instance from design time registration with parameter overrides.
            IDemoClass designTimeDemoClass =
                container.Resolve<IDemoClass>(
                    "design_time_register",
                    designTimeNameOverride,
                    designTimeValueOverride);
            Console.WriteLine($"Resolved design time demo class: {designTimeDemoClass}");
            designTimeDemoClass.DemoMethod();

            // define parameter overrides for runtime time resolving.
            ParameterOverride runtimeNameOverride =
                new ParameterOverride("name", "runtime_demo_2");
            ParameterOverride runtimeValueOverride =
                new ParameterOverride("value", "demo_value_2");

            // resolve demo class instance from runtime registration with parameter overrides.
            IDemoClass runtimeDemoClass =
                container.Resolve<IDemoClass>(
                    "runtime_register",
                    runtimeNameOverride,
                    runtimeValueOverride);
            Console.WriteLine($"Resolved runtime demo class: {runtimeDemoClass}");
            runtimeDemoClass.DemoMethod();

            // release the log file handler.
            loggerFactory.Dispose();

            // print unity config file
            Console.WriteLine();
            string configFilePath =
                Path.Combine(AppContext.BaseDirectory, fileMap.ExeConfigFilename);
            Console.WriteLine($"[Trace] unity config file: {configFilePath}");
            Console.WriteLine(File.ReadAllText(configFilePath, Encoding.UTF8));

            // print log file
            Console.WriteLine();
            Console.WriteLine($"[Trace] log file: {logFilePath}");
            Console.WriteLine(File.ReadAllText(logFilePath, Encoding.UTF8));
        }
    }
}