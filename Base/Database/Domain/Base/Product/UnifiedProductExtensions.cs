// <copyright file="UnifiedProductExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;

    public static partial class UnifiedProductExtensions
    {
        public static void CustomOnBuild(this UnifiedProduct @this, ObjectOnBuild method)
        {
            if (!@this.ExistScope)
            {
                @this.Scope = new Scopes(@this.Strategy.Session).Public;
            }
        }
    }
}
