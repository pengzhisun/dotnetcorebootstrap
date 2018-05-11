// -----------------------------------------------------------------------
// <copyright file="ExceptionExtensions.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;
    using System.Reflection;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see cref="Exception"/> class extension methods.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Get the <see cref="Exception"/> instance detail infomration.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> instance.</param>
        /// <returns>
        /// A string representation the detail information for the given
        /// <see cref="Exception"/> instance.
        /// </returns>
        public static string GetDetail(this Exception exception)
        {
            StringBuilder builder = new StringBuilder();

            Exception current = exception;

            do
            {
                builder.AppendLine($"{current.GetType().FullName}: {current.Message}");
                foreach (PropertyInfo propInfo in current.GetType().GetProperties())
                {
                    object propValue = null;
                    switch (propInfo.Name)
                    {
                        case "HResult":
                        case "InnerException":
                        case "StackTrace":
                        case "Message":
                            continue;
                        case "TargetSite":
                            propValue = $"{current.TargetSite.DeclaringType.FullName}.{current.TargetSite.Name}";
                            break;
                        case "Data":
                            if (current.Data.Count > 0)
                            {
                                propValue = JsonConvert.SerializeObject(current.Data);
                            }

                            break;
                        default:
                            propValue = propInfo.GetValue(current);
                            break;
                    }

                    if (propValue == null)
                    {
                        continue;
                    }

                    if (propInfo.PropertyType == typeof(string))
                    {
                        if (string.IsNullOrWhiteSpace((string)propValue))
                        {
                            continue;
                        }
                    }

                    if (propInfo.PropertyType.IsEnum)
                    {
                        propValue = $"{propValue}({(int)propValue})";
                    }

                    builder.AppendLine($"{propInfo.Name}: {propValue}");
                }

                builder.AppendLine("StackTrace:");
                builder.AppendLine(current.StackTrace);

                current = current.InnerException;
            }
            while (current != null);

            return builder.ToString();
        }
    }
}