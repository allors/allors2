// <copyright file="PurchaseShipmentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class PurchaseShipmentTests : DomainTest
    {
        [Fact]
        public void GivenPurchaseShipmentBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();

            var shipment = new PurchaseShipmentBuilder(this.Session).WithShipmentMethod(new ShipmentMethods(this.Session).Ground).WithShipFromParty(supplier).Build();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Created, shipment.ShipmentState);
            Assert.Equal(this.InternalOrganisation.GeneralCorrespondence, shipment.ShipToAddress);
            Assert.Equal(this.InternalOrganisation, shipment.ShipToParty);
        }

        [Fact]
        public void GivenPurchaseShipment_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();

            this.Session.Commit();

            var builder = new PurchaseShipmentBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithShipmentMethod(new ShipmentMethods(this.Session).Ground);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithShipFromParty(supplier);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPurchaseShipment_WhenGettingShipmentNumberWithoutFormat_ThenShipmentNumberShouldBeReturned()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();

            var internalOrganisation = this.InternalOrganisation;
            internalOrganisation.RemoveIncomingShipmentNumberPrefix();

            var shipment1 = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(supplier)
                .Build();

            this.Session.Derive();

            Assert.Equal("1", shipment1.ShipmentNumber);

            var shipment2 = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(supplier)
                .Build();

            this.Session.Derive();

            Assert.Equal("2", shipment2.ShipmentNumber);
        }

        [Fact]
        public void GivenPurchaseShipment_WhenGettingShipmentNumberWithFormat_ThenFormattedShipmentNumberShouldBeReturned()
        {
            this.InternalOrganisation.PurchaseShipmentSequence = new PurchaseShipmentSequences(this.Session).EnforcedSequence;
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();

            var shipment1 = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(supplier)
                .Build();

            this.Session.Derive();

            Assert.Equal("incoming shipmentno: 1", shipment1.ShipmentNumber);

            var shipment2 = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(supplier)
                .Build();

            this.Session.Derive();

            Assert.Equal("incoming shipmentno: 2", shipment2.ShipmentNumber);
        }

        [Fact]
        public void GivenShipToWithoutShipmentNumberPrefix_WhenDeriving_ThenSortableShipmentNumberIsSet()
        {
            this.InternalOrganisation.RemoveIncomingShipmentNumberPrefix();
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();

            var shipment = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(supplier)
                .Build();

            this.Session.Derive();

            Assert.Equal(int.Parse(shipment.ShipmentNumber), shipment.SortableShipmentNumber);
        }

        [Fact]
        public void GivenShipToWithShipmentNumberPrefix_WhenDeriving_ThenSortableShipmentNumberIsSet()
        {
            this.InternalOrganisation.PurchaseShipmentSequence = new PurchaseShipmentSequences(this.Session).EnforcedSequence;
            this.InternalOrganisation.IncomingShipmentNumberPrefix= "prefix-";
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();

            var shipment = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(supplier)
                .Build();

            this.Session.Derive();

            Assert.Equal(int.Parse(shipment.ShipmentNumber.Split('-')[1]), shipment.SortableShipmentNumber);
        }

        [Fact]
        public void GivenPurchaseShipmentWithShipToCustomerWithshippingAddress_WhenDeriving_ThenDerivedShipToCustomerAndDerivedShipToAddressMustExist()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithAddress1("Haverwerf 15").WithPostalAddressBoundary(mechelen).Build();

            var shippingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipToAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            this.InternalOrganisation.AddPartyContactMechanism(shippingAddress);

            this.Session.Derive();

            var order = new PurchaseShipmentBuilder(this.Session).WithShipmentMethod(new ShipmentMethods(this.Session).Ground).WithShipFromParty(supplier).Build();

            this.Session.Derive();

            Assert.Equal(shippingAddress.ContactMechanism, order.ShipToAddress);
        }
    }
}
