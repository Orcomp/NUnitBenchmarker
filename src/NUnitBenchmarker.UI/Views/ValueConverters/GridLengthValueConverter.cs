using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace NUnitBenchmarker.UI.Views.ValueConverters
{
	/// <summary>
	/// Converts instances of other types to and from <see cref="T:System.Windows.GridLength"/> instances.
	/// Simple wrapper around <see cref="T:System.Windows.GridLengthConverter"/> which is surprisingly 
	/// not a <see cref="T:System.Windows.Data.IValueConverter"/>
	/// </summary>
	internal class GridLengthValueConverter : IValueConverter
	{
		/// <summary>
		/// The converter
		/// </summary>
		readonly GridLengthConverter converter = new GridLengthConverter();

		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is string)
			{
				if (((string) value).Length > 1)
				{
					value = ((string) value).Trim('*');
				}
			}
			return converter.ConvertFrom(value);
		}

		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (targetType == typeof (string))
			{
				var result = (string) converter.ConvertTo(value, targetType);
				if (result != null && result.Length > 1)
				{
					result = result.Trim('*');
				}
				return result;
			}
			return converter.ConvertTo(value, targetType);
		}
	}
}
