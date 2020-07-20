// <copyright file="SalesOrderTransfer.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;
    using Allors.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Resources;

    public partial class SalesOrderTransfer
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (changeSet.HasChangedRole(this, this.Meta.From))
                {
                    iteration.AddDependency(this.From, this);
                    iteration.Mark(this.From);
                }

                if (changeSet.HasChangedRole(this, this.Meta.To))
                {
                    iteration.AddDependency(this.To, this);
                    iteration.Mark(this.To);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var session = this.Session();

            if (this.ExistFrom && this.ExistInternalOrganisation && !this.ExistTo)
            {
                var acl = new AccessControlLists(session.GetUser())[this.From];
                if (acl.CanExecute(M.SalesOrder.DoTransfer))
                {
                    this.To = this.Transfer();
                    this.From.SalesOrderState = new SalesOrderStates(session).Transferred;
                }
            }
        }

        private SalesOrder Transfer()
        {
            var salesOrder = new SalesOrderBuilder(this.Strategy.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithShipToCustomer(this.From.ShipToCustomer)
                .WithShipToAddress(this.From.ShipToAddress)
                .WithShipToContactPerson(this.ShipToContactPerson)
                .WithShipToEndCustomer(this.ShipToEndCustomer)
                .WithShipToEndCustomerAddress(this.ShipToEndCustomerAddress)
                .WithShipToEndCustomerContactPerson(this.ShipToEndCustomerContactPerson)
                .WithDescription(this.Description)
                .WithStore(this.Store)
                .WithInvoiceDate(this.Session().Now())
                .WithSalesChannel(this.SalesChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                .WithVatRegime(this.VatRegime)
                .WithIrpfRegime(this.IrpfRegime)
                .WithCustomerReference(this.CustomerReference)
                .WithPaymentMethod(this.PaymentMethod)
                .WithComment(this.Comment)
                .WithInternalComment(this.InternalComment)
                .WithMessage(this.Message)
                .WithBillingAccount(this.BillingAccount)
                .Build();

            foreach (OrderAdjustment orderAdjustment in this.OrderAdjustments)
            {
                OrderAdjustment newAdjustment = null;
                if (orderAdjustment.GetType().Name.Equals(typeof(DiscountAdjustment).Name))
                {
                    newAdjustment = new DiscountAdjustmentBuilder(this.Session()).Build();
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(SurchargeAdjustment).Name))
                {
                    newAdjustment = new SurchargeAdjustmentBuilder(this.Session()).Build();
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(Fee).Name))
                {
                    newAdjustment = new FeeBuilder(this.Session()).Build();
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(ShippingAndHandlingCharge).Name))
                {
                    newAdjustment = new ShippingAndHandlingChargeBuilder(this.Session()).Build();
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(MiscellaneousCharge).Name))
                {
                    newAdjustment = new MiscellaneousChargeBuilder(this.Session()).Build();
                }

                newAdjustment.Amount ??= orderAdjustment.Amount;
                newAdjustment.Percentage ??= orderAdjustment.Percentage;
                salesInvoice.AddOrderAdjustment(newAdjustment);
            }

            foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
            {
                var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                    .WithInvoiceItemType(salesInvoiceItem.InvoiceItemType)
                    .WithAssignedUnitPrice(salesInvoiceItem.AssignedUnitPrice)
                    .WithAssignedVatRegime(salesInvoiceItem.AssignedVatRegime)
                    .WithAssignedIrpfRegime(salesInvoiceItem.AssignedIrpfRegime)
                    .WithProduct(salesInvoiceItem.Product)
                    .WithQuantity(salesInvoiceItem.Quantity)
                    .WithDescription(salesInvoiceItem.Description)
                    .WithSerialisedItem(salesInvoiceItem.SerialisedItem)
                    .WithNextSerialisedItemAvailability(salesInvoiceItem.NextSerialisedItemAvailability)
                    .WithComment(salesInvoiceItem.Comment)
                    .WithInternalComment(salesInvoiceItem.InternalComment)
                    .WithMessage(salesInvoiceItem.Message)
                    .WithFacility(salesInvoiceItem.Facility)
                    .Build();

                invoiceItem.ProductFeatures = salesInvoiceItem.ProductFeatures;
                salesInvoice.AddSalesInvoiceItem(invoiceItem);

                foreach (SalesTerm salesTerm in salesInvoiceItem.SalesTerms)
                {
                    if (salesTerm.GetType().Name == typeof(IncoTerm).Name)
                    {
                        salesInvoiceItem.AddSalesTerm(new IncoTermBuilder(this.Strategy.Session)
                            .WithTermType(salesTerm.TermType)
                            .WithTermValue(salesTerm.TermValue)
                            .WithDescription(salesTerm.Description)
                            .Build());
                    }

                    if (salesTerm.GetType().Name == typeof(InvoiceTerm).Name)
                    {
                        salesInvoiceItem.AddSalesTerm(new InvoiceTermBuilder(this.Strategy.Session)
                            .WithTermType(salesTerm.TermType)
                            .WithTermValue(salesTerm.TermValue)
                            .WithDescription(salesTerm.Description)
                            .Build());
                    }

                    if (salesTerm.GetType().Name == typeof(OrderTerm).Name)
                    {
                        salesInvoiceItem.AddSalesTerm(new OrderTermBuilder(this.Strategy.Session)
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
                    salesInvoice.AddSalesTerm(new IncoTermBuilder(this.Strategy.Session)
                                                .WithTermType(salesTerm.TermType)
                                                .WithTermValue(salesTerm.TermValue)
                                                .WithDescription(salesTerm.Description)
                                                .Build());
                }

                if (salesTerm.GetType().Name == typeof(InvoiceTerm).Name)
                {
                    salesInvoice.AddSalesTerm(new InvoiceTermBuilder(this.Strategy.Session)
                        .WithTermType(salesTerm.TermType)
                        .WithTermValue(salesTerm.TermValue)
                        .WithDescription(salesTerm.Description)
                        .Build());
                }

                if (salesTerm.GetType().Name == typeof(OrderTerm).Name)
                {
                    salesInvoice.AddSalesTerm(new OrderTermBuilder(this.Strategy.Session)
                        .WithTermType(salesTerm.TermType)
                        .WithTermValue(salesTerm.TermValue)
                        .WithDescription(salesTerm.Description)
                        .Build());
                }
            }

            return salesOrder;
        }
    }
}
