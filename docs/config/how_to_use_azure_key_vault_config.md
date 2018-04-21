# How to use Azure Key Vault configuration in .Net Core

## Steps

1. Add Nuget package `Microsoft.Extensions.Configuration.AzureKeyVault` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/config_demo/config_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Configuration.AzureKeyVault
    dotnet restore
    ```

2. Create Azure Key Vault instance via [Azure Cli 2.0](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli).

    * Login and create service principal with contributor role
        ```bash
        az login
        az ad sp create-for-rbac -n {service_principal_name} --role contributor
        ```

        > sample output:
        ```json
        {
            "appId": "{client_id_guid}",
            "displayName": "{service_principal_name}",
            "name": "http://{service_principal_name}",
            "password": "{client_secret_guid}",
            "tenant": "{tenant_id_guid}"
        }
        ```
    * Login with the created service principal

        > use `{tenant_id_guid}`, `{client_id_guid}`, `{client_secret_guid}` from previous output

        ```bash
        az login --service-principal -t {tenant_id_guid} -u {client_id_guid} -p {client_secret_guid}
        ```

    * Create Resource Group and Azure Key Vault instance

        ```bash
        az group create --name {resourece_group_name} --location {location}
        az keyvault create --name {key_vault_name} --resource-group {resourece_group_name}
        ```

3. Set secrets into created Azure Key Vault.

    ```bash
    az keyvault secret set --vault-name {key_vault_name} --name str-secret-1 --value secret_value_1
    az keyvault secret set --vault-name {key_vault_name} --name int-secret-1 --value 2
    az keyvault secret set --vault-name {key_vault_name} --name section-1--nested-secret-3 --value nested-value-3
    ```

    > could use double dash '--' as section delimiter, refer [here](https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-2.1&tabs=aspnetcore2x#creating-key-vault-secrets-and-loading-configuration-values-basic-sample) for more detail.

4. Set `vaultName`, `clientId`, `clientSecret` in User Secrets.
    > For security consideration, **NEVER save these info in code or config file**, refer this guide: [How to use user secrets configuration in .Net Core](how_to_use_user_secrets_config.md)

    ```bash
    dotnet user-secrets set AzureKeyVaultConfigDemo:vaultName {key_vault_name}
    dotnet user-secrets set AzureKeyVaultConfigDemo:clientId {clientId}
    dotnet user-secrets set AzureKeyVaultConfigDemo:clientSecret {clientSecret}
    ```

5. Add `Microsoft.Extensions.Configuration` namespace.

    > e.g. [UserSecretsConfigDemo.cs](../../demos/config_demo/UserSecretsConfigDemo.cs)
    ```csharp
    using Microsoft.Extensions.Configuration;
    ```

6. Get `vaultName`, `clientId`, `clientSecret` from User Secrets.

    > refer this guide: [How to use user secrets configuration in .Net Core](how_to_use_user_secrets_config.md)
    > e.g. [AzureKeyVaultConfigDemo.cs](../../demos/config_demo/AzureKeyVaultConfigDemo.cs)

    ```csharp
    IConfigurationBuilder userSecretsConfigBuilder = new ConfigurationBuilder()
            .AddUserSecrets<AzureKeyVaultConfigDemo>();

    IConfiguration userSecretsConfig = userSecretsConfigBuilder.Build();

    string vaultName = userSecretsConfig["AzureKeyVaultConfigDemo:vaultName"];
    string clientId = userSecretsConfig["AzureKeyVaultConfigDemo:clientId"];
    string clientSecret = userSecretsConfig["AzureKeyVaultConfigDemo:clientSecret"];
    ```

7. Get [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance via [ConfigurationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbuilder).

    > e.g. [AzureKeyVaultConfigDemo.cs](../../demos/config_demo/AzureKeyVaultConfigDemo.cs)
    ```csharp
    string vaultUri = $"https://{vaultName}.vault.azure.net";

    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .AddAzureKeyVault(vaultUri, clientId, clientSecret);

    IConfiguration config = configBuilder.Build();
    ```

8. Get setting value via [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) instance.

    > e.g. [AzureKeyVaultConfigDemo.cs](../../demos/config_demo/AzureKeyVaultConfigDemo.cs)
    * Get string value:
    ```csharp
    string value = config["str-secret-1"];
    ```

    * Get nested string value, using ':' delimiter:
    ```csharp
    string value = config["section-1:nested-secret-3"];
    ```

    * Get integer value:
        * Add Nuget package `Microsoft.Extensions.Configuration.Binder` reference first.
        ```bash
        dotnet add {project} package Microsoft.Extensions.Configuration.Binder
        ```
        * using [GetValue&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.getvalue) method.
        ```csharp
        int value = config.GetValue<int>("int-secret-1", defaultValue: 0);
        ```

## References

* [Configuration in ASP.NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
* [Azure Key Vault configuration provider in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration)
* [Safe storage of app secrets in development in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
* [Create an Azure service principal with Azure CLI 2.0](https://docs.microsoft.com/en-us/cli/azure/create-an-azure-service-principal-azure-cli)
* [Quickstart: Create an Azure Key Vault using the CLI](https://docs.microsoft.com/en-us/azure/key-vault/quick-create-cli)
* [Microsoft.Extensions.Configuration Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration)
* [Microsoft.Extensions.Configuration.UserSecrets (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.UserSecrets)
* [Microsoft.Extensions.Configuration.AzureKeyVault (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.AzureKeyVault)
* [Microsoft.Extensions.Configuration.Binder (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)
