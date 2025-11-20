// <copyright file="ICycle.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations
{
    public interface ICycle
    {
        IAccumulatedChangeSet ChangeSet { get; }

        object this[string name] { get; set; }

        IDerivation Derivation { get; }

        IIteration Iteration { get; }
    }
}
