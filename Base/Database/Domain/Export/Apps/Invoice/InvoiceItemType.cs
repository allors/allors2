// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvoiceItemType.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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
