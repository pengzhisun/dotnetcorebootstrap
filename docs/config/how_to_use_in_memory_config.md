# How to use In-memory configuration in .Net Core

## Steps

1. Add Nuget package `Microsoft.Extensions.Configuration` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/config_demo/config_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Configuration
    ```

2. Create a configuration dictonary.

   > e.g. [InMemoryConfigDemo.cs](../../demos/config_demo/InMemoryConfigDemo.cs)

    ```csharp
    Dictionary<string, string> configDic = new Dictionary<string, string>()
        {
            { "str_setting_1", "str_value_1" },
            { "int_setting_1", "1" },
            { "poco_section:poco_setting_1", "poco_value_1" },
            { "array_section:0:item_setting", "item_value_1" },
            { "array_section:1:item_setting", "item_value_2" },
            { "graph_section:graph_sub_section:graph_setting_1", "graph_value_1" },
        };
    ```

3. Add `Microsoft.Extensions.Configuration` namespace.

    > e.g. [InMemoryConfigDemo.cs](../../demos/config_demo/InMemoryConfigDemo.cs)
    ```csharp
    using Microsoft.Extensions.Configuration;
    ```

4. Get [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance via [ConfigurationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbuilder).

    > e.g. [InMemoryConfigDemo.cs](../../demos/config_demo/InMemoryConfigDemo.cs)
    ```csharp
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .AddInMemoryCollection(configDic);

    IConfiguration config = configBuilder.Build();
    ```

5. Get setting value via [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance.

    > e.g. [InMemoryConfigDemo.cs](../../demos/config_demo/InMemoryConfigDemo.cs)
    * Get string value:
    ```csharp
    string value = config["str_setting_1"];
    ```

    * Get integer value:
        * Add Nuget package `Microsoft.Extensions.Configuration.Binder` reference first.
        ```bash
        dotnet add {project} package Microsoft.Extensions.Configuration.Binder
        ```
        * using [GetValue&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.getvalue) method.
        ```csharp
        int value = config.GetValue<int>("int_setting_1", defaultValue: 0);
        ```

    * Get [POCO](https://en.wikipedia.org/wiki/Plain_old_CLR_object) value:
        * Add Nuget package `Microsoft.Extensions.Configuration.Binder` reference first.
        ```bash
        dotnet add {project} package Microsoft.Extensions.Configuration.Binder
        ```
        * Defines POCO class.
        ```csharp
        public sealed class PocoSection
        {
            public string poco_setting_1 { get; set; }
        }
        ```
        * using [GetSection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration.getsection) and [Bind](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.bind) methods.
        ```csharp
        PocoSection pocoSection = new PocoSection();
        config.GetSection("poco_section").Bind(pocoSection);
        string value = pocoSection.poco_setting_1;
        ```

    * Get array item value:
        * Add Nuget package `Microsoft.Extensions.Configuration.Binder` reference first.
        ```bash
        dotnet add {project} package Microsoft.Extensions.Configuration.Binder
        ```
        * Defines ArrayItem class.
        ```csharp
        public sealed class ArrayItem
        {
            public string item_setting { get; set; }
        }
        ```
        * using [GetSection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration.getsection) and [Bind](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.bind) methods.
        ```csharp
        List<ArrayItem> array = new List<ArrayItem>();
        config.GetSection("array_section").Bind(array);
        string value = array[0].item_setting;
        ```

    * Get graph object value:
        * Add Nuget package `Microsoft.Extensions.Configuration.Binder` reference first.
        ```bash
        dotnet add {project} package Microsoft.Extensions.Configuration.Binder
        ```
        * Defines graph object classes.
        ```csharp
        public sealed class GraphSubSection
        {
            string graph_setting_1 { get; set; }
        }

        public sealed class GraphSection
        {
            GraphSubSection graph_sub_section { get; set; }
        }
        ```
        * using [GetSection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration.getsection) and [Get&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.get) methods.

        > [Get&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.get) method is more convenient than using [Bind](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.bind) method but do the same thing.

        ```csharp
        GraphSection graphSection = config.GetSection("graph_section").Get<GraphSection>();
        string value = graphSection.graph_sub_section.graph_setting_1;
        ```

## References

* [Configuration in ASP.NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
* [Microsoft.Extensions.Configuration Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration)
* [Microsoft.Extensions.Configuration (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration)
* [Microsoft.Extensions.Configuration.Binder (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)