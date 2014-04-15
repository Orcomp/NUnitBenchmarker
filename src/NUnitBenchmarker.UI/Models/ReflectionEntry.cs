// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionEntry.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Catel.Data;

    public abstract class ReflectionEntry : ModelBase
    {
        public ReflectionEntry(IEnumerable<ReflectionEntry> children = null)
        {
            Children = new ObservableCollection<ReflectionEntry>(children ?? new ReflectionEntry[] { });
        }

        #region Properties
        public string Path { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool UseIcons { get; set; }
        public bool LeafEntry { get; set; }

        public bool IsChecked { get; set; }

        [ExcludeFromValidation]
        public ReflectionEntry Parent { get; set; }

        public ObservableCollection<ReflectionEntry> Children { get; private set; }
        #endregion

        #region Methods
        // Note: automatically wired by Catel.Fody
        private void OnIsCheckedChanged()
        {
            if (Children != null)
            {
                foreach (var node in Children)
                {
                    node.IsChecked = IsChecked;
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}