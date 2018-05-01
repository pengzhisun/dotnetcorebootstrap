/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   MSTestDemoTestClass.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core MSTest test class demos.
 * Reference:   https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting
 *              https://www.nuget.org/packages/MSTest.TestFramework/
 *              https://github.com/microsoft/testfx
 *****************************************************************************/

namespace DotNetCoreBootstrap.TestDemo
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines the MSTest demo test class.
    /// </summary>
    [TestClass]
    public sealed class MSTestDemoTestClass
    {
        /// <summary>
        /// Gets or sets the test context.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// The test method initialize method.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // run following command to write test method metadata to console:
            // dotnet test --logger:"console;verbosity=normal"
            MethodInfo methodInfo =
                this.GetType().GetMethod(this.TestContext.TestName);

            DescriptionAttribute descriptionAttribute =
                methodInfo.GetCustomAttribute<DescriptionAttribute>();
            Console.WriteLine($"Description    : {descriptionAttribute.Description}");

            TestCategoryAttribute testCategoryAttribute =
                methodInfo.GetCustomAttribute<TestCategoryAttribute>();
            Console.WriteLine($"Test Categories: {string.Join(',', testCategoryAttribute.TestCategories)}");

            PriorityAttribute priorityAttribute =
                methodInfo.GetCustomAttribute<PriorityAttribute>();
            Console.WriteLine($"Priority       : {priorityAttribute.Priority}");

            OwnerAttribute ownerAttribute =
                methodInfo.GetCustomAttribute<OwnerAttribute>();
            Console.WriteLine($"Owner          : {ownerAttribute.Owner}");

            Stopwatch testMethodStopwatch = new Stopwatch();
            testMethodStopwatch.Start();

            this.TestContext.Properties["TestMethodStopwatch"] =
                testMethodStopwatch;
        }

        /// <summary>
        /// The test method cleanup method.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            Stopwatch testMethodStopwatch =
                (Stopwatch)this.TestContext.Properties["TestMethodStopwatch"];
            testMethodStopwatch.Stop();

            Console.WriteLine($"Execution time : {testMethodStopwatch.Elapsed}");
        }

        /// <summary>
        /// Tests for verifying ToBeTestedClass.IsOdd(int number) method
        /// with odd values worked correctly.
        /// </summary>
        /// <param name="number">The given integer value for testing.</param>
        [DataTestMethod]
        [Description("Tests for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with odd values worked correctly.")]
        [TestCategory("BVT")]
        [Priority(0)]
        [Owner("Pengzhi Sun")]
        [DataRow(-1)]
        [DataRow(-33)]
        [DataRow(-57)]
        [DataRow(101)]
        [DataRow(3123)]
        [DataRow(513129)]
        public void IsOddGivenOddValuesSuccessTest(int number)
        {
            ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

            bool actualResult = toBeTestedInstance.IsOdd(number);

            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Tests for verifying ToBeTestedClass.IsOdd(int number) method
        /// with even values worked correctly.
        /// </summary>
        /// <param name="number">The given integer value for testing.</param>
        [DataTestMethod]
        [Description("Tests for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with even values worked correctly.")]
        [TestCategory("BVT")]
        [Priority(0)]
        [Owner("Pengzhi Sun")]
        [DataRow(0)]
        [DataRow(-22)]
        [DataRow(-446)]
        [DataRow(-6192)]
        [DataRow(2468)]
        [DataRow(412456)]
        [DataRow(6123458)]
        public void IsOddGivenEvenValuesSuccessTest(int number)
        {
            ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

            bool actualResult = toBeTestedInstance.IsOdd(number);

            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Test for verifying ToBeTestedClass.IsOdd(int number) method
        /// with Int32.MaxValue (2,147,483,647) worked correctly.
        /// </summary>
        [TestMethod]
        [Description("Test for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with Int32.MaxValue (2,147,483,647) worked correctly.")]
        [TestCategory("BVT")]
        [Priority(0)]
        [Owner("Pengzhi Sun")]
        public void IsOddGivenIntMaxValueSuccessTest()
        {
            ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

            bool actualResult = toBeTestedInstance.IsOdd(int.MaxValue);

            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Test for verifying ToBeTestedClass.IsOdd(int number) method
        /// with Int32.MinValue (-2,147,483,648) worked correctly.
        /// </summary>
        [TestMethod]
        [Description("Test for verifying ToBeTestedClass.IsOdd(int number)"
            + " method with Int32.MinValue (-2,147,483,648) worked correctly.")]
        [TestCategory("BVT")]
        [Priority(0)]
        [Owner("Pengzhi Sun")]
        public void IsOddGivenIntMinValueSuccessTest()
        {
            ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

            bool actualResult = toBeTestedInstance.IsOdd(int.MinValue);

            Assert.IsFalse(actualResult);
        }
    }
}