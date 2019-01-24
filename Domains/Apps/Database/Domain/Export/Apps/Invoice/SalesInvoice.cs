// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoice.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Resources;

    public partial class SalesInvoice
    {
        private bool IsDeletable =>
            this.SalesInvoiceState.Equals(new SalesInvoiceStates(this.strategy.Session).ReadyForPosting) &&
            this.AllItemsDeletable() &&
            !this.ExistSalesOrder &&
            !this.ExistPurchaseInvoice &&
            !this.ExistRepeatingSalesInvoiceWhereSource &&
            !this.IsRepeatingInvoice;

        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.SalesInvoice, M.SalesInvoice.SalesInvoiceState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public int PaymentNetDays
        {
            get
            {
                if (this.ExistSalesTerms)
                {
                    foreach (AgreementTerm term in this.SalesTerms)
                    {
                        if (term.TermType.Equals(new InvoiceTermTypes(this.Strategy.Session).PaymentNetDays))
                        {
                            if (int.TryParse(term.TermValue, out var netDays))
                            {
                                return netDays;
                            }
                        }
                    }
                }

                if (this.BillToCustomer?.PaymentNetDays().HasValue == true)
                {
                    return this.BillToCustomer.PaymentNetDays().Value;
                }

                if (this.ExistStore && this.Store.ExistPaymentNetDays)
                {
                    return this.Store.PaymentNetDays;
                }

                return 0;
            }
        }

        public DateTime? DueDate
        {
            get
            {
                if (this.ExistInvoiceDate)
                {
                    return this.InvoiceDate.AddDays(this.PaymentNetDays);
                }

                return null;
            }
        }

        public InvoiceItem[] InvoiceItems => this.SalesInvoiceItems;

        public void SetActualDiscountAmount(decimal amount)
        {
            if (!this.ExistDiscountAdjustment)
            {
                this.DiscountAdjustment = new DiscountAdjustmentBuilder(this.Strategy.Session).Build();
            }

            this.DiscountAdjustment.Amount = amount;
            this.DiscountAdjustment.RemovePercentage();
        }

        public void SetActualDiscountPercentage(decimal percentage)
        {
            if (!this.ExistDiscountAdjustment)
            {
                this.DiscountAdjustment = new DiscountAdjustmentBuilder(this.Strategy.Session).Build();
            }

            this.DiscountAdjustment.Percentage = percentage;
            this.DiscountAdjustment.RemoveAmount();
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSalesInvoiceState)
            {
                this.SalesInvoiceState = new SalesInvoiceStates(this.Strategy.Session).ReadyForPosting;
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = DateTime.UtcNow;
            }

            if (!this.ExistInvoiceDate)
            {
                this.InvoiceDate = DateTime.UtcNow;
            }

            if (this.ExistBillToCustomer)
            {
                this.PreviousBillToCustomer = this.BillToCustomer;
            }

            if (!this.ExistSalesInvoiceType)
            {
                this.SalesInvoiceType = new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistBillToCustomer)
            {
                var customerRelationships = this.BillToCustomer.CustomerRelationshipsWhereCustomer;

                foreach (CustomerRelationship customerRelationship in customerRelationships)
                {
                    if (customerRelationship.FromDate <= DateTime.UtcNow && (!customerRelationship.ExistThroughDate || customerRelationship.ThroughDate >= DateTime.UtcNow))
                    {
                        derivation.AddDependency(this, customerRelationship);
                    }
                }
            }

            if (this.ExistShipToCustomer)
            {
                var customerRelationships = this.ShipToCustomer.CustomerRelationshipsWhereCustomer;

                foreach (CustomerRelationship customerRelationship in customerRelationships)
                {
                    if (customerRelationship.FromDate <= DateTime.UtcNow && (!customerRelationship.ExistThroughDate || customerRelationship.ThroughDate >= DateTime.UtcNow))
                    {
                        derivation.AddDependency(this, customerRelationship);
                    }
                }
            }

            foreach (SalesInvoiceItem invoiceItem in this.InvoiceItems)
            {
                derivation.AddDependency(this, invoiceItem);
            }

            if (this.ExistSalesOrder)
            {
                derivation.AddDependency(this.SalesOrder, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistBilledFrom && internalOrganisations.Count() == 1)
            {
                this.BilledFrom = internalOrganisations.First();
            }

            if (!this.ExistStore && this.ExistBilledFrom)
            {
                var store = new Stores(this.strategy.Session).Extent().FirstOrDefault(v => Equals(v.InternalOrganisation, this.BilledFrom));
                if (store != null)
                {
                    this.Store = store;
                }
            }

            if (!this.ExistInvoiceNumber && this.ExistStore)
            {
                this.InvoiceNumber = this.Store.DeriveNextTemporaryInvoiceNumber();
            }

            if (!this.ExistBilledFromContactMechanism)
            {
                this.BilledFromContactMechanism = this.BilledFrom.ExistBillingAddress ? this.BilledFrom.BillingAddress : this.BilledFrom.GeneralCorrespondence;
            }

            if (!this.ExistBillToContactMechanism && this.ExistBillToCustomer)
            {
                this.BillToContactMechanism = this.BillToCustomer.BillingAddress;
            }

            if (!this.ExistBillToEndCustomerContactMechanism && this.ExistBillToEndCustomer)
            {
                this.BillToEndCustomerContactMechanism = this.BillToEndCustomer.BillingAddress;
            }

            if (!this.ExistShipToEndCustomerAddress && this.ExistShipToEndCustomer)
            {
                this.ShipToEndCustomerAddress = this.ShipToEndCustomer.ShippingAddress;
            }

            if (!this.ExistShipToAddress && this.ExistShipToCustomer)
            {
                this.ShipToAddress = this.ShipToCustomer.ShippingAddress;
            }

            if (!this.ExistCurrency)
            {
                if (this.ExistBillToCustomer && (this.BillToCustomer.ExistPreferredCurrency || this.BillToCustomer.ExistLocale))
                {
                    this.Currency = this.BillToCustomer.ExistPreferredCurrency ? this.BillToCustomer.PreferredCurrency : this.BillToCustomer.Locale.Country.Currency;
                }
                else
                {
                    this.Currency = this.BilledFrom.ExistPreferredCurrency ?
                        this.BilledFrom.PreferredCurrency :
                        this.Strategy.Session.GetSingleton().DefaultLocale.Country.Currency;
                }
            }

            this.IsRepeatingInvoice = this.ExistRepeatingSalesInvoiceWhereSource && (!this.RepeatingSalesInvoiceWhereSource.ExistFinalExecutionDate || this.RepeatingSalesInvoiceWhereSource.FinalExecutionDate.Value.Date >= this.strategy.Session.Now().Date);

            this.AppsOnDeriveInvoiceItems(derivation);

            this.SalesInvoiceItems = this.SalesInvoiceItems.ToArray();

            this.AppsOnDeriveLocale(derivation);
            this.AppsOnDeriveInvoiceTotals(derivation);
            this.AppsOnDeriveCustomers(derivation);
            this.AppsOnDeriveSalesReps(derivation);
            this.AppsOnDeriveAmountPaid(derivation);

            if (this.ExistBillToCustomer && !this.BillToCustomer.AppsIsActiveCustomer(this.BilledFrom, this.InvoiceDate))
            {
                derivation.Validation.AddError(this, M.SalesInvoice.BillToCustomer, ErrorMessages.PartyIsNotACustomer);
            }

            if (this.ExistShipToCustomer && !this.ShipToCustomer.AppsIsActiveCustomer(this.BilledFrom, this.InvoiceDate))
            {
                derivation.Validation.AddError(this, M.SalesInvoice.ShipToCustomer, ErrorMessages.PartyIsNotACustomer);
            }

            this.DeriveCurrentPaymentStatus(derivation);
            this.DeriveCurrentObjectState(derivation);

            this.PreviousBillToCustomer = this.BillToCustomer;
            this.PreviousShipToCustomer = this.ShipToCustomer;

            //this.AppsOnDeriveRevenues(derivation);

            this.ResetPrintDocument();
        }

        private void DeriveCurrentPaymentStatus(IDerivation derivation)
        {
            var itemsPaid = false;
            var itemsPartiallyPaid = false;
            var itemsNotPaid = false;

            foreach (SalesInvoiceItem invoiceItem in this.SalesInvoiceItems)
            {
                if (invoiceItem.SalesInvoiceItemState.Equals(new SalesInvoiceItemStates(this.Strategy.Session).PartiallyPaid))
                {
                    itemsPartiallyPaid = true;
                }

                if (invoiceItem.SalesInvoiceItemState.Equals(new SalesInvoiceItemStates(this.Strategy.Session).Paid))
                {
                    itemsPaid = true;
                }

                if (invoiceItem.SalesInvoiceItemState.Equals(new SalesInvoiceItemStates(this.Strategy.Session).Sent))
                {
                    itemsNotPaid = true;
                }
            }

            if (itemsPaid && !itemsNotPaid && !itemsPartiallyPaid && (!this.SalesInvoiceState.Equals(new SalesInvoiceStates(this.Strategy.Session).Paid)))
            {
                this.SalesInvoiceState = new SalesInvoiceStates(this.Strategy.Session).Paid;
            }

            if ((itemsPartiallyPaid || (itemsPaid && itemsNotPaid)) && (!this.SalesInvoiceState.Equals(new SalesInvoiceStates(this.Strategy.Session).PartiallyPaid)))
            {
                this.SalesInvoiceState = new SalesInvoiceStates(this.Strategy.Session).PartiallyPaid;
            }

            if (this.ExistAmountPaid)
            {
                if (this.AmountPaid > 0 && this.AmountPaid < this.TotalIncVat
                    && (!this.SalesInvoiceState.Equals(new SalesInvoiceStates(this.Strategy.Session).PartiallyPaid)))
                {
                    this.SalesInvoiceState = new SalesInvoiceStates(this.Strategy.Session).PartiallyPaid;
                }

                if (this.AmountPaid > 0 && this.AmountPaid == this.TotalIncVat
                    && (!this.SalesInvoiceState.Equals(new SalesInvoiceStates(this.Strategy.Session).Paid)))
                {
                    this.SalesInvoiceState = new SalesInvoiceStates(this.Strategy.Session).Paid;
                }
            }
        }

        private void DeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.SalesInvoiceState.Equals(new SalesInvoiceStates(this.Strategy.Session).Paid))
            {
                this.AppsOnDeriveSalesOrderPaymentStatus(derivation);
            }
        }

        public void AppsSend(SalesInvoiceSend method)
        {
            if (object.Equals(this.SalesInvoiceType, new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice))
            {
                this.InvoiceNumber = this.Store.DeriveNextInvoiceNumber(this.InvoiceDate.Year);
            }

            if (object.Equals(this.SalesInvoiceType, new SalesInvoiceTypes(this.Strategy.Session).CreditNote))
            {
                this.InvoiceNumber = this.Store.DeriveNextCreditNoteNumber(this.InvoiceDate.Year);
            }

            this.SalesInvoiceState = new SalesInvoiceStates(this.Strategy.Session).Sent;

            if (this.BillToCustomer is Organisation organisation && organisation.IsInternalOrganisation)
            {
                var purchaseInvoice = new PurchaseInvoiceBuilder(this.Strategy.Session)
                    .WithBilledFrom(this.BilledFrom)
                    .WithBilledFromContactPerson(this.BilledFromContactPerson)
                    .WithBilledTo((InternalOrganisation)this.BillToCustomer)
                    .WithBilledToContactPerson(this.BillToContactPerson)
                    .WithBillToEndCustomer(this.BillToEndCustomer)
                    .WithBillToEndCustomerContactMechanism(this.BillToEndCustomerContactMechanism)
                    .WithBillToEndCustomerContactPerson(this.BillToEndCustomerContactPerson)
                    .WithBillToCustomerPaymentMethod(this.PaymentMethod)
                    .WithShipToCustomer(this.ShipToCustomer)
                    .WithShipToCustomerAddress(this.ShipToAddress)
                    .WithShipToCustomerContactPerson(this.ShipToContactPerson)
                    .WithShipToEndCustomer(this.ShipToEndCustomer)
                    .WithShipToEndCustomerAddress(this.ShipToEndCustomerAddress)
                    .WithShipToEndCustomerContactPerson(this.ShipToEndCustomerContactPerson)
                    .WithDescription(this.Description)
                    .WithInvoiceDate(DateTime.UtcNow)
                    .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Strategy.Session).PurchaseInvoice)
                    .WithVatRegime(this.VatRegime)
                    .WithDiscountAdjustment(this.DiscountAdjustment)
                    .WithSurchargeAdjustment(this.SurchargeAdjustment)
                    .WithShippingAndHandlingCharge(this.ShippingAndHandlingCharge)
                    .WithFee(this.Fee)
                    .WithCustomerReference(this.CustomerReference)
                    .WithComment(this.Comment)
                    .WithInternalComment(this.InternalComment)
                    .Build();

                foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
                {
                    var invoiceItem = new PurchaseInvoiceItemBuilder(this.Strategy.Session)
                        .WithInvoiceItemType(salesInvoiceItem.InvoiceItemType)
                        .WithActualUnitPrice(salesInvoiceItem.ActualUnitPrice)
                        .WithProduct(salesInvoiceItem.Product)
                        .WithQuantity(salesInvoiceItem.Quantity)
                        .WithComment(salesInvoiceItem.Comment)
                        .WithInternalComment(salesInvoiceItem.InternalComment)
                        .Build();

                    purchaseInvoice.AddPurchaseInvoiceItem(invoiceItem);
                }
            }
        }

        public void AppsWriteOff(SalesInvoiceWriteOff method)
        {
            this.SalesInvoiceState = new SalesInvoiceStates(this.Strategy.Session).WrittenOff;
        }

        public void AppsReopen(SalesInvoiceReopen method)
        {
            this.SalesInvoiceState = this.PreviousSalesInvoiceState;
        }

        public void AppsCancelInvoice(SalesInvoiceCancelInvoice method)
        {
            this.SalesInvoiceState = new SalesInvoiceStates(this.Strategy.Session).Cancelled;
        }

        public SalesInvoice AppsCopy(SalesInvoiceCopy method)
        {
            var salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                .WithSalesOrder(this.SalesOrder)
                .WithPurchaseInvoice(this.PurchaseInvoice)
                .WithBilledFrom(this.BilledFrom)
                .WithBilledFromContactMechanism(this.BilledFromContactMechanism)
                .WithBilledFromContactPerson(this.BilledFromContactPerson)
                .WithBillToCustomer(this.BillToCustomer)
                .WithBillToContactMechanism(this.BillToContactMechanism)
                .WithBillToContactPerson(this.BillToContactPerson)
                .WithBillToEndCustomer(this.BillToEndCustomer)
                .WithBillToEndCustomerContactMechanism(this.BillToEndCustomerContactMechanism)
                .WithBillToEndCustomerContactPerson(this.BillToEndCustomerContactPerson)
                .WithShipToCustomer(this.ShipToCustomer)
                .WithShipToAddress(this.ShipToAddress)
                .WithShipToContactPerson(this.ShipToContactPerson)
                .WithShipToEndCustomer(this.ShipToEndCustomer)
                .WithShipToEndCustomerAddress(this.ShipToEndCustomerAddress)
                .WithShipToEndCustomerContactPerson(this.ShipToEndCustomerContactPerson)
                .WithDescription(this.Description)
                .WithStore(this.Store)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithSalesChannel(this.SalesChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                .WithVatRegime(this.VatRegime)
                .WithDiscountAdjustment(this.DiscountAdjustment)
                .WithSurchargeAdjustment(this.SurchargeAdjustment)
                .WithShippingAndHandlingCharge(this.ShippingAndHandlingCharge)
                .WithFee(this.Fee)
                .WithCustomerReference(this.CustomerReference)
                .WithPaymentMethod(this.PaymentMethod)
                .WithComment(this.Comment)
                .WithInternalComment(this.InternalComment)
                .WithMessage(this.Message)
                .WithBillingAccount(this.BillingAccount)
                .Build();

            foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
            {
                var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                    .WithInvoiceItemType(salesInvoiceItem.InvoiceItemType)
                    .WithActualUnitPrice(salesInvoiceItem.ActualUnitPrice)
                    .WithProduct(salesInvoiceItem.Product)
                    .WithProductFeature(salesInvoiceItem.ProductFeature)
                    .WithQuantity(salesInvoiceItem.Quantity)
                    .WithDescription(salesInvoiceItem.Description)
                    .WithSerialisedItem(salesInvoiceItem.SerialisedItem)
                    .WithComment(salesInvoiceItem.Comment)
                    .WithDetails(salesInvoiceItem.Details)
                    .WithInternalComment(salesInvoiceItem.InternalComment)
                    .WithMessage(salesInvoiceItem.Message)
                    .WithFacility(salesInvoiceItem.Facility)
                    .Build();

                salesInvoice.AddSalesInvoiceItem(invoiceItem);

                foreach (SalesTerm salesTerm in salesInvoiceItem.SalesTerms)
                {
                    if (salesTerm.GetType().Name == typeof(IncoTerm).Name)
                    {
                        salesInvoiceItem.AddSalesTerm(new IncoTermBuilder(this.strategy.Session)
                            .WithTermType(salesTerm.TermType)
                            .WithTermValue(salesTerm.TermValue)
                            .WithDescription(salesTerm.Description)
                            .Build());
                    }

                    if (salesTerm.GetType().Name == typeof(InvoiceTerm).Name)
                    {
                        salesInvoiceItem.AddSalesTerm(new InvoiceTermBuilder(this.strategy.Session)
                            .WithTermType(salesTerm.TermType)
                            .WithTermValue(salesTerm.TermValue)
                            .WithDescription(salesTerm.Description)
                            .Build());
                    }

                    if (salesTerm.GetType().Name == typeof(OrderTerm).Name)
                    {
                        salesInvoiceItem.AddSalesTerm(new OrderTermBuilder(this.strategy.Session)
                            .WithTermType(salesTerm.TermType)
                            .WithTermValue(salesTerm.TermValue)
                            .WithDescription(salesTerm.Description)
                            .Build());
                    }
                }
            }

            foreach (SalesTerm salesTerm in this.SalesTerms)
            {
                if (salesTerm.GetType().Name == typeof(IncoTerm).Name)
                {
                    salesInvoice.AddSalesTerm(new IncoTermBuilder(this.strategy.Session)
                                                .WithTermType(salesTerm.TermType)
                                                .WithTermValue(salesTerm.TermValue)
                                                .WithDescription(salesTerm.Description)
                                                .Build());
                }

                if (salesTerm.GetType().Name == typeof(InvoiceTerm).Name)
                {
                    salesInvoice.AddSalesTerm(new InvoiceTermBuilder(this.strategy.Session)
                        .WithTermType(salesTerm.TermType)
                        .WithTermValue(salesTerm.TermValue)
                        .WithDescription(salesTerm.Description)
                        .Build());
                }

                if (salesTerm.GetType().Name == typeof(OrderTerm).Name)
                {
                    salesInvoice.AddSalesTerm(new OrderTermBuilder(this.strategy.Session)
                        .WithTermType(salesTerm.TermType)
                        .WithTermValue(salesTerm.TermValue)
                        .WithDescription(salesTerm.Description)
                        .Build());
                }
            }

            return salesInvoice;
        }

        public SalesInvoice AppsCredit(SalesInvoiceCredit method)
        {
            var salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                .WithSalesOrder(this.SalesOrder)
                .WithPurchaseInvoice(this.PurchaseInvoice)
                .WithBilledFrom(this.BilledFrom)
                .WithBilledFromContactMechanism(this.BilledFromContactMechanism)
                .WithBilledFromContactPerson(this.BilledFromContactPerson)
                .WithBillToCustomer(this.BillToCustomer)
                .WithBillToContactMechanism(this.BillToContactMechanism)
                .WithBillToContactPerson(this.BillToContactPerson)
                .WithBillToEndCustomer(this.BillToEndCustomer)
                .WithBillToEndCustomerContactMechanism(this.BillToEndCustomerContactMechanism)
                .WithBillToEndCustomerContactPerson(this.BillToEndCustomerContactPerson)
                .WithShipToCustomer(this.ShipToCustomer)
                .WithShipToAddress(this.ShipToAddress)
                .WithShipToContactPerson(this.ShipToContactPerson)
                .WithShipToEndCustomer(this.ShipToEndCustomer)
                .WithShipToEndCustomerAddress(this.ShipToEndCustomerAddress)
                .WithShipToEndCustomerContactPerson(this.ShipToEndCustomerContactPerson)
                .WithDescription(this.Description)
                .WithStore(this.Store)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithSalesChannel(this.SalesChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).CreditNote)
                .WithVatRegime(this.VatRegime)
                .WithDiscountAdjustment(this.DiscountAdjustment)
                .WithSurchargeAdjustment(this.SurchargeAdjustment)
                .WithShippingAndHandlingCharge(this.ShippingAndHandlingCharge)
                .WithCustomerReference(this.CustomerReference)
                .WithPaymentMethod(this.PaymentMethod)
                .WithBillingAccount(this.BillingAccount)
                .Build();

            if (this.ExistDiscountAdjustment)
            {
                salesInvoice.DiscountAdjustment = new DiscountAdjustmentBuilder(this.strategy.Session).WithAmount(this.DiscountAdjustment.Amount * -1).Build();
            }

            if (this.ExistSurchargeAdjustment)
            {
                salesInvoice.SurchargeAdjustment = new SurchargeAdjustmentBuilder(this.strategy.Session).WithAmount(this.SurchargeAdjustment.Amount * -1).Build();
            }

            foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
            {
                var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                    .WithInvoiceItemType(salesInvoiceItem.InvoiceItemType)
                    .WithActualUnitPrice(salesInvoiceItem.ActualUnitPrice * -1)
                    .WithProduct(salesInvoiceItem.Product)
                    .WithProductFeature(salesInvoiceItem.ProductFeature)
                    .WithQuantity(salesInvoiceItem.Quantity)
                    .WithDescription(salesInvoiceItem.Description)
                    .WithSerialisedItem(salesInvoiceItem.SerialisedItem)
                    .WithComment(salesInvoiceItem.Comment)
                    .WithDetails(salesInvoiceItem.Details)
                    .WithInternalComment(salesInvoiceItem.InternalComment)
                    .WithFacility(salesInvoiceItem.Facility)
                    .WithAssignedVatRegime(salesInvoiceItem.AssignedVatRegime)
                    .Build();

                salesInvoice.AddSalesInvoiceItem(invoiceItem);
            }

            return salesInvoice;
        }

        public void AppsOnDeriveLocale(IDerivation derivation)
        {
            if (this.ExistBillToCustomer && this.BillToCustomer.ExistLocale)
            {
                this.Locale = this.BillToCustomer.Locale;
            }
            else
            {
                this.Locale = this.Strategy.Session.GetSingleton().DefaultLocale;
            }
        }

        public void AppsOnDeriveSalesReps(IDerivation derivation)
        {
            this.RemoveSalesReps();
            foreach (SalesInvoiceItem item in this.SalesInvoiceItems)
            {
                foreach (Person salesRep in item.SalesReps)
                {
                    this.AddSalesRep(salesRep);
                }
            }
        }

        public void AppsOnDeriveAmountPaid(IDerivation derivation)
        {
            this.AmountPaid = this.AdvancePayment;
            foreach (PaymentApplication paymentApplication in this.PaymentApplicationsWhereInvoice)
            {
                this.AmountPaid += paymentApplication.AmountApplied;
            }

            //// Perhaps payments are recorded at the item level.
            if (this.AmountPaid == 0)
            {
                foreach (var invoiceItem in this.InvoiceItems)
                {
                    this.AmountPaid += invoiceItem.AmountPaid;
                }
            }
        }

        public void AppsOnDeriveInvoiceTotals(IDerivation derivation)
        {
            this.TotalBasePrice = 0;
            this.TotalDiscount = 0;
            this.TotalSurcharge = 0;
            this.TotalFee = 0;
            this.TotalShippingAndHandling = 0;
            this.TotalVat = 0;
            this.TotalExVat = 0;
            this.TotalIncVat = 0;
            this.TotalPurchasePrice = 0;
            this.TotalListPrice = 0;
            this.MaintainedMarkupPercentage = 0;
            this.InitialMarkupPercentage = 0;
            this.MaintainedProfitMargin = 0;
            this.InitialProfitMargin = 0;

            foreach (SalesInvoiceItem item in this.SalesInvoiceItems)
            {
                this.TotalBasePrice += item.TotalBasePrice;
                this.TotalDiscount += item.TotalDiscount;
                this.TotalSurcharge += item.TotalSurcharge;
                this.TotalVat += item.TotalVat;
                this.TotalExVat += item.TotalExVat;
                this.TotalIncVat += item.TotalIncVat;
                this.TotalPurchasePrice += item.UnitPurchasePrice;
                this.TotalListPrice += item.CalculatedUnitPrice;
            }

            this.DeriveDiscountAdjustments(derivation);
            this.DeriveSurchargeAdjustments(derivation);
            this.DeriveTotalFee(derivation);
            this.DeriveTotalShippingAndHandling(derivation);
            this.AppsOnDeriveMarkupAndProfitMargin(derivation);
        }

        private void DeriveDiscountAdjustments(IDerivation derivation)
        {
            if (this.DiscountAdjustment != null)
            {
                decimal discount = this.DiscountAdjustment.Percentage.HasValue ? Math.Round((this.TotalExVat * this.DiscountAdjustment.Percentage.Value) / 100, 2) : this.DiscountAdjustment.Amount.HasValue ? this.DiscountAdjustment.Amount.Value : 0;

                this.TotalDiscount += discount;
                this.TotalExVat -= discount;

                if (this.ExistVatRegime)
                {
                    decimal vat = Math.Round((discount * this.VatRegime.VatRate.Rate) / 100, 2);
                    this.TotalVat -= vat;
                    this.TotalIncVat -= discount + vat;
                }
            }
        }

        private void DeriveSurchargeAdjustments(IDerivation derivation)
        {
            if (this.ExistSurchargeAdjustment)
            {
                decimal surcharge = this.SurchargeAdjustment.Percentage.HasValue ? Math.Round((this.TotalExVat * this.SurchargeAdjustment.Percentage.Value) / 100, 2) : this.SurchargeAdjustment.Amount.HasValue ? this.SurchargeAdjustment.Amount.Value : 0;

                this.TotalSurcharge += surcharge;
                this.TotalExVat += surcharge;

                if (this.ExistVatRegime)
                {
                    decimal vat = Math.Round((surcharge * this.VatRegime.VatRate.Rate) / 100, 2);
                    this.TotalVat += vat;
                    this.TotalIncVat += surcharge + vat;
                }
            }
        }

        private void DeriveTotalFee(IDerivation derivation)
        {
            if (this.ExistFee)
            {
                decimal fee = this.Fee.Percentage.HasValue ? Math.Round((this.TotalExVat * this.Fee.Percentage.Value) / 100, 2) : this.Fee.Amount.HasValue ? this.Fee.Amount.Value : 0;

                this.TotalFee += fee;
                this.TotalExVat += fee;

                if (this.Fee.ExistVatRate)
                {
                    decimal vat = Math.Round((fee * this.Fee.VatRate.Rate) / 100, 2);
                    this.TotalVat += vat;
                    this.TotalIncVat += fee + vat;
                }
            }
        }

        private void DeriveTotalShippingAndHandling(IDerivation derivation)
        {
            if (this.ExistShippingAndHandlingCharge)
            {
                decimal shipping = this.ShippingAndHandlingCharge.Percentage.HasValue ?
                    Math.Round((this.TotalExVat * this.ShippingAndHandlingCharge.Percentage.Value) / 100, 2) : this.ShippingAndHandlingCharge.Amount.HasValue ? this.ShippingAndHandlingCharge.Amount.Value : 0;

                this.TotalShippingAndHandling += shipping;
                this.TotalExVat += shipping;

                if (this.ShippingAndHandlingCharge.ExistVatRate)
                {
                    decimal vat = Math.Round((shipping * this.ShippingAndHandlingCharge.VatRate.Rate) / 100, 2);

                    this.TotalVat += vat;
                    this.TotalIncVat += shipping + vat;
                }
            }
        }

        public void AppsOnDeriveMarkupAndProfitMargin(IDerivation derivation)
        {
            //// Only take into account items for which there is data at the item level.
            //// Skip negative sales.
            decimal totalPurchasePrice = 0;
            decimal totalUnitBasePrice = 0;
            decimal totalListPrice = 0;

            foreach (SalesInvoiceItem item in this.SalesInvoiceItems)
            {
                if (item.TotalExVat > 0 && item.InitialMarkupPercentage > 0)
                {
                    totalPurchasePrice += item.UnitPurchasePrice;
                    totalUnitBasePrice += item.UnitBasePrice;
                    totalListPrice += item.CalculatedUnitPrice;
                }
            }

            if (totalPurchasePrice != 0 && totalListPrice != 0 && totalUnitBasePrice != 0)
            {
                this.InitialMarkupPercentage = Math.Round(((totalUnitBasePrice / totalPurchasePrice) - 1) * 100, 2);
                this.MaintainedMarkupPercentage = Math.Round(((totalListPrice / totalPurchasePrice) - 1) * 100, 2);

                this.InitialProfitMargin = Math.Round(((totalUnitBasePrice - totalPurchasePrice) / totalUnitBasePrice) * 100, 2);
                this.MaintainedProfitMargin = Math.Round(((totalListPrice - totalPurchasePrice) / totalListPrice) * 100, 2);
            }
        }

        public void AppsOnDeriveSalesOrderPaymentStatus(IDerivation derivation)
        {
            foreach (SalesInvoiceItem invoiceItem in this.SalesInvoiceItems)
            {
                foreach (ShipmentItemBilling shipmentItemBilling in invoiceItem.ShipmentItemBillingsWhereInvoiceItem)
                {
                    foreach (OrderShipment orderShipment in shipmentItemBilling.ShipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        if (orderShipment.OrderItem is SalesOrderItem salesOrderItem)
                        {
                            salesOrderItem.AppsOnDerivePaymentState(derivation);
                            salesOrderItem.SalesOrderWhereSalesOrderItem.OnDerive(x => x.WithDerivation(derivation));
                        }
                    }
                }

                foreach (OrderItemBilling orderItemBilling in invoiceItem.OrderItemBillingsWhereInvoiceItem)
                {
                    foreach (OrderShipment orderShipment in orderItemBilling.OrderItem.OrderShipmentsWhereOrderItem)
                    {
                        if (orderShipment.OrderItem is SalesOrderItem salesOrderItem)
                        {
                            salesOrderItem.AppsOnDerivePaymentState(derivation);
                            salesOrderItem.SalesOrderWhereSalesOrderItem.OnDerive(x => x.WithDerivation(derivation));
                        }
                    }
                }
            }
        }

        public void AppsOnDeriveCustomers(IDerivation derivation)
        {
            this.RemoveCustomers();
            if (this.ExistBillToCustomer && !this.Customers.Contains(this.BillToCustomer))
            {
                this.AddCustomer(this.BillToCustomer);
            }

            if (this.ExistShipToCustomer && !this.Customers.Contains(this.ShipToCustomer))
            {
                this.AddCustomer(this.ShipToCustomer);
            }
        }

        public void AppsOnDeriveInvoiceItems(IDerivation derivation)
        {
            var quantityInvoicedByProduct = new Dictionary<Product, decimal>();
            var totalBasePriceByProduct = new Dictionary<Product, decimal>();

            var invoiceFromOrder = true;
            foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
            {
                salesInvoiceItem.AppsOnDeriveVatRegime(derivation);
                salesInvoiceItem.AppsOnDeriveSalesRep(derivation);
                salesInvoiceItem.AppsOnDeriveCurrentObjectState(derivation);
                salesInvoiceItem.OnDerive(x => x.WithDerivation(derivation));

                if (!salesInvoiceItem.ExistShipmentItemBillingsWhereInvoiceItem &&
                    !salesInvoiceItem.ExistOrderItemBillingsWhereInvoiceItem)
                {
                    invoiceFromOrder = false;
                    salesInvoiceItem.AppsOnDerivePrices(derivation, 0, 0);
                    salesInvoiceItem.AppsOnDeriveCurrentPaymentStatus(derivation);

                    if (salesInvoiceItem.ExistProduct)
                    {
                        if (!quantityInvoicedByProduct.ContainsKey(salesInvoiceItem.Product))
                        {
                            quantityInvoicedByProduct.Add(salesInvoiceItem.Product, salesInvoiceItem.Quantity);
                            totalBasePriceByProduct.Add(salesInvoiceItem.Product, salesInvoiceItem.TotalBasePrice);
                        }
                        else
                        {
                            quantityInvoicedByProduct[salesInvoiceItem.Product] += salesInvoiceItem.Quantity;
                            totalBasePriceByProduct[salesInvoiceItem.Product] += salesInvoiceItem.TotalBasePrice;
                        }
                    }
                }
            }

            if (!invoiceFromOrder)
            {
                foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
                {
                    decimal quantity = 0;
                    decimal totalBasePrice = 0;

                    if (salesInvoiceItem.ExistProduct)
                    {
                        quantityInvoicedByProduct.TryGetValue(salesInvoiceItem.Product, out quantity);
                        totalBasePriceByProduct.TryGetValue(salesInvoiceItem.Product, out totalBasePrice);
                    }

                    salesInvoiceItem.AppsOnDerivePrices(derivation, quantity, totalBasePrice);
                    salesInvoiceItem.AppsOnDeriveCurrentPaymentStatus(derivation);
                }
            }
        }

        public void AppsDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
                if (this.ExistShippingAndHandlingCharge)
                {
                    this.ShippingAndHandlingCharge.Delete();
                }

                if (this.ExistFee)
                {
                    this.Fee.Delete();
                }

                if (this.ExistDiscountAdjustment)
                {
                    this.DiscountAdjustment.Delete();
                }

                if (this.ExistSurchargeAdjustment)
                {
                    this.SurchargeAdjustment.Delete();
                }

                foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
                {
                    salesInvoiceItem.Delete();
                }

                foreach (SalesTerm salesTerm in this.SalesTerms)
                {
                    salesTerm.Delete();
                }
            }
        }

        public void AppsPrint(PrintablePrint method)
        {
            if (!method.IsPrinted)
            {
                var singleton = this.Strategy.Session.GetSingleton();
                var logo = this.BilledFrom?.ExistLogoImage == true ?
                               this.BilledFrom.LogoImage.MediaContent.Data :
                               singleton.LogoImage.MediaContent.Data;

                var images = new Dictionary<string, byte[]>
                                 {
                                     { "Logo", logo },
                                 };

                if (this.ExistInvoiceNumber)
                {
                    var session = this.Strategy.Session;
                    var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                    var barcode = barcodeService.Generate(this.InvoiceNumber, BarcodeType.CODE_128, 320, 80);
                    images.Add("Barcode", barcode);
                }

                var printModel = new Print.SalesInvoiceModel.Model(this);
                this.RenderPrintDocument(this.BilledFrom?.SalesInvoiceTemplate, printModel, images);

                this.PrintDocument.Media.FileName = $"{this.InvoiceNumber}.odt";
            }
        }

        private bool AllItemsDeletable()
        {
            foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
            {
                if (!salesInvoiceItem.IsDeletable)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
