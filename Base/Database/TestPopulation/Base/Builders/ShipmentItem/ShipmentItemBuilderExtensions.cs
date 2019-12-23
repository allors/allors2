// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentItemBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace Allors.Domain.TestPopulation
{
    public static partial class ShipmentItemBuilderExtensions
    {
        public static ShipmentItemBuilder WithDefaults(this ShipmentItemBuilder @this, CustomerShipment customerShipment)
        {
            var faker = @this.Session.Faker();

            //@this.WithShipFromParty(customerShipment);
            //@this.WithShipFromContactPerson(customerShipment.CurrentContacts.FirstOrDefault());
            //@this.WithShipToParty(customer);
            //@this.WithShipToContactPerson(customer.CurrentContacts.FirstOrDefault());
            //@this.WithShipmentMethod(faker.Random.ListItem(@this.Session.Extent<ShipmentMethod>()));
            //@this.WithCarrier(faker.Random.ListItem(@this.Session.Extent<Carrier>()));
            //@this.WithEstimatedReadyDate(@this.Session.Now());
            //@this.WithEstimatedShipDate(faker.Date.Between(start: @this.Session.Now(), end: @this.Session.Now().AddDays(5)));
            //@this.WithLatestCancelDate(faker.Date.Between(start: @this.Session.Now(), end: @this.Session.Now().AddDays(2)));
            //@this.WithEstimatedArrivalDate(faker.Date.Between(start: @this.Session.Now().AddDays(6), end: @this.Session.Now().AddDays(10)));

            //@this.WithElectronicDocument(new MediaBuilder(@this.Session).WithFileName("doc1.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());
            //@this.WithEstimatedShipCost(faker.Finance.Amount(100, 1000, 2));
            //@this.WithHandlingInstruction(faker.Lorem.Paragraph());
            //@this.WithComment(faker.Lorem.Sentence());

            //foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            //{
            //    @this.WithLocalisedComment(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());
            //}

            return @this;
        }
    }
}
