# How to use a XML format configuration file in .Net Core

## Steps

1. Add Nuget package `Microsoft.Extensions.Configuration.Xml` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/config_demo/config_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Configuration.Xml
    ```

2. Create XML configuration file

   > e.g. [appsettings.xml](../../demos/config_demo/appsettings.xml)

    ```xml
    <?xml version="1.0" encoding="UTF-8" ?>
    <config>
        <section1>
            <nested_setting_1>nested_value_1</nested_setting_1>
        </section1>
        <str_setting_1>str_value_1</str_setting_1>
        <int_setting_1>1</int_setting_1>
        <array_items>
            <array_item name="item_1">
            <item_setting>item_value_1</item_setting>
            </array_item>
            <array_item name="item_2">
            <item_setting>item_value_2</item_setting>
            </array_item>
        </array_items>
    </config>
    ```

3. Add XML file to .csproj file.

    > e.g. [config_demo.csproj](../../demos/config_demo/config_demo.csproj)
    ```xml
    <Project Sdk="Microsoft.NET.Sdk">
        ...
        <ItemGroup>
            <Content Include="appsettings.xml">
                <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
            </Content>
        </ItemGroup>
    </Project>
    ```

4. Add `Microsoft.Extensions.Configuration` namespace.

    > e.g. [XmlFileConfigDemo.cs](../../demos/config_demo/XmlFileConfigDemo.cs)
    ```csharp
    using Microsoft.Extensions.Configuration;
    ```

5. Get [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance via [ConfigurationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbuilder).

    > e.g. [XmlFileConfigDemo.cs](../../demos/config_demo/XmlFileConfigDemo.cs)
    ```csharp
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .SetBasePath(System.AppContext.BaseDirectory)
        .AddXmlFile("appsettings.xml",
        optional: true,
        reloadOnChange: true);

    IConfiguration config = configBuilder.Build();
    ```

6. Get setting value via [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance.

    > e.g. [XmlFileConfigDemo.cs](../../demos/config_demo/XmlFileConfigDemo.cs)
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

    * Get array item value, using ':' delimiter and name attribute:
    ```csharp
    string value = config["array_items:array_item:item_1:item_setting"];
    ```

## References

* [Configuration in ASP.NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
* [Microsoft.Extensions.Configuration Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration)
* [Microsoft.Extensions.Configuration.Xml (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Xml)
* [Microsoft.Extensions.Configuration.Binder (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)