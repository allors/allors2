// <copyright file="ListDerivationLog.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Logging
{
    using System.Collections.Generic;

    public class ListDerivationLog : IDerivationLog
    {
        public ListDerivationLog() => this.List = new List<string>();

        public List<string> List { get; }

        public string Text => string.Join("\n", this.List);

        public override string ToString() => this.Text;

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
        /// The dependee is derived before the dependent object;.
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
