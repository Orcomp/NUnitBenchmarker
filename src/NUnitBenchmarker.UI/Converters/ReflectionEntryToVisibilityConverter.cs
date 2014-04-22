// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionEntryToIsVisibleConverter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Converters
{
    using System;
    using System.Windows;
    using Catel.MVVM.Converters;

    public class ReflectionEntryToVisibilityConverter : VisibilityConverterBase
    {
        public ReflectionEntryToVisibilityConverter()
            : base(Visibility.Collapsed)
        {
        }

        protected override bool IsVisible(object value, Type targetType, object parameter)
        {
            var obj = value as bool?;
            if (obj ?? true)
            {
                return true;
            }

            return false;
        }
    }
}