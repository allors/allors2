// <copyright file="SalesInvoiceBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class SalesInvoiceBuilderExtensions
    {
        public static SalesInvoiceBuilder WithSalesInternalInvoiceDefaults(this SalesInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var internalOrganisations = @this.Session.Extent<Organisation>();

            var otherInternalOrganization = internalOrganisations.Except(new List<Organisation> { internalOrganisation }).FirstOrDefault();

            var endCustomer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);
            var salesInvoiceItem = new SalesInvoiceItemBuilder(@this.Session).WithSerializedProductDefaults(internalOrganisation).Build();

            var salesInvoiceType = faker.Random.ListItem(@this.Session.Extent<SalesInvoiceType>());
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16).ToUpper());
            @this.WithDescription(faker.Random.Words(10));
            @this.WithBilledFrom(internalOrganisation).Build();
            @this.WithBilledFromContactMechanism(internalOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBilledFromContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault()).Build();
            @this.WithDescription(faker.Lorem.Sentence()).Build();
            @this.WithComment(faker.Lorem.Sentence()).Build();
            @this.WithInternalComment(faker.Lorem.Sentence()).Build();
            @this.WithBillToCustomer(otherInternalOrganization.CurrentContacts.FirstOrDefault()).Build();
            @this.WithBillToContactMechanism(otherInternalOrganization.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBillToContactPerson(otherInternalOrganization.CurrentContacts.FirstOrDefault()).Build();
            @this.WithBillToEndCustomer(endCustomer).Build();
            @this.WithBillToEndCustomerContactMechanism(endCustomer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBillToEndCustomerContactPerson(endCustomer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithShipToEndCustomer(endCustomer).Build();
            @this.WithShipToEndCustomerAddress(endCustomer.ShippingAddress).Build();
            @this.WithShipToEndCustomerContactPerson(endCustomer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithShipToCustomer(otherInternalOrganization).Build();
            @this.WithShipToAddress(otherInternalOrganization.ShippingAddress).Build();
            @this.WithShipToContactPerson(otherInternalOrganization.CurrentContacts.FirstOrDefault()).Build();
            @this.WithSalesInvoiceType(salesInvoiceType).Build();
            @this.WithTotalListPrice(faker.Random.Decimal()).Build();
            @this.WithPaymentMethod(paymentMethod).Build();

            // TODO: WIP
            /*@this.WithSalesInvoiceItem(salesInvoiceItem).Build();
            @this.WithSalesInvoiceItem(salesInvoiceItem).Build();*/
            @this.WithAdvancePayment(faker.Random.Decimal()).Build();
            @this.WithPaymentDays(faker.Random.Int(7, 30)).Build();
            @this.WithIsRepeatingInvoice(faker.Random.Bool()).Build();
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build()).Build();
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build()).Build();
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build()).Build();

            return @this;
        }

        public static SalesInvoiceBuilder WithSalesExternalB2BInvoiceDefaults(this SalesInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var customer = internalOrganisation.ActiveCustomers.Where(v => v.GetType().Name == typeof(Organisation).Name).FirstOrDefault();
            var salesInvoiceItem = new SalesInvoiceItemBuilder(@this.Session).WithSerializedProductDefaults(internalOrganisation).Build();

            var salesInvoiceType = faker.Random.ListItem(@this.Session.Extent<SalesInvoiceType>());
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16).ToUpper());
            @this.WithDescription(faker.Random.Words(10));
            @this.WithBilledFrom(internalOrganisation).Build();
            @this.WithBilledFromContactMechanism(internalOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBilledFromContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault()).Build();
            @this.WithDescription(faker.Lorem.Sentence()).Build();
            @this.WithComment(faker.Lorem.Sentence()).Build();
            @this.WithInternalComment(faker.Lorem.Sentence()).Build();
            @this.WithBillToCustomer(customer).Build();
            @this.WithBillToContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBillToContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithShipToCustomer(customer).Build();
            @this.WithShipToAddress(customer.ShippingAddress).Build();
            @this.WithShipToContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithSalesInvoiceType(salesInvoiceType).Build();
            @this.WithTotalListPrice(faker.Random.Decimal()).Build();
            @this.WithPaymentMethod(paymentMethod).Build();

            // TODO: WIP
            /*@this.WithSalesInvoiceItem(salesInvoiceItem).Build();
            @this.WithSalesInvoiceItem(salesInvoiceItem).Build();*/
            @this.WithAdvancePayment(faker.Random.Decimal()).Build();
            @this.WithPaymentDays(faker.Random.Int(7, 30)).Build();
            @this.WithIsRepeatingInvoice(faker.Random.Bool()).Build();
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build()).Build();
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build()).Build();
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build()).Build();

            return @this;
        }

        public static SalesInvoiceBuilder WithSalesExternalB2CInvoiceDefaults(this SalesInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var customer = internalOrganisation.ActiveCustomers.Where(v => v.GetType().Name == typeof(Person).Name).FirstOrDefault();
            var salesInvoiceItem = new SalesInvoiceItemBuilder(@this.Session).WithSerializedProductDefaults(internalOrganisation).Build();

            var salesInvoiceType = faker.Random.ListItem(@this.Session.Extent<SalesInvoiceType>());
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16).ToUpper());
            @this.WithDescription(faker.Random.Words(10));
            @this.WithBilledFrom(internalOrganisation).Build();
            @this.WithBilledFromContactMechanism(internalOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBilledFromContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault()).Build();
            @this.WithDescription(faker.Lorem.Sentence()).Build();
            @this.WithComment(faker.Lorem.Sentence()).Build();
            @this.WithInternalComment(faker.Lorem.Sentence()).Build();
            @this.WithBillToCustomer(customer).Build();
            @this.WithBillToContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithShipToCustomer(customer).Build();
            @this.WithShipToAddress(customer.ShippingAddress).Build();
            @this.WithSalesInvoiceType(salesInvoiceType).Build();
            @this.WithTotalListPrice(faker.Random.Decimal()).Build();
            @this.WithPaymentMethod(paymentMethod).Build();

            // TODO: WIP
            /*@this.WithSalesInvoiceItem(salesInvoiceItem).Build();
            @this.WithSalesInvoiceItem(salesInvoiceItem).Build();*/
            @this.WithAdvancePayment(faker.Random.Decimal()).Build();
            @this.WithPaymentDays(faker.Random.Int(7, 30)).Build();
            @this.WithIsRepeatingInvoice(faker.Random.Bool()).Build();
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build()).Build();
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build()).Build();
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build()).Build();

            return @this;
        }

        public static SalesInvoiceBuilder WithCreditNoteDefaults(this SalesInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var customer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);
            var salesInvoiceItem = new SalesInvoiceItemBuilder(@this.Session).WithSerializedProductDefaults(internalOrganisation).Build();

            var salesInvoiceType = faker.Random.ListItem(@this.Session.Extent<SalesInvoiceType>());
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16).ToUpper());
            @this.WithDescription(faker.Random.Words(10));
            @this.WithBilledFrom(internalOrganisation).Build();
            @this.WithBilledFromContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBilledFromContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithDescription(faker.Lorem.Sentence()).Build();
            @this.WithComment(faker.Lorem.Sentence()).Build();
            @this.WithInternalComment(faker.Lorem.Sentence()).Build();
            @this.WithBillToCustomer(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithBillToContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBillToContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithBillToEndCustomer(customer).Build();
            @this.WithBillToEndCustomerContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBillToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithShipToEndCustomer(customer).Build();
            @this.WithShipToEndCustomerAddress(customer.ShippingAddress).Build();
            @this.WithShipToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithShipToCustomer(customer).Build();
            @this.WithShipToAddress(customer.ShippingAddress).Build();
            @this.WithShipToContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithSalesInvoiceType(salesInvoiceType).Build();
            @this.WithTotalListPrice(faker.Random.Decimal()).Build();
            @this.WithPaymentMethod(paymentMethod).Build();

            // TODO: WIP
            /*@this.WithSalesInvoiceItem(salesInvoiceItem).Build();
            @this.WithSalesInvoiceItem(salesInvoiceItem).Build();*/
            @this.WithAdvancePayment(faker.Random.Decimal()).Build();
            @this.WithPaymentDays(faker.Random.Int(7, 30)).Build();
            @this.WithIsRepeatingInvoice(faker.Random.Bool()).Build();
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build()).Build();
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build()).Build();
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build()).Build();

            return @this;
        }
    }
}
