// <copyright file="PurchaseInvoiceBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    using System.Linq;

    public static partial class PurchaseInvoiceBuilderExtensions
    {
        public static PurchaseInvoiceBuilder WithDefaults(this PurchaseInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var quoteItem = new QuoteItemBuilder(@this.Session).WithSerializedDefaults(internalOrganisation).Build();
            var customer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);
            var postalAddress = new PostalAddressBuilder(@this.Session).WithDefaults().Build();
            var salesInvoiceType = faker.Random.ListItem(@this.Session.Extent<SalesInvoiceType>());
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithBilledFrom(internalOrganisation).Build();
            @this.WithBilledFromContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBilledFromContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithDescription(faker.Lorem.Sentence()).Build();
            @this.WithComment(faker.Lorem.Sentence()).Build();
            @this.WithInternalComment(faker.Lorem.Sentence()).Build();
            @this.WithBillToEndCustomer(customer).Build();
            @this.WithBillToEndCustomerContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBillToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithShipToEndCustomer(customer).Build();
            @this.WithShipToEndCustomerAddress(postalAddress).Build();
            @this.WithShipToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithShipToCustomer(customer).Build();
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build()).Build();
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build()).Build();
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build()).Build();

            return @this;
        }
    }
}
