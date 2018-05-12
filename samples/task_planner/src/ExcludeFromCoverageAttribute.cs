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
    [AttributeUsage(
        AttributeTargets.Class
        | AttributeTargets.Constructor
        | AttributeTargets.Property
        | AttributeTargets.Method)]
    internal sealed class ExcludeFromCoverageAttribute
        : Attribute
    {
    }
}