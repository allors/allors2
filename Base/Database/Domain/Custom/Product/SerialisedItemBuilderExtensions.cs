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
using System.ComponentModel.Design;
using Allors.Meta;

namespace Allors.Domain.End2End
{
    public static partial class SerialisedItemBuilderExtensions
    {
        public static SerialisedItemBuilder WithDefaults(this SerialisedItemBuilder @this, ISession session, Config config, Organisation internalOrganisation)
        {
            if (config.End2End)
            {
                var state = config.faker.Random.ListItem(session.Extent<SerialisedItemState>());
                var serviceDate = config.faker.Date.Past(refDate: session.Now());
                var acquiredDate = config.faker.Date.Past(refDate: serviceDate);
                var replacementValue = Convert.ToDecimal(config.faker.Commerce.Price());
                var lifetime = config.faker.Random.Int(0, 20);
                var expectedSalesPrice = Convert.ToDecimal(config.faker.Commerce.Price(replacementValue + 1000, replacementValue + 10000));

                @this.WithName(config.faker.Lorem.Word());
                @this.WithSerialisedItemState(state);
                @this.WithDescription(config.faker.Lorem.Words().ToString());
                @this.WithKeywords(config.faker.Lorem.Words().ToString());
                @this.WithInternalComment(config.faker.Lorem.Words().ToString());
                @this.WithAcquiredDate(acquiredDate);
                @this.WithLastServiceDate(serviceDate);
                @this.WithNextServiceDate(config.faker.Date.Future(refDate: serviceDate));
                @this.WithSerialNumber(config.faker.Random.AlphaNumeric(12));
                @this.WithOwnership(config.faker.Random.ListItem(session.Extent<Ownership>()));
                @this.WithAcquisitionYear(serviceDate.Year - 1);
                @this.WithManufacturingYear(serviceDate.Year - 5);
                @this.WithLifeTime(lifetime);
                @this.WithDepreciationYears(config.faker.Random.Int(0, lifetime));
                @this.WithReplacementValue(replacementValue);
                @this.WithAssignedPurchasePrice(Convert.ToDecimal(config.faker.Commerce.Price(replacementValue)));
                @this.WithExpectedSalesPrice(expectedSalesPrice);
                @this.WithRefurbishCost(Convert.ToDecimal(config.faker.Commerce.Price(0, 1000)));
                @this.WithTransportCost(Convert.ToDecimal(config.faker.Commerce.Price(0, 1000)));
                @this.WithExpectedRentalPriceFullService(Convert.ToDecimal(config.faker.Commerce.Price(expectedSalesPrice / 25, expectedSalesPrice / 10)));
                @this.WithExpectedRentalPriceDryLease(Convert.ToDecimal(config.faker.Commerce.Price(expectedSalesPrice / 25, expectedSalesPrice / 20)));
                @this.WithPrimaryPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 800, height: 600)).Build());
                @this.WithSecondaryPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 800, height: 600)).Build());
                @this.WithSecondaryPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 800, height: 600)).Build());
                @this.WithAdditionalPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 800, height: 600)).Build());
                @this.WithAdditionalPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 800, height: 600)).Build());
                @this.WithAdditionalPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 800, height: 600)).Build());
                @this.WithPrivatePhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 800, height: 600)).Build());
                @this.WithAvailableForSale(config.faker.Random.Bool());

                if (state.IsSold)
                {
                    @this.WithOwnedBy(new Organisations(session).FindBy(M.Organisation.IsInternalOrganisation, false));
                }
                else if (state.IsInRent)
                {
                    @this.WithRentedBy(new Organisations(session).FindBy(M.Organisation.IsInternalOrganisation, false));
                    @this.WithRentalFromDate(config.faker.Date.Between(start: acquiredDate, end: acquiredDate.AddDays(10)));
                    @this.WithRentalThroughDate(config.faker.Date.Future(refDate: acquiredDate.AddYears(2)));
                    @this.WithExpectedReturnDate(config.faker.Date.Between(start: acquiredDate.AddYears(2).AddDays(1), end: acquiredDate.AddYears(2).AddDays(10)));
                }
                else
                {
                    @this.WithOwnedBy(internalOrganisation);
                }

                foreach (Locale additionalLocale in session.GetSingleton().AdditionalLocales)
                {
                    @this.WithLocalisedName(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                    @this.WithLocalisedDescription(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                    @this.WithLocalisedKeyword(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                }
            }

            return @this;
        }
    }
}
