// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.Views
{
    using Catel.Windows;

    /// <summary>
    ///     According to MVVM: Nothing to do here and nothing to see here :-)
    /// </summary>
    public partial class MainWindow : DataWindow
    {
        #region Constructors
        public MainWindow()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();
        }
        #endregion
    }
}