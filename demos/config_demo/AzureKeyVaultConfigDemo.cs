/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   AzureKeyVaultConfigDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core azure key vault configuration demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
 *              https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets
 *              https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.AzureKeyVault
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.UserSecrets
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder
 *****************************************************************************/

namespace DotNetCoreBootstrap.ConfigDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Rest.Azure;

    /// <summary>
    /// Defines the user secrets configuration demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// Microsoft.Azure.KeyVault
    /// Microsoft.IdentityModel.Clients.ActiveDirectory
    /// Microsoft.Extensions.Configuration.AzureKeyVault
    /// Microsoft.Extensions.Configuration.UserSecrets
    /// Microsoft.Extensions.Configuration.Binder
    /// </remarks>
    internal sealed class AzureKeyVaultConfigDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            // load user secrets first
            IConfigurationBuilder userSecretsConfigBuilder =
                new ConfigurationBuilder()
                    .AddUserSecrets<AzureKeyVaultConfigDemo>();

            IConfiguration userSecretsConfig = userSecretsConfigBuilder.Build();

            string vaultName =
                userSecretsConfig["AzureKeyVaultConfigDemo:vaultName"];
            string clientId =
                userSecretsConfig["AzureKeyVaultConfigDemo:clientId"];
            string clientSecret =
                userSecretsConfig["AzureKeyVaultConfigDemo:clientSecret"];

            string vaultUri = $"https://{vaultName}.vault.azure.net";

            // prepare secrets to be set in azure key vault
            // use '--' as section delimiter, refer: https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-2.1&tabs=aspnetcore2x#creating-key-vault-secrets-and-loading-configuration-values-basic-sample
            Dictionary<string, string> secretsDic = new Dictionary<string, string>()
                {
                    { "str-secret-1", "secret_value_1" },
                    { "int-secret-1", "2" },
                    { "section-1--nested-secret-3", "nested_value3" },
                };

            // set secrets to azure key vault
            SetSecretsAsync(vaultUri, clientId, clientSecret, secretsDic)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddAzureKeyVault(vaultUri, clientId, clientSecret);

            IConfiguration config = configBuilder.Build();

            Action<string, Func<object>> getValueAction =
                (path, getValueFunc) =>
                {
                    object value = getValueFunc();
                    Console.WriteLine($"path: '{path}', value: '{value}'");
                };

            // get string value demo
            getValueAction(
                "str-secret-1",
                () =>
                {
                    string value = config["str-secret-1"];
                    return value;
                });

            // get nested string value demo, using ':' delimiter
            getValueAction(
                "section-1:nested-secret-3",
                () =>
                {
                    string value = config["section-1:nested-secret-3"];
                    return value;
                });

            // get int value demo, using GetValue<T> method
            getValueAction(
                "int-secret-1",
                () =>
                {
                    int value =
                        config.GetValue<int>("int-secret-1", defaultValue: 0);
                    return value;
                });

            // print key vault secrets
            Console.WriteLine();
            Console.WriteLine($"[Trace] key vault secrets:");
            PrintKeyVaultSecretsAsync(vaultUri, clientId, clientSecret)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Set secrets to azure key vault
        /// </summary>
        /// <param name="vaultUri">The key vault uri.</param>
        /// <param name="clientId">The service principal id.</param>
        /// <param name="clientSecret">The service principal secret.</param>
        /// <param name="secretsDic">The secrets to be set.</param>
        /// <returns>The async task for set operation.</returns>
        private static async Task SetSecretsAsync(
            string vaultUri,
            string clientId,
            string clientSecret,
            Dictionary<string, string> secretsDic)
        {
            KeyVaultClient keyVaultClient =
                CreateKeyVaultClient(clientId, clientSecret);

            Console.WriteLine($"[Trace] Setting secrets to key vault '{vaultUri}'");

            foreach (var kvp in secretsDic)
            {
                string secretName = kvp.Key;
                string secretValue = kvp.Value;

                Console.WriteLine($"[Trace] Setting secret '{secretName}' with value '{secretValue}'");

                SecretBundle secretBundle =
                    await keyVaultClient.SetSecretAsync(
                        vaultUri,
                        secretName,
                        secretValue);

                Console.WriteLine($"[Trace] secret identifier: {secretBundle.SecretIdentifier}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Print all secrets in azure key vault.
        /// </summary>
        /// <param name="vaultUri">The key vault uri.</param>
        /// <param name="clientId">The service principal id.</param>
        /// <param name="clientSecret">The service principal secret.</param>
        /// <returns>The async task for printing operation.</returns>
        private static async Task PrintKeyVaultSecretsAsync(
            string vaultUri,
            string clientId,
            string clientSecret)
        {
            KeyVaultClient keyVaultClient =
                CreateKeyVaultClient(clientId, clientSecret);

            IPage<SecretItem> secretsPage
                = await keyVaultClient.GetSecretsAsync(vaultUri);

            do
            {
                foreach (SecretItem secret in secretsPage)
                {
                    string secretName = secret.Identifier.Name;

                    SecretBundle secretBundle =
                        await keyVaultClient.GetSecretAsync(
                            secret.Identifier.Identifier);

                    string secretValue = secretBundle.Value;

                    Console.WriteLine($"{secretName}: {secretValue}");
                }

                if (secretsPage.NextPageLink != null)
                {
                    secretsPage =
                        await keyVaultClient.GetSecretsNextAsync(
                            secretsPage.NextPageLink);
                }
            } while (secretsPage.NextPageLink != null);
        }

        /// <summary>
        /// Create <see ref="KeyVaultClient"/> instance.
        /// </summary>
        /// <param name="clientId">The service principal id.</param>
        /// <param name="clientSecret">The service principal secret.</param>
        /// <returns>The async task for creating operation.</returns>
        private static KeyVaultClient CreateKeyVaultClient(
            string clientId,
            string clientSecret)
        {
            ClientCredential clientCredential
                = new ClientCredential(clientId, clientSecret);

            KeyVaultClient keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                        async (authority, resource, scope) =>
                        {
                            AuthenticationContext context =
                                new AuthenticationContext(authority);

                            AuthenticationResult authResult =
                                await context.AcquireTokenAsync(
                                    resource,
                                    clientCredential);
                            return authResult.AccessToken;
                        }
                    ));

            return keyVaultClient;
        }
    }
}