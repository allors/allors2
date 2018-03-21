// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationGraph.cs" company="Allors bvba">
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

        public void Derive()
        {
            foreach (var dictionaryEntry in this.DerivationNodeByDerivable)
            {
                var derivationNode = dictionaryEntry.Value;
                derivationNode.TopologicalDerive(this.derivation);
            }
        }

        public DerivationNodeBase Add(Object derivable)
        {
            DerivationNodeBase derivationNode;
            if (!this.DerivationNodeByDerivable.TryGetValue(derivable, out derivationNode))
            {
                derivationNode = this.CreateDerivationNode(derivable);
                this.DerivationNodeByDerivable.Add(derivable, derivationNode);
            }

            return derivationNode;
        }

        public void AddDependency(Object dependent, Object dependee)
        {
            var dependentNode = this.Add(dependent);
            var dependeeeNode = this.Add(dependee);
            dependentNode.AddDependency(dependeeeNode);
        }

        protected abstract DerivationNodeBase CreateDerivationNode(Object derivable);
    }
}