// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseShipmentBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    using System.Linq;

    public static partial class PurchaseShipmentBuilderExtensions
    {
        public static PurchaseShipmentBuilder WithDefaults(this PurchaseShipmentBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var supplier = faker.Random.ListItem(internalOrganisation.ActiveSuppliers);

            @this.WithShipFromParty(supplier);
            @this.WithShipFromContactPerson(supplier.CurrentContacts.FirstOrDefault());
            @this.WithShipToParty(internalOrganisation);
            @this.WithShipToContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithShipmentMethod(faker.Random.ListItem(@this.Session.Extent<ShipmentMethod>()));
            @this.WithCarrier(faker.Random.ListItem(@this.Session.Extent<Carrier>()));
            @this.WithEstimatedShipDate(faker.Date.Between(start: @this.Session.Now(), end: @this.Session.Now().AddDays(5)));
            @this.WithEstimatedArrivalDate(faker.Date.Between(start: @this.Session.Now().AddDays(6), end: @this.Session.Now().AddDays(10)));

            @this.WithElectronicDocument(new MediaBuilder(@this.Session).WithInFileName("doc1.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());
            @this.WithEstimatedShipCost(faker.Finance.Amount(100, 1000, 2));
            @this.WithComment(faker.Lorem.Sentence());

            foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            {
                @this.WithLocalisedComment(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());
            }

            return @this;
        }
    }
}
