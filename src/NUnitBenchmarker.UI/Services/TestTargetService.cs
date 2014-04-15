// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestTargetService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System;
    using Catel;
    using NUnitBenchmarker.Data;

    public class TestTargetService : ITestTargetService
    {
        private readonly IUIServiceHost _uiServiceHost;

        public TestTargetService(IUIServiceHost uiServiceHost)
        {
            Argument.IsNotNull(() => uiServiceHost);

            _uiServiceHost = uiServiceHost;

            uiServiceHost.UpdateResult += OnUpdatedResult;
        }

        #region Events
        public event EventHandler<TestTargetUpdatedEventArgs> TestTargetUpdated;
        #endregion

        #region Methods
        private void OnUpdatedResult(BenchmarkResult result)
        {
            var typeSpecification = result.TypeSpecification;

            var eventArgs = new TestTargetUpdatedEventArgs(typeSpecification.AssemblyPath, typeSpecification.FullName);
            TestTargetUpdated.SafeInvoke(this, eventArgs);
        }
        #endregion
    }
}