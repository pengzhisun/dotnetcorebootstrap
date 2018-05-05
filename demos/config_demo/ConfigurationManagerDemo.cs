/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   ConfigurationManagerDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core ConfigurationManager demos.
 * Reference:   https://www.nuget.org/packages/System.Configuration.ConfigurationManager/
 *              https://github.com/dotnet/corefx/tree/master/src/System.Configuration.ConfigurationManager
 *              https://docs.microsoft.com/en-us/dotnet/api/system.configuration
 *              https://docs.microsoft.com/zh-cn/dotnet/api/system.configuration.configurationmanager
 *              https://docs.microsoft.com/zh-cn/dotnet/api/system.configuration.configurationmanager.openmappedexeconfiguration
 *              https://docs.microsoft.com/zh-cn/dotnet/api/system.configuration.configurationsectiongroup
 *              https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationsection
 *              https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationelement
 *              https://docs.microsoft.com/zh-cn/dotnet/api/system.configuration.configurationelementcollection
 *              https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/
 *****************************************************************************/

namespace DotNetCoreBootstrap.ConfigDemo
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines the ConfigurationManager demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// System.Configuration.ConfigurationManager
    /// </remarks>
    public sealed class ConfigurationManagerDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            // Load configuration from file
            ExeConfigurationFileMap fileMap =
                new ExeConfigurationFileMap
                {
                    ExeConfigFilename = "appsettings.config"
                };
            Configuration configuration =
                ConfigurationManager.OpenMappedExeConfiguration(
                    fileMap,
                    ConfigurationUserLevel.None);

            // Get app settings by key
            string appSettingKey = @"str_setting_1";
            string appSettingValue =
                configuration.AppSettings.Settings[appSettingKey].Value;
            Console.WriteLine($"app setting '{appSettingKey}': '{appSettingValue}'");

            // Get demo configuration sections.
            DemoConfigurationSection demoSection1 =
                (DemoConfigurationSection)configuration.GetSection("demoSection1");

            // get section name
            string demoSectionName1 = demoSection1.SectionInformation.SectionName;

            // get section property
            string demoSectionDescription1 = demoSection1.SectionDescription;
            Console.WriteLine($"Demo section name: '{demoSectionName1}', description: '{demoSectionDescription1}'");

            // get sub element inside the section
            DemoConfigurationElement demoElement1 = demoSection1.DemoElement;
            string demoElementName1 = demoElement1.Name;
            string demoElementValue1 = demoElement1.Value;
            Console.WriteLine($"Demo element name: '{demoElementName1}', description: '{demoElementValue1}'");

            DemoConfigurationSection demoSection2 =
                (DemoConfigurationSection)configuration.GetSection("demoSection2");
            string demoSectionName2 = demoSection2.SectionInformation.SectionName;
            string demoSectionDescription2 = demoSection2.SectionDescription;
            Console.WriteLine($"Demo section name: '{demoSectionName2}', description: '{demoSectionDescription2}'");

            // iterate sub elements collection inside the section
            foreach (DemoConfigurationElement subElement in
                    demoSection2.DemoElementsCollection)
            {
                string subElementName = subElement.Name;
                string subElementValue = subElement.Value;
                Console.WriteLine($"Demo sub element name: '{subElementName}', description: '{subElementValue}'");
            }

            // get demo configuration section group and inner section.
            DemoConfigurationSectionGroup demoSectionGroup =
                (DemoConfigurationSectionGroup)configuration.GetSectionGroup("demoSectionGroup");

            // get sub section inside the section group.
            DemoConfigurationSection demoSection3 = demoSectionGroup.DemoSection3;
            string demoSectionName3 = demoSection3.SectionInformation.SectionName;
            string demoSectionDescription3 = demoSection3.SectionDescription;
            Console.WriteLine($"Demo section name: '{demoSectionName3}', description: '{demoSectionDescription3}'");

            // print config file.
            Console.WriteLine();
            string appSettingsFilePath =
                Path.Combine(AppContext.BaseDirectory, "appsettings.config");
            Console.WriteLine($"[Trace] config file path: {appSettingsFilePath}");
            string appSettingsFileContent =
                File.ReadAllText(appSettingsFilePath, Encoding.UTF8);
            Console.WriteLine(appSettingsFileContent);
        }

        /// <summary>
        /// Defines the demo configuration section group.
        /// </summary>
        public sealed class DemoConfigurationSectionGroup : ConfigurationSectionGroup
        {
            /// <summary>
            /// Defines the demo section 3 property.
            /// </summary>
            public DemoConfigurationSection DemoSection3
            {
                get
                {
                    return (DemoConfigurationSection)this.Sections[@"demoSection3"];
                }
            }
        }

        /// <summary>
        /// Defiens the demo configuration section class.
        /// </summary>
        public sealed class DemoConfigurationSection : ConfigurationSection
        {
            /// <summary>
            /// Defines the section description property name.
            /// </summary>
            private const string SectionDescriptionPropertyName = @"sectionDescription";

            /// <summary>
            /// Defines the demo element property name.
            /// </summary>
            private const string DemoElementPropertyName = @"demoElement";

            /// <summary>
            /// Defines the demo elements collection property name.
            /// </summary>
            private const string DemoElementsCollectionPropertyName = @"demoElementsCollection";

            /// <summary>
            /// Gets or sets the section description property.
            /// </summary>
            [ConfigurationProperty(SectionDescriptionPropertyName,
                DefaultValue = "default_section_description",
                IsRequired = true,
                IsKey = false)]
            [RegexStringValidator(@"^\w+$")]
            public string SectionDescription
            {
                get
                {
                    return (string)this[SectionDescriptionPropertyName];
                }

                set
                {
                    this[SectionDescriptionPropertyName] = value;
                }
            }

            /// <summary>
            /// Gets or sets the demo element.
            /// </summary>
            [ConfigurationProperty(DemoElementPropertyName)]
            public DemoConfigurationElement DemoElement
            {
                get
                {
                    return (DemoConfigurationElement)this[DemoElementPropertyName];
                }

                set
                {
                    this[DemoElementPropertyName] = value;
                }
            }

            /// <summary>
            /// Gets or sets the demo elements collection.
            /// </summary>
            [ConfigurationProperty(
                DemoElementsCollectionPropertyName,
                IsDefaultCollection = false)]
            public DemoConfigurationElementCollection DemoElementsCollection
            {
                get
                {
                    return (DemoConfigurationElementCollection)this[DemoElementsCollectionPropertyName];
                }

                set
                {
                    this[DemoElementsCollectionPropertyName] = value;
                }
            }
        }

        /// <summary>
        /// Defines the demo configuration element.
        /// </summary>
        public sealed class DemoConfigurationElement : ConfigurationElement
        {
            /// <summary>
            /// Defines the name property name.
            /// </summary>
            private const string NamePropertyName = @"name";

            /// <summary>
            /// Defines the value property name.
            /// </summary>
            private const string ValuePropertyName = @"value";

            /// <summary>
            /// Gets or sets the name property.
            /// </summary>
            [ConfigurationProperty(NamePropertyName,
                DefaultValue = "default_name",
                IsRequired = true,
                IsKey = true)]
            public string Name
            {
                get
                {
                    return (string)this[NamePropertyName];
                }
                set
                {
                    this[NamePropertyName] = value;
                }
            }

            /// <summary>
            /// Gets or sets the value property.
            /// </summary>
            [ConfigurationProperty(ValuePropertyName,
                DefaultValue = null,
                IsRequired = true,
                IsKey = true)]
            public string Value
            {
                get
                {
                    return (string)this[ValuePropertyName];
                }
                set
                {
                    this[ValuePropertyName] = value;
                }
            }
        }

        /// <summary>
        /// Defines the demo configuration element collection.
        /// </summary>
        public sealed class DemoConfigurationElementCollection : ConfigurationElementCollection
        {
            /// <summary>
            /// Creates new configuration element instance.
            /// </summary>
            /// <returns>The configuration element instance.</returns>
            protected override ConfigurationElement CreateNewElement()
            {
                return new DemoConfigurationElement();
            }

            /// <summary>
            /// Gets the key of the given configuration element.
            /// </summary>
            /// <param name="element">The given configuration element.</param>
            /// <returns>The element key.</returns>
            protected override object GetElementKey(ConfigurationElement element)
            {
                return (element as DemoConfigurationElement)?.Name;
            }
        }
    }
}