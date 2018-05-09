namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;
    using System.Reflection;
    using System.Text;
    using Newtonsoft.Json;
    using Xunit.Abstractions;

    internal static class TestOutputHelperExtensions
    {
        public static void PrintException(
            this ITestOutputHelper output,
            Exception exception)
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

                    builder.AppendLine($"{propInfo.Name}: {propValue}");
                }

                builder.AppendLine("StackTrace:");
                builder.AppendLine(current.StackTrace);

                current = current.InnerException;
            }
            while (current != null);

            output.WriteLine(builder.ToString());
        }
    }
}