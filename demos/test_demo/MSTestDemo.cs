/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   MSTestDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core MSTest demos.
 * Reference:   https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
*               https://github.com/Microsoft/vstest-docs/blob/master/docs/report.md
 *****************************************************************************/

namespace DotNetCoreBootstrap.TestDemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Defines the MSTest demos.
    /// </summary>
    internal static class MSTestDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            const string ToBeTestedTempDir = @"../MSTestDemo_to_be_tested_temp";
            const string TestTempDir = @"../MSTestDemo_test_temp";

            List<string> commands = new List<string>
            {
                // cleanup old temp files
                $"rm -r -f {ToBeTestedTempDir}",
                $"rm -r -f {TestTempDir}",

                // create to-be-tested project and copy files
                $"dotnet new library -o {ToBeTestedTempDir}",
                $"rm -f {ToBeTestedTempDir}/Class1.cs",
                $"cp ToBeTestedClass.cs {ToBeTestedTempDir}/",

                // create temp mstest project, add reference to to-be-tested project and copy files
                $"dotnet new mstest -o {TestTempDir}",
                $"rm -f {TestTempDir}/UnitTest1.cs",
                $"dotnet add {TestTempDir}/*.csproj reference {ToBeTestedTempDir}/*.csproj",
                $"cp MSTestDemoTestClass.cs {TestTempDir}/",

                // switch working folder to mstest project temp dir
                $"pushd .",
                $"cd {TestTempDir}",

                // restore nuget packages
                $"dotnet restore",

                // build MSTest project
                $"dotnet build",

                // rund test with console logger enabled
                @"dotnet test --no-build --logger:""console;verbosity=normal""",

                // switch working folder back
                $"popd",
            };

            // remove temp dir if not in debug mode.
            if (!Debugger.IsAttached)
            {
                commands.Add($"rm -r -f {ToBeTestedTempDir}");
                commands.Add($"rm -r -f {TestTempDir}");
            }

            TestDemoHelper.RunCommands(commands);
        }
    }
}