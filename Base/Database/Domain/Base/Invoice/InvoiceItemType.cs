// <copyright file="InvoiceItemType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class InvoiceItemType
    {
        public bool IsFee => this.UniqueId == InvoiceItemTypes.FeeId;

        public bool IsDiscount => this.UniqueId == InvoiceItemTypes.DiscountId;

        public bool IsInterestCharge => this.UniqueId == InvoiceItemTypes.InterestChargeId;

        public bool IsMiscCharge => this.UniqueId == InvoiceItemTypes.MiscChargeId;

        public bool IsPromotion => this.UniqueId == InvoiceItemTypes.PromotionId;

        public bool IsShippingAndHandling => this.UniqueId == InvoiceItemTypes.ShippingAndHandlingId;

        public bool IsSurcharge => this.UniqueId == InvoiceItemTypes.SurchargeId;

        public bool IsWarranty => this.UniqueId == InvoiceItemTypes.WarrantyId;

        public bool IsProductFeatureItem => this.UniqueId == InvoiceItemTypes.ProductFeatureItemId;

        public bool IsPartItem => this.UniqueId == InvoiceItemTypes.PartItemId;

        public bool IsProductItem => this.UniqueId == InvoiceItemTypes.ProductItemId;

        public bool IsTime => this.UniqueId == InvoiceItemTypes.TimeId;
    }
}
