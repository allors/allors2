// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationNodeBase.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;

    public abstract class DerivationNodeBase : IEquatable<DerivationNodeBase>
    {
        private readonly Object derivable;

        private bool visited;
        private DerivationNodeBase currentRoot;
        private HashSet<DerivationNodeBase> dependencies;

        protected DerivationNodeBase(Object derivable)
        {
            this.derivable = derivable;
        }

        public void TopologicalDerive(DerivationBase derivation, List<Object> derivedObjects)
        {
            this.TopologicalDerive(derivation, this, derivedObjects);
        }

        public void TopologicalFinalize(DerivationBase derivation, List<Object> derivedObjects)
        {
            this.TopologicalFinalize(derivation, this, derivedObjects);
        }
        
        public void AddDependency(DerivationNodeBase derivationNode)
        {
            if (this.dependencies == null)
            {
                this.dependencies = new HashSet<DerivationNodeBase>();
            }

            this.dependencies.Add(derivationNode);
        }

        public bool Equals(DerivationNodeBase other)
        {
            return other != null && this.derivable.Equals(other.derivable);
        }

        public override bool Equals(object obj)
        {
            return this.Equals((DerivationNodeBase)obj);
        }

        public override int GetHashCode()
        {
            return this.derivable.GetHashCode();
        }

        public override string ToString()
        {
            return this.derivable.ToString();
        }

        protected abstract void OnCycle(Object root, Object derivable);

        private void TopologicalDerive(DerivationBase derivation, DerivationNodeBase root, List<Object> derivedObjects)
        {
            if (this.visited)
            {
                if (root.Equals(this.currentRoot))
                {
                    this.OnCycle(root.derivable, this.derivable);
                    throw new Exception("This derivation has a cycle. (" + this.currentRoot + " -> " + this + ")");
                }

                return;
            }

            this.visited = true;
            this.currentRoot = root;

            if (this.dependencies != null)
            {
                foreach (var dependency in this.dependencies)
                {
                    dependency.TopologicalDerive(derivation, root, derivedObjects);
                }
            }

            if (!this.derivable.Strategy.IsDeleted)
            {
                 this.OnDeriving(this.derivable);
                this.derivable.OnDerive(x => x.WithDerivation(derivation));
                this.OnDerived(this.derivable);

                derivedObjects.Add(this.derivable);
            }

            derivation.AddDerivedObject(this.derivable);

            this.currentRoot = null;
        }

        private void TopologicalFinalize(DerivationBase derivation, DerivationNodeBase root, List<Object> derivedObjects)
        {
            if (this.visited)
            {
                if (root.Equals(this.currentRoot))
                {
                    this.OnCycle(root.derivable, this.derivable);
                    throw new Exception("This derivation has a cycle. (" + this.currentRoot + " -> " + this + ")");
                }

                return;
            }

            this.visited = true;
            this.currentRoot = root;

            if (this.dependencies != null)
            {
                foreach (var dependency in this.dependencies)
                {
                    dependency.TopologicalFinalize(derivation, root, derivedObjects);
                }
            }

            if (!this.derivable.Strategy.IsDeleted)
            {
                this.OnFinalizing(this.derivable);
                this.derivable.OnFinalize(x => x.WithDerivation(derivation));
                this.OnFinalized(this.derivable);

                derivedObjects.Add(this.derivable);
            }

            derivation.AddDerivedObject(this.derivable);

            this.currentRoot = null;
        }

        protected abstract void OnDeriving(Object derivable);

        protected abstract void OnDerived(Object derivable);

        protected abstract void OnFinalizing(Object derivable);

        protected abstract void OnFinalized(Object derivable);
    }
}