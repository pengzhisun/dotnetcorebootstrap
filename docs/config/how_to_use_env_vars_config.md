# How to use environment variables configuration in .Net Core

## Steps

1. Add Nuget package `Microsoft.Extensions.Configuration.EnvironmentVariables` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/config_demo/config_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Configuration.EnvironmentVariables
    dotnet restore
    ```

2. Set environment variables.

   > e.g. [EnvironmentVariablesConfigDemo.cs](../../demos/config_demo/EnvironmentVariablesConfigDemo.cs)

    ```csharp
    Environment.SetEnvironmentVariable("str_setting_1", "str_value_1");
    Environment.SetEnvironmentVariable("int_setting_1", "1");
    Environment.SetEnvironmentVariable("section1:nested_setting_1", "nested_value_1");
    Environment.SetEnvironmentVariable("array_section:0:item_setting", "item_value_1");
    Environment.SetEnvironmentVariable("array_section:1:item_setting", "item_value_2");
    ```

3. Add `Microsoft.Extensions.Configuration` namespace.

    > e.g. [EnvironmentVariablesConfigDemo.cs](../../demos/config_demo/EnvironmentVariablesConfigDemo.cs)
    ```csharp
    using Microsoft.Extensions.Configuration;
    ```

4. Get [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance via [ConfigurationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbuilder).

    > e.g. [EnvironmentVariablesConfigDemo.cs](../../demos/config_demo/EnvironmentVariablesConfigDemo.cs)
    ```csharp
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .AddEnvironmentVariables();

    IConfiguration config = configBuilder.Build();
    ```

5. Get setting value via [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance.

    > e.g. [EnvironmentVariablesConfigDemo.cs](../../demos/config_demo/EnvironmentVariablesConfigDemo.cs)
    * Get string value:
    ```csharp
    string value = config["str_setting_1"];
    ```

    * Get nested string value, using ':' delimiter:
    ```csharp
    string value = config["section1:nested_setting_1"];
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

    * Get array item value, using ':' delimiter and indexer:
    ```csharp
    string value = config["array_section:0:item_setting"];
    ```

## References

* [Configuration in ASP.NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
* [Microsoft.Extensions.Configuration Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration)
* [Microsoft.Extensions.Configuration.EnvironmentVariables (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.EnvironmentVariables)
* [Microsoft.Extensions.Configuration.Binder (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)
* [Config.EnvironmentVariables (github.com)](https://github.com/aspnet/Configuration/tree/dev/src/Config.EnvironmentVariables)