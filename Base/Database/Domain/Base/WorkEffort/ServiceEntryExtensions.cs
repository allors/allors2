// <copyright file="ServiceEntryExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public static partial class ServiceEntryExtensions
    {

        public static void BaseOnDerive(this ServiceEntry @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!@this.ExistDerivationTrigger)
            {
                @this.DerivationTrigger = Guid.NewGuid();
            }
        }
    }
}
