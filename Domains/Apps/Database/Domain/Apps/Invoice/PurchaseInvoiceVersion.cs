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
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (PurchaseInvoiceVersionBuilder)method.Builder;
            var purchaseInvoice = builder.PurchaseInvoice;

            if (purchaseInvoice != null)
            {
                this.InternalComment = purchaseInvoice.InternalComment;
                this.TotalShippingAndHandlingCustomerCurrency = purchaseInvoice.TotalShippingAndHandlingCustomerCurrency;
                this.Description = purchaseInvoice.Description;
                this.ShippingAndHandlingCharge = purchaseInvoice.ShippingAndHandlingCharge;
                this.Fee = purchaseInvoice.Fee;
                this.CustomerReference = purchaseInvoice.CustomerReference;
                this.DiscountAdjustment = purchaseInvoice.DiscountAdjustment;
                this.BillingAccount = purchaseInvoice.BillingAccount;
                this.InvoiceDate = purchaseInvoice.InvoiceDate;
                this.SurchargeAdjustment = purchaseInvoice.SurchargeAdjustment;
                this.InvoiceTerms = purchaseInvoice.InvoiceTerms;
                this.InvoiceNumber = purchaseInvoice.InvoiceNumber;
                this.Message = purchaseInvoice.Message;
                this.VatRegime = purchaseInvoice.VatRegime;
                this.PurchaseInvoiceItems = purchaseInvoice.PurchaseInvoiceItems;
                this.BilledToInternalOrganisation = purchaseInvoice.BilledToInternalOrganisation;
                this.BilledFromParty = purchaseInvoice.BilledFromParty;
                this.PurchaseInvoiceType = purchaseInvoice.PurchaseInvoiceType;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}