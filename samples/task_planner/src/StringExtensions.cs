// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Defines the <see cref="string"/> class extension methods.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Replaces the format items in a specified string with the string
        /// representations of corresponding objects in a specified array in an
        /// invariant culture mode.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">
        /// An object array that contains zero or more objects to format.
        /// </param>
        /// <returns>
        /// If the format is null then return null, else return a copy of format
        /// in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.
        /// </returns>
        /// <exception cref="FormatException">
        /// Thrown if the given format is invalid,
        /// or the index of a format item is less than zero,
        /// or greater than or equal to the length of the args array.
        /// </exception>
        public static string FormatInvariant(
            this string format,
            params object[] args)
        {
            if (format == null)
            {
                return null;
            }

            return string.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}