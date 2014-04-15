// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionEntry.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Catel.Data;

    public abstract class ReflectionEntry : ModelBase
    {
        private List<ReflectionEntry> _children;
        private string _filter;
        private bool _isUpdatingCheckState;

        public ReflectionEntry(ReflectionEntry parent)
        {
            Parent = parent;
        }

        #region Properties
        public string Path { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool? IsChecked { get; set; }

        public ReflectionEntry Parent { get; private set; }
        public List<ReflectionEntry> Children { get; private set; }
        #endregion

        #region Methods
        public void InitializeChildren(IEnumerable<ReflectionEntry> children)
        {
            if (_children != null)
            {
                throw new InvalidOperationException("Children are already initialized");
            }

            _children = new List<ReflectionEntry>(children);

            foreach (var child in _children)
            {
                child.PropertyChanged += OnChildPropertyChanged;
            }

            Children = new List<ReflectionEntry>(_children);
        }

        private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_isUpdatingCheckState)
            {
                return;
            }

            _isUpdatingCheckState = true;

            IsChecked = IsAnyChildChecked();

            _isUpdatingCheckState = false;
        }

        private bool? IsAnyChildChecked()
        {
            bool allChecked = true;
            bool noneChecked = true;

            foreach (var child in _children)
            {
                var childChecked = child.IsChecked ?? true;
                if (childChecked)
                {
                    noneChecked = false;
                }
                else
                {
                    allChecked = false;
                }
            }

            if (allChecked)
            {
                return true;
            }

            if (noneChecked)
            {
                return false;
            }

            return null;
        }

        // Note: automatically wired by Catel.Fody
        private void OnIsCheckedChanged()
        {
            if (_isUpdatingCheckState)
            {
                return;
            }

            if (!IsChecked.HasValue)
            {
                return;
            }

            _isUpdatingCheckState = true;

            if (_children != null)
            {
                foreach (var node in _children)
                {
                    node.IsChecked = IsChecked;
                }
            }

            _isUpdatingCheckState = false;
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

            if (_children != null)
            {
                foreach (var child in _children)
                {
                    if (child.ApplyFilter(finalFilter))
                    {
                        filterApplies = true;

                        children.Add(child);
                    }
                }
            }

            Children = children;

            return filterApplies;
        }
        #endregion
    }
}