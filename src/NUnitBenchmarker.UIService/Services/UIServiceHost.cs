﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIServiceHost.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using NUnitBenchmarker.Data;

    public class UIServiceHost : IUIServiceHost
    {
        #region Fields
        private ServiceHost _host;
        #endregion

        #region Methods
        public string OnPing(string message)
        {
            // Prevent race condition if other thread accidentally unsubscribes
            var handler = Ping;
            // Call the handler if any:
            if (handler != null)
            {
                return handler(message);
            }

            return null;
        }

        public IEnumerable<TypeSpecification> OnGetImplementations(TypeSpecification interfaceType)
        {
            // Prevent race condition if other thread accidentally unsubscribes
            var handler = GetImplementations;
            // Call the handler if any:
            if (handler != null)
            {
                return handler(interfaceType);
            }

            return null;
        }

        public void OnUpdateResult(BenchmarkResult result)
        {
            // Prevent race condition if other thread accidentally unsubscribes
            var handler = UpdateResult;
            // Call the handler if any:
            if (handler != null)
            {
                handler(result);
            }
        }

        /// <summary>
        /// Starts the Runner - UI communication service.
        /// </summary>
        public void Start()
        {
            _host = new ServiceHost(typeof (UIService));
            _host.Open();
        }

        /// <summary>
        /// Stops the Runner - UI communication service.
        /// </summary>
        public void Stop()
        {
            _host.Close();
        }
        #endregion

        public event Func<string, string> Ping;
        public event Func<TypeSpecification, IEnumerable<TypeSpecification>> GetImplementations;
        public event Action<BenchmarkResult> UpdateResult;
    }
}