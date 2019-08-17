// <copyright file="ProductQuoteApprovals.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ProductQuoteApprovals
    {
        protected override void BaseSecure(Security config) => config.GrantOwner(this.ObjectType, Operations.Read, Operations.Write, Operations.Execute);
    }
}
