// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridLengthValueConverter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Converters
{
    using System;
    using System.Windows;
    using Catel.MVVM.Converters;

    /// <summary>
    /// Converts instances of other types to and from <see cref="T:System.Windows.GridLength"/> instances.
    /// Simple wrapper around <see cref="T:System.Windows.GridLengthConverter"/> which is surprisingly 
    /// not a <see cref="T:System.Windows.Data.IValueConverter"/>
    /// </summary>
    internal class GridLengthValueConverter : ValueConverterBase
    {
        private readonly GridLengthConverter _converter = new GridLengthConverter();

        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (value is string)
            {
                if (((string)value).Length > 1)
                {
                    value = ((string)value).Trim('*');
                }
            }

            return _converter.ConvertFrom(value);
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                var result = (string)_converter.ConvertTo(value, targetType);
                if (result != null && result.Length > 1)
                {
                    result = result.Trim('*');
                }
                return result;
            }

            return _converter.ConvertTo(value, targetType);
        }
    }
}