/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   NUnitDemoTestClass.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core NUnit test class demos.
 * Reference:   https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-nunit
 *              https://github.com/nunit/docs/wiki/.NET-Core-and-.NET-Standard
 *              https://github.com/nunit/docs/wiki
 *              https://www.nuget.org/packages/NUnit/
 *              https://github.com/nunit/nunit
 *****************************************************************************/

namespace DotNetCoreBootstrap.TestDemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;

    /// <summary>
    /// Defines the NUnit demo test class.
    /// </summary>
    [TestFixture]
    public sealed class NUnitDemoTestClass
    {
        /// <summary>
        /// Tests for verifying ToBeTestedClass.IsOdd(int number) method
        /// with odd values worked correctly.
        /// </summary>
        /// <param name="number">The given integer value for testing.</param>
        [Theory]
        [Description("Test for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with odd values worked correctly.")]
        [Category("BVT")]
        [Property("Priority", 0)]
        [Author("Pengzhi Sun")]
        public void IsOddGivenOddValuesSuccessTest(
            [Values(-1, -33, -57, 101, 3123, 513129)]
            int number)
        {
            RunNUnitTest(
                () =>
                {
                    ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

                    bool actualResult = toBeTestedInstance.IsOdd(number);

                    Assert.That(actualResult, Is.True);
                });
        }

        /// <summary>
        /// Tests for verifying ToBeTestedClass.IsOdd(int number) method
        /// with even values worked correctly.
        /// </summary>
        /// <param name="number">The given integer value for testing.</param>
        [Theory]
        [Description("Test for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with even values worked correctly.")]
        [Category("BVT")]
        [Property("Priority", 0)]
        [Author("Pengzhi Sun")]
        public void IsOddGivenEvenValuesSuccessTest(
            [Values(0, -22, -446, -6192, 2468, 412456, 6123458)]
            int number)
        {
            RunNUnitTest(
                () =>
                {
                    ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

                    bool actualResult = toBeTestedInstance.IsOdd(number);

                    Assert.That(actualResult, Is.False);
                });
        }

        /// <summary>
        /// Test for verifying ToBeTestedClass.IsOdd(int number) method
        /// with Int32.MaxValue (2,147,483,647) worked correctly.
        /// </summary>
        [Test]
        [Description("Test for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with Int32.MaxValue (2,147,483,647) worked correctly.")]
        [Category("BVT")]
        [Property("Priority", 0)]
        [Author("Pengzhi Sun")]
        public void IsOddGivenIntMaxValueSuccessTest()
        {
            RunNUnitTest(
                () =>
                {
                    ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

                    bool actualResult = toBeTestedInstance.IsOdd(int.MaxValue);

                    Assert.That(actualResult, Is.True);
                });
        }

        /// <summary>
        /// Test for verifying ToBeTestedClass.IsOdd(int number) method
        /// with Int32.MinValue (-2,147,483,648) worked correctly.
        /// </summary>
        [Test]
        [Description("Test for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with Int32.MinValue (-2,147,483,648) worked correctly.")]
        [Category("BVT")]
        [Property("Priority", 0)]
        [Author("Pengzhi Sun")]
        public void IsOddGivenIntMinValueSuccessTest()
        {
            RunNUnitTest(
                () =>
                {
                    ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

                    bool actualResult = toBeTestedInstance.IsOdd(int.MinValue);

                    Assert.That(actualResult, Is.False);
                }
            );
        }

        /// <summary>
        /// Runs NUnit test and appends meta-data to output.
        /// </summary>
        /// <param name="testAction">The actual test action.</param>
        private void RunNUnitTest(Action testAction)
        {
            MethodInfo testMethodInfo =
                this.GetType().GetMethod(TestContext.CurrentContext.Test.MethodName);

            CategoryAttribute categoryAttribute =
                testMethodInfo.GetCustomAttribute<CategoryAttribute>();
            TestContext.Out.WriteLine($"Category       : {categoryAttribute.Name}");

            IEnumerable<KeyValuePair<string, object>> properties =
                testMethodInfo.GetCustomAttributes().OfType<PropertyAttribute>()
                    .Select(p =>
                        p.Properties.Keys.ToDictionary(
                            k => k,
                            k => p.Properties.Get(k)
                        ))
                    .SelectMany(kvp => kvp);

            foreach (KeyValuePair<string, object> property in properties)
            {
                TestContext.Out.WriteLine($"{property.Key.PadRight(15)}: {property.Value}");
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
                TestContext.Out.WriteLine($"Execution Time : {testMethodStopwatch.Elapsed}");
            }
        }
    }
}