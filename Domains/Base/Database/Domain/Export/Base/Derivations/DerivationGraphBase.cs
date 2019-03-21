// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationGraphBase.cs" company="Allors bvba">
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

// ReSharper disable StyleCop.SA1121
namespace Allors.Domain
{
    using System.Collections.Generic;

    public abstract class DerivationGraphBase
    {
        private readonly DerivationBase derivation;

        protected DerivationGraphBase(DerivationBase derivation)
        {
            this.derivation = derivation;
            this.DerivationNodeByDerivable = new Dictionary<Object, DerivationNodeBase>();
        }

        public Dictionary<Object, DerivationNodeBase> DerivationNodeByDerivable { get; }

        public int Count => this.DerivationNodeByDerivable.Count;

        public void Derive(ISet<IObject> dependees, List<Object> derivedObjects)
        {
            foreach (var kvp in this.DerivationNodeByDerivable)
            {
                var derivable = kvp.Key;
                if (!dependees.Contains(derivable))
                {
                    var derivationNode = kvp.Value;
                    derivationNode.Derive(this.derivation, derivedObjects);
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