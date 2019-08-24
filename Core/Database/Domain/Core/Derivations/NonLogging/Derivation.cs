// <copyright file="Derivation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.NonLogging
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Allors Object")]
    public sealed class Derivation : DerivationBase
    {
        public Derivation(ISession session, DerivationConfig config = null)
            : base(session, config) =>
            this.Validation = new Validation(this);

        protected override DerivationNodesBase CreateDerivationGraph(DerivationBase derivation) => new DerivationNodes(derivation);

        protected override void OnAddedDerivable(Object derivable)
        {
        }

        protected override void OnAddedDependency(Object dependent, Object dependee)
        {
        }

        protected override void OnStartedGeneration(int generation)
        {
        }

        protected override void OnStartedPreparation(int preparationRun)
        {
        }

        protected override void OnPreDeriving(Object derivable)
        {
        }

        protected override void OnPreDerived(Object derivable)
        {
        }

        protected override void OnPostDeriving(Object derivable)
        {
        }

        protected override void OnPostDerived(Object derivable)
        {
        }
    }
}
