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
            const string TempDir = @"../MSTestDemo_temp";

            List<string> commands = new List<string>
            {
                // cleanup old temp files
                $"rm -r -f {TempDir}",

                // create temp dir and copy files
                $"mkdir {TempDir}",
                $"cp ToBeTestedClass.cs {TempDir}/",
                $"cp MSTestDemoTestClass.cs {TempDir}/",
                $"cp MSTestDemo.csproj.xml {TempDir}/MSTestDemo.csproj",

                // switch working folder to temp dir
                $"pushd .",
                $"cd {TempDir}",

                // restore nuget packages
                $"dotnet restore",

                // build MSTest project
                $"dotnet build",

                // rund test with console logger enabled
                @"dotnet test --no-build --logger:""console;verbosity=normal""",

                // switch working folder back
                $"popd",

                // remove temp dir
                $"rm -r -f {TempDir}",
            };

            TestDemoHelper.RunCommands(commands);
        }
    }
}