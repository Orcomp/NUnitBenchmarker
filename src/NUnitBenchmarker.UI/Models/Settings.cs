// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Models
{
    using Catel.Data;

    public class Settings : ModelBase, ISettings
    {
        public bool IsLogarithmicTimeAxisChecked { get; set; }
    }
}