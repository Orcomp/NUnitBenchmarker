// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionEntry.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Models
{
    using System.Collections.Generic;
    using Catel.Data;

    public abstract class ReflectionEntry : ModelBase
    {
        private readonly List<ReflectionEntry> _children;
        private string _filter;

        public ReflectionEntry(IEnumerable<ReflectionEntry> children = null)
        {
            _children = new List<ReflectionEntry>(children ?? new ReflectionEntry[] { });
            Children = new List<ReflectionEntry>(_children);
        }

        #region Properties
        public string Path { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool LeafEntry { get; set; }
        public bool IsChecked { get; set; }

        public List<ReflectionEntry> Children { get; private set; }
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

        public bool ApplyFilter(string filter)
        {
            bool filterApplies = false;

            var finalFilter = filter.PrepareAsSearchFilter();

            var lowerName = Name.ToLower();
            if (lowerName.Contains(finalFilter))
            {
                filterApplies = true;
            }

            var children = new List<ReflectionEntry>();

            foreach (var child in _children)
            {
                if (child.ApplyFilter(finalFilter))
                {
                    filterApplies = true;

                    children.Add(child);
                }
            }

            Children = children;

            return filterApplies;
        }
        #endregion
    }
}