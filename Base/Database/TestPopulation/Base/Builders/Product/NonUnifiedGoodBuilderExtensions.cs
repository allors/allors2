// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonUnifiedGoodBuilderExtensions.cs" company="Allors bvba">
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

using Allors.Meta;

namespace Allors.Domain.TestPopulation
{
    using Bogus;

    public static partial class NonUnifiedGoodBuilderExtensions
    {
        public static NonUnifiedGoodBuilder WithNonSerialisedPartDefaults(this NonUnifiedGoodBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var dutchLocale = new Locales(@this.Session).DutchNetherlands;

            var nonSerialisedProductType = new ProductTypes(@this.Session).FindBy(M.ProductType.Name, "nonSerialisedProductType");

            if (nonSerialisedProductType == null)
            {
                var size = new SerialisedItemCharacteristicTypeBuilder(@this.Session)
                    .WithName("Size")
                    .WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText("Afmeting").WithLocale(dutchLocale).Build())
                    .Build();

                var weight = new SerialisedItemCharacteristicTypeBuilder(@this.Session)
                    .WithName("Weight")
                    .WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText("Gewicht").WithLocale(dutchLocale).Build())
                    .WithUnitOfMeasure(new UnitsOfMeasure(@this.Session).Kilogram)
                    .Build();

                nonSerialisedProductType = new ProductTypeBuilder(@this.Session)
                    .WithName("nonSerialisedProductType")
                    .WithSerialisedItemCharacteristicType(size)
                    .WithSerialisedItemCharacteristicType(weight)
                    .Build();
            }

            @this.WithName(faker.Commerce.ProductMaterial());
            @this.WithPart(new NonUnifiedPartBuilder(@this.Session).WithNonSerialisedDefaults(internalOrganisation).Build());
            @this.WithDescription(faker.Lorem.Words().ToString());
            @this.WithComment(faker.Lorem.Words().ToString());
            @this.WithInternalComment(faker.Lorem.Words().ToString());
            @this.WithKeywords(faker.Lorem.Words().ToString());
            @this.WithUnitOfMeasure(new UnitsOfMeasure(@this.Session).Piece);
            @this.WithPrimaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 200, height: 56)).Build());
            @this.WithPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 200, height: 56)).Build());
            @this.WithPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 200, height: 56)).Build());
            @this.WithElectronicDocument(new MediaBuilder(@this.Session).WithFileName("doc1.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());
            @this.WithElectronicDocument(new MediaBuilder(@this.Session).WithFileName("doc2.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());
            @this.WithProductIdentification(new SkuIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithProductIdentification(new EanIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithProductIdentification(new ManufacturerIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithVatRate(faker.Random.ListItem(@this.Session.Extent<VatRate>()));

            foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            {
                @this.WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedDescription(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedComment(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedKeyword(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());

                var localisedDocument = new MediaBuilder(@this.Session).WithFileName($"doc1.{additionalLocale.Country.IsoCode}.pdf").WithInData(faker.Random.Bytes(1000)).Build();
                @this.WithLocalisedElectronicDocument(new LocalisedMediaBuilder(@this.Session).WithMedia(localisedDocument).WithLocale(additionalLocale).Build());
            }

            return @this;
        }

        public static NonUnifiedGoodBuilder WithSerialisedPartDefaults(this NonUnifiedGoodBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var dutchLocale = new Locales(@this.Session).DutchNetherlands;

            var nonSerialisedProductType = new ProductTypes(@this.Session).FindBy(M.ProductType.Name, "nonSerialisedProductType");

            if (nonSerialisedProductType == null)
            {
                var size = new SerialisedItemCharacteristicTypeBuilder(@this.Session)
                    .WithName("Size")
                    .WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText("Afmeting").WithLocale(dutchLocale).Build())
                    .Build();

                var weight = new SerialisedItemCharacteristicTypeBuilder(@this.Session)
                    .WithName("Weight")
                    .WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText("Gewicht").WithLocale(dutchLocale).Build())
                    .WithUnitOfMeasure(new UnitsOfMeasure(@this.Session).Kilogram)
                    .Build();

                nonSerialisedProductType = new ProductTypeBuilder(@this.Session)
                    .WithName("serialisedProductType")
                    .WithSerialisedItemCharacteristicType(size)
                    .WithSerialisedItemCharacteristicType(weight)
                    .Build();
            }

            @this.WithName(faker.Commerce.ProductMaterial());
            @this.WithPart(new NonUnifiedPartBuilder(@this.Session).WithSerialisedDefaults(internalOrganisation, faker).Build());
            @this.WithDescription(faker.Lorem.Words().ToString());
            @this.WithComment(faker.Lorem.Words().ToString());
            @this.WithInternalComment(faker.Lorem.Words().ToString());
            @this.WithKeywords(faker.Lorem.Words().ToString());
            @this.WithUnitOfMeasure(new UnitsOfMeasure(@this.Session).Piece);
            @this.WithPrimaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 200, height: 56)).Build());
            @this.WithPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 200, height: 56)).Build());
            @this.WithPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl(width: 200, height: 56)).Build());
            @this.WithElectronicDocument(new MediaBuilder(@this.Session).WithFileName("doc1.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());
            @this.WithElectronicDocument(new MediaBuilder(@this.Session).WithFileName("doc2.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());
            @this.WithProductIdentification(new SkuIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithProductIdentification(new EanIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithProductIdentification(new ManufacturerIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithVatRate(faker.Random.ListItem(@this.Session.Extent<VatRate>()));

            foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            {
                @this.WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedDescription(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedComment(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedKeyword(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());

                var localisedDocument = new MediaBuilder(@this.Session).WithFileName($"doc1.{additionalLocale.Country.IsoCode}.pdf").WithInData(faker.Random.Bytes(1000)).Build();
                @this.WithLocalisedElectronicDocument(new LocalisedMediaBuilder(@this.Session).WithMedia(localisedDocument).WithLocale(additionalLocale).Build());
            }

            return @this;
        }
    }
}
