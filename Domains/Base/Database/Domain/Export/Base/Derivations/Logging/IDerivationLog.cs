// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDerivationLog.cs" company="Allors bvba">
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
    public interface IDerivationLog
    {
        // Derivation
        void StartedGeneration(int generation);

        void StartedPreparation(int preparationRun);

        void PreDeriving(Object derivable);

        void PreDerived(Object derivable);

        void AddedDerivable(Object derivable);

        /// <summary>
        /// The dependee is derived before the dependent object;
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

        void CycleDetected(Object derivable);

        void CycleDetected(Object dependent, Object dependee);
    }
}