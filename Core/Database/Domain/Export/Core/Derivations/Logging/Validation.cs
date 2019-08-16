// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugValidation.cs" company="Allors bvba">
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

namespace Allors.Domain.Logging
{
    public sealed partial class Validation : ValidationBase
    {
        private readonly IDerivationLog derivationLog;

        public Validation(IDerivation derivation, IDerivationLog derivationLog) : base(derivation)
        {
            this.derivationLog = derivationLog;
        }

        protected override void OnAddedError(IDerivationError derivationError)
        {
            this.derivationLog.AddedError(derivationError);
        }
    }
}
