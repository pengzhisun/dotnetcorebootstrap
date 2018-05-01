/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   XUnitDemoTestClass.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core xUnit test class demos.
 * Reference:   https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test
 *              http://xunit.github.io/docs/getting-started-dotnet-core
 *              https://www.nuget.org/packages/xunit/
 *              https://www.nuget.org/packages/xunit.runner.visualstudio/
 *              https://github.com/xunit/xunit
 *              https://xunit.github.io/docs/capturing-output
 *              https://github.com/xunit/xunit/blob/master/src/xunit.execution/Sdk/TraitHelper.cs
 *****************************************************************************/

namespace DotNetCoreBootstrap.TestDemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// Defines the xUnit demo test class.
    /// </summary>
    public sealed class XUnitDemoTestClass
    {
        /// <summary>
        /// The internal xUnit test output helper.
        /// </summary>
        private readonly ITestOutputHelper output;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitDemoTestClass"/> class.
        /// </summary>
        /// <param name="output">The internal xUnit test output helper.</param>
        public XUnitDemoTestClass(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// Tests for verifying ToBeTestedClass.IsOdd(int number) method
        /// with odd values worked correctly.
        /// </summary>
        /// <param name="number">The given integer value for testing.</param>
        [Theory]
        [InlineData(-1)]
        [InlineData(-33)]
        [InlineData(-57)]
        [InlineData(101)]
        [InlineData(3123)]
        [InlineData(513129)]
        [Trait("Description", "Test for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with odd values worked correctly.")]
        [Trait("Category", "BVT")]
        [Trait("Priority", "0")]
        [Trait("Owner", "Pengzhi Sun")]
        public void IsOddGivenOddValuesSuccessTest(int number)
        {
            RunXUnitTest(
                MethodBase.GetCurrentMethod().Name,
                () =>
                {
                    ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

                    bool actualResult = toBeTestedInstance.IsOdd(number);

                    Assert.True(actualResult);
                },
                number);
        }

        /// <summary>
        /// Tests for verifying ToBeTestedClass.IsOdd(int number) method
        /// with even values worked correctly.
        /// </summary>
        /// <param name="number">The given integer value for testing.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-22)]
        [InlineData(-446)]
        [InlineData(-6192)]
        [InlineData(2468)]
        [InlineData(412456)]
        [InlineData(6123458)]
        [Trait("Description", "Test for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with even values worked correctly.")]
        [Trait("Category", "BVT")]
        [Trait("Priority", "0")]
        [Trait("Owner", "Pengzhi Sun")]
        public void IsOddGivenEvenValuesSuccessTest(int number)
        {
            RunXUnitTest(
                MethodBase.GetCurrentMethod().Name,
                () =>
                {
                    ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

                    bool actualResult = toBeTestedInstance.IsOdd(number);

                    Assert.False(actualResult);
                },
                number);
        }

        /// <summary>
        /// Test for verifying ToBeTestedClass.IsOdd(int number) method
        /// with Int32.MaxValue (2,147,483,647) worked correctly.
        /// </summary>
        [Fact]
        [Trait("Description", "Test for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with Int32.MaxValue (2,147,483,647) worked correctly.")]
        [Trait("Category", "BVT")]
        [Trait("Priority", "0")]
        [Trait("Owner", "Pengzhi Sun")]
        public void IsOddGivenIntMaxValueSuccessTest()
        {
            RunXUnitTest(
                MethodBase.GetCurrentMethod().Name,
                () =>
                {
                    ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

                    bool actualResult = toBeTestedInstance.IsOdd(int.MaxValue);

                    Assert.True(actualResult);
                });
        }

        /// <summary>
        /// Test for verifying ToBeTestedClass.IsOdd(int number) method
        /// with Int32.MinValue (-2,147,483,648) worked correctly.
        /// </summary>
        [Fact]
        [Trait("Description", "Test for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with Int32.MinValue (-2,147,483,648) worked correctly.")]
        [Trait("Category", "BVT")]
        [Trait("Priority", "0")]
        [Trait("Owner", "Pengzhi Sun")]
        public void IsOddGivenIntMinValueSuccessTest()
        {
            RunXUnitTest(
                MethodBase.GetCurrentMethod().Name,
                () =>
                {
                    ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

                    bool actualResult = toBeTestedInstance.IsOdd(int.MinValue);

                    Assert.False(actualResult);
                });
        }

        /// <summary>
        /// Runs xUnit test and appends meta-data to output.
        /// </summary>
        /// <param name="methodName">The test method name.</param>
        /// <param name="testAction">The actual test action.</param>
        /// <param name="testMethodParameters">The given parameters for test method.</param>
        private void RunXUnitTest(
            string methodName,
            Action testAction,
            params object[] testMethodParameters)
        {
            MethodInfo testMethodInfo = this.GetType().GetMethod(methodName);

            foreach (KeyValuePair<string, string> trait in
                TraitHelper.GetTraits(testMethodInfo))
            {
                this.output.WriteLine($"{trait.Key.PadRight(15)}: {trait.Value}");
            }

            Stopwatch testMethodStopwatch = new Stopwatch();
            testMethodStopwatch.Start();

            try
            {
                testAction();
            }
            finally
            {
                testMethodStopwatch.Stop();
                this.output.WriteLine($"Execution Time : {testMethodStopwatch.Elapsed}");
            }
        }
    }
}