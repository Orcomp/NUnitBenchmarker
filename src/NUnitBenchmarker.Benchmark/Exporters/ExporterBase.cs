// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExporterBase.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Exporters
{
    using System;
    using System.IO;

    public abstract class ExporterBase
    {
        private readonly DateTime _timeStamp = DateTime.Now;

        protected string GetFolderPath(string folderPath)
        {
            if (folderPath == null)
            {
                folderPath = @"./Plots/Plots-" + _timeStamp.ToString("yy-MM-dd-HH-mm") + "/";
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }
    }
}