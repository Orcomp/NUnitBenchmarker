// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsFilterEnabledConverter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Converters
{
    using System;
    using Catel.MVVM.Converters;

    public class IsFilterEnabledConverter : ValueConverterBase
    {
        protected override object Convert(object value, Type targetType, object parameter)
        {
            var filterString = (value as string).PrepareAsSearchFilter();
            return !string.IsNullOrWhiteSpace(filterString);
        }
    }
}