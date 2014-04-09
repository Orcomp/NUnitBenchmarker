// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NUnitBenchmarkerConfigurationSection.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Configuration
{
    using System;
    using System.Configuration;

    public class NUnitBenchmarkerConfigurationSection : ConfigurationSection
    {
        #region Constants
        private const string DisplayUIAttributeName = "displayUI";

        private const string SearchFoldersElementName = "SearchFolders";

        private const string ImplementationFiltersElementName = "ImplementationFilters";

        private const string TestCaseFiltersElementName = "TestCaseFilters";
        #endregion

        #region Properties
        public string ConfigFile { get; set; }

        [ConfigurationProperty(DisplayUIAttributeName, DefaultValue = "false", IsRequired = false)]
        public Boolean DisplayUI
        {
            get { return (Boolean) this[DisplayUIAttributeName]; }
            set { this[DisplayUIAttributeName] = value; }
        }

        [ConfigurationProperty(SearchFoldersElementName)]
        public SearchFolderCollection SearchFolders
        {
            get { return ((SearchFolderCollection) (this[SearchFoldersElementName])); }
        }

        [ConfigurationProperty(ImplementationFiltersElementName)]
        public ExcludeIncludeCollection ImplementationFilters
        {
            get { return ((ExcludeIncludeCollection) (base[ImplementationFiltersElementName])); }
        }

        [ConfigurationProperty(TestCaseFiltersElementName)]
        public ExcludeIncludeCollection TestCaseFilters
        {
            get { return ((ExcludeIncludeCollection) (base[TestCaseFiltersElementName])); }
        }
        #endregion
    }

    [ConfigurationCollection(typeof (SearchFolder))]
    public class SearchFolderCollection : ConfigurationElementCollection
    {
        #region Properties
        public SearchFolder this[int idx]
        {
            get { return (SearchFolder) BaseGet(idx); }
        }
        #endregion

        #region Methods
        protected override ConfigurationElement CreateNewElement()
        {
            return new SearchFolder();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SearchFolder) (element)).Folder;
        }

        public void Add(SearchFolder searchFolder)
        {
            BaseAdd(searchFolder);
        }
        #endregion
    }

    [ConfigurationCollection(typeof (ExcludeIncludeElement))]
    public class ExcludeIncludeCollection : ConfigurationElementCollection
    {
        #region Properties
        public ExcludeIncludeElement this[int idx]
        {
            get { return (ExcludeIncludeElement) BaseGet(idx); }
        }
        #endregion

        #region Methods
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExcludeIncludeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExcludeIncludeElement) (element)).Exclude + ((ExcludeIncludeElement) (element)).Include;
        }

        public void Add(ExcludeIncludeElement item)
        {
            base.BaseAdd(item);
        }
        #endregion
    }

    public class SearchFolder : ConfigurationElement
    {
        #region Constants
        private const string ExcludeAttributeName = "Exclude";
        private const string IncludeAttributeName = "Include";
        private const string FolderAttributeName = "Folder";
        #endregion

        #region Properties
        [ConfigurationProperty(FolderAttributeName, IsKey = true, IsRequired = true)]
        // MinLength has a known issue [StringValidator(InvalidCharacters = "<>|?*/", MinLength=1, MaxLength = 255)]
        [StringValidator(InvalidCharacters = "<>|?*/", MaxLength = 255)]
        public string Folder
        {
            get { return ((string) (this[FolderAttributeName])); }
            set { this[FolderAttributeName] = value; }
        }

        [ConfigurationProperty(ExcludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Exclude
        {
            get { return ((string) (this[ExcludeAttributeName])); }
            set { this[ExcludeAttributeName] = value; }
        }

        [ConfigurationProperty(IncludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Include
        {
            get { return ((string) (this[IncludeAttributeName])); }
            set { this[IncludeAttributeName] = value; }
        }
        #endregion
    }

    public class ExcludeIncludeElement : ConfigurationElement
    {
        #region Constants
        private const string ExcludeAttributeName = "Exclude";
        private const string IncludeAttributeName = "Include";
        #endregion

        #region Properties
        [ConfigurationProperty(ExcludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Exclude
        {
            get { return ((string) (this[ExcludeAttributeName])); }
            set { this[ExcludeAttributeName] = value; }
        }

        [ConfigurationProperty(IncludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Include
        {
            get { return ((string) (this[IncludeAttributeName])); }
            set { this[IncludeAttributeName] = value; }
        }
        #endregion
    }
}