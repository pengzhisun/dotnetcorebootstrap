/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   GtkDemoHelper.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core GTK# demo helper methods.
 *****************************************************************************/

namespace DotNetCoreBootstrap.GtkDemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the GTK# demo helper class.
    /// </summary>
    internal static class GtkDemoHelper
    {
        /// <summary>
        /// Run commands in a background bash process.
        /// </summary>
        /// <param name="commands">The commands to be executed.</param>
        public static void RunCommands(IEnumerable<string> commands)
        {
            // start a background bash process
            ProcessStartInfo startInfo =
                new ProcessStartInfo("bash", string.Empty)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
            Process process = new Process
            {
                StartInfo = startInfo
            };
            process.Start();

            // prepare task factory to start backgroud stream reader tasks.
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource();
            CancellationToken cancellationToken =
                cancellationTokenSource.Token;
            TaskFactory taskFactory =
                new TaskFactory(cancellationToken);

            // manual reset flag for waiting dotnet command execution
            ManualResetEvent resetEvent = new ManualResetEvent(true);

            // background stream reader tasks collection
            List<Task> tasks = new List<Task>();
            Action<string, StreamReader> readAction =
                (prefix, reader) =>
                {
                    Task task =
                        taskFactory.StartNew(() =>
                        {
                            while (true)
                            {
                                // break the infinite loop until received cancel token
                                if (cancellationToken.IsCancellationRequested)
                                {
                                    break;
                                }

                                string msg = reader.ReadLine();
                                if (msg != null)
                                {
                                    Console.WriteLine($"[{prefix}] {msg}");

                                    // set manual reset flag, unblock command execution
                                    resetEvent.Set();
                                }
                            }
                        });

                    tasks.Add(task);
                };

            readAction("stdout", process.StandardOutput);
            readAction("stderr", process.StandardError);

            foreach (string command in commands)
            {
                Console.WriteLine($"[trace] running command: {command}");
                process.StandardInput.WriteLine(command);

                // waiting dotnet command execution until manual reset flag set
                if (command.StartsWith("dotnet"))
                {
                    resetEvent.Reset();
                    resetEvent.WaitOne();
                }
            }

            // exit the bash process
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();

            // cancel all background stream reader threads
            cancellationTokenSource.Cancel(false);
            Task.WaitAll(tasks.ToArray());

            process.Dispose();
        }
    }
}