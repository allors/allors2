// <copyright file="DerivationNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.NonLogging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Reviewed. Suppression is OK here.")]
    public class Node : IEquatable<Node>
    {
        private readonly Domain.Object derivable;

        private bool visited;
        private Node currentRoot;

        private Node dependency;
        private HashSet<Node> dependencies;

        public Node(Domain.Object derivable) => this.derivable = derivable;

        public bool IsMarked { get; set; }

        public bool IsScheduled { get; set; }

        public void TopologicalDerive(Graph graph, IList<Domain.Object> postDeriveObjects) => this.TopologicalDerive(graph, postDeriveObjects, this);

        public void AddDependency(Node node)
        {
            if (this.dependencies != null)
            {
                this.dependencies.Add(node);
            }
            else
            {
                if (this.dependency == null)
                {
                    this.dependency = node;
                }
                else
                {
                    if (!this.dependency.Equals(node))
                    {
                        this.dependencies = new HashSet<Node>
                        {
                            this.dependency,
                            node,
                        };

                        this.dependency = null;
                    }
                }
            }
        }

        public bool Equals(Node other) => other != null && this.derivable.Equals(other.derivable);

        public override bool Equals(object obj) => this.Equals((Node)obj);

        public override int GetHashCode() => this.derivable.GetHashCode();

        private void TopologicalDerive(Graph graph, IList<Domain.Object> postDeriveObjects, Node root)
        {
            if (this.visited)
            {
                if (root.Equals(this.currentRoot))
                {
                    throw new Exception("This derivation has a cycle. (" + this.currentRoot + " -> " + this + ")");
                }

                return;
            }

            this.visited = true;
            this.currentRoot = root;

            this.dependency?.TopologicalDerive(graph, postDeriveObjects, root);
            if (this.dependencies != null)
            {
                foreach (var dep in this.dependencies)
                {
                    dep.TopologicalDerive(graph, postDeriveObjects, root);
                }
            }

            if (!this.derivable.Strategy.IsDeleted && graph.IsScheduled(this.derivable))
            {
                this.derivable.OnDerive(x => x.WithDerivation(graph.Cycle.Derivation));
                postDeriveObjects.Add(this.derivable);
            }

            this.currentRoot = null;
        }
    }
}
