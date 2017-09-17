// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderVersion.cs" company="Allors bvba">
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
    public partial class PurchaseOrderVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (PurchaseOrderVersionBuilder) method.Builder;
            var purchaseOrder = builder.PurchaseOrder;

            if (purchaseOrder != null)
            {
                this.InternalComment = purchaseOrder.InternalComment;
                this.CustomerCurrency = purchaseOrder.CustomerCurrency;
                this.TotalBasePriceCustomerCurrency = purchaseOrder.TotalBasePriceCustomerCurrency;
                this.TotalIncVatCustomerCurrency = purchaseOrder.TotalIncVatCustomerCurrency;
                this.TotalDiscountCustomerCurrency = purchaseOrder.TotalDiscountCustomerCurrency;
                this.CustomerReference = purchaseOrder.CustomerReference;
                this.Fee = purchaseOrder.Fee;
                this.TotalExVat = purchaseOrder.TotalExVat;
                this.OrderTerms = purchaseOrder.OrderTerms;
                this.TotalVat = purchaseOrder.TotalVat;
                this.TotalSurcharge = purchaseOrder.TotalSurcharge;
                this.ValidOrderItems = purchaseOrder.ValidOrderItems;
                this.OrderNumber = purchaseOrder.OrderNumber;
                this.TotalVatCustomerCurrency = purchaseOrder.TotalVatCustomerCurrency;
                this.TotalDiscount = purchaseOrder.TotalDiscount;
                this.Message = purchaseOrder.Message;
                this.TotalShippingAndHandlingCustomerCurrency = purchaseOrder.TotalShippingAndHandlingCustomerCurrency;
                this.EntryDate = purchaseOrder.EntryDate;
                this.DiscountAdjustment = purchaseOrder.DiscountAdjustment;
                this.OrderKind = purchaseOrder.OrderKind;
                this.TotalIncVat = purchaseOrder.TotalIncVat;
                this.TotalSurchargeCustomerCurrency = purchaseOrder.TotalSurchargeCustomerCurrency;
                this.VatRegime = purchaseOrder.VatRegime;
                this.TotalFeeCustomerCurrency = purchaseOrder.TotalFeeCustomerCurrency;
                this.TotalShippingAndHandling = purchaseOrder.TotalShippingAndHandling;
                this.ShippingAndHandlingCharge = purchaseOrder.ShippingAndHandlingCharge;
                this.OrderDate = purchaseOrder.OrderDate;
                this.TotalExVatCustomerCurrency = purchaseOrder.TotalExVatCustomerCurrency;
                this.DeliveryDate = purchaseOrder.DeliveryDate;
                this.TotalBasePrice = purchaseOrder.TotalBasePrice;
                this.TotalFee = purchaseOrder.TotalFee;
                this.SurchargeAdjustment = purchaseOrder.SurchargeAdjustment;
                this.PurchaseOrderItems = purchaseOrder.PurchaseOrderItems;
                this.TakenViaSupplier = purchaseOrder.TakenViaSupplier;
                this.TakenViaContactMechanism = purchaseOrder.TakenViaContactMechanism;
                this.BillToContactMechanism = purchaseOrder.BillToContactMechanism;
                this.ShipToBuyer = purchaseOrder.ShipToBuyer;
                this.Facility = purchaseOrder.Facility;
                this.ShipToAddress = purchaseOrder.ShipToAddress;
                this.BillToPurchaser = purchaseOrder.BillToPurchaser;
                this.CurrentObjectState = purchaseOrder.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}