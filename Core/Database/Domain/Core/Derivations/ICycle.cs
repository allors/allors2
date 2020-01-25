// <copyright file="ICycle.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;

    public interface ICycle : IDerive
    {
        IDerivation Derivation { get; }

        IIteration Iteration { get; }

        ISet<Object> DerivedObjects { get; }
    }
}
