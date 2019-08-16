// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationNodes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.NonLogging
{
    public class DerivationNodes : DerivationNodesBase
    {
        public DerivationNodes(DerivationBase derivation)
            : base(derivation)
        {
        }

        protected override DerivationNodeBase CreateDerivationNode(Object derivable)
        {
            return new DerivationNode(derivable);
        }
    }
}
