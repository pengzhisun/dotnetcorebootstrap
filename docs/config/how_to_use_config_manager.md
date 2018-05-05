# How to use ConfigurationManager in .Net Core

## Steps

1. Add Nuget package `System.Configuration.ConfigurationManager` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/config_demo/config_demo.csproj

    ```bash
    dotnet add {project} package System.Configuration.ConfigurationManager
    dotnet restore
    ```

2. Add `System.Configuration` namespace.

    > e.g. [ConfigurationManagerDemo.cs](../../demos/config_demo/ConfigurationManagerDemo.cs)
    ```csharp
    using System.Configuration;
    ```

3. Defines `ConfigurationSectionGroup`, `ConfigurationsSection`, `ConfigurationElement` and `ConfigurationElementCollection` for demo purpose.

    > e.g. [ConfigurationManagerDemo.cs](../../demos/config_demo/ConfigurationManagerDemo.cs)

    * `DemoConfigurationElement`

        ```csharp
        public sealed class DemoConfigurationElement : ConfigurationElement
        {
            [ConfigurationProperty("name", DefaultValue = "default_name", IsRequired = true, IsKey = true)]
            public string Name
            {
                get { return (string)this["name"]; }
                set { this["name"] = value; }
            }
            [ConfigurationProperty("value", DefaultValue = "default_value", IsRequired = true, IsKey = false)]
            public string Value
            {
                get { return (string)this["value"]; }
                set { this["value"] = value; }
            }
        }
        ```

    * `DemoConfigurationElementCollection`

        ```csharp
        public sealed class DemoConfigurationElementCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
                => new DemoConfigurationElement();

            protected override object GetElementKey(ConfigurationElement element)
                => (element as DemoConfigurationElement)?.Name;
        }
        ```

    * `DemoConfigurationSection`

        ```csharp
        public sealed class DemoConfigurationSection : ConfigurationSection
        {
            [ConfigurationProperty("sectionDescription", DefaultValue = "default_section_description", IsRequired = true, IsKey = false)]
            [RegexStringValidator(@"^\w+$")]
            public string SectionDescription
            {
                get { return (string)this["sectionDescription"]; }
                set { this["sectionDescription"] = value; }
            }
            [ConfigurationProperty("demoElement")]
            public DemoConfigurationElement DemoElement
            {
                get { return (DemoConfigurationElement)this["demoElement"]; }
                set { this["demoElement"] = value; }
            }
            [ConfigurationProperty("demoElementsCollection", IsDefaultCollection = false)]
            public DemoConfigurationElementCollection DemoElement
            {
                get { return (DemoConfigurationElementCollection)this["demoElementsCollection"]; }
                set { this["demoElementsCollection"] = value; }
            }
        }
        ```

    * `DemoConfigurationSectionGroup`

        ```csharp
        public sealed class DemoConfigurationSectionGroup : ConfigurationSectionGroup
        {
            public DemoConfigurationSection DemoSection3 =>
                (DemoConfigurationSection)this.Sections[@"demoSection3"];
        }
        ```

4. Create `.config` file based on previous configuration definitions.

   > e.g. [appsettings.config](../../demos/config_demo/appsettings.config)

    ```xml
    <configuration>
        <configSections>
            <section name="demoSection1" type="DotNetCoreBootstrap.ConfigDemo.ConfigurationManagerDemo+DemoConfigurationSection, config_demo" />
            <section name="demoSection2" type="DotNetCoreBootstrap.ConfigDemo.ConfigurationManagerDemo+DemoConfigurationSection, config_demo" />
            <sectionGroup name="demoSectionGroup" type="DotNetCoreBootstrap.ConfigDemo.ConfigurationManagerDemo+DemoConfigurationSectionGroup, config_demo">
                <section name="demoSection3" type="DotNetCoreBootstrap.ConfigDemo.ConfigurationManagerDemo+DemoConfigurationSection, config_demo" />
            </sectionGroup>
        </configSections>
        <appSettings>
            <add key="str_setting_1" value="str_value_1"/>
        </appSettings>
        <demoSection1 sectionDescription="demo_section_description_1">
            <demoElement name="demo_element_name_1" value="demo_element_value_1" />
        </demoSection1>
        <demoSection2 sectionDescription="demo_section_description_2">
            <demoElementsCollection>
                <add name="demo_sub_element_name_2_1" value="demo_element_value_2_1" />
                <add name="demo_sub_element_name_2_2" value="demo_element_value_2_2" />
            </demoElementsCollection>
        </demoSection2>
        <demoSectionGroup>
            <demoSection3 sectionDescription="demo_section_description_3"/>
        </demoSectionGroup>
    </configuration>
    ```

5. Add `.config` file to `.csproj` file.

    > e.g. [config_demo.csproj](../../demos/config_demo/config_demo.csproj)
    ```xml
    <Project Sdk="Microsoft.NET.Sdk">
        ...
        <ItemGroup>
            <Content Include="appsettings.config">
                <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
            </Content>
        </ItemGroup>
    </Project>
    ```

6. Load configuration from `.config` file.

    > e.g. [ConfigurationManagerDemo.cs](../../demos/config_demo/ConfigurationManagerDemo.cs)
    ```csharp
    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = "appsettings.config"
        };
    Configuration configuration =
        ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
    ```

7. Get app setting value.

    > e.g. [ConfigurationManagerDemo.cs](../../demos/config_demo/ConfigurationManagerDemo.cs)

    ```csharp
    string appSettingValue =
        configuration.AppSettings.Settings["str_setting_1"].Value;
    ```

8. Get section and inner properties.

    > e.g. [ConfigurationManagerDemo.cs](../../demos/config_demo/ConfigurationManagerDemo.cs)

    * Get section name and section property.

        ```csharp
            DemoConfigurationSection demoSection1 =
                (DemoConfigurationSection)configuration.GetSection("demoSection1");
            string demoSectionName1 = demoSection1.SectionInformation.SectionName;
            string demoSectionDescription1 = demoSection1.SectionDescription;
        ```

    * Get inner configuration element property.

        ```csharp
            DemoConfigurationSection demoSection1 =
                (DemoConfigurationSection)configuration.GetSection("demoSection1");
            DemoConfigurationElement demoElement1 = demoSection1.DemoElement;
            string demoElementName1 = demoElement1.Name;
            string demoElementValue1 = demoElement1.Value;
        ```

    * Get inner configuration element collection property.

        ```csharp
            DemoConfigurationSection demoSection2 =
                (DemoConfigurationSection)configuration.GetSection("demoSection2");
            foreach (DemoConfigurationElement subElement in demoSection2.DemoElementsCollection)
            {
                string subElementName = subElement.Name;
                string subElementValue = subElement.Value;
            }
        ```

9. Get section group and sub section.

    ```csharp
    DemoConfigurationSectionGroup demoSectionGroup =
        (DemoConfigurationSectionGroup)configuration.GetSectionGroup("demoSectionGroup");

    DemoConfigurationSection demoSection3 = demoSectionGroup.DemoSection3;
    string demoSectionName3 = demoSection3.SectionInformation.SectionName;
    string demoSectionDescription3 = demoSection3.SectionDescription;
    ```

## References

* [System.Configuration Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/system.configuration)
* [System.Configuration.ConfigurationManager (nuget.org)](https://www.nuget.org/packages/System.Configuration.ConfigurationManager)
* [System.Configuration.ConfigurationManager (github.com)](https://github.com/dotnet/corefx/tree/master/src/System.Configuration.ConfigurationManager)
* [.NET Reference Source (github.com)](https://github.com/Microsoft/referencesource)
* [Configuration file schema for the .NET Framework (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/)