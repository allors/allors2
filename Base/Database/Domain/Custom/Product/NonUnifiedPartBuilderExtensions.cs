// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonUnifiedPartBuilderExtensions.cs" company="Allors bvba">
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

namespace Allors.Domain.End2End
{
    public static partial class NonUnifiedPartBuilderExtensions
    {
        public static NonUnifiedPartBuilder WithNonSerialisedDefaults(this NonUnifiedPartBuilder @this, ISession session, Config config, Organisation internalOrganisation)
        {
            if (config.End2End)
            {
                var dutchLocale = new Locales(session).DutchNetherlands;
                var brand = new BrandBuilder(session).WithDefaults(session, config).Build();

                var nonSerialisedProductType = new ProductTypes(session).FindBy(M.ProductType.Name, "nonSerialisedProductType");

                if (nonSerialisedProductType == null)
                {
                    var size = new SerialisedItemCharacteristicTypeBuilder(session)
                        .WithName("Size")
                        .WithLocalisedName(new LocalisedTextBuilder(session).WithText("Afmeting").WithLocale(dutchLocale).Build())
                        .Build();

                    var weight = new SerialisedItemCharacteristicTypeBuilder(session)
                        .WithName("Weight")
                        .WithLocalisedName(new LocalisedTextBuilder(session).WithText("Gewicht").WithLocale(dutchLocale).Build())
                        .WithUnitOfMeasure(new UnitsOfMeasure(session).Kilogram)
                        .Build();

                    nonSerialisedProductType = new ProductTypeBuilder(session)
                        .WithName("nonSerialisedProductType")
                        .WithSerialisedItemCharacteristicType(size)
                        .WithSerialisedItemCharacteristicType(weight)
                        .Build();
                }

                @this.WithInventoryItemKind(new InventoryItemKinds(session).NonSerialised);
                @this.WithName(config.faker.Commerce.ProductMaterial());
                @this.WithDescription(config.faker.Lorem.Words().ToString());
                @this.WithComment(config.faker.Lorem.Words().ToString());
                @this.WithInternalComment(config.faker.Lorem.Words().ToString());
                @this.WithKeywords(config.faker.Lorem.Words().ToString());
                @this.WithUnitOfMeasure(new UnitsOfMeasure(session).Piece);
                @this.WithPrimaryPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 200, height: 56)).Build());
                @this.WithPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 200, height: 56)).Build());
                @this.WithPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 200, height: 56)).Build());
                @this.WithElectronicDocument(new MediaBuilder(session).WithFileName("doc1.en.pdf").WithInData(config.faker.Random.Bytes(1000)).Build());
                @this.WithElectronicDocument(new MediaBuilder(session).WithFileName("doc2.en.pdf").WithInData(config.faker.Random.Bytes(1000)).Build());
                @this.WithProductIdentification(new SkuIdentificationBuilder(session).WithDefaults(session, config).Build());
                @this.WithProductIdentification(new EanIdentificationBuilder(session).WithDefaults(session, config).Build());
                @this.WithProductIdentification(new ManufacturerIdentificationBuilder(session).WithDefaults(session, config).Build());
                @this.WithDefaultFacility(internalOrganisation.FacilitiesWhereOwner?.First);
                @this.WithProductType(nonSerialisedProductType);
                @this.WithBrand(brand);
                @this.WithModel(brand.Models.First);
                @this.WithHsCode(config.faker.Random.Number(99999999).ToString());
                @this.WithManufacturedBy(new OrganisationBuilder(session).WithManufacturerDefaults(session, config).Build());
                @this.WithReorderLevel(config.faker.Random.Number(99));
                @this.WithReorderQuantity(config.faker.Random.Number(999));

                foreach (Locale additionalLocale in session.GetSingleton().AdditionalLocales)
                {
                    @this.WithLocalisedName(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                    @this.WithLocalisedDescription(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                    @this.WithLocalisedComment(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                    @this.WithLocalisedKeyword(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());

                    var localisedDocument = new MediaBuilder(session).WithFileName($"doc1.{additionalLocale.Country.IsoCode}.pdf").WithInData(config.faker.Random.Bytes(1000)).Build();
                    @this.WithLocalisedElectronicDocument(new LocalisedMediaBuilder(session).WithMedia(localisedDocument).WithLocale(additionalLocale).Build());
                }
            }

            return @this;
        }

        public static NonUnifiedPartBuilder WithSerialisedDefaults(this NonUnifiedPartBuilder @this, ISession session, Config config, Organisation internalOrganisation)
        {
            if (config.End2End)
            {
                var dutchLocale = new Locales(session).DutchNetherlands;
                var brand = new BrandBuilder(session).WithDefaults(session, config).Build();

                var serialisedProductType = new ProductTypes(session).FindBy(M.ProductType.Name, "serialisedProductType");

                if (serialisedProductType == null)
                {
                    var size = new SerialisedItemCharacteristicTypeBuilder(session)
                        .WithName("Size")
                        .WithLocalisedName(new LocalisedTextBuilder(session).WithText("Afmeting").WithLocale(dutchLocale).Build())
                        .Build();

                    var weight = new SerialisedItemCharacteristicTypeBuilder(session)
                        .WithName("Weight")
                        .WithLocalisedName(new LocalisedTextBuilder(session).WithText("Gewicht").WithLocale(dutchLocale).Build())
                        .WithUnitOfMeasure(new UnitsOfMeasure(session).Kilogram)
                        .Build();

                    serialisedProductType = new ProductTypeBuilder(session)
                        .WithName("serialisedProductType")
                        .WithSerialisedItemCharacteristicType(size)
                        .WithSerialisedItemCharacteristicType(weight)
                        .Build();
                }

                @this.WithInventoryItemKind(new InventoryItemKinds(session).Serialised);
                @this.WithName(config.faker.Commerce.ProductMaterial());
                @this.WithDescription(config.faker.Lorem.Words().ToString());
                @this.WithComment(config.faker.Lorem.Words().ToString());
                @this.WithInternalComment(config.faker.Lorem.Words().ToString());
                @this.WithKeywords(config.faker.Lorem.Words().ToString());
                @this.WithUnitOfMeasure(new UnitsOfMeasure(session).Piece);
                @this.WithPrimaryPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 200, height: 56)).Build());
                @this.WithPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 200, height: 56)).Build());
                @this.WithPhoto(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl(width: 200, height: 56)).Build());
                @this.WithElectronicDocument(new MediaBuilder(session).WithFileName("doc1.en.pdf").WithInData(config.faker.Random.Bytes(1000)).Build());
                @this.WithElectronicDocument(new MediaBuilder(session).WithFileName("doc2.en.pdf").WithInData(config.faker.Random.Bytes(1000)).Build());
                @this.WithProductIdentification(new SkuIdentificationBuilder(session).WithDefaults(session, config).Build());
                @this.WithProductIdentification(new EanIdentificationBuilder(session).WithDefaults(session, config).Build());
                @this.WithProductIdentification(new ManufacturerIdentificationBuilder(session).WithDefaults(session, config).Build());
                @this.WithDefaultFacility(internalOrganisation.FacilitiesWhereOwner?.First);
                @this.WithProductType(serialisedProductType);
                @this.WithBrand(brand);
                @this.WithModel(brand.Models.First);
                @this.WithHsCode(config.faker.Random.Number(99999999).ToString());
                @this.WithManufacturedBy(new OrganisationBuilder(session).WithManufacturerDefaults(session, config).Build());

                foreach (Locale additionalLocale in session.GetSingleton().AdditionalLocales)
                {
                    @this.WithLocalisedName(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                    @this.WithLocalisedDescription(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                    @this.WithLocalisedComment(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                    @this.WithLocalisedKeyword(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());

                    var localisedDocument = new MediaBuilder(session).WithFileName($"doc1.{additionalLocale.Country.IsoCode}.pdf").WithInData(config.faker.Random.Bytes(1000)).Build();
                    @this.WithLocalisedElectronicDocument(new LocalisedMediaBuilder(session).WithMedia(localisedDocument).WithLocale(additionalLocale).Build());
                }
            }

            return @this;
        }
    }
}
