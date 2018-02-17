// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoice.cs" company="Allors bvba">
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

using System.Linq;
using Allors.Domain.NonLogging;

namespace Allors.Domain
{
    using System;
    using Meta;
    using Resources;

    public partial class PurchaseInvoice
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PurchaseInvoice, M.PurchaseInvoice.PurchaseInvoiceState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public InvoiceItem[] InvoiceItems => this.PurchaseInvoiceItems;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistPurchaseInvoiceState)
            {
                this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).InProcess;
            }

            if (!this.ExistInvoiceDate)
            {
                this.InvoiceDate = DateTime.UtcNow;
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = DateTime.UtcNow;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            var internalOrganisation = this.Strategy.Session.GetSingleton();

            // TODO:
            if (derivation.HasChangedRoles(this))
            {
                derivation.AddDependency(this, internalOrganisation);
            }

            if (this.ExistBilledFrom)
            {
                var supplier = this.BilledFrom as Organisation;
                if (supplier != null)
                {
                    // TODO: Isn't this too broad?
                    foreach (Organisation supplierRelationship in new Organisations(this.strategy.Session).Suppliers)
                    {
                        derivation.AddDependency(this, supplierRelationship);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistBilledTo && internalOrganisations.Count() == 1)
            {
                this.BilledTo = internalOrganisations.First();
            }

            if (!this.ExistInvoiceNumber)
            {
                this.InvoiceNumber = this.BilledTo.NextPurchaseInvoiceNumber();
            }

            Organisation supplier = this.BilledFrom as Organisation;
            if (supplier != null)
            {
                if (!this.BilledTo.ActiveSuppliers.Contains(supplier))
                {
                    derivation.Validation.AddError(this, this.Meta.BilledFrom, ErrorMessages.PartyIsNotASupplier);
                }
            }

            this.AppsOnDeriveInvoiceItems(derivation);
            this.AppsOnDeriveInvoiceTotals();
        }

        public void AppsInvoice(PurchaseInvoiceCreateSalesInvoice method)
        {
            var derivation = new Derivation(this.Strategy.Session);

            var salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                .WithPurchaseInvoice(this)
                .WithBilledFrom(this.BilledTo)
                .WithDescription(this.Description)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                .WithVatRegime(this.VatRegime)
                .WithContactPerson(this.ContactPerson)
                .WithBillToContactMechanism(this.BillToCustomerContactMechanism)
                .WithBillToCustomer(this.BillToCustomer)
                .WithDiscountAdjustment(this.DiscountAdjustment)
                .WithSurchargeAdjustment(this.SurchargeAdjustment)
                .WithShippingAndHandlingCharge(this.ShippingAndHandlingCharge)
                .WithFee(this.Fee)
                .WithCustomerReference(this.CustomerReference)
                .WithPaymentMethod(this.BillToCustomerPaymentMethod)
                .WithComment(this.Comment)
                .WithInternalComment(this.InternalComment)
                .Build();

            foreach (SalesOrderItem orderItem in this.PurchaseInvoiceItems)
            {
                var amountAlreadyInvoiced = orderItem.OrderItemBillingsWhereOrderItem.Sum(v => v.Amount);

                var invoiceAmount = (orderItem.QuantityOrdered * orderItem.ActualUnitPrice) - amountAlreadyInvoiced;

                if (invoiceAmount != amountAlreadyInvoiced)
                {
                    var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                        .WithInvoiceItemType(orderItem.InvoiceItemType)
                        .WithActualUnitPrice(orderItem.ActualUnitPrice)
                        .WithProduct(orderItem.Product)
                        .WithQuantity(orderItem.QuantityOrdered)
                        .WithComment(orderItem.Comment)
                        .WithInternalComment(orderItem.InternalComment)
                        .Build();

                    salesInvoice.AddSalesInvoiceItem(invoiceItem);

                    new OrderItemBillingBuilder(this.strategy.Session)
                        .WithQuantity(orderItem.QuantityOrdered)
                        .WithAmount(invoiceAmount)
                        .WithOrderItem(orderItem)
                        .WithSalesInvoiceItem(invoiceItem)
                        .Build();
                }
            }
        }

        public void AppsOnDeriveInvoiceTotals()
        {
            if (this.ExistPurchaseInvoiceItems)
            {
                this.TotalBasePrice = 0;
                this.TotalDiscount = 0;
                this.TotalSurcharge = 0;
                this.TotalVat = 0;
                this.TotalExVat = 0;
                this.TotalIncVat = 0;

                foreach (PurchaseInvoiceItem item in this.PurchaseInvoiceItems)
                {
                    this.TotalBasePrice += item.TotalBasePrice;
                    this.TotalSurcharge += item.TotalSurcharge;
                    this.TotalSurcharge += item.TotalSurcharge;
                    this.TotalVat += item.TotalVat;
                    this.TotalExVat += item.TotalExVat;
                    this.TotalIncVat += item.TotalIncVat;
                }
            }
        }

        public void AppsSearchDataApprove(IDerivation derivation)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).Approved;
        }

        public void AppsReady(IDerivation derivation)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).ReadyForPosting;
        }

        public void AppsCancel(IDerivation derivation)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).Cancelled;
        }

        public void AppsOnDeriveInvoiceItems(IDerivation derivation)
        {
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.PurchaseInvoiceItems)
            {
                purchaseInvoiceItem.AppsOnDerivePrices();
            }
        }
    }
}
