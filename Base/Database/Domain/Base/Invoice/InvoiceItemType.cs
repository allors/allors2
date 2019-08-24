// <copyright file="InvoiceItemType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class InvoiceItemType
    {
        public bool Fee => this.UniqueId == InvoiceItemTypes.FeeId;

        public bool Discount => this.UniqueId == InvoiceItemTypes.DiscountId;

        public bool InterestCharge => this.UniqueId == InvoiceItemTypes.InterestChargeId;

        public bool MiscCharge => this.UniqueId == InvoiceItemTypes.MiscChargeId;

        public bool Promotion => this.UniqueId == InvoiceItemTypes.PromotionId;

        public bool ShippingAndHandling => this.UniqueId == InvoiceItemTypes.ShippingAndHandlingId;

        public bool Surcharge => this.UniqueId == InvoiceItemTypes.SurchargeId;

        public bool Warranty => this.UniqueId == InvoiceItemTypes.WarrantyId;

        public bool ProductFeatureItem => this.UniqueId == InvoiceItemTypes.ProductFeatureItemId;

        public bool PartItem => this.UniqueId == InvoiceItemTypes.PartItemId;

        public bool ProductItem => this.UniqueId == InvoiceItemTypes.ProductItemId;

        public bool Time => this.UniqueId == InvoiceItemTypes.TimeId;
    }
}
