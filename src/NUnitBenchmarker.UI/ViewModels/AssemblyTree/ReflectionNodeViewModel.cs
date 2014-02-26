using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using NUnitBenchmarker.UI.Model;

namespace NUnitBenchmarker.UI.ViewModels.AssemblyTree
{
	/// <summary>
	///     Foundation class for all ViewModel classes displayed by TreeViewItems.
	///     Uses mediator / adapter design pattern between domain model and TreeViewItem.
	/// </summary>
	public class ReflectionNodeViewModel : TreeViewItemViewModel<ReflectionEntry>
	{
		#region Constants and Fields
	
		private ImageSource image;
		private int imageHeight;
		private Thickness imageMargin;
		private int imageWidth;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Initializes a new instance of the <see cref="ReflectionNodeViewModel" /> class.
		/// </summary>
		/// <param name="data">The data of the node.</param>
		/// <param name="parent">The parent of the node.</param>
		/// <param name="lazyLoadChildren">
		///     if set to <c>true</c> [lazy load children].
		/// </param>
		public ReflectionNodeViewModel(
			ReflectionEntry data,
			TreeViewItemViewModel<ReflectionEntry> parent,
			bool lazyLoadChildren = true
			)
			: base(data, parent, lazyLoadChildren)
		{
			IsChecked = true;
		}

		#endregion

		/// <summary>
		/// Raised when this tab should be removed from the UI.
		/// </summary>
		public event EventHandler RequestRemove;

		protected virtual void OnRequestRemove()
		{
			EventHandler handler = RequestRemove;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		private ICommand removeCommand;

		/// <summary>
		///     Gets the Remove command for MVVM binding.
		/// </summary>
		/// <value>The Remove command.</value>
		public ICommand RemoveCommand
		{
			get { return removeCommand ?? (removeCommand = new RelayCommand<object>(RemoveAction, o => data is AssemblyEntry)); }
		}

		/// <summary>
		///     Remove event handler.
		/// </summary>
		/// <param name="dummy">not used here</param>
		private void RemoveAction(object dummy)
		{
			OnRequestRemove();
		}




		#region Public Properties

		/// <summary>
		///     Gets the image.
		/// </summary>
		/// <value>The image.</value>
		public ImageSource Image
		{
			get
			{
				if (image != null)
				{
					return image;
				}
				if (data.UseIcons)
				{
					// image = new 
				}
				int oldImageWidth = imageWidth;
				if (image == null)
				{
					imageWidth = 0;
					imageHeight = 0;
					imageMargin = new Thickness(0, 0, 0, 0);
				}
				else
				{
					imageWidth = 16;
					imageHeight = 16;
					imageMargin = new Thickness(2, 0, 0, 0);
				}
				bool imageChanged = oldImageWidth != imageWidth;

				if (imageChanged)
				{
					RaisePropertyChanged(() => ImageWidth);
					RaisePropertyChanged(() => ImageHeight);
					RaisePropertyChanged(() => ImageMargin);
				}
				return image;
			}
		}

		/// <summary>
		///     Gets the height of the image if any.
		/// </summary>
		/// <value>The height of the image.</value>
		public int ImageHeight
		{
			get
			{
				return imageHeight;
			}
		}

		/// <summary>
		///     Gets the image margin if any.
		/// </summary>
		/// <value>The image margin.</value>
		public Thickness ImageMargin
		{
			get
			{
				return imageMargin;
			}
		}
		/// <summary>
		///     Gets the width of the image if any.
		/// </summary>
		/// <value>The width of the image.</value>
		public int ImageWidth
		{
			get
			{
				return imageWidth;
			}
		}

		/// <summary>
		///     Gets the name of the node.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return data.Name;
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Clears the specified lazy load children of the node.
		/// </summary>
		/// <param name="lazyLoadChildren">
		///     if set to <c>true</c> [lazy load children].
		/// </param>
		public override void Clear(bool lazyLoadChildren = true)
		{
			base.Clear(lazyLoadChildren);
			RaisePropertyChanged(() => Name);
		}

		public override ReflectionEntry Data
		{
			get
			{
				return base.Data;
			}
			set
			{
				base.Data = value;
				RaisePropertyChanged(() => Name);
				RaisePropertyChanged(() => Children);
			}
		}

		public string ToolTip
		{
			get { return data.Description; }
		}


		#endregion

		#region Methods

		/// <summary>
		///     Loads the children of the node.
		/// </summary>
		public override void LoadChildren(Action<object> completion = null)
		{
			var children = data.GetChildren();
			foreach (var reflectionEntry in children)
			{
				var child = new ReflectionNodeViewModel(reflectionEntry, this, !reflectionEntry.LeafEntry)
				{
					IsChecked = IsChecked
				};
				Children.Add(child);
			}


			if (completion != null)
			{
				completion(this);
			}
		}

		#endregion
	}
}
