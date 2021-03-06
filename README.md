# The .Net Core Bootstrap Project

**Talk is cheap, show me the code!**

This bootstrap project provides the sample code and simple how-to guide for regular development works.

## Prerequisites

* [.NET Core 2.x SDK](https://github.com/dotnet/core/tree/master/release-notes)

* [Visual Studio Code](https://code.visualstudio.com/download)

  Recommended Extensions:
  * [.NET Core Starter's Pack (by Blair Leduc)](https://marketplace.visualstudio.com/items?itemName=blairleduc.net-core-starters-pack)

    A extension pack containing the set of extensions that every .NET Core developer needs.

    * [Better Comments (by Aaron Bond)](https://marketplace.visualstudio.com/items?itemName=aaron-bond.better-comments)

      Improve your code commenting by annotating with alert, informational, TODOs, and more!

    * [Bracket Pair Colorizer (by CoenraadS)](https://marketplace.visualstudio.com/items?itemName=CoenraadS.bracket-pair-colorizer)

      A customizable extension for colorizing matching brackets

    * [C# (by Microsoft)](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)

      C# for Visual Studio Code (powered by [OmniSharp](https://github.com/OmniSharp/omnisharp-roslyn)).

    * [C# XML Documentation Comments (by Keisuke Kato)](https://marketplace.visualstudio.com/items?itemName=k--kato.docomment)

      Generate C# XML documentation comments for ///

    * [EditorConfig for VS Code (by EditorConfig)](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig)

      EditorConfig Support for Visual Studio Code

    * [GitLens — Git supercharged (by Eric Amodio)](https://marketplace.visualstudio.com/items?itemName=eamodio.gitlens)

      Supercharge the Git capabilities built into Visual Studio Code — Visualize code authorship at a glance via Git blame annotations and code lens, seamlessly navigate and explore Git repositories, gain valuable insights via powerful comparison commands, and so much more

    * [Highlight Trailing White Spaces (by Yves Baumes)](https://marketplace.visualstudio.com/items?itemName=ybaumes.highlight-trailing-white-spaces)

      This extension highlight trailing white spaces in red.

    * [Material Icon Theme (by Philipp Kief)](https://marketplace.visualstudio.com/items?itemName=PKief.material-icon-theme)

      Material Design Icons for Visual Studio Code

    * [Quick and Simple Text Selection (by David Bankier)](https://marketplace.visualstudio.com/items?itemName=dbankier.vscode-quick-select)

      Jump to select between quote, brackets, tags, etc

  * [C# Extensions (by jchannon)](https://marketplace.visualstudio.com/items?itemName=jchannon.csharpextensions)

    C# IDE Extensions for VSCode

  * [gitignore (by CodeZombie)](https://marketplace.visualstudio.com/items?itemName=codezombiech.gitignore)

    Language support for .gitignore files. Lets you pull .gitignore files from the [gitignore](https://github.com/github/gitignore) repository.

  * [markdownlint (by David Anson)](https://marketplace.visualstudio.com/items?itemName=DavidAnson.vscode-markdownlint)

    Markdown linting and style checking for Visual Studio Code

  * [XML Tools (by Josh Johnson)](https://marketplace.visualstudio.com/items?itemName=DotJoshJohnson.xml)

    XML Formatting, XQuery, and XPath Tools for Visual Studio Code

## Demos

* [Configuration](demos/config_demo)

  This project contains following demo scenarios:

  * [ConfigurationManager demo](demos/config_demo/ConfigurationManagerDemo.cs)
    * [How to use ConfigurationManager in .Net Core](docs/config/how_to_use_config_manager.md)

  * [JSON format configuration file demo](demos/config_demo/JsonFileConfigDemo.cs)
    * [How to use a JSON format configuration file in .Net Core](docs/config/how_to_use_json_config_file.md)

  * [XML format configuration file demo](demos/config_demo/XmlFileConfigDemo.cs)
    * [How to use a XML format configuration file in .Net Core](docs/config/how_to_use_xml_config_file.md)

  * [INI format configuration file demo](demos/config_demo/IniFileConfigDemo.cs)
    * [How to use an INI format configuration file in .Net Core](docs/config/how_to_use_ini_config_file.md)

  * [In-memory configuration demo](demos/config_demo/InMemoryConfigDemo.cs)
    * [How to use In-memory configuration in .Net Core](docs/config/how_to_use_in_memory_config.md)

  * [Environment variables configuration demo](demos/config_demo/EnvironmentVariablesConfigDemo.cs)
    * [How to use environment variables configuration in .Net Core](docs/config/how_to_use_env_vars_config.md)

  * [Command line configuration demo](demos/config_demo/CommandLineConfigDemo.cs)
    * [How to use command line configuration in .Net Core](docs/config/how_to_use_cmd_line_config.md)

  * [User secrets configuration demo](demos/config_demo/UserSecretsConfigDemo.cs)
    * [How to use user secrets configuration in .Net Core](docs/config/how_to_use_user_secrets_config.md)

  * [Azure Key Vault configuration demo](demos/config_demo/AzureKeyVaultConfigDemo.cs)
    * [How to use Azure Key Vault configuration in .Net Core](docs/config/how_to_use_azure_key_vault_config.md)

* [Logging](demos/log_demo)

  This project contains following demo scenarios:

  * [Console logging demo](demos/logging_demo/ConsoleLogDemo.cs)
    * [How to use console log in .Net Core](docs/logging/how_to_use_console_log.md)

  * [Debug logging demo](demos/logging_demo/DebugLogDemo.cs)
    * [How to use debug log in .Net Core](docs/logging/how_to_use_debug_log.md)

  * [Trace Source logging demo](demos/logging_demo/TraceSourceLogDemo.cs)
    * [How to use trace source log in .Net Core](docs/logging/how_to_use_trace_source_log.md)

  * [LoggerMessage demo](demos/logging_demo/LoggerMessageDemo.cs)
    * [How to use LoggerMessage in .Net Core](docs/logging/how_to_use_logger_message.md)

* [Database](demos/database_demo)

  This project contains following demo scenarios:

  * [Entity Framework Core for Sqlite demo](demos/database_demo/EntityFrameworkSqliteDemo.cs)
    * [How to use Entity Framework Core to access Sqlite database in .Net Core](docs/database/how_to_use_ef_sqlite.md)

  * [Entity Framework Core with in-memory database provider demo](demos/database_demo/EntityFrameworkSqliteDemo.cs)
    * [How to use Entity Framework Core with in-memory database provider in .Net Core](docs/database/how_to_use_ef_in_memory.md)

  * [Entity Framework Core with Sqlite database in-memory mode demo](demos/database_demo/EntityFrameworkSqliteInMemoryDemo.cs)
    * [How to use Entity Framework Core with Sqlite database in-memory mode in .Net Core](docs/database/how_to_use_ef_sqlite_in_memory.md)

* [Test](demos/test_demo)

  This project contains following demo scenarios:

  * [MSTest demo](demos/test_demo/MSTestDemo.cs)
    * [How to use MSTest in .Net Core](docs/test/how_to_use_mstest.md)

  * [xUnit demo](demos/test_demo/XUnitDemo.cs)
    * [How to use xUnit in .Net Core](docs/test/how_to_use_xunit.md)

  * [NUnit demo](demos/test_demo/NUnitDemo.cs)
    * [How to use NUnit in .Net Core](docs/test/how_to_use_nunit.md)

* [Dependency Injection](demos/di_demo)

  This project contains following demo scenarios:

  * [Unity demo](demos/di_demo/UnityDemo.cs)
    * [How to use Unity in .Net Core](docs/di/how_to_use_unity.md)

## Cheat Sheets

* [dotnet cli](cheatsheets/dotnet_cli.sh)

  The *dotnet cli* cheatsheets.

* [git cli](cheatsheets/git_cli.sh)

  The *git cli* cheatsheets.

## References

* [.NET Core Guide (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/core/index)
* [.NET API Browser (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/index?view=netcore-2.0)
* [.NET Core Home Repository (github.com)](https://github.com/dotnet/core)
* [.NET Core Blog (blogs.msdn.microsoft.com)](https://blogs.msdn.microsoft.com/dotnet/tag/net-core/)
* [Using .NET Core in Visual Studio Code (code.visualstudio.com)](https://code.visualstudio.com/docs/other/dotnet)