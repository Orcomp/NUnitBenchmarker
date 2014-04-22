// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsDataView.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Views
{
    using System.ComponentModel;
    using ViewModels;
    using UserControl = Catel.Windows.Controls.UserControl;

    /// <summary>
    /// Interaction logic for ResultsDataView.xaml.
    /// </summary>
    public partial class ResultsDataView : UserControl
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsDataView"/> class.
        /// </summary>
        public ResultsDataView()
        {
            InitializeComponent();
        }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            UpdateSource();
        }

        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnViewModelPropertyChanged(e);

            if (string.Equals(e.PropertyName, "DataTable"))
            {
                UpdateSource();
            }
        }

        private void UpdateSource()
        {
            var vm = (ResultsDataViewModel)ViewModel;

            var dataTable = vm.DataTable;
            var dataTableView = dataTable.DefaultView;

            dataGrid.ItemsSource = dataTableView;
            dataGrid.Items.Refresh();
        }
        #endregion
    }
}