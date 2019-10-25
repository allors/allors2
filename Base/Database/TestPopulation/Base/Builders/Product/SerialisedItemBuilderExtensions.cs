// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialisedItemBuilderExtensions.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Allors.Meta;

namespace Allors.Domain.TestPopulation
{
    public static partial class SerialisedItemBuilderExtensions
    {
        public static SerialisedItemBuilder WithDefaults(this SerialisedItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var state = faker.Random.ListItem(@this.Session.Extent<SerialisedItemState>());
            var serviceDate = faker.Date.Past(refDate: @this.Session.Now());
            var acquiredDate = faker.Date.Past(refDate: serviceDate);
            var replacementValue = Convert.ToDecimal(faker.Commerce.Price());
            var lifetime = faker.Random.Int(0, 20);
            var expectedSalesPrice = Convert.ToDecimal(faker.Commerce.Price(replacementValue + 1000, replacementValue + 10000));

            @this.WithName(faker.Lorem.Word());
            @this.WithSerialisedItemState(state);
            @this.WithDescription(faker.Lorem.Words().ToString());
            @this.WithKeywords(faker.Lorem.Words().ToString());
            @this.WithInternalComment(faker.Lorem.Words().ToString());
            @this.WithAcquiredDate(acquiredDate);
            @this.WithLastServiceDate(serviceDate);
            @this.WithNextServiceDate(faker.Date.Future(refDate: serviceDate));
            @this.WithSerialNumber(faker.Random.AlphaNumeric(12));
            @this.WithOwnership(faker.Random.ListItem(@this.Session.Extent<Ownership>()));
            @this.WithManufacturingYear(serviceDate.Year - 5);
            @this.WithLifeTime(lifetime);
            @this.WithDepreciationYears(faker.Random.Int(0, lifetime));
            @this.WithReplacementValue(replacementValue);
            @this.WithAssignedPurchasePrice(Convert.ToDecimal(faker.Commerce.Price(replacementValue)));
            @this.WithExpectedSalesPrice(expectedSalesPrice);
            @this.WithRefurbishCost(Convert.ToDecimal(faker.Commerce.Price(0, 1000)));
            @this.WithTransportCost(Convert.ToDecimal(faker.Commerce.Price(0, 1000)));
            @this.WithExpectedRentalPriceFullService(Convert.ToDecimal(faker.Commerce.Price(expectedSalesPrice / 25, expectedSalesPrice / 10)));
            @this.WithExpectedRentalPriceDryLease(Convert.ToDecimal(faker.Commerce.Price(expectedSalesPrice / 25, expectedSalesPrice / 20)));
            @this.WithPrimaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 800, height: 600)).Build());
            @this.WithSecondaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 800, height: 600)).Build());
            @this.WithSecondaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 800, height: 600)).Build());
            @this.WithAdditionalPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 800, height: 600)).Build());
            @this.WithAdditionalPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 800, height: 600)).Build());
            @this.WithAdditionalPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 800, height: 600)).Build());
            @this.WithPrivatePhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 800, height: 600)).Build());
            @this.WithAvailableForSale(faker.Random.Bool());

            if (state.IsSold)
            {
                @this.WithOwnedBy(new Organisations(@this.Session).FindBy(M.Organisation.IsInternalOrganisation, false));
            }
            else if (state.IsInRent)
            {
                @this.WithRentedBy(new Organisations(@this.Session).FindBy(M.Organisation.IsInternalOrganisation, false));
                @this.WithRentalFromDate(faker.Date.Between(start: acquiredDate, end: acquiredDate.AddDays(10)));
                @this.WithRentalThroughDate(faker.Date.Future(refDate: acquiredDate.AddYears(2)));
                @this.WithExpectedReturnDate(faker.Date.Between(start: acquiredDate.AddYears(2).AddDays(1), end: acquiredDate.AddYears(2).AddDays(10)));
            }
            else
            {
                @this.WithOwnedBy(internalOrganisation);
            }

            foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            {
                @this.WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedDescription(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedKeyword(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
            }

            return @this;
        }
    }
}
