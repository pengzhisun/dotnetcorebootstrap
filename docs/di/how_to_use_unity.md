# How to use Unity in .Net Core

## Steps

1. Add Nuget package `Unity` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: `demos/di_demo/di_demo.csproj`

    ```bash
    dotnet add {project} package Unity
    dotnet restore
    ```
2. Add `Unity` namespace.

    > e.g. [UnityDemo.cs](../../demos/di_demo/UnityDemo.cs)
    ```csharp
    using Unity;
    ```

3. Create [UnityContainer](https://github.com/unitycontainer/container/blob/v5.x/src/UnityContainer.Public.cs) instance.

    > e.g. [UnityDemo.cs](../../demos/di_demo/UnityDemo.cs)
    ```csharp
    UnityContainer container = new UnityContainer();
    ```

4. Add runtime registration to the `UnityContainer` instance.

    > e.g. [UnityDemo.cs](../../demos/di_demo/UnityDemo.cs)
    ```csharp
    container.RegisterType<IDemoClass, DemoClass>(
        "runtime_register",
        new InjectionConstructor(
            new ResolvedParameter(typeof(ILogger<DemoClass>), "logger"),
            new ResolvedParameter(typeof(string), "name"),
            new ResolvedParameter(typeof(string), "value")));
    ```

    > The interface [IDemoClass](../../demos/di_demo/IDemoClass.cs) and concrete class [DemoClass](../../demos/di_demo/DemoClass.cs) used for dependency inversion demo.

    ```csharp
    public interface IDemoClass
    {
        string TypeName { get; }
        string InstanceId { get; }
        string Name { get; }
        string Value { get; }

        void DemoMethod();
    }

    public sealed class DemoClass : IDemoClass
    {
        private readonly ILogger<DemoClass> logger;
        private readonly string instanceId;

        public DemoClass(ILogger<DemoClass> logger, string name, string value)
        {
            this.logger = logger;
            this.instanceId = Guid.NewGuid().ToString("D");
            this.Name = name;
            this.Value = value;

            this.logger.LogDebug(
                $"Resolved object {this}");
        }

        public string TypeName => this.GetType().Name;
        public string InstanceId => this.instanceId;
        public string Name { get; private set; }
        public string Value { get; private set; }

        public void DemoMethod()
        {
            this.logger.LogDebug($"Run demo method from {this}");
            Console.WriteLine($"Run demo method from {this}");
        }

        public override string ToString()
            => $"[TypeName = '{this.TypeName}', InstanceId = '{this.instanceId}', Name = '{this.Name}', Value = '{this.Value}']";
    }
    ```

5. Add logging extension for `Microsoft.Extensions.Logging`.

    > we use trace source logger factory for demo, could refer [How to use trace source log in .Net Core](../logging/how_to_use_trace_source_log.md) for more detail.

    * Add `Unity.Microsoft.Logging`, `Microsoft.Extensions.Logging` and `Microsoft.Extensions.Logging.TraceSource` package references.
        > `{project}` sample: demos/di_demo/di_demo.csproj
        ```bash
        dotnet add {project} package Unity.Microsoft.Logging
        dotnet add {project} package Microsoft.Extensions.Logging
        dotnet add {project} package Microsoft.Extensions.Logging.TraceSource
        dotnet restore
        ```

    * Define logger factory instance.
        > e.g. [UnityDemo.cs](../../demos/di_demo/UnityDemo.cs)
        ```csharp
        string logFilePath =
            Path.Combine(AppContext.BaseDirectory, "UnityDemo.log");
        Stream logStream = File.Create(logFilePath);
        TextWriterTraceListener traceListener =
            new TextWriterTraceListener(logStream);
        SourceSwitch verboseSwitch =
            new SourceSwitch("VerboseSwitch", "Verbose");
        ILoggerFactory loggerFactory =
            new LoggerFactory().AddTraceSource(verboseSwitch, traceListener);
        ```

    * Add logging extension.
        > e.g. [UnityDemo.cs](../../demos/di_demo/UnityDemo.cs)
        ```csharp
        container.AddExtension(new LoggingExtension(loggerFactory));
        ```

    > the logger factory should be disposed to release the file handler
    ```csharp
    loggerFactory.Dispose();
    ```

6. Resolve instance from `UnityContainer` runtime registration.

    > e.g. [UnityDemo.cs](../../demos/di_demo/UnityDemo.cs)
    ```csharp
    ParameterOverride runtimeNameOverride =
        new ParameterOverride("name", "runtime_demo_2");
    ParameterOverride runtimeValueOverride =
        new ParameterOverride("value", "demo_value_2");

    IDemoClass runtimeDemoClass =
        container.Resolve<IDemoClass>(
            "runtime_register",
            runtimeNameOverride,
            runtimeValueOverride);
    ```
    > the logger parameter for `DemoClass` constructor will be injected by the logging extension.

7. Typically, we could defines design time registrations in `unity.config` file.

    * Add `Unity.Configuration` package reference.

        > `{project}` sample: demos/di_demo/di_demo.csproj
        ```bash
        dotnet add {project} package Unity.Configuration
        dotnet restore
        ```

    * Create `unity.config` file.

        > e.g. [unity.config](../../demos/di_demo/UnityDemo.unity.config)
        ```xml
        <configuration>
            <configSections>
                <section name="unity" type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Unity.Configuration"/>
            </configSections>
            <unity xmlns="http://schemas.microsoft.com/practices/2010/unity">
                <assembly name="di_demo" />
                <namespace name="DotNetCoreBootstrap.DIDemo" />
                <assembly name="Microsoft.Extensions.Logging.Abstractions" />
                <namespace name="Microsoft.Extensions.Logging" />
                <container>
                    <register type="IDemoClass" mapTo="DemoClass" name="design_time_register">
                        <constructor>
                            <param name="logger" dependencyType="ILogger[DemoClass]" />
                            <param name="name" dependencyType="string" />
                            <param name="value" dependencyType="string" />
                        </constructor>
                    </register>
                </container>
            </unity>
        </configuration>
        ```

    * Add `unity.config` file to .csproj file.

        > `{unity_config_file}` sample: `UnityDemo.unity.config`, e.g. [di_demo.csproj](../../demos/di_demo/di_demo.csproj)
        ```xml
        <Project Sdk="Microsoft.NET.Sdk">
            ...
            <ItemGroup>
                <Content Include="{unity_config_file}">
                    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
                </Content>
            </ItemGroup>
        </Project>
        ```

    * Load `unity.confg` file

        > `{unity_config_file}` sample: `UnityDemo.unity.config`, e.g. [UnityDemo.cs](../../demos/di_demo/UnityDemo.cs)
        ```csharp
        ExeConfigurationFileMap fileMap =
            new ExeConfigurationFileMap
            {
                ExeConfigFilename = "{unity_config_file}"
            };
        Configuration configuration =
            ConfigurationManager.OpenMappedExeConfiguration(
                fileMap,
                ConfigurationUserLevel.None);
        UnityConfigurationSection unitySection =
            (UnityConfigurationSection)configuration.GetSection("unity");
        ```

    * Load design-time configuration to `UnityContainer`.

        > e.g. [UnityDemo.cs](../../demos/di_demo/UnityDemo.cs)
        ```csharp
        container.LoadConfiguration(unitySection);
        ```

    * Resolve instance from `UnityContainer` design time registration same as from runtime registration

        > e.g. [UnityDemo.cs](../../demos/di_demo/UnityDemo.cs)
        ```csharp
        ParameterOverride designTimeNameOverride =
            new ParameterOverride("name", "design_time_demo_1");
        ParameterOverride designTimeValueOverride =
            new ParameterOverride("value", "demo_value_1");

        IDemoClass designTimeDemoClass =
            container.Resolve<IDemoClass>(
                "design_time_register",
                designTimeNameOverride,
                designTimeValueOverride);
        ```

## References

* [Unity API Documentation (unitycontainer.github.io)](https://unitycontainer.github.io/api/index.html)
* [The Unity Configuration Schema (msdn.microsoft.com)](https://msdn.microsoft.com/en-us/library/ff660914.aspx)
* [Unity (nuget.org)](https://www.nuget.org/packages/Unity/)
* [Unity.Configuration (nuget.org)](https://www.nuget.org/packages/Unity.Configuration/)
* [Unity.Microsoft.Logging (nuget.org)](https://www.nuget.org/packages/Unity.Microsoft.Logging/)
* [Unity (github.com)](https://github.com/unitycontainer/unity)
* [Unity.Configuration (github.com)](https://github.com/unitycontainer/configuration)
* [Unity.Microsoft.Logging (github.com)](https://github.com/unitycontainer/microsoft-logging)