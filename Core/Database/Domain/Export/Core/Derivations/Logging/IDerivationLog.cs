
// <copyright file="IDerivationLog.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Logging
{
    public interface IDerivationLog
    {
        // Derivation
        void StartedGeneration(int generation);

        void StartedPreparation(int preparationRun);

        void PreDeriving(Object derivable);

        void PreDerived(Object derivable);

        void AddedDerivable(Object derivable);

        /// <summary>
        /// The dependee is derived before the dependent object;.
        /// </summary>
        void AddedDependency(Object dependent, Object dependee);

        // Validation
        void AddedError(IDerivationError derivationError);

        // DerivationNode
        void Cycle(Object root, Object derivable);

        void Deriving(Object derivable);

        void Derived(Object derivable);

        void PostDeriving(Object derivable);

        void PostDerived(Object derivable);
    }
}
