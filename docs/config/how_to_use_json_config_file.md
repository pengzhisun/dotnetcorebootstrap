# How to use a JSON format configuration file in .Net Core

## Steps

1. Add Nuget package **Microsoft.Extensions.Configuration.Json** reference via **dotnet add {project} package {package}** command.

    > {project} sample: demos/config_demo/config_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Configuration.Json
    ```

2. Create JSON configuration file

   > e.g. [appsettings.json](demos/config_demo/appsettings.json)

    ```json
    {
        "section1": {
            "nested_setting_1": "nested_value_1"
        },
        "str_setting_1": "str_value_1",
        "int_setting_1": 1,
        "array_section": [
            {
                "item_setting": "item_value_1"
            },
            {
                "item_setting": "item_value_2"
            }
        ]
    }
    ```

3. Add JSON file to .csproj file.

    > e.g. [config_demo.csproj](demos/config_demo/config_demo.csproj)
    ```xml
    <Project Sdk="Microsoft.NET.Sdk">
        ...
        <ItemGroup>
            <Content Include="appsettings.json">
                <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
            </Content>
        </ItemGroup>
    </Project>
    ```

4. Add **Microsoft.Extensions.Configuration** namespace.

    > e.g. [Program.cs](demos/config_demo/Program.cs)
    ```csharp
    using Microsoft.Extensions.Configuration;
    ```

5. Get [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance via [ConfigurationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbuilder).

    > e.g. [Program.cs](demos/config_demo/Program.cs)
    ```csharp
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .SetBasePath(System.AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", 
        optional: true, 
        reloadOnChange: true);

    IConfiguration config = configBuilder.Build();
    ```

6. Get setting value via [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance.

    > e.g. [Program.cs](demos/config_demo/Program.cs)
    * Get string value:
    ```csharp
    string value = config["str_setting_1"];
    ```

    * Get nested string value, using ':' delimiter:
    ```csharp
    string value = config["section1:nested_setting_1"];
    ```

    * Get integer value:
        * Add Nuget package **Microsoft.Extensions.Configuration.Binder** reference first.
        ```bash
        dotnet add {project} package Microsoft.Extensions.Configuration.Binder
        ```
        * using [GetValue&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.getvalue) method.
        ```csharp
        int value = config.GetValue<int>("int_setting_1", defaultValue: 0);
        ```

    * Get array item value:
    ```csharp
    string value = config["array_section:0:item_setting"];
    ```

## References

* [Configuration in ASP.NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
* [Microsoft.Extensions.Configuration.Json (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json)