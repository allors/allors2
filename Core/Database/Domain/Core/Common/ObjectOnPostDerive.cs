// <copyright file="ObjectOnPostDerive.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Derivations;

    public abstract partial class ObjectOnPostDerive
    {
        public IDerivation Derivation { get; set; }

        public ObjectOnPostDerive WithDerivation(IDerivation derivation)
        {
            this.Derivation = derivation;
            return this;
        }
    }
}
