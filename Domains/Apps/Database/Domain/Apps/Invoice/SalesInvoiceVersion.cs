// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceVersion.cs" company="Allors bvba">
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

    public partial class SalesInvoiceVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (SalesInvoiceVersionBuilder) method.Builder;
            var salesInvoice = builder.SalesInvoice;

            if (salesInvoice != null)
            {
                this.InternalComment = salesInvoice.InternalComment;
                this.TotalShippingAndHandlingCustomerCurrency = salesInvoice.TotalShippingAndHandlingCustomerCurrency;
                this.CustomerCurrency = salesInvoice.CustomerCurrency;
                this.Description = salesInvoice.Description;
                this.ShippingAndHandlingCharge = salesInvoice.ShippingAndHandlingCharge;
                this.TotalFeeCustomerCurrency = this.TotalFeeCustomerCurrency;
                this.Fee = salesInvoice.Fee;
                this.TotalExVatCustomerCurrency = salesInvoice.TotalExVatCustomerCurrency;
                this.CustomerReference = salesInvoice.CustomerReference;
                this.DiscountAdjustment = salesInvoice.DiscountAdjustment;
                this.AmountPaid = salesInvoice.AmountPaid;
                this.TotalDiscount = salesInvoice.TotalDiscount;
                this.BillingAccount = salesInvoice.BillingAccount;
                this.TotalIncVat = salesInvoice.TotalIncVat;
                this.TotalSurcharge = salesInvoice.TotalSurcharge;
                this.TotalBasePrice = salesInvoice.TotalBasePrice;
                this.TotalVatCustomerCurrency = salesInvoice.TotalVatCustomerCurrency;
                this.InvoiceDate = salesInvoice.InvoiceDate;
                this.EntryDate = salesInvoice.EntryDate;
                this.TotalIncVatCustomerCurrency = salesInvoice.TotalIncVatCustomerCurrency;
                this.TotalShippingAndHandling = salesInvoice.TotalShippingAndHandling;
                this.TotalBasePriceCustomerCurrency = salesInvoice.TotalBasePriceCustomerCurrency;
                this.SurchargeAdjustment = salesInvoice.SurchargeAdjustment;
                this.TotalExVat = salesInvoice.TotalExVat;
                this.InvoiceTerms = salesInvoice.InvoiceTerms;
                this.InvoiceNumber = salesInvoice.InvoiceNumber;
                this.Message = salesInvoice.Message;
                this.VatRegime = salesInvoice.VatRegime;
                this.TotalDiscountCustomerCurrency = salesInvoice.TotalDiscountCustomerCurrency;
                this.TotalVat = salesInvoice.TotalVat;
                this.TotalFee = salesInvoice.TotalFee;
                this.TotalListPrice = salesInvoice.TotalListPrice;
                this.BilledFromInternalOrganisation = salesInvoice.BilledFromInternalOrganisation;
                this.BillToContactMechanism = salesInvoice.BillToContactMechanism;
                this.SalesInvoiceType = salesInvoice.SalesInvoiceType;
                this.PaymentMethod = salesInvoice.PaymentMethod;
                this.BillToCustomer = salesInvoice.BillToCustomer;
                this.SalesInvoiceItems = salesInvoice.SalesInvoiceItems;
                this.ShipToCustomer = salesInvoice.ShipToCustomer;
                this.BilledFromContactMechanism = salesInvoice.BilledFromContactMechanism;
                this.TotalPurchasePrice = salesInvoice.TotalPurchasePrice;
                this.SalesChannel = salesInvoice.SalesChannel;
                this.Customers = salesInvoice.Customers;
                this.ShipToAddress = salesInvoice.ShipToAddress;
                this.Store = salesInvoice.Store;
                this.CurrentObjectState = salesInvoice.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}