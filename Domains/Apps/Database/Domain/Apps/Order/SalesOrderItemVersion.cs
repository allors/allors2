// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItemVersion.cs" company="Allors bvba">
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
    public partial class SalesOrderItemVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (SalesOrderItemVersionBuilder) method.Builder;
            var salesOrderItem = builder.SalesOrderItem;

            if (salesOrderItem != null)
            {
                this.TotalDiscountAsPercentage = salesOrderItem.TotalDiscountAsPercentage;
                this.DiscountAdjustment = salesOrderItem.DiscountAdjustment;
                this.UnitVat = salesOrderItem.UnitVat;
                this.TotalVatCustomerCurrency = salesOrderItem.TotalVatCustomerCurrency;
                this.VatRegime = salesOrderItem.VatRegime;
                this.TotalVat = salesOrderItem.TotalVat;
                this.UnitSurcharge = salesOrderItem.UnitSurcharge;
                this.UnitDiscount = salesOrderItem.UnitDiscount;
                this.TotalExVatCustomerCurrency = salesOrderItem.TotalExVatCustomerCurrency;
                this.DerivedVatRate = salesOrderItem.DerivedVatRate;
                this.ActualUnitPrice = salesOrderItem.ActualUnitPrice;
                this.TotalIncVatCustomerCurrency = salesOrderItem.TotalIncVatCustomerCurrency;
                this.UnitBasePrice = salesOrderItem.UnitBasePrice;
                this.CalculatedUnitPrice = salesOrderItem.CalculatedUnitPrice;
                this.TotalSurchargeCustomerCurrency = salesOrderItem.TotalSurchargeCustomerCurrency;
                this.TotalIncVat = salesOrderItem.TotalIncVat;
                this.TotalSurchargeAsPercentage = salesOrderItem.TotalSurchargeAsPercentage;
                this.TotalDiscountCustomerCurrency = salesOrderItem.TotalDiscountCustomerCurrency;
                this.TotalDiscount = salesOrderItem.TotalDiscount;
                this.TotalSurcharge = salesOrderItem.TotalSurcharge;
                this.ActualUnitPrice = salesOrderItem.ActualUnitPrice;
                this.AssignedVatRegime = salesOrderItem.AssignedVatRegime;
                this.TotalBasePrice = salesOrderItem.TotalBasePrice;
                this.TotalExVat = salesOrderItem.TotalExVat;
                this.TotalBasePriceCustomerCurrency = salesOrderItem.TotalBasePriceCustomerCurrency;
                this.CurrentPriceComponents = salesOrderItem.CurrentPriceComponents;
                this.SurchargeAdjustment = salesOrderItem.SurchargeAdjustment;
                this.InternalComment = salesOrderItem.InternalComment;
                this.BudgetItem = salesOrderItem.BudgetItem;
                this.QuantityOrdered = salesOrderItem.QuantityOrdered;
                this.Description = salesOrderItem.Description;
                this.CorrespondingPurchaseOrder = salesOrderItem.CorrespondingPurchaseOrder;
                this.TotalOrderAdjustmentCustomerCurrency = salesOrderItem.TotalOrderAdjustmentCustomerCurrency;
                this.TotalOrderAdjustment = salesOrderItem.TotalOrderAdjustment;
                this.QuoteItem = salesOrderItem.QuoteItem;
                this.AssignedDeliveryDate = salesOrderItem.AssignedDeliveryDate;
                this.DeliveryDate = salesOrderItem.DeliveryDate;
                this.OrderTerms = salesOrderItem.OrderTerms;
                this.ShippingInstruction = salesOrderItem.ShippingInstruction;
                this.Associations = salesOrderItem.Associations;
                this.Message = salesOrderItem.Message;
                this.InitialProfitMargin = salesOrderItem.InitialProfitMargin;
                this.QuantityShortFalled = salesOrderItem.QuantityShortFalled;
                this.OrderedWithFeatures = salesOrderItem.OrderedWithFeatures;
                this.MaintainedProfitMargin = salesOrderItem.MaintainedProfitMargin;
                this.RequiredProfitMargin = salesOrderItem.RequiredProfitMargin;
                this.QuantityShipNow = salesOrderItem.QuantityShipNow;
                this.RequiredMarkupPercentage = salesOrderItem.RequiredMarkupPercentage;
                this.QuantityShipped = salesOrderItem.QuantityShipped;
                this.ShipToAddress = salesOrderItem.ShipToAddress;
                this.QuantityPicked = salesOrderItem.QuantityPicked;
                this.UnitPurchasePrice = salesOrderItem.UnitPurchasePrice;
                this.ShipToParty = salesOrderItem.ShipToParty;
                this.AssignedShipToAddress = salesOrderItem.AssignedShipToAddress;
                this.QuantityReturned = salesOrderItem.QuantityReturned;
                this.QuantityReserved = salesOrderItem.QuantityReserved;
                this.SalesRep = salesOrderItem.SalesRep;
                this.AssignedShipToParty = salesOrderItem.AssignedShipToParty;
                this.QuantityPendingShipment = salesOrderItem.QuantityPendingShipment;
                this.MaintainedMarkupPercentage = salesOrderItem.MaintainedMarkupPercentage;
                this.InitialMarkupPercentage = salesOrderItem.InitialMarkupPercentage;
                this.ReservedFromInventoryItem = salesOrderItem.ReservedFromInventoryItem;
                this.Product = salesOrderItem.Product;
                this.ProductFeature = salesOrderItem.ProductFeature;
                this.QuantityRequestsShipping = salesOrderItem.QuantityRequestsShipping;
                this.CurrentObjectState = salesOrderItem.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}