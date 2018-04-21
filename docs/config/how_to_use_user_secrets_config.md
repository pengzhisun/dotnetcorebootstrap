# How to use user secrets configuration in .Net Core

## Steps

1. Add Nuget package `Microsoft.Extensions.Configuration.UserSecrets` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/config_demo/config_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Configuration.UserSecrets
    dotnet restore
    ```

2. Add `UserSecretsId` in csproj file, typically use a GUID value.

   > e.g. [config_demo.csproj](../../demos/config_demo/config_demo.csproj)

    ```xml
    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>netcoreapp2.0</TargetFramework>
        <UserSecretsId>{unique_id}</UserSecretsId>
    </PropertyGroup>
    ```

3. Set user secrets via `dotnet user-secrets set {key} {value}` command.

    > e.g. [UserSecretsConfigDemo.cs](../../demos/config_demo/UserSecretsConfigDemo.cs)
    ```bash
    dotnet user-secrets set str_setting_1 str_value_1
    dotnet user-secrets set int_setting_1 1
    dotnet user-secrets set section1:nested_setting_1 nested_value_1
    ```

4. Add `Microsoft.Extensions.Configuration` namespace.

    > e.g. [UserSecretsConfigDemo.cs](../../demos/config_demo/UserSecretsConfigDemo.cs)
    ```csharp
    using Microsoft.Extensions.Configuration;
    ```

5. Get [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance via [ConfigurationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbuilder).

    > e.g. [UserSecretsConfigDemo.cs](../../demos/config_demo/UserSecretsConfigDemo.cs)
    ```csharp
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .AddUserSecrets<UserSecretsConfigDemo>();

    IConfiguration config = configBuilder.Build();
    ```

6. Get setting value via [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance.

    > e.g. [UserSecretsConfigDemo.cs](../../demos/config_demo/UserSecretsConfigDemo.cs)
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
* [Safe storage of app secrets in development in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
* [Microsoft.Extensions.Configuration Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration)
* [Microsoft.Extensions.Configuration.UserSecrets (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.UserSecrets)
* [Microsoft.Extensions.Configuration.Binder (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)
* [Config.UserSecrets (github.com)](https://github.com/aspnet/Configuration/tree/dev/src/Config.UserSecrets)