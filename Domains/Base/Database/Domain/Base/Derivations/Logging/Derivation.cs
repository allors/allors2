// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugDerivation.cs" company="Allors bvba">
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
    using System.Collections.Generic;

    public sealed class Derivation : DerivationBase
    {
        private readonly IDerivationLog derivationLog;

        public Derivation(ISession session, IDerivationLog derivationLog, DerivationConfig config = null)
            : base(session, config)
        {
            this.derivationLog = derivationLog;
            this.Validation = new Validation(this, this.derivationLog);
        }

        public Derivation(ISession session, IDerivationLog derivationLog, IEnumerable<long> markedAsModified, DerivationConfig config = null)
            : base(session, markedAsModified, config)
        {
            this.derivationLog = derivationLog;
            this.Validation = new Validation(this, this.derivationLog);
        }

        public Derivation(ISession session, IDerivationLog derivationLog, IEnumerable<IObject> markedAsModified, DerivationConfig config = null)
            : base(session, markedAsModified, config)
        {
            this.derivationLog = derivationLog;
            this.Validation = new Validation(this, this.derivationLog);
        }

        protected override DerivationGraphBase CreateDerivationGraph(DerivationBase derivation)
        {
            return new DerivationGraph(derivation, this.derivationLog);
        }

        protected override void OnAddedDerivable(Object derivable)
        {
            this.derivationLog.AddedDerivable(derivable);
        }

        protected override void OnAddedDependency(Object dependent, Object dependee)
        {
            this.derivationLog.AddedDependency(dependent, dependee);
        }

        protected override void OnStartedGeneration(int generation)
        {
            this.derivationLog.StartedGeneration(generation);
        }

        protected override void OnStartedPreparation(int preparationRun)
        {
            this.derivationLog.StartedPreparation(preparationRun);
        }

        protected override void OnPreDeriving(Object derivable)
        {
            this.derivationLog.PreDeriving(derivable);
        }

        protected override void OnPreDerived(Object derivable)
        {
            this.derivationLog.PreDerived(derivable);
        }

        protected override void OnCycleDetected(Object derivable)
        {
            this.derivationLog.CycleDetected(derivable);
        }

        protected override void OnCycleDetected(Object dependent, Object dependee)
        {
            this.derivationLog.CycleDetected(dependent, dependee);
        }
    }
}