// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListBoxBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Behaviors
{
    using System;
    using System.Collections.Specialized;
    using System.Windows.Controls;
    using Catel.Windows.Data;
    using Catel.Windows.Interactivity;

    public class ScrollToEndBehavior : BehaviorBase<ListBox>
    {
        private Capture _capture;

        #region Methods
        protected override void OnAssociatedObjectLoaded(object sender, EventArgs e)
        {
            base.OnAssociatedObjectLoaded(sender, e);

            var listBox = (ListBox)sender;
            _capture = new Capture(listBox);
        }

        protected override void OnAssociatedObjectUnloaded(object sender, EventArgs e)
        {
            base.OnAssociatedObjectUnloaded(sender, e);

            if (_capture != null)
            {
                _capture.Dispose();
                _capture = null;
            }
        }
        #endregion

        #region Nested type: Capture
        private class Capture : IDisposable
        {
            private ListBox _listBox;
            private INotifyCollectionChanged _notifyCollectionChanged;

            #region Constructors
            public Capture(ListBox listBox)
            {
                _listBox = listBox;
                _listBox.SubscribeToDependencyProperty("ItemsSource", OnItemsSourceChanged);

                SubscribeToEvents();
            }
            #endregion

            #region IDisposable Members
            public void Dispose()
            {
                UnsubscribeFromEvents();
            }

            private void OnItemsSourceChanged(object sender, DependencyPropertyValueChangedEventArgs e)
            {
                UnsubscribeFromEvents();
                SubscribeToEvents();
            }
            #endregion

            #region Methods
            private void SubscribeToEvents()
            {
                _notifyCollectionChanged = _listBox.ItemsSource as INotifyCollectionChanged;
                if (_notifyCollectionChanged != null)
                {
                    _notifyCollectionChanged.CollectionChanged += OnCollectionChanged;
                }
            }

            private void UnsubscribeFromEvents()
            {
                if (_notifyCollectionChanged != null)
                {
                    _notifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
                }
            }

            private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    _listBox.ScrollIntoView(e.NewItems[0]);
                    _listBox.SelectedItem = e.NewItems[0];
                }
            }
            #endregion
        }
        #endregion
    }
}