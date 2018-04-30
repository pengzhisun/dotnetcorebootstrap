/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   ToBeTestedClass.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core to-be-tested class.
 *****************************************************************************/

namespace DotNetCoreBootstrap.TestDemo
{
    using System;

    /// <summary>
    /// Defines the to-be-tested class.
    /// </summary>
    public sealed class ToBeTestedClass
    {
        /// <summary>
        /// Check the given integer number is odd or not.
        /// </summary>
        /// <param name="number">The integer number.</param>
        /// <returns>True if the given number is odd, otherwise false.</returns>
        public bool IsOdd(int number)
            => number % 2 != 0;
    }
}