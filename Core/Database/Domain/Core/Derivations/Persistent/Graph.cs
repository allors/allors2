// <copyright file="Graph.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

// ReSharper disable StyleCop.SA1121
namespace Allors.Domain.Derivations.Persistent
{
    using System.Collections.Generic;
    using System.Linq;
    using Object = Domain.Object;

    internal class Graph
    {
        internal Graph(Derivation derivation)
        {
            this.Derivation = derivation;
            this.NodeByObject = new Dictionary<Object, Node>();
        }

        internal Derivation Derivation { get; }

        internal Dictionary<Object, Node> NodeByObject { get; }

        internal bool IsMarked(Object @object)
        {
            if (this.NodeByObject.TryGetValue(@object, out var node))
            {
                return node.IsMarked;
            }

            return false;
        }

        internal void Mark(Object @object) => this.GetOrAddDerivationNode(@object).IsMarked = true;

        internal void Mark(Object[] marked)
        {
            foreach (var @object in marked)
            {
                this.Mark(@object);
            }
        }

        internal bool IsScheduled(Object @object)
        {
            if (this.NodeByObject.TryGetValue(@object, out var node))
            {
                return node.IsScheduled;
            }

            return false;
        }

        internal void Schedule(Object @object) => this.GetOrAddDerivationNode(@object).IsScheduled = true;

        internal void AddDependency(Object dependent, Object[] dependencies)
        {
            if (dependent != null && dependencies.Length > 0)
            {
                var dependentNode = this.GetOrAddDerivationNode(dependent);
                if (dependentNode.Dependencies == null)
                {
                    dependentNode.Dependencies = dependencies
                        .Where(v => v != null)
                        .Select(this.GetOrAddDerivationNode)
                        .Distinct()
                        .Where(v => !v.Equals(dependentNode))
                        .ToArray();
                }
                else
                {
                    dependentNode.Dependencies = dependentNode.Dependencies
                        .Union(dependencies
                            .Where(v => v != null)
                            .Select(this.GetOrAddDerivationNode))
                        .Distinct()
                        .Where(v => !v.Equals(dependentNode))
                        .ToArray();
                }

                if (dependentNode.Dependencies.Length == 0)
                {
                    dependentNode.Dependencies = null;
                }
            }
        }

        internal IEnumerable<Object> Derive(IList<Object> postDeriveObjects)
        {
            foreach (var kvp in this.NodeByObject)
            {
                var derivationNode = kvp.Value;
                derivationNode.TopologicalDerive(this, postDeriveObjects);
            }

            return postDeriveObjects;
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
