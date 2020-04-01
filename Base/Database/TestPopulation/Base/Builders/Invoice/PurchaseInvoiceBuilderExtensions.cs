// <copyright file="PurchaseInvoiceBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

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
            var purchaseInvoiceType = faker.Random.ListItem(@this.Session.Extent<PurchaseInvoiceType>());
            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithBilledFrom(internalOrganisation);
            @this.WithBilledFromContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBilledFromContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBillToEndCustomer(customer);
            @this.WithBillToEndCustomerContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithShipToEndCustomer(customer);
            @this.WithShipToEndCustomerAddress(customer.ShippingAddress);
            @this.WithShipToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithShipToCustomer(customer);
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }
    }
}
