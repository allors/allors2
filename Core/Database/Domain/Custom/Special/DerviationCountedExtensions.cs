// <copyright file="DerviationCountedExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public static partial class DerivationCountedExtensions
    {
        public static void CustomOnDerive(this DerivationCounted @this, ObjectOnDerive method) => @this.DerivationCount += 1;
    }
}
