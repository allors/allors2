// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceItemVersion.cs" company="Allors bvba">
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

    public partial class SalesInvoiceItemVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (SalesInvoiceItemVersionBuilder) method.Builder;
            var salesInvoiceItem = builder.SalesInvoiceItem;

            if (salesInvoiceItem != null)
            {
                this.TotalDiscountAsPercentage = salesInvoiceItem.TotalDiscountAsPercentage;
                this.DiscountAdjustment = salesInvoiceItem.DiscountAdjustment;
                this.UnitVat = salesInvoiceItem.UnitVat;
                this.TotalVatCustomerCurrency = salesInvoiceItem.TotalVatCustomerCurrency;
                this.VatRegime = salesInvoiceItem.VatRegime;
                this.TotalVat = salesInvoiceItem.TotalVat;
                this.UnitSurcharge = salesInvoiceItem.UnitSurcharge;
                this.UnitDiscount = salesInvoiceItem.UnitDiscount;
                this.TotalExVatCustomerCurrency = salesInvoiceItem.TotalExVatCustomerCurrency;
                this.DerivedVatRate = salesInvoiceItem.DerivedVatRate;
                this.ActualUnitPrice = salesInvoiceItem.ActualUnitPrice;
                this.TotalIncVatCustomerCurrency = salesInvoiceItem.TotalIncVatCustomerCurrency;
                this.UnitBasePrice = salesInvoiceItem.UnitBasePrice;
                this.CalculatedUnitPrice = salesInvoiceItem.CalculatedUnitPrice;
                this.TotalSurchargeCustomerCurrency = salesInvoiceItem.TotalSurchargeCustomerCurrency;
                this.TotalIncVat = salesInvoiceItem.TotalIncVat;
                this.TotalSurchargeAsPercentage = salesInvoiceItem.TotalSurchargeAsPercentage;
                this.TotalDiscountCustomerCurrency = salesInvoiceItem.TotalDiscountCustomerCurrency;
                this.TotalDiscount = salesInvoiceItem.TotalDiscount;
                this.TotalSurcharge = salesInvoiceItem.TotalSurcharge;
                this.ActualUnitPrice = salesInvoiceItem.ActualUnitPrice;
                this.AssignedVatRegime = salesInvoiceItem.AssignedVatRegime;
                this.TotalBasePrice = salesInvoiceItem.TotalBasePrice;
                this.TotalExVat = salesInvoiceItem.TotalExVat;
                this.TotalBasePriceCustomerCurrency = salesInvoiceItem.TotalBasePriceCustomerCurrency;
                this.CurrentPriceComponents = salesInvoiceItem.CurrentPriceComponents;
                this.SurchargeAdjustment = salesInvoiceItem.SurchargeAdjustment;
                this.InternalComment = salesInvoiceItem.InternalComment;
                this.InvoiceTerms = salesInvoiceItem.InvoiceTerms;
                this.TotalInvoiceAdjustment = salesInvoiceItem.TotalInvoiceAdjustment;
                this.InvoiceVatRateItems = salesInvoiceItem.InvoiceVatRateItems;
                this.AdjustmentFor = salesInvoiceItem.AdjustmentFor;
                this.SerializedInventoryItem = salesInvoiceItem.SerializedInventoryItem;
                this.Message = salesInvoiceItem.Message;
                this.TotalInvoiceAdjustmentCustomerCurrency = salesInvoiceItem.TotalInvoiceAdjustmentCustomerCurrency;
                this.Quantity = salesInvoiceItem.Quantity;
                this.Description = salesInvoiceItem.Description;
                this.ProductFeature = salesInvoiceItem.ProductFeature;
                this.RequiredProfitMargin = salesInvoiceItem.RequiredProfitMargin;
                this.InitialMarkupPercentage = salesInvoiceItem.InitialMarkupPercentage;
                this.MaintainedMarkupPercentage = salesInvoiceItem.MaintainedMarkupPercentage;
                this.Product = salesInvoiceItem.Product;
                this.UnitPurchasePrice = salesInvoiceItem.UnitPurchasePrice;
                this.SalesOrderItem = salesInvoiceItem.SalesOrderItem;
                this.SalesInvoiceItemType = salesInvoiceItem.SalesInvoiceItemType;
                this.SalesRep = salesInvoiceItem.SalesRep;
                this.InitialProfitMargin = salesInvoiceItem.InitialProfitMargin;
                this.MaintainedProfitMargin = salesInvoiceItem.MaintainedProfitMargin;
                this.TimeEntries = salesInvoiceItem.TimeEntries;
                this.RequiredMarkupPercentage = salesInvoiceItem.RequiredMarkupPercentage;
                this.CurrentObjectState = salesInvoiceItem.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}