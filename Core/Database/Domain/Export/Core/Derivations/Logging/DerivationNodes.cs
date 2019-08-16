// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationNodes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.Logging
{
    public class DerivationNodes : DerivationNodesBase
    {
        private readonly IDerivationLog derivationLog;

        public DerivationNodes(DerivationBase derivation, IDerivationLog derivationLog)
            : base(derivation)
        {
            this.derivationLog = derivationLog;
        }

        protected override DerivationNodeBase CreateDerivationNode(Object derivable)
        {
            return new DerivationNode(derivable, this.derivationLog);
        }
    }
}
