// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeService.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Services
{
    using Allors.Domain;

    public class DerivationService : IDerivationService
    {
        private readonly DerivationConfig config;

        public DerivationService(DerivationConfig config = null)
        {
            this.config = config ?? new DerivationConfig();
        }


        public IDerivation CreateDerivation(ISession session)
        {
            if (this.config.DerivationLogFunc == null)
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