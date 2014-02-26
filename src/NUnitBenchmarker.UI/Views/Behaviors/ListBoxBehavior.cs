#region using...

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace NUnitBenchmarker.UI.Views.Behaviors
{
	public class ListBoxBehavior
	{
		private static readonly Dictionary<ListBox, Capture> Associations =
			new Dictionary<ListBox, Capture>();

		public static readonly DependencyProperty ScrollOnNewItemProperty =
			DependencyProperty.RegisterAttached(
				"ScrollOnNewItem",
				typeof (bool),
				typeof (ListBoxBehavior),
				new UIPropertyMetadata(false, OnScrollOnNewItemChanged));

		public static bool GetScrollOnNewItem(DependencyObject obj)
		{
			return (bool) obj.GetValue(ScrollOnNewItemProperty);
		}

		public static void SetScrollOnNewItem(DependencyObject obj, bool value)
		{
			obj.SetValue(ScrollOnNewItemProperty, value);
		}

		public static void OnScrollOnNewItemChanged(
			DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			var listBox = d as ListBox;
			if (listBox == null)
			{
				return;
			}
			bool oldValue = (bool) e.OldValue, newValue = (bool) e.NewValue;
			if (newValue == oldValue)
			{
				return;
			}
			if (newValue)
			{
				listBox.Loaded += ListBox_Loaded;
				listBox.Unloaded += ListBox_Unloaded;
			}
			else
			{
				listBox.Loaded -= ListBox_Loaded;
				listBox.Unloaded -= ListBox_Unloaded;
				if (Associations.ContainsKey(listBox))
				{
					Associations[listBox].Dispose();
				}
			}
		}

		private static void ListBox_Unloaded(object sender, RoutedEventArgs e)
		{
			var listBox = (ListBox) sender;
			if (Associations.ContainsKey(listBox))
			{
				Associations[listBox].Dispose();
			}
			listBox.Unloaded -= ListBox_Unloaded;
		}

		private static void ListBox_Loaded(object sender, RoutedEventArgs e)
		{
			var listBox = (ListBox) sender;
			var incc = listBox.Items as INotifyCollectionChanged;
			if (incc == null)
			{
				return;
			}
			listBox.Loaded -= ListBox_Loaded;
			Associations[listBox] = new Capture(listBox);
		}

		#region Nested type: Capture

		private class Capture : IDisposable
		{
			public Capture(ListBox listBox)
			{
				ListBox = listBox;
				NotifyCollectionChanged = listBox.ItemsSource as INotifyCollectionChanged;
				if (NotifyCollectionChanged != null)
				{
					NotifyCollectionChanged.CollectionChanged +=
						incc_CollectionChanged;
				}
			}

			private ListBox ListBox { get; set; }
			private INotifyCollectionChanged NotifyCollectionChanged { get; set; }

			#region IDisposable Members

			public void Dispose()
			{
				if (NotifyCollectionChanged != null)
				{
					NotifyCollectionChanged.CollectionChanged -= incc_CollectionChanged;
				}
			}

			#endregion

			private void incc_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					ListBox.ScrollIntoView(e.NewItems[0]);
					ListBox.SelectedItem = e.NewItems[0];
				}
			}
		}

		#endregion
	}
}