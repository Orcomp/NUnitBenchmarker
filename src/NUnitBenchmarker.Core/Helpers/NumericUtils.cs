// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericUtils.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System;
    using System.Globalization;

    public static class NumericUtils
    {
        #region Methods
        public static string TryToFormatAsNumber(string text)
        {
            long longNumber;
            if (long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out longNumber))
            {
                return longNumber.ToString("#,0", CultureInfo.CurrentCulture);
            }
            return text;
        }

        public static double RoundToSignificantDigits(this double d, int digits)
        {
            if (d == 0)
            {
                return 0;
            }

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale*Math.Round(d/scale, digits);
        }
        #endregion
    }
}