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

            var postalAddress = new PostalAddressBuilder(@this.Session).WithDefaults().Build();
            var supplier = faker.Random.ListItem(internalOrganisation.ActiveSuppliers);

            @this.WithCustomerReference(faker.Lorem.Word());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithShipToContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithBillToContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithTakenViaContactPerson(supplier.CurrentContacts.FirstOrDefault());
            @this.WithTakenViaSupplier(supplier);
            @this.WithStoredInFacility(faker.Random.ListItem(internalOrganisation.FacilitiesWhereOwner));
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithOrderedBy(internalOrganisation);

            return @this;
        }
    }
}
