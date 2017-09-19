// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItemVersion.cs" company="Allors bvba">
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
    public partial class PurchaseOrderItemVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (PurchaseOrderItemVersionBuilder) method.Builder;
            var purchaseOrderItem = builder.PurchaseOrderItem;

            if (purchaseOrderItem != null)
            {
                this.TotalDiscountAsPercentage = purchaseOrderItem.TotalDiscountAsPercentage;
                this.DiscountAdjustment = purchaseOrderItem.DiscountAdjustment;
                this.UnitVat = purchaseOrderItem.UnitVat;
                this.TotalVatCustomerCurrency = purchaseOrderItem.TotalVatCustomerCurrency;
                this.VatRegime = purchaseOrderItem.VatRegime;
                this.TotalVat = purchaseOrderItem.TotalVat;
                this.UnitSurcharge = purchaseOrderItem.UnitSurcharge;
                this.UnitDiscount = purchaseOrderItem.UnitDiscount;
                this.TotalExVatCustomerCurrency = purchaseOrderItem.TotalExVatCustomerCurrency;
                this.DerivedVatRate = purchaseOrderItem.DerivedVatRate;
                this.ActualUnitPrice = purchaseOrderItem.ActualUnitPrice;
                this.TotalIncVatCustomerCurrency = purchaseOrderItem.TotalIncVatCustomerCurrency;
                this.UnitBasePrice = purchaseOrderItem.UnitBasePrice;
                this.CalculatedUnitPrice = purchaseOrderItem.CalculatedUnitPrice;
                this.TotalSurchargeCustomerCurrency = purchaseOrderItem.TotalSurchargeCustomerCurrency;
                this.TotalIncVat = purchaseOrderItem.TotalIncVat;
                this.TotalSurchargeAsPercentage = purchaseOrderItem.TotalSurchargeAsPercentage;
                this.TotalDiscountCustomerCurrency = purchaseOrderItem.TotalDiscountCustomerCurrency;
                this.TotalDiscount = purchaseOrderItem.TotalDiscount;
                this.TotalSurcharge = purchaseOrderItem.TotalSurcharge;
                this.ActualUnitPrice = purchaseOrderItem.ActualUnitPrice;
                this.AssignedVatRegime = purchaseOrderItem.AssignedVatRegime;
                this.TotalBasePrice = purchaseOrderItem.TotalBasePrice;
                this.TotalExVat = purchaseOrderItem.TotalExVat;
                this.TotalBasePriceCustomerCurrency = purchaseOrderItem.TotalBasePriceCustomerCurrency;
                this.CurrentPriceComponents = purchaseOrderItem.CurrentPriceComponents;
                this.SurchargeAdjustment = purchaseOrderItem.SurchargeAdjustment;
                this.InternalComment = purchaseOrderItem.InternalComment;
                this.BudgetItem = purchaseOrderItem.BudgetItem;
                this.QuantityOrdered = purchaseOrderItem.QuantityOrdered;
                this.Description = purchaseOrderItem.Description;
                this.CorrespondingPurchaseOrder = purchaseOrderItem.CorrespondingPurchaseOrder;
                this.TotalOrderAdjustmentCustomerCurrency = purchaseOrderItem.TotalOrderAdjustmentCustomerCurrency;
                this.TotalOrderAdjustment = purchaseOrderItem.TotalOrderAdjustment;
                this.QuoteItem = purchaseOrderItem.QuoteItem;
                this.AssignedDeliveryDate = purchaseOrderItem.AssignedDeliveryDate;
                this.DeliveryDate = purchaseOrderItem.DeliveryDate;
                this.OrderTerms = purchaseOrderItem.OrderTerms;
                this.ShippingInstruction = purchaseOrderItem.ShippingInstruction;
                this.Associations = purchaseOrderItem.Associations;
                this.Message = purchaseOrderItem.Message;
                this.QuantityReceived = purchaseOrderItem.QuantityReceived;
                this.Product = purchaseOrderItem.Product;
                this.Part = purchaseOrderItem.Part;
                this.CurrentObjectState = purchaseOrderItem.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}