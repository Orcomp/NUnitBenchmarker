// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    public static class StringExtensions
    {
        #region Methods
        public static string PrepareAsSearchFilter(this string filter)
        {
            var filterText = filter;
            if (string.IsNullOrWhiteSpace(filterText))
            {
                filterText = string.Empty;
            }

            filterText = filterText.ToLower().Trim();
            return filterText;
        }
        #endregion
    }
}