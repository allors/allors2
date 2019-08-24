// <copyright file="DerivationLogFormatter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Logging
{
    public static class DerivationLogFormatter
    {
        public const int TabWith = 20;

        public static string FormatStartedGeneration(int generation) => $"{"Generation",-TabWith} #{generation}";

        public static string FormatStartedPreparation(int preparationRun) => $"{"Preparation",-TabWith} #{preparationRun}";

        public static string FormatPreDeriving(Object derivable) => $"{"PreDerive",-TabWith} " + FormatDerivable(derivable);

        public static string FormatPreDerived(Object derivable) => $"{"PreDerive¶",-TabWith} " + FormatDerivable(derivable);

        public static string FormatAddedDerivable(Object derivable) => $"{"Add",-TabWith} " + FormatDerivable(derivable);

        public static string FormatAddedDependency(Object dependent, Object dependee) => $"{"Dependency",-TabWith} " + FormatDerivable(dependent) + " <- " + FormatDerivable(dependee);

        public static string FormatAddedError(IDerivationError derivationError) => $"Error {derivationError}";

        public static string FormatCycle(Object root, Object derivable) => $"Cycle root: {root}, object: {derivable}";

        public static string FormatCycleDetected(Object dependent, Object dependee) => $"Cycle detected between dependent: {FormatDerivable(dependent)}, dependee: {FormatDerivable(dependee)}";

        public static string FormatDeriving(Object derivable) => $"{"Derive",-TabWith} " + FormatDerivable(derivable);

        public static string FormatDerived(Object derivable) => $"{"Derive¶",-TabWith} " + FormatDerivable(derivable);

        public static string FormatPostDeriving(Object derivable) => $"{"PostDerive",-TabWith} " + FormatDerivable(derivable);

        public static string FormatPostDerived(Object derivable) => $"{"PostDerive¶",-TabWith} " + FormatDerivable(derivable);

        private static object FormatDerivable(Object derivable)
        {
            var info = $"{derivable.Strategy.Class.Name}: {derivable}";
            return $"[{info,80}]";
        }
    }
}
