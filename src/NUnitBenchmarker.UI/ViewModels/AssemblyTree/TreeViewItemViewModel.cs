// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeViewItemViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.ViewModels.AssemblyTree
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Catel.MVVM;

    /// <summary>
    /// Foundation class for all ViewModel classes displayed by TreeViewItems.
    /// Uses mediator / adapter design pattern between domain model and TreeViewItem.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeViewItemViewModel<T> : ViewModelBase
    {
        #region Constants
        /// <summary>
        /// The dummy child
        /// </summary>
        private static readonly TreeViewItemViewModel<T> DummyChild = new TreeViewItemViewModel<T>();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemViewModel{T}" /> class.
        /// </summary>
        /// <param name="data">The data of the node.</param>
        /// <param name="parent">The parent of the node.</param>
        /// <param name="lazyLoadChildren">if set to <c>true</c> [lazy load children].</param>
        protected TreeViewItemViewModel(T data, TreeViewItemViewModel<T> parent, bool lazyLoadChildren)
        {
            Data = data;
            Parent = parent;

            Children = new ObservableCollection<TreeViewItemViewModel<T>>();

            if (lazyLoadChildren)
            {
                Children.Add(DummyChild);
            }
        }

        private TreeViewItemViewModel()
        {
        }
        #endregion

        #region Properties
        public ObservableCollection<TreeViewItemViewModel<T>> Children { get; set; }

        public virtual T Data { get; set; }

        public bool HasDummyChild
        {
            get { return Children.Count == 1 && Children[0] == DummyChild; }
        }

        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }

        public bool IsChecked { get; set; }

        public TreeViewItemViewModel<T> Parent { get; private set; }

        public TreeViewItemViewModel<T> Root
        {
            get
            {
                var result = this;

                while (result.Parent != null)
                {
                    result = result.Parent;
                }

                return result;
            }
        }
        #endregion

        #region Events
        public static event Action<TreeViewItemViewModel<T>> SelectedItemChanged;
        #endregion

        #region Methods
        // Note: automatically wired by Catel.Fody
        private void OnIsExpandedChanged()
        {
            // Expand all the way up to the root.
            var parent = Parent;
            if (IsExpanded && parent != null)
            {
                parent.IsExpanded = true;
            }

            // Lazy load the child items, if necessary.
            if (HasDummyChild)
            {
                Children.Remove(DummyChild);
                LoadChildren();
            }
        }

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

        /// <summary>
        ///     Clears the specified lazy load children.
        /// </summary>
        /// <param name="lazyLoadChildren">
        ///     if set to <c>true</c> [lazy load children].
        /// </param>
        public virtual void Clear(bool lazyLoadChildren = true)
        {
            if (HasDummyChild)
            {
                return;
            }

            foreach (var treeViewItemViewModel in Children)
            {
                treeViewItemViewModel.Clear(lazyLoadChildren);
            }

            Children = new ObservableCollection<TreeViewItemViewModel<T>>();
            if (lazyLoadChildren)
            {
                Children.Add(DummyChild);
            }

            IsExpanded = false;
        }

        public virtual IEnumerable<T> GetChildrenData(IList<T> result = null, bool onlyChecked = true)
        {
            if (result == null)
            {
                result = new List<T>();
            }

            if (!onlyChecked || IsChecked)
            {
                result.Add(Data);
            }

            if (HasDummyChild)
            {
                Children.Remove(DummyChild);
                LoadChildren();
            }

            foreach (var child in Children)
            {
                child.GetChildrenData(result, onlyChecked);
            }

            return result;
        }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        public virtual void LoadChildren(Action<object> completion = null)
        {
        }

        /// <summary>
        /// Called when [selected item changed].
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
            var handler = SelectedItemChanged;
            if (handler != null)
            {
                handler(this);
            }
        }
        #endregion
    }
}