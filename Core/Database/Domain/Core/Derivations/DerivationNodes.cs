// <copyright file="DerivationNodes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

// ReSharper disable StyleCop.SA1121
namespace Allors.Domain
{
    using System.Collections.Generic;

    public class DerivationNodes
    {
        public DerivationNodes(IDerivation derivation)
        {
            this.Derivation = derivation;
            this.Scheduled = new HashSet<Object>();
            this.NodeByDerivable = new Dictionary<Object, DerivationNode>();
            this.Dependees = new HashSet<Object>();
            this.Marked = new HashSet<Object>();
        }

        public IDerivation Derivation { get; }

        public ISet<Object> Scheduled;

        public Dictionary<Object, DerivationNode> NodeByDerivable { get; }

        public ISet<Object> Dependees { get; }

        public ISet<Object> Marked { get; }

        public void Derive(List<Object> postDeriveObjects)
        {
            foreach (var kvp in this.NodeByDerivable)
            {
                var derivable = kvp.Key;
                if (!this.Dependees.Contains(derivable))
                {
                    var derivationNode = kvp.Value;
                    derivationNode.Derive(postDeriveObjects);
                }
            }
        }

        public DerivationNode Schedule(Object @object)
        {
            this.Scheduled.Add(@object);
            return this.GetOrAddDerivationNode(@object);
        }

        public void AddDependency(Object dependent, Object dependee)
        {
            this.Dependees.Add(dependee);

            var dependentNode = this.GetOrAddDerivationNode(dependent);
            var dependeeNode = this.GetOrAddDerivationNode(dependee);
            dependentNode.AddDependency(dependeeNode);
        }

        private DerivationNode GetOrAddDerivationNode(Object @object)
        {
            if (!this.NodeByDerivable.TryGetValue(@object, out var derivationNode))
            {
                derivationNode = new DerivationNode(this, @object);
                this.NodeByDerivable.Add(@object, derivationNode);
            }

            return derivationNode;
        }
    }
}
