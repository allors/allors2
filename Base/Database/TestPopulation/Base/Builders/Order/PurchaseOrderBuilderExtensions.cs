// <copyright file="PurchaseOrderBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors.Domain.TestPopulation
{
    using System.Linq;

    public static partial class PurchaseOrderBuilderExtensions
    {
        public static PurchaseOrderBuilder WithDefaults(this PurchaseOrderBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var quoteItem = new QuoteItemBuilder(@this.Session).WithSerializedDefaults(internalOrganisation).Build();
            var customer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);
            var postalAddress = new PostalAddressBuilder(@this.Session).WithDefaults().Build();
            var supplier = faker.Random.ListItem(internalOrganisation.ActiveSuppliers);

            var nonSerializedPart = new PurchaseOrderItemBuilder(@this.Session).WithNonSerializedPartDefaults(internalOrganisation, supplier).Build();
            var serializedPart = new PurchaseOrderItemBuilder(@this.Session).WithSerializedPartDefaults(internalOrganisation).Build();

            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence()).Build();
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithShipToContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithShipToAddress(customer.CurrentContacts.FirstOrDefault().ShippingAddress);
            @this.WithBillToContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithBillToContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithTakenViaContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithTakenViaContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithTakenViaSupplier(supplier);
            @this.WithStoredInFacility(faker.Random.ListItem(internalOrganisation.FacilitiesWhereOwner));
            @this.WithPurchaseOrderItem(nonSerializedPart);
            @this.WithPurchaseOrderItem(serializedPart);
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }
    }
}
