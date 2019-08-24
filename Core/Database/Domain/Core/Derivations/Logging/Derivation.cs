// <copyright file="Derivation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Logging
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Allors Object")]
    public sealed class Derivation : DerivationBase
    {
        public Derivation(ISession session, DerivationConfig config)
            : base(session, config)
        {
            this.DerivationLog = config.DerivationLogFunc();
            this.Validation = new Validation(this, this.DerivationLog);
        }

        public IDerivationLog DerivationLog { get; }

        protected override DerivationNodesBase CreateDerivationGraph(DerivationBase derivation) => new DerivationNodes(derivation, this.DerivationLog);

        protected override void OnAddedDerivable(Object derivable) => this.DerivationLog.AddedDerivable(derivable);

        protected override void OnAddedDependency(Object dependent, Object dependee) => this.DerivationLog.AddedDependency(dependent, dependee);

        protected override void OnStartedGeneration(int generation) => this.DerivationLog.StartedGeneration(generation);

        protected override void OnStartedPreparation(int preparationRun) => this.DerivationLog.StartedPreparation(preparationRun);

        protected override void OnPreDeriving(Object derivable) => this.DerivationLog.PreDeriving(derivable);

        protected override void OnPreDerived(Object derivable) => this.DerivationLog.PreDerived(derivable);

        protected override void OnPostDeriving(Object derivable) => this.DerivationLog.PostDeriving(derivable);

        protected override void OnPostDerived(Object derivable) => this.DerivationLog.PostDerived(derivable);
    }
}
