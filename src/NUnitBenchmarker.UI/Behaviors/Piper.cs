// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Piper.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Behaviors
{
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    /// Piper Class is a general solution/workaround for pushing read-only GUI properties back into ViewModel
    /// Even we are trying to use OneWayToSource Binding for a ReadOnly Dependency Property, we got 
    /// error MC3065: "..." property is read-only and cannot be set from markup.
    /// (This project uses it actually for WCF Window class RestoreBounds property)
    /// </summary>
    public class Piper : Behavior<Window>
    {
        #region Constants
        /// <summary>
        /// The data pipes property
        /// </summary>
        public static readonly DependencyProperty DataPipesProperty =
            DependencyProperty.RegisterAttached(
                "DataPipes",
                typeof (DataPipeCollection),
                typeof (Piper),
                new UIPropertyMetadata(null));
        #endregion

        #region Methods
        /// <summary>
        /// Gets the data pipes.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns>DataPipeCollection.</returns>
        public static DataPipeCollection GetDataPipes(DependencyObject o)
        {
            return (DataPipeCollection) o.GetValue(DataPipesProperty);
        }

        /// <summary>
        /// Sets the data pipes.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="value">The value.</param>
        public static void SetDataPipes(DependencyObject o, DataPipeCollection value)
        {
            o.SetValue(DataPipesProperty, value);
        }
        #endregion
    }

    public class DataPipeCollection : FreezableCollection<DataPipe>
    {
    }

    public class DataPipe : Freezable
    {
        #region Constants
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                "Source",
                typeof (object),
                typeof (DataPipe),
                new FrameworkPropertyMetadata(null, OnSourceChanged));

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(
                "Target",
                typeof (object),
                typeof (DataPipe),
                new FrameworkPropertyMetadata(null));
        #endregion

        #region Properties
        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public object Target
        {
            get { return GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        #endregion

        #region Methods
        protected override Freezable CreateInstanceCore()
        {
            return new DataPipe();
        }

        protected virtual void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            Target = e.NewValue;
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataPipe) d).OnSourceChanged(e);
        }
        #endregion
    }
}