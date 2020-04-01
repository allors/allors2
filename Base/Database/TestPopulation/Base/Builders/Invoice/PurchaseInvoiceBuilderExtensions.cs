// <copyright file="PurchaseInvoiceBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors.Domain.TestPopulation
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class PurchaseInvoiceBuilderExtensions
    {
        // TODO: Replace erroneous properties
        public static PurchaseInvoiceBuilder WithSalesInternalInvoiceDefaults(this PurchaseInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var internalOrganisations = @this.Session.Extent<Organisation>();

            var otherInternalOrganization = internalOrganisations.Except(new List<Organisation> { internalOrganisation }).FirstOrDefault();

            var endCustomer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);
            var salesInvoiceItem_NonGSE = new SalesInvoiceItemBuilder(@this.Session).WithDefaults(internalOrganisation).Build();
            var salesInvoiceItem_GSE = new SalesInvoiceItemBuilder(@this.Session).WithGSEDefaults(internalOrganisation).Build();

            var salesInvoiceType = faker.Random.ListItem(@this.Session.Extent<SalesInvoiceType>());
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16).ToUpper());
            @this.WithBilledFrom(internalOrganisation);
            @this.WithBilledFromContactMechanism(internalOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBilledFromContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            /*@this.WithBillToCustomer(otherInternalOrganization.CurrentContacts.FirstOrDefault());
            @this.WithBillToContactMechanism(otherInternalOrganization.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToContactPerson(otherInternalOrganization.CurrentContacts.FirstOrDefault());*/
            @this.WithBillToEndCustomer(endCustomer);
            @this.WithBillToEndCustomerContactMechanism(endCustomer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToEndCustomerContactPerson(endCustomer.CurrentContacts.FirstOrDefault());
            @this.WithShipToEndCustomer(endCustomer);
            @this.WithShipToEndCustomerAddress(endCustomer.ShippingAddress);
            @this.WithShipToEndCustomerContactPerson(endCustomer.CurrentContacts.FirstOrDefault());
            @this.WithShipToCustomer(otherInternalOrganization);
            /*@this.WithShipToAddress(otherInternalOrganization.ShippingAddress);
            @this.WithShipToContactPerson(otherInternalOrganization.CurrentContacts.FirstOrDefault());
            @this.WithSalesInvoiceType(salesInvoiceType);
            @this.WithTotalListPrice(faker.Random.Decimal());
            @this.WithPaymentMethod(paymentMethod);
            @this.WithSalesInvoiceItem(salesInvoiceItem_NonGSE).Build();
            @this.WithSalesInvoiceItem(salesInvoiceItem_GSE).Build();
            @this.WithAdvancePayment(faker.Random.Decimal());
            @this.WithPaymentDays(faker.Random.Int(7, 30));
            @this.WithIsRepeatingInvoice(faker.Random.Bool());*/
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }

        // TODO: Replace erroneous properties
        public static PurchaseInvoiceBuilder WithSalesExternalB2BInvoiceDefaults(this PurchaseInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var customer = internalOrganisation.ActiveCustomers.Where(v => v.GetType().Name == typeof(Organisation).Name).FirstOrDefault();

            var salesInvoiceType = faker.Random.ListItem(@this.Session.Extent<SalesInvoiceType>());
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            var salesInvoiceItem_NonGSE = new SalesInvoiceItemBuilder(@this.Session).WithDefaults(internalOrganisation).Build();
            var salesInvoiceItem_GSE = new SalesInvoiceItemBuilder(@this.Session).WithGSEDefaults(internalOrganisation).Build();

            @this.WithCustomerReference(faker.Random.String(16).ToUpper());
            @this.WithBilledFrom(internalOrganisation);
            @this.WithBilledFromContactMechanism(internalOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBilledFromContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            /*@this.WithBillToCustomer(customer);
            @this.WithBillToContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToContactPerson(customer.CurrentContacts.FirstOrDefault());*/
            @this.WithShipToCustomer(customer);
            /*@this.WithShipToAddress(customer.ShippingAddress);
            @this.WithShipToContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithSalesInvoiceType(salesInvoiceType);
            @this.WithTotalListPrice(faker.Random.Decimal());
            @this.WithPaymentMethod(paymentMethod);
            @this.WithSalesInvoiceItem(salesInvoiceItem_NonGSE).Build();
            @this.WithSalesInvoiceItem(salesInvoiceItem_GSE).Build();
            @this.WithAdvancePayment(faker.Random.Decimal());
            @this.WithPaymentDays(faker.Random.Int(7, 30));
            @this.WithIsRepeatingInvoice(faker.Random.Bool());*/
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }
    }
}
