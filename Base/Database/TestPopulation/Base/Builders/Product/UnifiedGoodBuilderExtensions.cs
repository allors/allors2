// <copyright file="UnifiedGoodBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    using System;
    using Allors.Meta;

    public static partial class UnifiedGoodBuilderExtensions
    {
        public static UnifiedGoodBuilder WithNonSerialisedDefaults(this UnifiedGoodBuilder @this, Organisation internalOrganisation)
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

            var replacementValue = Convert.ToDecimal(faker.Commerce.Price());
            var lifetime = faker.Random.Int(0, 20);

            @this.WithName(faker.Commerce.ProductName());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithLifeTime(lifetime);
            @this.WithDepreciationYears(faker.Random.Int(0, lifetime));
            @this.WithReplacementValue(replacementValue);
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithInventoryItemKind(new InventoryItemKinds(@this.Session).NonSerialised);
            @this.WithKeywords(faker.Lorem.Sentence());
            @this.WithUnitOfMeasure(new UnitsOfMeasure(@this.Session).Piece);
            @this.WithPrimaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 200, height: 56)).Build());
            @this.WithPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 200, height: 56)).Build());
            @this.WithPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 200, height: 56)).Build());
            @this.WithPublicElectronicDocument(new MediaBuilder(@this.Session).WithInFileName("doc1.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());
            @this.WithPrivateElectronicDocument(new MediaBuilder(@this.Session).WithInFileName("doc2.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());
            @this.WithProductIdentification(new SkuIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithProductIdentification(new EanIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithProductIdentification(new ManufacturerIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithVatRegime(faker.Random.ListItem(@this.Session.Extent<VatRegime>()));

            foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            {
                @this.WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedDescription(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedComment(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedKeyword(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());

                var localisedDocument = new MediaBuilder(@this.Session).WithInFileName($"doc1.{additionalLocale.Country.IsoCode}.pdf").WithInData(faker.Random.Bytes(1000)).Build();
                @this.WithPublicLocalisedElectronicDocument(new LocalisedMediaBuilder(@this.Session).WithMedia(localisedDocument).WithLocale(additionalLocale).Build());
                @this.WithPrivateLocalisedElectronicDocument(new LocalisedMediaBuilder(@this.Session).WithMedia(localisedDocument).WithLocale(additionalLocale).Build());
            }

            return @this;
        }

        public static UnifiedGoodBuilder WithSerialisedDefaults(this UnifiedGoodBuilder @this, Organisation internalOrganisation)
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

            var replacementValue = Convert.ToDecimal(faker.Commerce.Price());
            var lifetime = faker.Random.Int(0, 20);

            @this.WithName(faker.Commerce.ProductName());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithLifeTime(lifetime);
            @this.WithDepreciationYears(faker.Random.Int(0, lifetime));
            @this.WithReplacementValue(replacementValue);
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithInventoryItemKind(new InventoryItemKinds(@this.Session).Serialised);
            @this.WithKeywords(faker.Lorem.Sentence());
            @this.WithUnitOfMeasure(new UnitsOfMeasure(@this.Session).Piece);
            @this.WithPrimaryPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 200, height: 56)).Build());
            @this.WithPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 200, height: 56)).Build());
            @this.WithPhoto(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 200, height: 56)).Build());
            @this.WithPublicElectronicDocument(new MediaBuilder(@this.Session).WithInFileName("doc1.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());
            @this.WithPrivateElectronicDocument(new MediaBuilder(@this.Session).WithInFileName("doc2.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());
            @this.WithProductIdentification(new SkuIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithProductIdentification(new EanIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithProductIdentification(new ManufacturerIdentificationBuilder(@this.Session).WithDefaults().Build());
            @this.WithSerialisedItem(new SerialisedItemBuilder(@this.Session).WithDefaults(internalOrganisation).Build());
            @this.WithVatRegime(faker.Random.ListItem(@this.Session.Extent<VatRegime>()));

            foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            {
                @this.WithLocalisedName(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedDescription(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedComment(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());
                @this.WithLocalisedKeyword(new LocalisedTextBuilder(@this.Session).WithText(faker.Lorem.Sentence()).WithLocale(additionalLocale).Build());

                var localisedDocument = new MediaBuilder(@this.Session).WithInFileName($"doc1.{additionalLocale.Country.IsoCode}.pdf").WithInData(faker.Random.Bytes(1000)).Build();
                @this.WithPublicLocalisedElectronicDocument(new LocalisedMediaBuilder(@this.Session).WithMedia(localisedDocument).WithLocale(additionalLocale).Build());
                @this.WithPrivateLocalisedElectronicDocument(new LocalisedMediaBuilder(@this.Session).WithMedia(localisedDocument).WithLocale(additionalLocale).Build());
            }

            return @this;
        }
    }
}
