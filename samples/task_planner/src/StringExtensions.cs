namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System.Globalization;

    public static class StringExtensions
    {
        public static string FormatInvariant(this string format, params object[] args)
            => string.Format(CultureInfo.InvariantCulture, format, args);
    }
}