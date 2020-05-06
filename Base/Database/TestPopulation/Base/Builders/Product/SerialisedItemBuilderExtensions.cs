// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialisedItemBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    using System;
    using System.Linq;
    using Allors.Meta;

    public static partial class SerialisedItemBuilderExtensions
    {
        public static SerialisedItemBuilder WithDefaults(this SerialisedItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var availability = faker.Random.ListItem(@this.Session.Extent<SerialisedItemAvailability>());
            var serviceDate = faker.Date.Past(refDate: @this.Session.Now());
            var acquiredDate = faker.Date.Past(refDate: serviceDate);
            var replacementValue = Convert.ToDecimal(faker.Commerce.Price());
            var expectedSalesPrice = Convert.ToDecimal(faker.Commerce.Price(replacementValue + 1000, replacementValue + 10000));

            @this.WithName(faker.Lorem.Word());
            @this.WithSerialisedItemAvailability(availability);
            @this.WithSerialisedItemState(faker.Random.ListItem(@this.Session.Extent<SerialisedItemState>()));
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithKeywords(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithAcquiredDate(acquiredDate);
            @this.WithLastServiceDate(serviceDate);
            @this.WithNextServiceDate(faker.Date.Future(refDate: serviceDate));
            @this.WithSerialNumber(faker.Random.AlphaNumeric(12));
            @this.WithOwnership(faker.Random.ListItem(@this.Session.Extent<Ownership>()));
            @this.WithManufacturingYear(serviceDate.Year - 5);
            @this.WithAssignedPurchasePrice(Convert.ToDecimal(faker.Commerce.Price(replacementValue)));
            @this.WithExpectedSalesPrice(expectedSalesPrice);
            @this.WithPrimaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            if (@this.SecondaryPhotos != null)
            {
                @this.WithSecondaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
                @this.WithSecondaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            }
            @this.WithAdditionalPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithAdditionalPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithAdditionalPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithPrivatePhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithAvailableForSale(faker.Random.Bool());

            @this.WithBuyer(internalOrganisation);
            @this.WithSeller(internalOrganisation);
            @this.OwnedBy = (availability.IsSold ? new Organisations(@this.Session).FindBy(M.Organisation.IsInternalOrganisation, false) : internalOrganisation) ?? internalOrganisation;

            if (availability.IsInRent)
            {
                @this.WithRentedBy(new Organisations(@this.Session).FindBy(M.Organisation.IsInternalOrganisation, false));
                @this.WithRentalFromDate(faker.Date.Between(start: acquiredDate, end: acquiredDate.AddDays(10)));
                @this.WithRentalThroughDate(faker.Date.Future(refDate: acquiredDate.AddYears(2)));
                @this.WithExpectedReturnDate(faker.Date.Between(start: acquiredDate.AddYears(2).AddDays(1), end: acquiredDate.AddYears(2).AddDays(10)));
            }

            foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            {
                @this.WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedDescription(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedKeyword(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());
            }

            return @this;
        }

        public static SerialisedItemBuilder WithForSaleDefaults(this SerialisedItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var serviceDate = faker.Date.Past(refDate: @this.Session.Now());
            var acquiredDate = faker.Date.Past(refDate: serviceDate);
            var replacementValue = Convert.ToDecimal(faker.Commerce.Price());
            var expectedSalesPrice = Convert.ToDecimal(faker.Commerce.Price(replacementValue + 1000, replacementValue + 10000));

            @this.WithName(faker.Lorem.Word());
            @this.WithSerialisedItemAvailability(new SerialisedItemAvailabilities(@this.Session).Available);
            @this.WithSerialisedItemState(faker.Random.ListItem(@this.Session.Extent<SerialisedItemState>()));
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithKeywords(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithAcquiredDate(acquiredDate);
            @this.WithLastServiceDate(serviceDate);
            @this.WithNextServiceDate(faker.Date.Future(refDate: serviceDate));
            @this.WithSerialNumber(faker.Random.AlphaNumeric(12));
            @this.WithOwnership(faker.Random.ListItem(@this.Session.Extent<Ownership>()));
            @this.WithManufacturingYear(serviceDate.Year - 5);
            @this.WithAssignedPurchasePrice(Convert.ToDecimal(faker.Commerce.Price(replacementValue)));
            @this.WithExpectedSalesPrice(expectedSalesPrice);
            @this.WithPrimaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithSecondaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithSecondaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithAdditionalPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithAdditionalPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithAdditionalPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithPrivatePhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 800, height: 600)).Build());
            @this.WithAvailableForSale(true);
            @this.WithOwnedBy(internalOrganisation);

            foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            {
                @this.WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedDescription(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedKeyword(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());
            }

            return @this;
        }
    }
}
