// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUrlReservationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    public interface IUrlReservationService
    {
        #region Properties
        bool IsReserved { get; set; }
        #endregion

        #region Methods
        void AddReservation();
        void RemoveReservation();
        #endregion
    }
}