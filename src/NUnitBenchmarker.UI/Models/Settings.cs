// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Models
{
	using System;
	using System.ComponentModel;
	using Catel.Data;
	using Catel.Logging;

	public class Settings : ModelBase, ISettings
    {
		private static readonly ILog Log = LogManager.GetCurrentClassLogger();
		public Settings()
	    {
		    try
		    {
			    IsLogarithmicTimeAxisChecked = Properties.Settings.Default.IsLogarithmicTimeAxis;
		    }
		    catch (Exception e)
		    {
				Log.Error(e);
		    }
	    }

	    public bool IsLogarithmicTimeAxisChecked { get; set; }
	    public void Save()
	    {
		    try
		    {
			    Properties.Settings.Default.IsLogarithmicTimeAxis = IsLogarithmicTimeAxisChecked;
			    Properties.Settings.Default.Save();
		    }
		    catch (Exception e)
		    {
				Log.Error(e);
		    }
	    }
    }
}