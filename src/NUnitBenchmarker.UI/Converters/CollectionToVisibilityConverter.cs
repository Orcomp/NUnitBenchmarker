// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionToVisibilityConverter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Converters
{
    using System;
    using System.Collections;
    using System.Windows;
    using Catel.MVVM.Converters;

    public class CollectionToVisibilityConverter : VisibilityConverterBase
    {
        #region Constructors
        public CollectionToVisibilityConverter()
            : base(Visibility.Hidden)
        {
        }
        #endregion

        protected override bool IsVisible(object value, Type targetType, object parameter)
        {
            bool isVisible = false;

            var collection = value as ICollection;
            if (collection != null)
            {
                isVisible = collection.Count > 0;
            }

            var invertParameter = parameter as string;
            if (!string.IsNullOrWhiteSpace(invertParameter))
            {
                bool invert = false;
                bool.TryParse(invertParameter, out invert);
                if (invert)
                {
                    isVisible = !isVisible;
                }
            }

            return isVisible;
        }
    }
}