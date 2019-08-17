// <copyright file="SupplierOfferingTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;
    using Xunit;

    public class SupplierOfferingTests : DomainTest
    {
        [Fact]
        public void GivenSupplierOffering_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("organisation").Build();
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.Session.Commit();

            var builder = new SupplierOfferingBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPrice(1);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithSupplier(supplier);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Pack);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithPart(part);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenNewGood_WhenDeriving_ThenNonSerialisedInventryItemIsCreatedForEveryFacility()
        {
            var settings = this.Session.GetSingleton().Settings;

            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var internalOrganisation = this.InternalOrganisation;
            var before = settings.DefaultFacility.InventoryItemsWhereFacility.Count;

            var secondFacility = new FacilityBuilder(this.Session)
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithOwner(this.InternalOrganisation)
                .WithName("second facility")
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .Build();

            var good = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("1")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good.Part)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(1)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .Build();

            this.Session.Derive();

            Assert.Equal(2, good.Part.InventoryItemsWherePart.Count);
            Assert.Equal(before + 1, settings.DefaultFacility.InventoryItemsWhereFacility.Count);
            Assert.Single(secondFacility.InventoryItemsWhereFacility);
        }

        [Fact]
        public void GivenNewGoodBasedOnPart_WhenDeriving_ThenNonSerialisedInventryItemIsCreatedForEveryPartAndFacility()
        {
            var settings = this.Session.GetSingleton().Settings;
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var before = settings.DefaultFacility.InventoryItemsWhereFacility.Count;

            var secondFacility = new FacilityBuilder(this.Session)
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithName("second facility")
                .WithOwner(this.InternalOrganisation)
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .Build();

            var good = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("1")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good.Part)
                .WithSupplier(supplier)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithFromDate(this.Session.Now())
                .WithPrice(1)
                .Build();

            this.Session.Derive();

            Assert.Equal(2, good.Part.InventoryItemsWherePart.Count);
            Assert.Equal(before + 1, settings.DefaultFacility.InventoryItemsWhereFacility.Count);
            Assert.Single(secondFacility.InventoryItemsWhereFacility);
        }
    }
}
