// <copyright file="SupplierOfferingTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Domain.TestPopulation;
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
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
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
                    .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
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

        [Fact]
        public void GivenSupplierOffering_WhenCalculatingUnitSellingPrice_ThenConsiderHighestHistoricalPurchaseRate()
        {
            var settings = this.Session.GetSingleton().Settings;

            var supplier_1 = new OrganisationBuilder(this.Session).WithName("supplier uno").Build();
            var supplier_2 = new OrganisationBuilder(this.Session).WithName("supplier dos").Build();
            var supplier_3 = new OrganisationBuilder(this.Session).WithName("supplier tres").Build();
            var supplier_4 = new OrganisationBuilder(this.Session).WithName("supplier cuatro").Build();

            var internalOrganisation = new Organisations(this.Session).Extent().First(v => Equals(v.Name, "internalOrganisation"));

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier_1)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(this.Session.Now().AddYears(-3))
                .Build();
            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier_2)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(this.Session.Now().AddYears(-2))
                .Build();
            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier_3)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .Build();
            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier_4)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(this.Session.Now().AddMonths(-6))
                .Build();

            var finishedGood = new NonUnifiedPartBuilder(this.Session)
                .WithNonSerialisedDefaults(internalOrganisation)
                .Build();

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment).WithPart(finishedGood).Build();

            this.Session.Derive();

            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var piece = new UnitsOfMeasure(this.Session).Piece;

            new BasePriceBuilder(this.Session)
                .WithPart(finishedGood)
                .WithFromDate(this.Session.Now())
                .WithPrice(100)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(finishedGood)
                .WithSupplier(supplier_1)
                .WithFromDate(this.Session.Now().AddMonths(-6))
                .WithThroughDate(this.Session.Now().AddMonths(-3))
                .WithUnitOfMeasure(piece)
                .WithPrice(100)
                .WithCurrency(euro)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(finishedGood)
                .WithSupplier(supplier_2)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .WithUnitOfMeasure(piece)
                .WithPrice(120)
                .WithCurrency(euro)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(finishedGood)
                .WithSupplier(supplier_3)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(piece)
                .WithPrice(99)
                .WithCurrency(euro)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(finishedGood)
                .WithSupplier(supplier_4)
                .WithFromDate(this.Session.Now().AddDays(7))
                .WithThroughDate(this.Session.Now().AddDays(30))
                .WithUnitOfMeasure(piece)
                .WithPrice(135)
                .WithCurrency(euro)
                .Build();

            this.Session.Derive();

            var customer = internalOrganisation.CreateB2BCustomer(this.Session.Faker());

            var workEffort = new WorkTaskBuilder(this.Session)
                .WithName("Activity")
                .WithCustomer(customer)
                .WithTakenBy(internalOrganisation)
                .Build();

            var workEffortInventoryAssignement = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(finishedGood.InventoryItemsWherePart.First())
                .WithQuantity(1)
                .Build();

            this.Session.Derive();

            /*Purchase price times InternalSurchargePercentage
            var sellingPrice = Math.Round(135 * (1 + (this.Session.GetSingleton().Settings.PartSurchargePercentage / 100)), 2);*/

            Assert.Equal(100, workEffortInventoryAssignement.UnitSellingPrice);
        }
    }
}
