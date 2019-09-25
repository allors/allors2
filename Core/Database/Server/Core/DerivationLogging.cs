// <copyright file="DerivationLogging.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using Domain;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public static class DerivationLogging
    {
        public static void AddDerivationLogging(this IServiceCollection services)
        {
            var listDerivationService = new DerivationService(new DerivationConfig { DerivationLogFunc = () => new CustomListDerivationLog() });
            services.AddSingleton<IDerivationService>(listDerivationService);  // Set object ID's for breakpoints in CustomListDerivationLog

        }

        private class CustomListDerivationLog : Allors.Domain.Logging.ListDerivationLog
        {
            public Allors.Domain.Logging.Derivation Derivation { get; set; }

            public override void AddedDerivable(Object derivable)
            {
                base.AddedDerivable(derivable);

                if (derivable.Id == 787812)
                {
                    var setBreakpointHere = derivable;  // Set a breakpoint here to debug why derivable was added
                }
            }

            public override void AddedDependency(Object dependent, Object dependee)
            {
                base.AddedDependency(dependent, dependee);

                if (dependent.Id == 787812 || dependee.Id == 787812)
                {
                    var setBreakpointHere = (dependent.Id == 787812) ? dependent : dependee;  // Set breakpoint here to debug why dependency was added
                }
            }
        }
    }
}
