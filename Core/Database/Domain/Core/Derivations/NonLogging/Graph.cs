// <copyright file="DerivationNodes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

// ReSharper disable StyleCop.SA1121
namespace Allors.Domain.NonLogging
{
    using System.Collections.Generic;

    public class Graph
    {
        public Graph(IDerivation derivation)
        {
            this.Derivation = derivation;
            this.NodeByObject = new Dictionary<Object, Node>();
        }

        public IDerivation Derivation { get; }

        public Dictionary<Object, Node> NodeByObject { get; }

        public bool IsMarked(Object @object)
        {
            if (this.NodeByObject.TryGetValue(@object, out var node))
            {
                return node.IsMarked;
            }

            return false;
        }

        public void Mark(Object @object) => this.GetOrAddDerivationNode(@object).IsMarked = true;

        public void Mark(Object[] marked)
        {
            foreach (var @object in marked)
            {
                this.Mark(@object);
            }
        }

        public bool IsScheduled(Object @object)
        {
            if (this.NodeByObject.TryGetValue(@object, out var node))
            {
                return node.IsScheduled;
            }

            return false;
        }

        public void Schedule(Object @object) => this.GetOrAddDerivationNode(@object).IsScheduled = true;

        public IEnumerable<Object> Derive(IList<Object> postDeriveObjects)
        {
            foreach (var kvp in this.NodeByObject)
            {
                var derivationNode = kvp.Value;
                derivationNode.TopologicalDerive(this, postDeriveObjects);
            }

            return postDeriveObjects;
        }

        public void AddDependency(Object dependent, Object dependee)
        {
            var dependentNode = this.GetOrAddDerivationNode(dependent);
            var dependeeNode = this.GetOrAddDerivationNode(dependee);
            dependentNode.AddDependency(dependeeNode);
        }

        private Node GetOrAddDerivationNode(Object @object)
        {
            if (!this.NodeByObject.TryGetValue(@object, out var derivationNode))
            {
                derivationNode = new Node(@object);
                this.NodeByObject.Add(@object, derivationNode);
            }

            return derivationNode;
        }
    }
}
