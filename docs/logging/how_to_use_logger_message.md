# How to use LoggerMessage in .Net Core

## Steps

1. Add Nuget package `Microsoft.Extensions.Logging` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/logging_demo/logging_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Logging
    dotnet restore
    ```

2. Add `Microsoft.Extensions.Logging` namespace.

    > e.g. [LoggerMessageDemo.cs](../../demos/logging_demo/LoggerMessageDemo.cs)
    ```csharp
    using Microsoft.Extensions.Logging;
    ```

3. Create console logger factory for demo purpose.

    > e.g. [LoggerMessageDemo.cs](../../demos/logging_demo/LoggerMessageDemo.cs), could refer this guide: [How to use console log in .Net Core](how_to_use_console_log.md)

        ```csharp
        ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory
                .AddConsole(
                    minLevel: LogLevel.Trace,
                    includeScopes: true);
        ```
        > this console logger factory supports all log levels and include scopes

4. Create logger instance via [CreateLogger&lt;T&gt;()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerfactoryextensions.createlogger?view=aspnetcore-2.0#Microsoft_Extensions_Logging_LoggerFactoryExtensions_CreateLogger__1_Microsoft_Extensions_Logging_ILoggerFactory_) method.

    > e.g. [LoggerMessageDemo.cs](../../demos/logging_demo/LoggerMessageDemo.cs)

    ```csharp
    ILogger logger = loggerFactory.CreateLogger<LoggerMessageDemo>();
    ```

5. Create a define scope delegate method via [LoggerMessage.DefineScope&lt;T1&gt;(string)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggermessage.definescope?view=aspnetcore-2.0#Microsoft_Extensions_Logging_LoggerMessage_DefineScope__1_System_String_)

    > e.g. [LoggerMessageDemo.cs](../../demos/logging_demo/LoggerMessageDemo.cs)

    ```csharp
    Func<ILogger, string, IDisposable> defineScopeFunc =
        LoggerMessage.DefineScope<string>("LoggerScope: {ScopeName}");
    ```

    > The LoggerMessage.DefineScope method support 0-3 type parameters.
    > Typically, define a well named extension method for ILogger interface.

6. Create a define delegate method via [LoggerMessage.Define&lt;T1,T2&gt;(LogLevel, EventId, string)](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.logging.loggermessage.define?view=aspnetcore-2.0#Microsoft_Extensions_Logging_LoggerMessage_Define__2_Microsoft_Extensions_Logging_LogLevel_Microsoft_Extensions_Logging_EventId_System_String_)

    > e.g. [LoggerMessageDemo.cs](../../demos/logging_demo/LoggerMessageDemo.cs)

    ```csharp
    EventId eventId = new EventId(1004, "LoggerMessageDemoEvent");

    Action<ILogger, string, int, Exception> logAction =
        LoggerMessage.Define<string, int>(
            LogLevel.Information,
            eventId,
            "Log Message: stringValue = '{StringValue}', intValue = '{IntValue}'");
    ```

    > The LoggerMessage.Define method support 0-6 type parameters.
    > Typically, define a well named extension method for ILogger interface.

7. Write log message via created delegate methods.

    > e.g. [LoggerMessageDemo.cs](../../demos/logging_demo/LoggerMessageDemo.cs)

    ```csharp
    using (defineScopeFunc(logger, "DemoScope"))
    {
        logAction(logger, "string_value_1", 1, null);
    }
    ```

    > the console log will be processed in backgroud thread, for demo purpose we need to sleep main thread a little.

    ```csharp
    Thread.Sleep(TimeSpan.FromMilliseconds(10));
    ```

## References

* [Logging in ASP.NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/)
* [Logging Guidelines (github.com)](https://github.com/aspnet/Logging/wiki/Guidelines)
* [High-performance logging with LoggerMessage in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage)
* [Microsoft.Extensions.Logging Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging)
* [Microsoft.Extensions.Logging (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Logging)
* [Microsoft.Extensions.Logging (github.com)](https://github.com/aspnet/Logging/tree/dev/src/Microsoft.Extensions.Logging)
