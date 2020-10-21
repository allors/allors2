// <copyright file="SalesInvoiceBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors.Domain.TestPopulation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public static partial class SalesInvoiceBuilderExtensions
    {
        public static SalesInvoiceBuilder WithSalesInternalInvoiceDefaults(this SalesInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var internalOrganisations = @this.Session.Extent<Organisation>();

            var otherInternalOrganization = internalOrganisations.Except(new List<Organisation> { internalOrganisation }).FirstOrDefault();

            var endCustomer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);
            var salesInvoiceItem_Default = new SalesInvoiceItemBuilder(@this.Session).WithDefaults().Build();
            var salesInvoiceItem_Product = new SalesInvoiceItemBuilder(@this.Session).WithProductItemDefaults().Build();
            var salesInvoiceItem_Part = new SalesInvoiceItemBuilder(@this.Session).WithPartItemDefaults().Build();

            var salesInvoiceType = new SalesInvoiceTypes(@this.Session).SalesInvoice;
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16).ToUpper(CultureInfo.CurrentCulture));
            @this.WithBilledFrom(internalOrganisation);
            @this.WithBilledFromContactMechanism(internalOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBilledFromContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBillToCustomer(otherInternalOrganization.CurrentContacts.FirstOrDefault());
            @this.WithBillToContactMechanism(otherInternalOrganization.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToContactPerson(otherInternalOrganization.CurrentContacts.FirstOrDefault());
            @this.WithBillToEndCustomer(endCustomer);
            @this.WithBillToEndCustomerContactMechanism(endCustomer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToEndCustomerContactPerson(endCustomer.CurrentContacts.FirstOrDefault());
            @this.WithShipToEndCustomer(endCustomer);
            @this.WithShipToEndCustomerAddress(endCustomer.ShippingAddress);
            @this.WithShipToEndCustomerContactPerson(endCustomer.CurrentContacts.FirstOrDefault());
            @this.WithShipToCustomer(otherInternalOrganization);
            @this.WithShipToAddress(otherInternalOrganization.ShippingAddress);
            @this.WithShipToContactPerson(otherInternalOrganization.CurrentContacts.FirstOrDefault());
            @this.WithSalesInvoiceType(salesInvoiceType);
            @this.WithTotalListPrice(faker.Random.Decimal());
            @this.WithPaymentMethod(paymentMethod);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Default);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Product);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Part);
            @this.WithAdvancePayment(faker.Random.Decimal());
            @this.WithPaymentDays(faker.Random.Int(7, 30));
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }

        public static SalesInvoiceBuilder WithSalesExternalB2BInvoiceDefaults(this SalesInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var customer = internalOrganisation.ActiveCustomers.Where(v => v.GetType().Name == typeof(Organisation).Name).FirstOrDefault();

            var salesInvoiceType = new SalesInvoiceTypes(@this.Session).SalesInvoice;
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            var salesInvoiceItem_Default = new SalesInvoiceItemBuilder(@this.Session).WithDefaults().Build();
            var salesInvoiceItem_Product = new SalesInvoiceItemBuilder(@this.Session).WithProductItemDefaults().Build();
            var salesInvoiceItem_Part = new SalesInvoiceItemBuilder(@this.Session).WithPartItemDefaults().Build();

            @this.WithCustomerReference(faker.Random.String(16).ToUpper(CultureInfo.CurrentCulture));
            @this.WithBilledFrom(internalOrganisation);
            @this.WithBilledFromContactMechanism(internalOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBilledFromContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBillToCustomer(customer);
            @this.WithBillToContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithShipToCustomer(customer);
            @this.WithShipToAddress(customer.ShippingAddress);
            @this.WithShipToContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithSalesInvoiceType(salesInvoiceType);
            @this.WithTotalListPrice(faker.Random.Decimal());
            @this.WithPaymentMethod(paymentMethod);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Default);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Product);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Part);
            @this.WithAdvancePayment(faker.Random.Decimal());
            @this.WithPaymentDays(faker.Random.Int(7, 30));
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }

        public static SalesInvoiceBuilder WithSalesExternalB2CInvoiceDefaults(this SalesInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var customer = internalOrganisation.ActiveCustomers.Where(v => v.GetType().Name == typeof(Person).Name).FirstOrDefault();

            var salesInvoiceItem_Default = new SalesInvoiceItemBuilder(@this.Session).WithDefaults().Build();
            var salesInvoiceItem_Product = new SalesInvoiceItemBuilder(@this.Session).WithProductItemDefaults().Build();
            var salesInvoiceItem_Part = new SalesInvoiceItemBuilder(@this.Session).WithPartItemDefaults().Build();

            var salesInvoiceType = new SalesInvoiceTypes(@this.Session).SalesInvoice;
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16).ToUpper(CultureInfo.CurrentCulture));
            @this.WithBilledFrom(internalOrganisation);
            @this.WithBilledFromContactMechanism(internalOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBilledFromContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBillToCustomer(customer);
            @this.WithBillToContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithShipToCustomer(customer);
            @this.WithShipToAddress(customer.ShippingAddress);
            @this.WithSalesInvoiceType(salesInvoiceType);
            @this.WithTotalListPrice(faker.Random.Decimal());
            @this.WithPaymentMethod(paymentMethod);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Default);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Product);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Part);
            @this.WithAdvancePayment(faker.Random.Decimal());
            @this.WithPaymentDays(faker.Random.Int(7, 30));
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }

        public static SalesInvoiceBuilder WithCreditNoteDefaults(this SalesInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var customer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);

            var salesInvoiceItem_Default = new SalesInvoiceItemBuilder(@this.Session).WithDefaults().Build();
            var salesInvoiceItem_Product = new SalesInvoiceItemBuilder(@this.Session).WithProductItemDefaults().Build();
            var salesInvoiceItem_Part = new SalesInvoiceItemBuilder(@this.Session).WithPartItemDefaults().Build();

            var salesInvoiceType = new SalesInvoiceTypes(@this.Session).CreditNote;
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16).ToUpper(CultureInfo.CurrentCulture));
            @this.WithBilledFrom(internalOrganisation);
            @this.WithBilledFromContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBilledFromContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBillToCustomer(customer.CurrentContacts.FirstOrDefault());
            @this.WithBillToContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithBillToEndCustomer(customer);
            @this.WithBillToEndCustomerContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithShipToEndCustomer(customer);
            @this.WithShipToEndCustomerAddress(customer.ShippingAddress);
            @this.WithShipToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithShipToCustomer(customer);
            @this.WithShipToAddress(customer.ShippingAddress);
            @this.WithShipToContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithSalesInvoiceType(salesInvoiceType);
            @this.WithTotalListPrice(faker.Random.Decimal());
            @this.WithPaymentMethod(paymentMethod);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Default);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Product);
            @this.WithSalesInvoiceItem(salesInvoiceItem_Part);
            @this.WithAdvancePayment(faker.Random.Decimal());
            @this.WithPaymentDays(faker.Random.Int(7, 30));
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }
    }
}
