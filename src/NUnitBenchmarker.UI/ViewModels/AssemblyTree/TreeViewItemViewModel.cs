using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace NUnitBenchmarker.UI.ViewModels.AssemblyTree
{
	/// <summary>
	///     Foundation class for all ViewModel classes displayed by TreeViewItems.
	///     Uses mediator / adapter design pattern between domain model and TreeViewItem.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TreeViewItemViewModel<T> : ViewModelBase
	{
		#region Constants and Fields

		/// <summary>
		///     The data
		/// </summary>
		protected T data;
		/// <summary>
		///     The dummy child
		/// </summary>
		private static readonly TreeViewItemViewModel<T> DummyChild = new TreeViewItemViewModel<T>();

		/// <summary>
		///     The parent of the node
		/// </summary>
		private readonly TreeViewItemViewModel<T> parent;
		/// <summary>
		///     The children of the node
		/// </summary>
		private ObservableCollection<TreeViewItemViewModel<T>> children;

		/// <summary>
		///     Is the node expanded?
		/// </summary>
		private bool isExpanded;
		/// <summary>
		///     The is selected
		/// </summary>
		private bool isSelected;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Initializes a new instance of the <see cref="TreeViewItemViewModel{T}" /> class.
		/// </summary>
		/// <param name="data">The data of the node.</param>
		/// <param name="parent">The parent of the node.</param>
		/// <param name="lazyLoadChildren">
		///     if set to <c>true</c> [lazy load children].
		/// </param>
		protected TreeViewItemViewModel(T data, TreeViewItemViewModel<T> parent, bool lazyLoadChildren)
		{
			this.data = data;
			this.parent = parent;
			
			children = new ObservableCollection<TreeViewItemViewModel<T>>();

			if (lazyLoadChildren)
			{
				children.Add(DummyChild);
			}
		}

		// This is used to create the DummyChild instance.
		/// <summary>
		///     Prevents a default instance of the <see cref="TreeViewItemViewModel{T}" /> class from being created.
		/// </summary>
		private TreeViewItemViewModel()
		{
		}

		#endregion

		#region Public Events

		/// <summary>
		///     Occurs when [selected item changed] in the tree.
		/// </summary>
		public static event Action<TreeViewItemViewModel<T>> SelectedItemChanged;

		#endregion

		#region Public Properties

		/// <summary>
		///     Returns the child items of this node.
		/// </summary>
		/// <value>The children.</value>
		public ObservableCollection<TreeViewItemViewModel<T>> Children
		{
			get
			{
				return children;
			}
			set
			{
				children = value;
				RaisePropertyChanged(() => Children);
			}
		}
		/// <summary>
		///     Gets or sets the node data.
		/// </summary>
		/// <value>The data.</value>
		virtual public T Data
		{
			get
			{
				return data;
			}
			set
			{
				data = value;
				RaisePropertyChanged(() => Data);
			}
		}

		/// <summary>
		///     Returns true if this object's Children have not yet been populated.
		/// </summary>
		/// <value>
		///     <c>true</c> if this instance has dummy child; otherwise, <c>false</c>.
		/// </value>
		public bool HasDummyChild
		{
			get
			{
				return Children.Count == 1 && Children[0] == DummyChild;
			}
		}

		/// <summary>
		///     Gets/sets whether the TreeViewItem
		///     associated with this object is expanded.
		/// </summary>
		/// <value>
		///     <c>true</c> if this instance is expanded; otherwise, <c>false</c>.
		/// </value>
		public bool IsExpanded
		{
			get
			{
				return isExpanded;
			}
			set
			{
				if (value != isExpanded)
				{
					isExpanded = value;
					RaisePropertyChanged(() => IsExpanded);
				}
				if (!value)
				{
					return;
				}

				// Expand all the way up to the root.
				if (isExpanded && parent != null)
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
		}

		/// <summary>
		///     Gets/sets whether the TreeViewItem
		///     associated with this object is selected.
		/// </summary>
		/// <value>
		///     <c>true</c> if this instance is selected; otherwise, <c>false</c>.
		/// </value>
		public bool IsSelected
		{
			get
			{
				return isSelected;
			}
			set
			{
				if (value != isSelected)
				{
					isSelected = value;
					RaisePropertyChanged(() => IsSelected);
					if (IsSelected)
					{
						OnSelectedItemChanged();
					}
				}
			}
		}
		private bool isChecked; // Backing field for property IsChecked
		/// <summary>
		/// Observable property for MVVM. Gets or sets state IsChecked. 
		/// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
		/// </summary>
		/// <value>The property value. If the new value is the same as the current property value
		/// then no PropertyChange event is raised.
		/// </value>
		public bool IsChecked
		{
			get { return isChecked; }

			set
			{
				if (isChecked == value)
				{
					return;
				}
				isChecked = value;
				if (Children == null)
				{
					return;
				}
				foreach (var node in Children)
				{
					node.IsChecked = value;
				}
				RaisePropertyChanged(() => IsChecked);
			}
		}


		/// <summary>
		///     Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public TreeViewItemViewModel<T> Parent
		{
			get
			{
				return parent;
			}
		}
		/// <summary>
		///     Gets the root.
		/// </summary>
		/// <value>The root.</value>
		public TreeViewItemViewModel<T> Root
		{
			get
			{
				TreeViewItemViewModel<T> result = this;
				while (result.Parent != null)
				{
					result = result.Parent;
				}
				return result;
			}
		}

		#endregion

		#region Public Methods and Operators

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
				children.Add(DummyChild);
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


		#endregion

		#region Methods

		/// <summary>
		///     Invoked when the child items need to be loaded on demand.
		///     Subclasses can override this to populate the Children collection.
		/// </summary>
		public virtual void LoadChildren(Action<object> completion = null)
		{
		}

		/// <summary>
		///     Called when [selected item changed].
		/// </summary>
		protected virtual void OnSelectedItemChanged()
		{
			Action<TreeViewItemViewModel<T>> handler = SelectedItemChanged;
			if (handler != null)
			{
				handler(this);
			}
		}



		#endregion
	}
}