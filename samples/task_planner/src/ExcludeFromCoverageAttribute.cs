// -----------------------------------------------------------------------
// <copyright file="ExcludeFromCoverageAttribute.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;

    [ExcludeFromCoverage]
    [AttributeUsage(
        AttributeTargets.Class
        | AttributeTargets.Property
        | AttributeTargets.Method
        | AttributeTargets.Constructor)]
    public sealed class ExcludeFromCoverageAttribute
        : Attribute
    {
    }
}