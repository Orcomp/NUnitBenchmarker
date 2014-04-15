// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestTargetUpdatedEventArgs.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System;
    using System.Reflection;
    using Catel;

    public class TestTargetUpdatedEventArgs : EventArgs
    {
        public TestTargetUpdatedEventArgs(string assembly, string type)
        {
            Argument.IsNotNullOrWhitespace(() => assembly);
            Argument.IsNotNullOrWhitespace(() => type);

            Assembly = Assembly.Load(assembly);
            if (Assembly != null)
            {
                Type = Assembly.GetType(type);
            }
        }

        public Assembly Assembly { get; private set; }

        public Type Type { get; private set; }
    }
}