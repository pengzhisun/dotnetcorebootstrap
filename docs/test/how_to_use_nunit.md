# How to use NUnit in .Net Core

## Steps

1. Create a NUnit project.

    * Import the NUnit project template via `dotnet new -i NUnit3.DotNetNew.Template`.

        > could refer template repo from [here](https://github.com/nunit/dotnet-new-nunit).

        ```bash
        dotnet new -i NUnit3.DotNetNew.Template
        ```

    * Create new project via `dotnet new nunit -o {output_dir}` command, e.g. [NUnitDemo.cs](../../demos/test_demo/NUnitDemo.cs)

        > `{output_dir}` sample: NUnitDemo_Test
        ```bash
        dotnet new nunit -o {output_dir}
        ```

        > Remove the default `UnitTest1.cs` file.

    * Optionally, could manually add `Microsoft.NET.Test.Sdk`, `nunit`, `NUnit3TestAdapter` references in existed library project, e.g. [NUnitDemo.csproj](../../demos/test_demo/NUnitDemo.csproj.xml)

        ```bash
        dotnet add package Microsoft.NET.Test.Sdk
        dotnet add package nunit
        dotnet add package NUnit3TestAdapter
        dotnet restore
        ```

2. Add reference to to-be-tested project.

    > `{to_be_tested_project}` sample: NUnitDemo_ToBeTested.csproj, e.g. [NUnitDemo.cs](../../demos/test_demo/NUnitDemo.cs)

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

3. Add test class in the NUnit project.

    > The test class sample: [NUnitDemoTestClass.cs](../../demos/test_demo/NUnitDemoTestClass.cs)

    ```csharp
    [TestFixture]
    public sealed class NUnitDemoTestClass
    {
        ...

        [Test]
        public void IsOddGivenIntMinValueSuccessTest()
        {
            ToBeTestedClass toBeTestedInstance = new ToBeTestedClass();

            bool actualResult = toBeTestedInstance.IsOdd(int.MinValue);

            Assert.That(actualResult, Is.False);
        }
    }
    ```

    > Could refer tests conventions from here: [Unit tests and functional tests](https://github.com/aspnet/Home/wiki/Engineering-guidelines#unit-tests-and-functional-tests)

4. Run NUnit cases via `dotnet test` command inside the NUnit project directory.

    > e.g. [NUnitDemo.cs](../../demos/test_demo/NUnitDemo.cs)

    ```bash
    dotnet test
    ```

    > could use `dotnet test --logger:"console;verbosity=normal"` command to print the tracelog to console, e.g. [NUnitDemoTestClass.cs](../../demos/test_demo/NUnitDemoTestClass.cs)

    ```csharp
    [TestFixture]
    public sealed class NUnitDemoTestClass
    {
        ...

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
    ```

## References

* [Unit Testing in .NET Core and .NET Standard (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/core/testing/)
* [Unit testing C# with NUnit and .NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-nunit)
* [Engineering guidelines (github.com)](https://github.com/aspnet/Home/wiki/Engineering-guidelines#unit-tests-and-functional-tests)
* [Microsoft.NET.Test.Sdk (nuget.org)](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk)
* [NUnit (nuget.org)](https://www.nuget.org/packages/NUnit/)
* [NUnit3TestAdapter (nuget.org)](https://www.nuget.org/packages/NUnit3TestAdapter/)
* [NUnit 3 Framework (github.com)](https://github.com/nunit/nunit)
* [NUnit Documentation Wiki (github.com)](https://github.com/nunit/docs/wiki)
* [.NET Core and .NET Standard (github.com)](https://github.com/nunit/docs/wiki/.NET-Core-and-.NET-Standard)
* [NUnit 3 Test Project Template for dotnet new CLI (github.com)](https://github.com/nunit/dotnet-new-nunit)
* [Reporting test results (github.com)](https://github.com/Microsoft/vstest-docs/blob/master/docs/report.md)