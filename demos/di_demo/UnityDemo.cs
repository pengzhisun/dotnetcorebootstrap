namespace DotNetCoreBootstrap.DIDemo
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.Extensions.Logging;
    using Microsoft.Practices.Unity.Configuration;
    using Unity;
    using Unity.Microsoft.Logging;
    using Unity.Injection;
    using Unity.Resolution;

    internal sealed class UnityDemo
    {
        public static void Run()
        {
            string logFilePath =
                Path.Combine(AppContext.BaseDirectory, "UnityDemo.log");
            Stream logStream = File.Create(logFilePath);
            TextWriterTraceListener traceListener =
                new TextWriterTraceListener(logStream);
            SourceSwitch verboseSwitch =
                new SourceSwitch("VerboseSwitch", "Verbose");
            ILoggerFactory loggerFactory =
                new LoggerFactory().AddTraceSource(verboseSwitch, traceListener);

            ILogger logger = loggerFactory.CreateLogger<UnityDemo>();

            UnityContainer container = new UnityContainer();
            logger.LogDebug("Unity Container created.");

            container.AddExtension(new LoggingExtension(loggerFactory));
            logger.LogDebug("Logging extension added.");

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

            container.LoadConfiguration(unitySection);
            logger.LogDebug("Design-time configuration loaded.");

            container.RegisterType<IDemoClass, DemoClass>(
                "runtime_register",
                new InjectionConstructor(
                    new ResolvedParameter(typeof(ILogger<DemoClass>), "logger"),
                    new ResolvedParameter(typeof(string), "name"),
                    new ResolvedParameter(typeof(string), "value")));
            logger.LogDebug("Runtime type mapping registered.");

            ParameterOverride designTimeNameOverride =
                new ParameterOverride("name", "demo_name_1");
            ParameterOverride designTimeValueOverride =
                new ParameterOverride("value", "demo_value_1");

            IDemoClass designTimeDemoClass =
                container.Resolve<IDemoClass>(
                    "design_time_register",
                    designTimeNameOverride,
                    designTimeValueOverride);
            Console.WriteLine($"Resolved design time demo class: {designTimeDemoClass}");
            designTimeDemoClass.DemoMethod();

            ParameterOverride runtimeNameOverride =
                new ParameterOverride("name", "demo_name_2");
            ParameterOverride runtimeValueOverride =
                new ParameterOverride("value", "demo_value_2");

            IDemoClass runtimeDemoClass =
                container.Resolve<IDemoClass>(
                    "runtime_register",
                    runtimeNameOverride,
                    runtimeValueOverride);
            Console.WriteLine($"Resolved runtime demo class: {runtimeDemoClass}");
            runtimeDemoClass.DemoMethod();

            loggerFactory.Dispose();
        }
    }
}