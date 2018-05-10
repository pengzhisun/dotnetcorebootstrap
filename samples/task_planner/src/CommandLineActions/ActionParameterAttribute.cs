// -----------------------------------------------------------------------
// <copyright file="ActionParameterAttribute.cs" company="Pengzhi Sun">
// Copyright (c) Pengzhi Sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;

    [AttributeUsage(
        AttributeTargets.Property,
        Inherited = false,
        AllowMultiple = false)]
    internal sealed class ActionParameterAttribute : Attribute
    {
        public ActionParameterAttribute(
            object defaultValue,
            params string[] aliases)
        {
            if (aliases == null)
            {
                throw new ArgumentNullException(nameof(aliases));
            }

            if (aliases.Length == 0)
            {
                throw new ArgumentException(
                    ExceptionMessages.ActionParamNoAlias,
                    nameof(aliases));
            }

            this.DefaultValue = defaultValue;
            this.Aliases = aliases as IReadOnlyCollection<string>;
        }

        public object DefaultValue { get; private set; }

        public IReadOnlyCollection<string> Aliases { get; private set; }
    }
}