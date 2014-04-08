// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionNodeViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.ViewModels.AssemblyTree
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel;
    using Catel.MVVM;
    using NUnitBenchmarker.UI.Model;

    /// <summary>
    ///     Foundation class for all ViewModel classes displayed by TreeViewItems.
    ///     Uses mediator / adapter design pattern between domain model and TreeViewItem.
    /// </summary>
    public class ReflectionNodeViewModel : TreeViewItemViewModel<ReflectionEntry>
    {
        #region Fields
        private ImageSource image;
        private int imageHeight;
        private Thickness imageMargin;
        private int imageWidth;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="ReflectionNodeViewModel" /> class.
        /// </summary>
        /// <param name="data">The data of the node.</param>
        /// <param name="parent">The parent of the node.</param>
        /// <param name="lazyLoadChildren">
        ///     if set to <c>true</c> [lazy load children].
        /// </param>
        public ReflectionNodeViewModel(ReflectionEntry data, TreeViewItemViewModel<ReflectionEntry> parent, bool lazyLoadChildren = true)
            : base(data, parent, lazyLoadChildren)
        {
            IsChecked = true;

            Remove = new Command(OnRemoveExecute, OnRemoveCanExecute);
        }
        #endregion

        #region Properties
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
            get { return imageHeight; }
        }

        /// <summary>
        ///     Gets the image margin if any.
        /// </summary>
        /// <value>The image margin.</value>
        public Thickness ImageMargin
        {
            get { return imageMargin; }
        }

        /// <summary>
        ///     Gets the width of the image if any.
        /// </summary>
        /// <value>The width of the image.</value>
        public int ImageWidth
        {
            get { return imageWidth; }
        }

        /// <summary>
        ///     Gets the name of the node.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return data.Name; }
        }

        public override ReflectionEntry Data
        {
            get { return base.Data; }
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

        #region Commands
        /// <summary>
        /// Gets the Remove command.
        /// </summary>
        public Command Remove { get; private set; }

        /// <summary>
        /// Method to check whether the Remove command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnRemoveCanExecute()
        {
            return data is AssemblyEntry;
        }

        /// <summary>
        /// Method to invoke when the Remove command is executed.
        /// </summary>
        private void OnRemoveExecute()
        {
            OnRequestRemove();
        }
        #endregion

        #region Methods
        protected virtual void OnRequestRemove()
        {
            RequestRemove.SafeInvoke(this);
        }

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

        /// <summary>
        /// Raised when this tab should be removed from the UI.
        /// </summary>
        public event EventHandler RequestRemove;
    }
}