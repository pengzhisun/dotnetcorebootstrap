# How to use an INI format configuration file in .Net Core

## Steps

1. Add Nuget package `Microsoft.Extensions.Configuration.Ini` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/config_demo/config_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Configuration.Ini
    ```

2. Create INI configuration file

   > e.g. [appsettings.ini](../../demos/config_demo/appsettings.ini)

    ```ini
    str_setting_1=str_value_1
    int_setting_1=1

    [section1]
    nested_setting_1=nested_value_1
    ```

3. Add INI file to .csproj file.

    > e.g. [config_demo.csproj](../../demos/config_demo/config_demo.csproj)
    ```xml
    <Project Sdk="Microsoft.NET.Sdk">
        ...
        <ItemGroup>
            <Content Include="appsettings.ini">
                <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
            </Content>
        </ItemGroup>
    </Project>
    ```

4. Add `Microsoft.Extensions.Configuration` namespace.

    > e.g. [IniFileConfigDemo.cs](../../demos/config_demo/IniFileConfigDemo.cs)
    ```csharp
    using Microsoft.Extensions.Configuration;
    ```

5. Get [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance via [ConfigurationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbuilder).

    > e.g. [IniFileConfigDemo.cs](../../demos/config_demo/IniFileConfigDemo.cs)
    ```csharp
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .SetBasePath(System.AppContext.BaseDirectory)
        .AddIniFile("appsettings.ini",
        optional: true,
        reloadOnChange: true);

    IConfiguration config = configBuilder.Build();
    ```

6. Get setting value via [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance.

    > e.g. [IniFileConfigDemo.cs](../../demos/config_demo/IniFileConfigDemo.cs)
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

## References

* [Configuration in ASP.NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
* [Microsoft.Extensions.Configuration Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration)
* [Microsoft.Extensions.Configuration.Ini (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Ini)
* [Microsoft.Extensions.Configuration.Binder (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)