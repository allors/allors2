
// <copyright file="DerivationNodesBase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

// ReSharper disable StyleCop.SA1121
namespace Allors.Domain
{
    using System.Collections.Generic;

    public abstract class DerivationNodesBase
    {
        private readonly DerivationBase derivation;

        protected DerivationNodesBase(DerivationBase derivation)
        {
            this.derivation = derivation;
            this.DerivationNodeByDerivable = new Dictionary<Object, DerivationNodeBase>();
        }

        public Dictionary<Object, DerivationNodeBase> DerivationNodeByDerivable { get; }

        public int Count => this.DerivationNodeByDerivable.Count;

        public void Derive(ISet<IObject> dependees, List<Object> postDeriveObjects)
        {
            foreach (var kvp in this.DerivationNodeByDerivable)
            {
                var derivable = kvp.Key;
                if (!dependees.Contains(derivable))
                {
                    var derivationNode = kvp.Value;
                    derivationNode.Derive(this.derivation, postDeriveObjects);
                }
            }
        }

        public DerivationNodeBase Add(Object derivable)
        {
            if (!this.DerivationNodeByDerivable.TryGetValue(derivable, out var derivationNode))
            {
                derivationNode = this.CreateDerivationNode(derivable);
                this.DerivationNodeByDerivable.Add(derivable, derivationNode);
            }

            return derivationNode;
        }

        public void AddDependency(Object dependent, Object dependee)
        {
            var dependentNode = this.Add(dependent);
            var dependeeNode = this.Add(dependee);
            dependentNode.AddDependency(dependeeNode);
        }

        protected abstract DerivationNodeBase CreateDerivationNode(Object derivable);
    }
}
