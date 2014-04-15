// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionEntryToImageConverter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Converters
{
    using System;
    using System.Windows.Media.Imaging;
    using Catel.MVVM.Converters;
    using NUnitBenchmarker.Models;

    public class ReflectionEntryToImageConverter : ValueConverterBase
    {
        public readonly BitmapImage _assembly = new BitmapImage(new Uri("/Resources/Images/assembly.png", UriKind.RelativeOrAbsolute));
        public readonly BitmapImage _namespace = new BitmapImage(new Uri("/Resources/Images/namespace.png", UriKind.RelativeOrAbsolute));
        public readonly BitmapImage _class = new BitmapImage(new Uri("/Resources/Images/class.png", UriKind.RelativeOrAbsolute));

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (value is AssemblyEntry)
            {
                return _assembly;
            }

            if (value is NamespaceEntry)
            {
                return _namespace;
            }

            if (value is TypeEntry)
            {
                return _class;
            }

            return null;
        }
        #endregion
    }
}