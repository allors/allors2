// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceItemVersion.cs" company="Allors bvba">
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
    using Meta;

    public partial class PurchaseInvoiceItemVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (PurchaseInvoiceItemVersionBuilder) method.Builder;
            var purchaseInvoiceItem = builder.PurchaseInvoiceItem;

            if (purchaseInvoiceItem != null)
            {
                this.TotalDiscountAsPercentage = purchaseInvoiceItem.TotalDiscountAsPercentage;
                this.DiscountAdjustment = purchaseInvoiceItem.DiscountAdjustment;
                this.UnitVat = purchaseInvoiceItem.UnitVat;
                this.TotalVatCustomerCurrency = purchaseInvoiceItem.TotalVatCustomerCurrency;
                this.VatRegime = purchaseInvoiceItem.VatRegime;
                this.TotalVat = purchaseInvoiceItem.TotalVat;
                this.UnitSurcharge = purchaseInvoiceItem.UnitSurcharge;
                this.UnitDiscount = purchaseInvoiceItem.UnitDiscount;
                this.TotalExVatCustomerCurrency = purchaseInvoiceItem.TotalExVatCustomerCurrency;
                this.DerivedVatRate = purchaseInvoiceItem.DerivedVatRate;
                this.ActualUnitPrice = purchaseInvoiceItem.ActualUnitPrice;
                this.TotalIncVatCustomerCurrency = purchaseInvoiceItem.TotalIncVatCustomerCurrency;
                this.UnitBasePrice = purchaseInvoiceItem.UnitBasePrice;
                this.CalculatedUnitPrice = purchaseInvoiceItem.CalculatedUnitPrice;
                this.TotalSurchargeCustomerCurrency = purchaseInvoiceItem.TotalSurchargeCustomerCurrency;
                this.TotalIncVat = purchaseInvoiceItem.TotalIncVat;
                this.TotalSurchargeAsPercentage = purchaseInvoiceItem.TotalSurchargeAsPercentage;
                this.TotalDiscountCustomerCurrency = purchaseInvoiceItem.TotalDiscountCustomerCurrency;
                this.TotalDiscount = purchaseInvoiceItem.TotalDiscount;
                this.TotalSurcharge = purchaseInvoiceItem.TotalSurcharge;
                this.ActualUnitPrice = purchaseInvoiceItem.ActualUnitPrice;
                this.AssignedVatRegime = purchaseInvoiceItem.AssignedVatRegime;
                this.TotalBasePrice = purchaseInvoiceItem.TotalBasePrice;
                this.TotalExVat = purchaseInvoiceItem.TotalExVat;
                this.TotalBasePriceCustomerCurrency = purchaseInvoiceItem.TotalBasePriceCustomerCurrency;
                this.CurrentPriceComponents = purchaseInvoiceItem.CurrentPriceComponents;
                this.SurchargeAdjustment = purchaseInvoiceItem.SurchargeAdjustment;
                this.InternalComment = purchaseInvoiceItem.InternalComment;
                this.InvoiceTerms = purchaseInvoiceItem.InvoiceTerms;
                this.TotalInvoiceAdjustment = purchaseInvoiceItem.TotalInvoiceAdjustment;
                this.InvoiceVatRateItems = purchaseInvoiceItem.InvoiceVatRateItems;
                this.AdjustmentFor = purchaseInvoiceItem.AdjustmentFor;
                this.SerializedInventoryItem = purchaseInvoiceItem.SerializedInventoryItem;
                this.Message = purchaseInvoiceItem.Message;
                this.TotalInvoiceAdjustmentCustomerCurrency = purchaseInvoiceItem.TotalInvoiceAdjustmentCustomerCurrency;
                this.Quantity = purchaseInvoiceItem.Quantity;
                this.Description = purchaseInvoiceItem.Description;
                this.PurchaseInvoiceItemType = purchaseInvoiceItem.PurchaseInvoiceItemType;
                this.Part = purchaseInvoiceItem.Part;
                this.CurrentObjectState = purchaseInvoiceItem.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}