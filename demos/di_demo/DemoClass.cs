/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   DemoClass.cs
 * Author:      Pengzhi Sun
 * Description: Defines the demo class for dependency inversion demos.
 *****************************************************************************/

namespace DotNetCoreBootstrap.DIDemo
{
    using System;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the demo class for dependency inversion demos.
    /// </summary>
    public sealed class DemoClass : IDemoClass
    {
        /// <summary>
        /// The internal logger instance.
        /// </summary>
        private readonly ILogger<DemoClass> logger;

        /// <summary>
        /// The internal identifier of this demo class instance.
        /// </summary>
        private readonly string instanceId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DemoClass"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="name">The name of the demo class instance.</param>
        /// <param name="value">The value of the demo class instance.</param>
        public DemoClass(ILogger<DemoClass> logger, string name, string value)
        {
            this.logger = logger;
            this.instanceId = Guid.NewGuid().ToString("D");
            this.Name = name;
            this.Value = value;

            this.logger.LogDebug(
                $"Resolved object {this}");
        }

        /// <summary>
        /// Gets the type name of the demo class.
        /// </summary>
        public string TypeName => this.GetType().Name;

        /// <summary>
        /// Gets the identifier of the demo class instance.
        /// </summary>
        public string InstanceId => this.instanceId;

        /// <summary>
        /// Gets the name of the demo class instance.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value of the demo class instance.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Execute demo method from the demo class instance.
        /// </summary>
        public void DemoMethod()
        {
            this.logger.LogDebug($"Run demo method from {this}");
            Console.WriteLine($"Run demo method from {this}");
        }

        /// <summary>
        /// Returns a string that represents the current demo class instance.
        /// </summary>
        /// <returns>A string that represents the current demo class instance.</returns>
        public override string ToString()
            => $"[TypeName = '{this.TypeName}', InstanceId = '{this.instanceId}', Name = '{this.Name}', Value = '{this.Value}']";
    }
}