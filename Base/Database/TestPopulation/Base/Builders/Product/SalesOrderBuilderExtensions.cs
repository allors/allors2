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

            @this.WithTakenBy(internalOrganisation).Build();
            @this.WithTakenByContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithTakenByContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithDescription(faker.Lorem.Sentence()).Build();
            @this.WithComment(faker.Lorem.Sentence()).Build();
            @this.WithInternalComment(faker.Lorem.Sentence()).Build();
            @this.WithShipToCustomer(customer).Build();
            @this.WithShipToContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithBillToCustomer(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithBillToContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBillToContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithBillToEndCustomer(customer).Build();
            @this.WithBillToEndCustomerContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithBillToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithShipToEndCustomer(customer).Build();
            @this.WithShipToEndCustomerAddress(postalAddress).Build();
            @this.WithShipToEndCustomerContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithPlacingCustomer(customer).Build();
            @this.WithPlacingCustomerContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault()).Build();
            @this.WithPlacingCustomerContactPerson(customer.CurrentContacts.FirstOrDefault()).Build();
            @this.WithOriginFacility(new FacilityBuilder(@this.Session).WithDefaults(internalOrganisation).Build()).Build();
            @this.WithPartiallyShip(faker.Random.Bool()).Build();

            return @this;
        }
    }
}
