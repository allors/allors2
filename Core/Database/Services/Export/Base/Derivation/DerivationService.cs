
// <copyright file="DerivationService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using Allors.Domain;

    public class DerivationService : IDerivationService
    {
        private readonly DerivationConfig config;

        public DerivationService(DerivationConfig config) => this.config = config;

        public IDerivation CreateDerivation(ISession session)
        {
            if (this.config?.DerivationLogFunc == null)
            {
                return new Domain.NonLogging.Derivation(session, this.config);
            }
            else
            {
                return new Domain.Logging.Derivation(session, this.config);
            }
        }
    }
}
