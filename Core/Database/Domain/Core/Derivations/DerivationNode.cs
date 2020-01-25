// <copyright file="DerivationNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Reviewed. Suppression is OK here.")]
    public class DerivationNode : IEquatable<DerivationNode>
    {
        private readonly DerivationNodes nodes;
        private readonly Object derivable;
        private HashSet<DerivationNode> dependencies;

        public DerivationNode(DerivationNodes nodes, Object derivable)
        {
            this.nodes = nodes;
            this.derivable = derivable;
        }

        public void Derive(List<Object> postDeriveObjects)
        {
            if (this.dependencies != null)
            {
                foreach (var dependency in this.dependencies)
                {
                    dependency.Derive(postDeriveObjects);
                }
            }

            if (!this.derivable.Strategy.IsDeleted && this.nodes.Scheduled.Contains(this.derivable))
            {
                this.derivable.OnDerive(x => x.WithDerivation(this.nodes.Derivation));
                postDeriveObjects.Add(this.derivable);
            }
        }

        public void AddDependency(DerivationNode derivationNode)
        {
            if (this.dependencies == null)
            {
                this.dependencies = new HashSet<DerivationNode>();
            }

            this.dependencies.Add(derivationNode);
        }

        public bool Equals(DerivationNode other) => other != null && this.derivable.Equals(other.derivable);

        public override bool Equals(object obj) => this.Equals((DerivationNode)obj);

        public override int GetHashCode() => this.derivable.GetHashCode();

        public override string ToString() => this.derivable.ToString();
    }
}
