using System;
using System.Configuration;

namespace NUnitBenchmarker.Benchmark.Configuration
{
	public class NUnitBenchmarkerConfigurationSection : ConfigurationSection
	{
		public string ConfigFile { get; set; }
		
		private const string DisplayUIAttributeName = "displayUI";
		[ConfigurationProperty(DisplayUIAttributeName, DefaultValue = "false", IsRequired = false)]
		public Boolean DisplayUI
		{
			get
			{
				return (Boolean)this[DisplayUIAttributeName];
			}
			set
			{
				this[DisplayUIAttributeName] = value;
			}
		}

		private const string SearchFoldersElementName = "SearchFolders";
		[ConfigurationProperty(SearchFoldersElementName)]
		public SearchFolderCollection SearchFolders
		{
			get
			{
				return ((SearchFolderCollection)(this[SearchFoldersElementName]));
			}
		}

		private const string ImplementationFiltersElementName = "ImplementationFilters";
		[ConfigurationProperty(ImplementationFiltersElementName)]
		public ExcludeIncludeCollection ImplementationFilters
		{
			get { return ((ExcludeIncludeCollection)(base[ImplementationFiltersElementName])); }
		}

		private const string TestCaseFiltersElementName = "TestCaseFilters";
		[ConfigurationProperty(TestCaseFiltersElementName)]
		public ExcludeIncludeCollection TestCaseFilters
		{
			get { return ((ExcludeIncludeCollection)(base[TestCaseFiltersElementName])); }
		}
	}

	[ConfigurationCollection(typeof(SearchFolder))]
	public class SearchFolderCollection : ConfigurationElementCollection
	{

		protected override ConfigurationElement CreateNewElement()
		{
			return new SearchFolder();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SearchFolder)(element)).Folder;
		}

		public void Add(SearchFolder searchFolder)
		{
			BaseAdd(searchFolder);
		}


		public SearchFolder this[int idx]
		{
			get
			{
				return (SearchFolder)BaseGet(idx);
			}
		}
	}

	[ConfigurationCollection(typeof(ExcludeIncludeElement))]
	public class ExcludeIncludeCollection : ConfigurationElementCollection
	{

		protected override ConfigurationElement CreateNewElement()
		{
			return new ExcludeIncludeElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ExcludeIncludeElement)(element)).Exclude + ((ExcludeIncludeElement)(element)).Include;
		}

		public void Add(ExcludeIncludeElement item)
		{
			base.BaseAdd(item);
		}

		public ExcludeIncludeElement this[int idx]
		{
			get
			{
				return (ExcludeIncludeElement)BaseGet(idx);
			}
		}
	}	
	
	public class SearchFolder : ConfigurationElement
	{
		private const string ExcludeAttributeName = "Exclude";
		private const string IncludeAttributeName = "Include";
		private const string FolderAttributeName = "Folder";
		
		[ConfigurationProperty(FolderAttributeName, IsKey = true, IsRequired = true)]
		// MinLength has a known issue [StringValidator(InvalidCharacters = "<>|?*/", MinLength=1, MaxLength = 255)]
		[StringValidator(InvalidCharacters = "<>|?*/", MaxLength = 255)]
		public string Folder
		{
			get
			{
				return ((string)(this[FolderAttributeName]));
			}
			set
			{
				this[FolderAttributeName] = value;
			}
		}

		[ConfigurationProperty(ExcludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
		public string Exclude
		{
			get
			{
				return ((string)(this[ExcludeAttributeName]));
			}
			set
			{
				this[ExcludeAttributeName] = value;
			}
		}

		[ConfigurationProperty(IncludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
		public string Include
		{
			get
			{
				return ((string)(this[IncludeAttributeName]));
			}
			set
			{
				this[IncludeAttributeName] = value;
			}
		}

	}

	public class ExcludeIncludeElement : ConfigurationElement
	{
		private const string ExcludeAttributeName = "Exclude";
		private const string IncludeAttributeName = "Include";

		[ConfigurationProperty(ExcludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
		public string Exclude
		{
			get
			{
				return ((string)(this[ExcludeAttributeName]));
			}
			set
			{
				this[ExcludeAttributeName] = value;
			}
		}

		[ConfigurationProperty(IncludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
		public string Include
		{
			get
			{
				return ((string)(this[IncludeAttributeName]));
			}
			set
			{
				this[IncludeAttributeName] = value;
			}
		}
	}

}
