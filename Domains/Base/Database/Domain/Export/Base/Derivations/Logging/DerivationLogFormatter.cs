// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationLogFormatter.cs" company="Allors bvba">
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
    public static class DerivationLogFormatter
    {
        public const int TabWith = 20;

        public static string FormatStartedGeneration(int generation)
        {
            return $"{"Generation",-TabWith} #{generation}";
        }

        public static string FormatStartedPreparation(int preparationRun)
        {
            return $"{"Preparation",-TabWith} #{preparationRun}";
        }

        public static string FormatPreDeriving(Object derivable)
        {
            return $"{"PreDerive",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatPreDerived(Object derivable)
        {
            return $"{"PreDerive¶",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatAddedDerivable(Object derivable)
        {
            return $"{"Add",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatAddedDependency(Object dependent, Object dependee)
        {
            return $"{"Dependency",-TabWith} " + FormatDerivable(dependent) + " -> " + FormatDerivable(dependee);
        }

        public static string FormatAddedError(IDerivationError derivationError)
        {
            return $"Error {derivationError}";
        }

        public static string FormatCycle(Object root, Object derivable)
        {
            return $"Cycle root: {root}, object: {derivable}";
        }

        public static string FormatCycleDetected(Object dependent, Object dependee)
        {
            return $"Cycle detected between dependent: {FormatDerivable(dependent)}, dependee: {FormatDerivable(dependee)}";
        }

        public static string FormatDeriving(Object derivable)
        {
            return $"{"Derive",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatDerived(Object derivable)
        {
            return $"{"Derive¶",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatPostDeriving(Object derivable)
        {
            return $"{"PostDerive",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatPostDerived(Object derivable)
        {
            return $"{"PostDerive¶",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatPreFinalizing(Object derivable)
        {
            return $"{"PreFinalize",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatPreFinalized(Object derivable)
        {
            return $"{"PreFinalize¶",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatFinalizing(Object derivable)
        {
            return $"{"Finalize",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatFinalized(Object derivable)
        {
            return $"{"Finalize¶",-TabWith} " + FormatDerivable(derivable);
        }
        
        public static string FormatPostFinalizing(Object derivable)
        {
            return $"{"PostFinalize",-TabWith} " + FormatDerivable(derivable);
        }

        public static string FormatPostFinalized(Object derivable)
        {
            return $"{"PostFinalize¶",-TabWith} " + FormatDerivable(derivable);
        }

        private static object FormatDerivable(Object derivable)
        {
            return $"'{derivable,-40}' [{derivable.Strategy.Class.Name,20}#{derivable.Id,-10}]";
        }
    }
}