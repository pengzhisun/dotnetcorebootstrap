/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   GladeDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core GTK# + Glade demos.
 * Reference:   https://github.com/GtkSharp/GtkSharp
 *              https://github.com/GtkSharp/GtkSharp/tree/develop/Source/Templates/GtkSharp.Template.CSharp/content/GtkSharp.Application.CSharp
 *              https://www.nuget.org/packages/GtkSharp
 *****************************************************************************/

namespace DotNetCoreBootstrap.GtkDemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// Defines the GTK# + Glade demos.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// GtkSharp
    /// </remarks>
    internal static class GladeDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            const string TempDir = @"../GladeDemo_temp";

            List<string> commands = new List<string>
            {
                // cleanup old temp files
                $"rm -r -f {TempDir}",

                // create glade demo project and copy files
                $"dotnet new --install GtkSharp.Template.CSharp",
                $"dotnet new gtkapp -o {TempDir}",
                $"rm -f {TempDir}/*.*",
                $"cp GladeDemo.csproj.xml {TempDir}/GladeDemo.csproj",
                $"cp GladeDemoProgram.cs {TempDir}/",
                $"cp GladeDemoMainWindow.cs {TempDir}/",
                $"cp GladeDemoMainWindow.glade {TempDir}/",

                // switch working folder to glade demo project temp dir
                $"pushd .",
                $"cd {TempDir}",

                // restore nuget packages
                $"dotnet restore",

                // build glade demo project
                $"dotnet build",
            };

            // reference: https://github.com/dotnet/corefx/issues/19694
            bool isMacOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            const string config = "Release";
            const string framework = "netcoreapp2.0";
            if (isLinux)
            {
                const string runtime = "linux-x64";
                const string appName = "GladeDemo";
                commands.AddRange(new[]{
                    // publish glade demo project
                    $"dotnet publish --configuration {config} --framework {framework} --runtime {runtime}",

                    // execute linux app
                    $"bin/{config}/{framework}/{runtime}/publish/{appName}",
                });
            }
            else if (isWindows)
            {
                const string runtime = "win-x64";
                const string appName = "GladeDemo.exe";
                commands.AddRange(new[]{
                    // publish glade demo project
                    $"dotnet publish --configuration {config} --framework {framework} --runtime {runtime}",

                    // execute windows app
                    $"bin/{config}/{framework}/{runtime}/publish/{appName}",
                });
            }
            else if (isMacOSX)
            {
                const string runtime = "osx-x64";
                const string appDir = "app";
                const string appName = "GladeDemo.app";
                commands.AddRange(new[]{
                    // publish glade demo project
                    $"dotnet publish --configuration {config} --framework {framework} --runtime {runtime}",

                    // prepare Mac OSX app folder
                    $"mkdir -p {appDir}/{appName}/Contents",

                    // copy publish files to Mac OSX app folder
                    $"cp bin/{config}/{framework}/{runtime}/publish/* {appDir}/{appName}/Contents",

                    // make Max OSX app executable
                    $"chmod +x {appDir}/{appName}",

                    // switch to app folder
                    $"cd {appDir}",

                    // execute Mac OSX app
                    $"open -a {appName}",
                });
            }

            commands.AddRange(new[]{
                // switch working folder back
                $"popd",
            });

            GtkDemoHelper.RunCommands(commands);

            // wait GladeDemo app quit
            Thread.Sleep(TimeSpan.FromSeconds(10));

            // remove temp dir if not in debug mode.
            if (!Debugger.IsAttached)
            {
                Directory.Delete(TempDir, true);
            }
        }
    }
}