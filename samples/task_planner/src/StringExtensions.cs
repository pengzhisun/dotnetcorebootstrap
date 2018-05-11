// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System.Globalization;

    internal static class StringExtensions
    {
        public static string FormatInvariant(this string format, params object[] args)
            => string.Format(CultureInfo.InvariantCulture, format, args);
    }
}