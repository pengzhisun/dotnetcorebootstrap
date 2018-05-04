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
 *              https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationsection
 *              https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationelement
 *              https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/
 *****************************************************************************/

namespace DotNetCoreBootstrap.ConfigDemo
{
    using System;
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// Defines the ConfigurationManager demo class.
    /// </summary>
    public sealed class ConfigurationManagerDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            ExeConfigurationFileMap fileMap =
                new ExeConfigurationFileMap
                {
                    ExeConfigFilename = "appsettings.config"
                };

            Configuration configuration =
                ConfigurationManager.OpenMappedExeConfiguration(
                    fileMap,
                    ConfigurationUserLevel.None);

            string appSettingKey = @"str_setting_1";
            string appSettingValue =
                configuration.AppSettings.Settings[appSettingKey].Value;
            Console.WriteLine($"app setting '{appSettingKey}': '{appSettingValue}'");

            foreach (DemoConfigurationSection demoSection in
                configuration.Sections.OfType<DemoConfigurationSection>())
            {
                Console.WriteLine($"Demo section name: '{demoSection.SectionInformation.SectionName}', description: '{demoSection.SectionDescription}'");
                Console.WriteLine($"Demo element name: '{demoSection.DemoElement.Name}', description: '{demoSection.DemoElement.Value}'");
                foreach (DemoConfigurationElement demoElement in
                    demoSection.DemoElementsCollection)
                {
                    Console.WriteLine($"Demo sub element name: '{demoElement.Name}', description: '{demoElement.Value}'");
                }
            }
        }

        public sealed class DemoConfigurationSection : ConfigurationSection
        {
            private const string SectionDescriptionPropertyName = @"sectionDescription";

            private const string DemoElementPropertyName = @"demoElement";

            private const string DemoElementsCollectionPropertyName = @"demoElementsCollection";

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

        public sealed class DemoConfigurationElement : ConfigurationElement
        {
            private const string NamePropertyName = @"name";
            private const string ValuePropertyName = @"value";

            public DemoConfigurationElement()
            {
            }

            public DemoConfigurationElement(string name)
            {
                this.Name = name;
            }

            public DemoConfigurationElement(string name, string value)
            {
                this.Name = name;
                this.Value = value;
            }

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

        public sealed class DemoConfigurationElementCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new DemoConfigurationElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return (element as DemoConfigurationElement)?.Name;
            }
        }
    }
}