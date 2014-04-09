// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionNodeViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using Catel;
    using Catel.MVVM;
    using NUnitBenchmarker.Model;

    /// <summary>
    ///     Foundation class for all ViewModel classes displayed by TreeViewItems.
    ///     Uses mediator / adapter design pattern between domain model and TreeViewItem.
    /// </summary>
    public class ReflectionNodeViewModel : TreeViewItemViewModel<ReflectionEntry>
    {
        #region Fields
        private ImageSource _image;
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
                if (_image != null)
                {
                    return _image;
                }

                if (Data.UseIcons)
                {
                    // image = new 
                }

                int oldImageWidth = ImageWidth;
                if (_image == null)
                {
                    ImageWidth = 0;
                    ImageHeight = 0;
                    ImageMargin = new Thickness(0, 0, 0, 0);
                }
                else
                {
                    ImageWidth = 16;
                    ImageHeight = 16;
                    ImageMargin = new Thickness(2, 0, 0, 0);
                }

                bool imageChanged = oldImageWidth != ImageWidth;
                if (imageChanged)
                {
                    RaisePropertyChanged(() => ImageWidth);
                    RaisePropertyChanged(() => ImageHeight);
                    RaisePropertyChanged(() => ImageMargin);
                }

                return _image;
            }
        }

        /// <summary>
        ///     Gets the height of the image if any.
        /// </summary>
        /// <value>The height of the image.</value>
        public int ImageHeight { get; private set; }

        /// <summary>
        ///     Gets the image margin if any.
        /// </summary>
        /// <value>The image margin.</value>
        public Thickness ImageMargin { get; private set; }

        /// <summary>
        ///     Gets the width of the image if any.
        /// </summary>
        /// <value>The width of the image.</value>
        public int ImageWidth { get; private set; }

        /// <summary>
        ///     Gets the name of the node.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return Data.Name; }
        }

        public string ToolTip
        {
            get { return Data.Description; }
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
            return Data is AssemblyEntry;
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

        public override void Clear(bool lazyLoadChildren = true)
        {
            base.Clear(lazyLoadChildren);

            RaisePropertyChanged(() => Name);
        }

        public override void LoadChildren(Action<object> completion = null)
        {
            var children = Data.GetChildren();
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


        public event EventHandler RequestRemove;
    }
}