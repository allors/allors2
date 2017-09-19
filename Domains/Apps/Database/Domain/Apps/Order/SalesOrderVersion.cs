// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderVersion.cs" company="Allors bvba">
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
    public partial class SalesOrderVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (SalesOrderVersionBuilder) method.Builder;
            var salesOrder = builder.SalesOrder;

            if (salesOrder != null)
            {
                this.InternalComment = salesOrder.InternalComment;
                this.CustomerCurrency = salesOrder.CustomerCurrency;
                this.TotalBasePriceCustomerCurrency = salesOrder.TotalBasePriceCustomerCurrency;
                this.TotalIncVatCustomerCurrency = salesOrder.TotalIncVatCustomerCurrency;
                this.TotalDiscountCustomerCurrency = salesOrder.TotalDiscountCustomerCurrency;
                this.CustomerReference = salesOrder.CustomerReference;
                this.Fee = salesOrder.Fee;
                this.TotalExVat = salesOrder.TotalExVat;
                this.OrderTerms = salesOrder.OrderTerms;
                this.TotalVat = salesOrder.TotalVat;
                this.TotalSurcharge = salesOrder.TotalSurcharge;
                this.ValidOrderItems = salesOrder.ValidOrderItems;
                this.OrderNumber = salesOrder.OrderNumber;
                this.TotalVatCustomerCurrency = salesOrder.TotalVatCustomerCurrency;
                this.TotalDiscount = salesOrder.TotalDiscount;
                this.Message = salesOrder.Message;
                this.TotalShippingAndHandlingCustomerCurrency = salesOrder.TotalShippingAndHandlingCustomerCurrency;
                this.EntryDate = salesOrder.EntryDate;
                this.DiscountAdjustment = salesOrder.DiscountAdjustment;
                this.OrderKind = salesOrder.OrderKind;
                this.TotalIncVat = salesOrder.TotalIncVat;
                this.TotalSurchargeCustomerCurrency = salesOrder.TotalSurchargeCustomerCurrency;
                this.VatRegime = salesOrder.VatRegime;
                this.TotalFeeCustomerCurrency = salesOrder.TotalFeeCustomerCurrency;
                this.TotalShippingAndHandling = salesOrder.TotalShippingAndHandling;
                this.ShippingAndHandlingCharge = salesOrder.ShippingAndHandlingCharge;
                this.OrderDate = salesOrder.OrderDate;
                this.TotalExVatCustomerCurrency = salesOrder.TotalExVatCustomerCurrency;
                this.DeliveryDate = salesOrder.DeliveryDate;
                this.TotalBasePrice = salesOrder.TotalBasePrice;
                this.TotalFee = salesOrder.TotalFee;
                this.SurchargeAdjustment = salesOrder.SurchargeAdjustment;
                this.TakenByContactMechanism = salesOrder.TakenByContactMechanism;
                this.ShipToCustomer = salesOrder.ShipToCustomer;
                this.BillToCustomer = salesOrder.BillToCustomer;
                this.TotalPurchasePrice = salesOrder.TotalPurchasePrice;
                this.ShipmentMethod = salesOrder.ShipmentMethod;
                this.TotalListPriceCustomerCurrency = salesOrder.TotalListPriceCustomerCurrency;
                this.MaintainedProfitMargin = salesOrder.MaintainedProfitMargin;
                this.ShipToAddress = salesOrder.ShipToAddress;
                this.BillToContactMechanism = salesOrder.BillToContactMechanism;
                this.SalesReps = salesOrder.SalesReps;
                this.InitialProfitMargin = salesOrder.InitialProfitMargin;
                this.TotalListPrice = salesOrder.TotalListPrice;
                this.PartiallyShip = salesOrder.PartiallyShip;
                this.Customers = salesOrder.Customers;
                this.Store = salesOrder.Store;
                this.MaintainedMarkupPercentage = salesOrder.MaintainedMarkupPercentage;
                this.BillFromContactMechanism = salesOrder.BillFromContactMechanism;
                this.PaymentMethod = salesOrder.PaymentMethod;
                this.PlacingContactMechanism = salesOrder.PlacingContactMechanism;
                this.SalesChannel = salesOrder.SalesChannel;
                this.PlacingCustomer = salesOrder.PlacingCustomer;
                this.ProformaInvoice = salesOrder.ProformaInvoice;
                this.SalesOrderItems = salesOrder.SalesOrderItems;
                this.InitialMarkupPercentage = salesOrder.InitialMarkupPercentage;
                this.TakenByInternalOrganisation = salesOrder.TakenByInternalOrganisation;
                this.Quote = salesOrder.Quote;
                this.CurrentObjectState = salesOrder.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}