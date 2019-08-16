// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Services
{
    using Allors.Domain;

    public class DerivationService : IDerivationService
    {
        private readonly DerivationConfig config;

        public DerivationService(DerivationConfig config)
        {
            this.config = config;
        }


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
