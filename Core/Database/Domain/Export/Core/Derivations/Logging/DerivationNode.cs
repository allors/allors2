
// <copyright file="DerivationNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Logging
{
    public class DerivationNode : DerivationNodeBase
    {
        private readonly IDerivationLog derivationLog;

        public DerivationNode(Object derivable, IDerivationLog derivationLog)
            : base(derivable) =>
            this.derivationLog = derivationLog;

        protected override void OnCycle(Object root, Object derivable) => this.derivationLog.Cycle(root, derivable);

        protected override void OnDeriving(Object derivable) => this.derivationLog.Deriving(derivable);

        protected override void OnDerived(Object derivable) => this.derivationLog.Derived(derivable);
    }
}
