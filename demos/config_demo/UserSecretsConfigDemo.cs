/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   UserSecretsConfigDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core user secrets configuration demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.UserSecrets
 *              https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder
 *****************************************************************************/

namespace DotNetCoreBootstrap.ConfigDemo
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.UserSecrets;

    /// <summary>
    /// Defines the user secrets configuration demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages: Microsoft.Extensions.Configuration.UserSecrets
    /// and Microsoft.Extensions.Configuration.Binder .
    /// </remarks>
    internal sealed class UserSecretsConfigDemo
    {
         /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            // run command helper action
            Action<string, string> runCommandAction =
                new Action<string, string>(
                    (cmd, args) =>
                    {
                        Console.WriteLine($"[Trace] running command: {cmd} {args}");
                        var startInfo = new ProcessStartInfo(cmd, args)
                        {
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                        };

                        Process process = new Process
                        {
                            StartInfo = startInfo
                        };

                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                        Console.WriteLine($"[Trace] {output}");
                    });

            // set user secret helper action
            Action<string, string> setUserSecretAction =
                new Action<string, string>(
                    (key, value) => runCommandAction("dotnet", $"user-secrets set {key} {value}")
                );

            // set user secrets:

            // dotnet user-secrets set str_setting_1 str_value_1
            setUserSecretAction("str_setting_1", "str_value_1" );

            // dotnet user-secrets set int_setting_1 1
            setUserSecretAction("int_setting_1", "1" );

            // dotnet user-secrets set section1:nested_setting_1 nested_value_1
            setUserSecretAction("section1:nested_setting_1", "nested_value_1" );

            Console.WriteLine();

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddUserSecrets<UserSecretsConfigDemo>();

            IConfiguration config = configBuilder.Build();

            Action<string, Func<object>> getValueAction =
                (path, getValueFunc) =>
                {
                    object value = getValueFunc();
                    Console.WriteLine($"path: '{path}', value: '{value}'");
                };

            // get string value demo
            getValueAction(
                "str_setting_1",
                () =>
                {
                    string value = config["str_setting_1"];
                    return value;
                });

            // get nested string value demo, using ':' delimiter
            getValueAction(
                "section1:nested_setting_1",
                () =>
                {
                    string value = config["section1:nested_setting_1"];
                    return value;
                });

            // get int value demo, using GetValue<T> method
            getValueAction(
                "int_setting_1",
                () =>
                {
                    int value =
                        config.GetValue<int>("int_setting_1", defaultValue: 0);
                    return value;
                });

            // print user secrets file info:

            Console.WriteLine();
            Assembly assembly = Assembly.GetExecutingAssembly();
            UserSecretsIdAttribute attribute =
                assembly.GetCustomAttribute<UserSecretsIdAttribute>();
            string userSecretsId = attribute.UserSecretsId;
            string userSecretsFilePath =
                PathHelper.GetSecretsPathFromSecretsId(userSecretsId);
            Console.WriteLine($"[Trace] User secrets file: {userSecretsFilePath}");
            string userSecretsFileContent =
                File.ReadAllText(userSecretsFilePath, Encoding.UTF8);
            Console.WriteLine(userSecretsFileContent);
        }
    }
}