// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationNode.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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
    public class DerivationNode : DerivationNodeBase
    {
        private readonly IDerivationLog derivationLog;

        public DerivationNode(Object derivable, IDerivationLog derivationLog)
            : base(derivable)
        {
            this.derivationLog = derivationLog;
        }

        protected override void OnCycle(Object root, Object derivable)
        {
            this.derivationLog.Cycle(root, derivable);
        }

        protected override void OnDeriving(Object derivable)
        {
            this.derivationLog.Deriving(derivable);
        }

        protected override void OnDerived(Object derivable)
        {
            this.derivationLog.Derived(derivable);
        }

        protected override void OnPostDeriving(Object derivable)
        {
            this.derivationLog.PostDeriving(derivable);
        }

        protected override void OnPostDerived(Object derivable)
        {
            this.derivationLog.PostDerived(derivable);
        }
    }
}