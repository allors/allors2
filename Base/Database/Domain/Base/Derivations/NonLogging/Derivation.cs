// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Derivation.cs" company="Allors bvba">
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

namespace Allors.Domain.NonLogging
{
    using System.Collections.Generic;

    public sealed class Derivation : DerivationBase
    {
        public Derivation(ISession session)
            : base(session)
        {
            this.Validation = new Validation(this);
        }

        public Derivation(ISession session, IEnumerable<long> markedAsModified)
            : base(session, markedAsModified)
        {
            this.Validation = new Validation(this);
        }

        public Derivation(ISession session, IEnumerable<IObject> markedAsModified)
            : base(session, markedAsModified)
        {
            this.Validation = new Validation(this);
        }

        protected override DerivationGraphBase CreateDerivationGraph(DerivationBase derivation)
        {
            return new DerivationGraph(derivation);
        }

        protected override void OnAddedDerivable(Domain.Object derivable)
        {
        }

        protected override void OnAddedDependency(Domain.Object dependent, Domain.Object dependee)
        {
        }

        protected override void OnStartedGeneration(int generation)
        {
        }

        protected override void OnStartedPreparation(int preparationRun)
        {
        }

        protected override void OnPreDeriving(Domain.Object derivable)
        {
        }

        protected override void OnPreDerived(Object derivable)
        {
        }
    }
}