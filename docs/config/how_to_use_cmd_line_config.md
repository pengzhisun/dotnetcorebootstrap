# How to use command line configuration in .Net Core

## Steps

1. Add Nuget package `Microsoft.Extensions.Configuration.CommandLine` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/config_demo/config_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Configuration.CommandLine
    dotnet restore
    ```

2. Defines a arguments array to receive input arguments or set default value.

   > e.g. [CommandLineConfigDemo.cs](../../demos/config_demo/CommandLineConfigDemo.cs)

    ```csharp
    string[] cmdArgs = args.Any() ? args : new string[]
    {
        "str_setting_1=def_str_value_1",
        "-int_setting_1=1",
        "/section1:nested_setting_1=def_nested_value_1",
        "-nested_setting_2",
        "def_nested_value_2"
    };
    ```

3. Defines a [switch mapping](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1&tabs=basicconfiguration#switch-mappings) dictionary.

    > e.g. [CommandLineConfigDemo.cs](../../demos/config_demo/CommandLineConfigDemo.cs)
    ```csharp
    Dictionary<string, string> switchMapping = new Dictionary<string, string>()
        {
            { "--nested_setting_1", "section1:nested_setting_1" },
            { "-nested_setting_2", "section1:nested_setting_2" },
        };
    ```

4. Add `Microsoft.Extensions.Configuration` namespace.

    > e.g. [CommandLineConfigDemo.cs](../../demos/config_demo/CommandLineConfigDemo.cs)
    ```csharp
    using Microsoft.Extensions.Configuration;
    ```

5. Get [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance via [ConfigurationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbuilder).

    > e.g. [CommandLineConfigDemo.cs](../../demos/config_demo/CommandLineConfigDemo.cs)
    ```csharp
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .AddCommandLine(cmdArgs, switchMapping);

    IConfiguration config = configBuilder.Build();
    ```

6. Get setting value via [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance.

    > e.g. [CommandLineConfigDemo.cs](../../demos/config_demo/CommandLineConfigDemo.cs)
    * Get string value:
    ```csharp
    string value = config["str_setting_1"];
    ```

    * Get nested string value, using ':' delimiter:
    ```csharp
    string value = config["section1:nested_setting_1"];
    string value = config["section1:nested_setting_2"];
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
7. Run config demo application with arguments.

    > `{project}` sample: demos/config_demo/config_demo.csproj
    ```bash
    dotnet run -p {project} str_setting_1=cmd_str_value_1 /int_setting_1=2 /section1:nested_setting_1=cmd_nested_value_1 -nested_setting_2 cmd_nested_value_2
    ```

    > Command line arguments description:
    * str_setting_1=cmd_str_value_1 (`key=value` pattern)
    * /int_setting_1=2 (`/key=value` pattern)
    * /section1:nested_setting_1=cmd_nested_value_1 (`/key=value` pattern, key also support ':' delimiter)
    * -nested_setting_2 cmd_nested_value_2 (`-switch value` pattern, also support `--switch value`)

## References

* [Configuration in ASP.NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
* [Microsoft.Extensions.Configuration Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration)
* [Microsoft.Extensions.Configuration.CommandLine (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.CommandLine)
* [Microsoft.Extensions.Configuration.Binder (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)
* [Config.CommandLine (github.com)](https://github.com/aspnet/Configuration/tree/dev/src/Config.CommandLine)