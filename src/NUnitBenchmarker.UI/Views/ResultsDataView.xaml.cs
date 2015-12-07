// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsDataView.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Views
{
    using System.Windows.Controls;
    using ViewModels;

    /// <summary>
    /// Interaction logic for ResultsDataView.xaml.
    /// </summary>
    public partial class ResultsDataView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsDataView"/> class.
        /// </summary>
        public ResultsDataView()
        {
            InitializeComponent();

            dataGrid.AutoGeneratingColumn += OnDataGridAutoGeneratingColumn;
        }

        private void OnDataGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var dataTable = ((ResultsDataViewModel) ViewModel).DataTable;

            if (dataTable.Columns.Contains(e.PropertyName))
            {
                e.Column.Header = dataTable.Columns[e.PropertyName].Caption;
            }
        }
    }
}