/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   XUnitDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core xUnit demos.
 * Reference:   https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test
 *              https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/
 *              https://github.com/Microsoft/vstest
 *              https://www.nuget.org/packages/xunit/
 *              https://www.nuget.org/packages/xunit.runner.visualstudio/
 *              https://github.com/xunit/xunit
 *              https://github.com/Microsoft/vstest-docs/blob/master/docs/report.md
 *****************************************************************************/

namespace DotNetCoreBootstrap.TestDemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Defines the xUnit demos.
    /// </summary>
    internal static class XUnitDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            const string ToBeTestedTempDir = @"../XUnitDemo_to_be_tested_temp";
            const string TestTempDir = @"../XUnitDemo_test_temp";

            List<string> commands = new List<string>
            {
                // cleanup old temp files
                $"rm -r -f {ToBeTestedTempDir}",
                $"rm -r -f {TestTempDir}",

                // create to-be-tested project and copy files
                $"dotnet new library -o {ToBeTestedTempDir}",
                $"rm -f {ToBeTestedTempDir}/Class1.cs",
                $"cp ToBeTestedClass.cs {ToBeTestedTempDir}/",

                // create temp xUnit project, add reference to to-be-tested project and copy files
                $"dotnet new xunit -o {TestTempDir}",
                $"rm -f {TestTempDir}/UnitTest1.cs",
                $"dotnet add {TestTempDir}/*.csproj reference {ToBeTestedTempDir}/*.csproj",
                $"cp XUnitDemoTestClass.cs {TestTempDir}/",

                // switch working folder to xUnit project temp dir
                $"pushd .",
                $"cd {TestTempDir}",

                // restore nuget packages
                $"dotnet restore",

                // build xUnit project
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