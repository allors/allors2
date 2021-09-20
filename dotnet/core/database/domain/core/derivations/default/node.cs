// <copyright file="DerivationNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations.Default
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Reviewed. Suppression is OK here.")]
    internal class Node : IEquatable<Node>
    {
        private readonly Domain.Object derivable;
        private Node currentRoot;

        private bool isVisited;

        internal Node(Domain.Object derivable) => this.derivable = derivable;

        internal bool IsMarked { get; set; }

        internal bool IsScheduled { get; set; }

        internal Node[] Dependencies { get; set; }

        public bool Equals(Node other) => other != null && this.derivable.Equals(other.derivable);

        public override bool Equals(object obj) => this.Equals((Node)obj);

        public override int GetHashCode() => this.derivable.GetHashCode();

        internal void TopologicalDerive(Graph graph, IList<Domain.Object> postDeriveBacklog) => this.TopologicalDerive(graph, postDeriveBacklog, this);

        private void TopologicalDerive(Graph graph, IList<Domain.Object> postDeriveBacklog, Node root)
        {
            if (this.isVisited)
            {
                if (root.Equals(this.currentRoot))
                {
                    throw new Exception("This derivation has a cycle. (" + this.currentRoot + " -> " + this + ")");
                }

                return;
            }

            this.isVisited = true;
            this.currentRoot = root;

            if (this.Dependencies != null)
            {
                foreach (var dep in this.Dependencies)
                {
                    dep.TopologicalDerive(graph, postDeriveBacklog, root);
                }
            }

            if (!this.derivable.Strategy.IsDeleted && graph.IsScheduled(this.derivable))
            {
                // TODO: Remove OnDerive
                this.derivable.OnDerive(x => x.WithDerivation(graph.Derivation));
                postDeriveBacklog.Add(this.derivable);
            }

            this.currentRoot = null;
        }
    }
}
