// <copyright file="InvoiceItemType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class InvoiceItemType
    {
        public bool IsProductFeatureItem => this.UniqueId == InvoiceItemTypes.ProductFeatureItemId;

        public bool IsPartItem => this.UniqueId == InvoiceItemTypes.PartItemId;

        public bool IsProductItem => this.UniqueId == InvoiceItemTypes.ProductItemId;

        public bool IsService => this.UniqueId == InvoiceItemTypes.ServiceId;
    }
}
