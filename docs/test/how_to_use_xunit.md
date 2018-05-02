# How to use xUnit in .Net Core

## Steps

1. Create a xUnit project.

    * Create new project via `dotnet new xunit -o {output_dir}` command, e.g. [XUnitDemo.cs](../../demos/test_demo/XUnitDemo.cs)

        > `{output_dir}` sample: XUnitDemo_Test
        ```bash
        dotnet new xunit -o {output_dir}
        ```

        > Remove the default `UnitTest1.cs` file.

    * Optionally, could manually add `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio` references in existed library project, e.g. [XUnitDemo.csproj](../../demos/test_demo/XUnitDemo.csproj.xml)

        ```bash
        dotnet add package Microsoft.NET.Test.Sdk
        dotnet add package xunit
        dotnet add package xunit.runner.visualstudio
        dotnet restore
        ```

2. Add reference to to-be-tested project.

    > `{to_be_tested_project}` sample: XUnitDemo_ToBeTested.csproj, e.g. [XUnitDemo.cs](../../demos/test_demo/XUnitDemo.cs)

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

3. Add test class in the xUnit project.

    > The test class sample: [XUnitDemoTestClass.cs](../../demos/test_demo/XUnitDemoTestClass.cs)

    ```csharp
    public sealed class XUnitDemoTestClass
    {
        ...

        [Fact]
        public void IsOddGivenIntMinValueSuccessTest()
        {
            ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

            bool actualResult = toBeTestedInstance.IsOdd(int.MinValue);

            Assert.False(actualResult);
        }
    }
    ```

    > Could refer tests conventions from here: [Unit tests and functional tests](https://github.com/aspnet/Home/wiki/Engineering-guidelines#unit-tests-and-functional-tests)

4. Run xUnit cases via `dotnet test` command inside the xUnit project directory.

    > e.g. [XUnitDemo.cs](../../demos/test_demo/XUnitDemo.cs)

    ```bash
    dotnet test
    ```

    > could use `dotnet test --logger:"console;verbosity=normal"` command to print the tracelog to console, e.g. [XUnitDemoTestClass.cs](../../demos/test_demo/XUnitDemoTestClass.cs)

    ```csharp
    public sealed class XUnitDemoTestClass
    {
        private readonly ITestOutputHelper output;

        public XUnitDemoTestClass(ITestOutputHelper output)
        {
            this.output = output;
        }

        ...

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

        private void RunXUnitTest(string methodName, Action testAction)
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
    ```

## References

* [Unit Testing in .NET Core and .NET Standard (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/core/testing/)
* [Unit testing C# in .NET Core using dotnet test and xUnit (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)
* [Engineering guidelines (github.com)](https://github.com/aspnet/Home/wiki/Engineering-guidelines#unit-tests-and-functional-tests)
* [Microsoft.NET.Test.Sdk (nuget.org)](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk)
* [xunit (nuget.org)](https://www.nuget.org/packages/xunit)
* [xunit.runner.visualstudio (nuget.org)](https://www.nuget.org/packages/xunit.runner.visualstudio)
* [xunit (github.com)](https://github.com/xunit/xunit)
* [xUnit.net project home (xunit.github.io)](https://xunit.github.io/)
* [Getting started with xUnit.net (.NET Core / ASP.NET Core) > xUnit.net](http://xunit.github.io/docs/getting-started-dotnet-core)
* [Capturing Output (xunit.github.io)](https://xunit.github.io/docs/capturing-output)
* [Reporting test results (github.com)](https://github.com/Microsoft/vstest-docs/blob/master/docs/report.md)