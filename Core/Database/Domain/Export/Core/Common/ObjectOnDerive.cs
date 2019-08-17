// <copyright file="ObjectOnDerive.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public abstract partial class ObjectOnDerive
    {
        public IDerivation Derivation { get; set; }

        public ObjectOnDerive WithDerivation(IDerivation derivation)
        {
            this.Derivation = derivation;
            return this;
        }
    }
}
