// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceVersion.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (PurchaseInvoiceVersionBuilder)method.Builder;
            var purchaseInvoice = builder.PurchaseInvoice;

            if (purchaseInvoice != null)
            {
                this.InternalComment = purchaseInvoice.InternalComment;
                this.TotalShippingAndHandlingCustomerCurrency = purchaseInvoice.TotalShippingAndHandlingCustomerCurrency;
                this.CustomerCurrency = purchaseInvoice.CustomerCurrency;
                this.Description = purchaseInvoice.Description;
                this.ShippingAndHandlingCharge = purchaseInvoice.ShippingAndHandlingCharge;
                this.TotalFeeCustomerCurrency = this.TotalFeeCustomerCurrency;
                this.Fee = purchaseInvoice.Fee;
                this.TotalExVatCustomerCurrency = purchaseInvoice.TotalExVatCustomerCurrency;
                this.CustomerReference = purchaseInvoice.CustomerReference;
                this.DiscountAdjustment = purchaseInvoice.DiscountAdjustment;
                this.AmountPaid = purchaseInvoice.AmountPaid;
                this.TotalDiscount = purchaseInvoice.TotalDiscount;
                this.BillingAccount = purchaseInvoice.BillingAccount;
                this.TotalIncVat = purchaseInvoice.TotalIncVat;
                this.TotalSurcharge = purchaseInvoice.TotalSurcharge;
                this.TotalBasePrice = purchaseInvoice.TotalBasePrice;
                this.TotalVatCustomerCurrency = purchaseInvoice.TotalVatCustomerCurrency;
                this.InvoiceDate = purchaseInvoice.InvoiceDate;
                this.EntryDate = purchaseInvoice.EntryDate;
                this.TotalIncVatCustomerCurrency = purchaseInvoice.TotalIncVatCustomerCurrency;
                this.TotalShippingAndHandling = purchaseInvoice.TotalShippingAndHandling;
                this.TotalBasePriceCustomerCurrency = purchaseInvoice.TotalBasePriceCustomerCurrency;
                this.SurchargeAdjustment = purchaseInvoice.SurchargeAdjustment;
                this.TotalExVat = purchaseInvoice.TotalExVat;
                this.InvoiceTerms = purchaseInvoice.InvoiceTerms;
                this.InvoiceNumber = purchaseInvoice.InvoiceNumber;
                this.Message = purchaseInvoice.Message;
                this.VatRegime = purchaseInvoice.VatRegime;
                this.TotalDiscountCustomerCurrency = purchaseInvoice.TotalDiscountCustomerCurrency;
                this.TotalVat = purchaseInvoice.TotalVat;
                this.TotalFee = purchaseInvoice.TotalFee;
                this.PurchaseInvoiceItems = purchaseInvoice.PurchaseInvoiceItems;
                this.BilledToInternalOrganisation = purchaseInvoice.BilledToInternalOrganisation;
                this.BilledFromParty = purchaseInvoice.BilledFromParty;
                this.PurchaseInvoiceType = purchaseInvoice.PurchaseInvoiceType;
                this.CurrentObjectState = purchaseInvoice.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}