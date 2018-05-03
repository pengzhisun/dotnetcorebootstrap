/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   IDemoClass.cs
 * Author:      Pengzhi Sun
 * Description: Defines the demo class interface for dependency inversion demos.
 *****************************************************************************/

namespace DotNetCoreBootstrap.DIDemo
{
    /// <summary>
    /// Defines the demo class interface for dependency inversion demos.
    /// </summary>
    public interface IDemoClass
    {
        /// <summary>
        /// Gets the type name of the demo class.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Gets the identifier of the demo class instance.
        /// </summary>
        string InstanceId { get; }

        /// <summary>
        /// Gets the name of the demo class instance.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the value of the demo class instance.
        /// </summary>
        string Value { get; }

        /// <summary>
        /// Execute demo method from the demo class instance.
        /// </summary>
        void DemoMethod();
    }
}