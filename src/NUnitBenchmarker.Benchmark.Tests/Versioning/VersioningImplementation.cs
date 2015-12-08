// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersioningImplementation.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Benchmark.Tests.Versioning
{
    using System;
    using System.Threading;

    public class VersioningImplementation
    {
        private readonly Random _random = new Random();

        public void DoMagic()
        {
            var msToWait = _random.Next(10, 250);

            // Of course Thread.Sleep is no best practice, but I just need to mimick some duration
            Thread.Sleep(msToWait);
        }
    }
}