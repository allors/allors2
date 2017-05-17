// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugLog.cs" company="Allors bvba">
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

namespace Allors.Domain.Logging
{
    using System.Collections.Generic;

    public class ListDerivationLog : IDerivationLog
    {
        public List<string> List { get; }

        public ListDerivationLog()
        {
            this.List = new List<string>();
        }

        public string Text => string.Join("\n", this.List);

        public override string ToString()
        {
            return this.Text;
        }

        // Derivation
        public virtual void StartedGeneration(int generation)
        {
            var message = DerivationLogFormatter.FormatStartedGeneration(generation);
            this.List.Add(message);
        }

        public virtual void StartedPreparation(int preparationRun)
        {
            var message = DerivationLogFormatter.FormatStartedPreparation(preparationRun);
            this.List.Add(message);
        }

        public virtual void PreDeriving(Object derivable)
        {
            var message = DerivationLogFormatter.FormatPreDeriving(derivable);
            this.List.Add(message);
        }

        public virtual void PreDerived(Object derivable)
        {
            var message = DerivationLogFormatter.FormatPreDerived(derivable);
            this.List.Add(message);
        }

        public virtual void AddedDerivable(Object derivable)
        {
            var message = DerivationLogFormatter.FormatAddedDerivable(derivable);
            this.List.Add(message);
        }

        /// <summary>
        /// The dependee is derived before the dependent object;
        /// </summary>
        public virtual void AddedDependency(Object dependent, Object dependee)
        {
            var message = DerivationLogFormatter.FormatAddedDependency(dependent, dependee);
            this.List.Add(message);
        }

        // Validation
        public virtual void AddedError(IDerivationError derivationError)
        {
            var message = DerivationLogFormatter.FormatAddedError(derivationError);
            this.List.Add(message);
        }

        // DerivationNode
        public virtual void Cycle(Object root, Object derivable)
        {
            var message = DerivationLogFormatter.FormatCycle(root, derivable);
            this.List.Add(message);
        }

        public virtual void Deriving(Object derivable)
        {
            var message = DerivationLogFormatter.FormatDeriving(derivable);
            this.List.Add(message);
        }

        public virtual void Derived(Object derivable)
        {
            var message = DerivationLogFormatter.FormatDerived(derivable);
            this.List.Add(message);
        }

        public virtual void PostDeriving(Object derivable)
        {
            var message = DerivationLogFormatter.FormatPostDeriving(derivable);
            this.List.Add(message);
        }

        public virtual void PostDerived(Object derivable)
        {
            var message = DerivationLogFormatter.FormatPostDerived(derivable);
            this.List.Add(message);
        }
    }
}