// -----------------------------------------------------------------------
// <copyright file="ExcludeFromCoverageAttribute.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;

    /// <summary>
    /// Defines the exclude from coverage attribute, workaround for coverlet.
    /// </summary>
    /// <remarks>
    /// Reference: https://github.com/tonerdo/coverlet
    /// </remarks>
    [ExcludeFromCoverage(@"Only used for coverlet workaround.")]
    [AttributeUsage(
        AttributeTargets.Class
        | AttributeTargets.Constructor
        | AttributeTargets.Property
        | AttributeTargets.Method)]
    internal sealed class ExcludeFromCoverageAttribute
        : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcludeFromCoverageAttribute"/>
        /// class with an optional justification message.
        /// </summary>
        /// <param name="justification">
        /// The justification message about why excludes it from code coverage.
        /// </param>
        public ExcludeFromCoverageAttribute(string justification = null)
        {
            this.Justification = justification;
        }

        /// <summary>
        /// Gets the justification message about why excludes it from code
        /// coverage.
        /// </summary>
        public string Justification { get; private set; }
    }
}