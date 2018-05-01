# How to use MSTest in .Net Core

## Steps

1. Create a MSTest project.

    * Create new project via `dotnet new mstest -o {output_dir}` command, e.g. [MSTestDemo.cs](../../demos/test_demo/MSTestDemo.cs)

        > `{output_dir}` sample: MSTestDemo_Test
        ```bash
        dotnet new mstest -o {output_dir}
        ```

        > Remove the default `UnitTest1.cs` file.

    * Optionally, could manually add `Microsoft.NET.Test.Sdk`, `MSTest.TestAdapter`, `MSTest.TestFramework` references in existed library project, e.g. [MSTestDemo.csproj](../../demos/test_demo/MSTestDemo.csproj.xml)

        ```bash
        dotnet add package Microsoft.NET.Test.Sdk
        dotnet add package MSTest.TestAdapter
        dotnet add package MSTest.TestFramework
        dotnet restore
        ```

2. Add reference to to-be-tested project.

    > `{to_be_tested_project}` sample: MSTestDemo_ToBeTested.csproj, e.g. [MSTestDemo.cs](../../demos/test_demo/MSTestDemo.cs)

    ```bash
    dotnet add reference {to_be_tested_project}
    ```

    > The to-be-tested class sample: [ToBeTestedClass.cs](../../demos/test_demo/ToBeTestedClass.cs)

    ```csharp
    public sealed class ToBeTestedClass
    {
        public bool IsOdd(int number)
            => number % 2 != 0;
    }
    ```

3. Add test class in the MSTest project.

    > The test class sample: [MSTestDemoTestClass.cs](../../demos/test_demo/MSTestDemoTestClass.cs)

    ```csharp
    [TestClass]
    public sealed class MSTestDemoTestClass
    {
        ...

        [TestMethod]
        public void IsOddGivenIntMinValueSuccessTest()
        {
            ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

            bool actualResult = toBeTestedInstance.IsOdd(int.MinValue);

            Assert.IsFalse(actualResult);
        }
    }
    ```

    > Could refer tests conventions from here: [Unit tests and functional tests](https://github.com/aspnet/Home/wiki/Engineering-guidelines#unit-tests-and-functional-tests)

4. Run MSTest cases via `dotnet test` command inside the MSTest project directory.

    > e.g. [MSTestDemo.cs](../../demos/test_demo/MSTestDemo.cs)

    ```bash
    dotnet test
    ```

    > could use `dotnet test --logger:"console;verbosity=normal"` command to print the tracelog to console, e.g. [MSTestDemoTestClass.cs](../../demos/test_demo/MSTestDemoTestClass.cs)

    ```csharp
    [TestClass]
    public sealed class MSTestDemoTestClass
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
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

        [TestCleanup]
        public void Cleanup()
        {
            Stopwatch testMethodStopwatch =
                (Stopwatch)this.TestContext.Properties["TestMethodStopwatch"];
            testMethodStopwatch.Stop();

            Console.WriteLine($"Execution time : {testMethodStopwatch.Elapsed}");
        }

        ...

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
    ```

## References

* [Unit Testing in .NET Core and .NET Standard (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/core/testing/)
* [Unit testing C# with MSTest and .NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
* [Engineering guidelines (github.com)](https://github.com/aspnet/Home/wiki/Engineering-guidelines#unit-tests-and-functional-tests)
* [Microsoft.VisualStudio.TestTools.UnitTesting Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting)
* [Microsoft.NET.Test.Sdk (nuget.org)](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk)
* [MSTest.TestAdapter (nuget.org)](https://www.nuget.org/packages/MSTest.TestAdapter)
* [MSTest.TestFramework (nuget.org)](https://www.nuget.org/packages/MSTest.TestFramework)
* [Visual Studio Test Platform (github.com)](https://github.com/Microsoft/vstest)
* [Microsoft Test Framework (github.com)](https://github.com/microsoft/testfx)
* [Reporting test results (github.com)](https://github.com/Microsoft/vstest-docs/blob/master/docs/report.md)