// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationNodeBase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class DerivationNodeBase : IEquatable<DerivationNodeBase>
    {
        private readonly Object derivable;
        private HashSet<DerivationNodeBase> dependencies;

        protected DerivationNodeBase(Object derivable) => this.derivable = derivable;

        public void Derive(DerivationBase derivation, List<Object> postDeriveObjects)
        {
            if (this.dependencies != null)
            {
                foreach (var dependency in this.dependencies)
                {
                    dependency.Derive(derivation, postDeriveObjects);
                }
            }

            if (!this.derivable.Strategy.IsDeleted)
            {
                this.OnDeriving(this.derivable);
                this.derivable.OnDerive(x => x.WithDerivation(derivation));
                this.OnDerived(this.derivable);

                postDeriveObjects.Add(this.derivable);
            }

            derivation.AddDerivedObject(this.derivable);
        }

        public void AddDependency(DerivationNodeBase derivationNode)
        {
            if (this.dependencies == null)
            {
                this.dependencies = new HashSet<DerivationNodeBase>();
            }

            this.dependencies.Add(derivationNode);
        }

        public bool Equals(DerivationNodeBase other) => other != null && this.derivable.Equals(other.derivable);

        public override bool Equals(object obj) => this.Equals((DerivationNodeBase)obj);

        public override int GetHashCode() => this.derivable.GetHashCode();

        public override string ToString() => this.derivable.ToString();

        protected abstract void OnCycle(Object root, Object derivable);

        protected abstract void OnDeriving(Object derivable);

        protected abstract void OnDerived(Object derivable);
    }
}
