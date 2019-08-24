// <copyright file="DerivationNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.NonLogging
{
    public class DerivationNode : DerivationNodeBase
    {
        public DerivationNode(Object derivable)
            : base(derivable)
        {
        }

        protected override void OnCycle(Object root, Object derivable)
        {
        }

        protected override void OnDeriving(Object derivable)
        {
        }

        protected override void OnDerived(Object derivable)
        {
        }
    }
}
