// <copyright file="SalesOrderBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    using System.Linq;

    public static partial class SalesOrderBuilderExtensions
    {
        public static SalesOrderBuilder WithDefaults(this SalesOrderBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var quoteItem = new QuoteItemBuilder(@this.Session).WithSerializedDefaults(internalOrganisation).Build();
            var customer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);
            var postalAddress = new PostalAddressBuilder(@this.Session).WithDefaults().Build();

            @this.WithTakenBy(internalOrganisation);
            @this.WithTakenByContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithTakenByContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence()).Build();
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithShipToCustomer(customer);
            @this.WithShipToContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithBillToCustomer(customer.CurrentContacts.FirstOrDefault());
            @this.WithBillToContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithBillToEndCustomer(customer);
            @this.WithBillToEndCustomerContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithShipToEndCustomer(customer);
            @this.WithShipToEndCustomerAddress(postalAddress);
            @this.WithShipToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithPlacingCustomer(customer);
            @this.WithPlacingCustomerContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithPlacingCustomerContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithOriginFacility(new FacilityBuilder(@this.Session).WithDefaults(internalOrganisation).Build());
            @this.WithPartiallyShip(faker.Random.Bool());
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }
    }
}
