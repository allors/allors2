// <copyright file="EnumerationExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public static partial class EnumerationExtensions
    {
        public static void BaseOnBuild(this Enumeration @this, ObjectOnBuild method)
        {
            if (!@this.IsActive.HasValue)
            {
                @this.IsActive = true;
            }
        }
    }
}
