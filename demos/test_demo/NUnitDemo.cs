/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   NUnitDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core NUnit demos.
 * Reference:   https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-nunit
 *              https://github.com/nunit/docs/wiki/.NET-Core-and-.NET-Standard
 *              https://github.com/nunit/docs/wiki
 *              https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/
 *              https://github.com/Microsoft/vstest
 *              https://www.nuget.org/packages/NUnit/
 *              https://www.nuget.org/packages/NUnit3TestAdapter/
 *              https://github.com/nunit/nunit
 *              https://github.com/Microsoft/vstest-docs/blob/master/docs/report.md
 *****************************************************************************/

namespace DotNetCoreBootstrap.TestDemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Defines the NUnit demos.
    /// </summary>
    internal static class NUnitDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            const string ToBeTestedTempDir = @"../NUnitDemo_to_be_tested_temp";
            const string TestTempDir = @"../NUnitDemo_test_temp";

            List<string> commands = new List<string>
            {
                // cleanup old temp files
                $"rm -r -f {ToBeTestedTempDir}",
                $"rm -r -f {TestTempDir}",

                // create to-be-tested project and copy files
                $"dotnet new library -o {ToBeTestedTempDir}",
                $"rm -f {ToBeTestedTempDir}/Class1.cs",
                $"cp ToBeTestedClass.cs {ToBeTestedTempDir}/",

                // create temp NUnit project, add reference to to-be-tested project and copy files
                $"dotnet new -i NUnit3.DotNetNew.Template",
                $"dotnet new nunit -o {TestTempDir}",
                $"rm -f {TestTempDir}/UnitTest1.cs",
                $"dotnet add {TestTempDir}/*.csproj reference {ToBeTestedTempDir}/*.csproj",
                $"cp NUnitDemoTestClass.cs {TestTempDir}/",

                // switch working folder to NUnit project temp dir
                $"pushd .",
                $"cd {TestTempDir}",

                // restore nuget packages
                $"dotnet restore",

                // build NUnit project
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