//------------------------------------------------------------------------------------------------- 
// <copyright file="PurchaseShipmentTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Meta;
    using Xunit;

    
    public class PurchaseShipmentTests : DomainTest
    {
        [Fact]
        public void GivenPurchaseShipmentBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(new PurchaseShipmentObjectStates(this.DatabaseSession).Created, shipment.CurrentObjectState);
            Assert.Equal(internalOrganisation, shipment.ShipToParty);
            Assert.Equal(internalOrganisation.ShippingAddress, shipment.ShipToAddress);
            Assert.Equal(shipment.ShipToParty, shipment.ShipToParty);
        }

        [Fact]
        public void GivenPurchaseShipment_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();

            this.DatabaseSession.Commit();

            var builder = new PurchaseShipmentBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithShipFromParty(supplier);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPurchaseShipment_WhenGettingShipmentNumberWithoutFormat_ThenShipmentNumberShouldBeReturned()
        {
            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession).Build();

            var shipment1 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipToParty(internalOrganisation).Build();

            Assert.Equal("1", shipment1.ShipmentNumber);

            var shipment2 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipToParty(internalOrganisation).Build();

            Assert.Equal("2", shipment2.ShipmentNumber);
        }

        [Fact]
        public void GivenPurchaseShipment_WhenGettingShipmentNumberWithFormat_ThenFormattedShipmentNumberShouldBeReturned()
        {
            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession).Build();
            internalOrganisation.IncomingShipmentNumberPrefix = "the format is ";

            var shipment1 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipToParty(internalOrganisation).Build();

            Assert.Equal("the format is 1", shipment1.ShipmentNumber);

            var shipment2 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipToParty(internalOrganisation).Build();

            Assert.Equal("the format is 2", shipment2.ShipmentNumber);
        }

        [Fact]
        public void GivenPurchaseShipmentWithShipToAddress_WhenDeriving_ThenDerivedShipToAddressMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            PostalAddress postalAddress = null;
            foreach (PartyContactMechanism partyContactMechanism in internalOrganisation.PartyContactMechanisms)
            {
                if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress))
                {
                    postalAddress = partyContactMechanism.ContactMechanism as PostalAddress;
                }
            }

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();
            
            this.DatabaseSession.Derive(true);

            Assert.Equal(postalAddress, shipment.ShipToAddress);
        }

        [Fact]
        public void GivenPurchaseShipmentWithShipToCustomerWithshippingAddress_WhenDeriving_ThenDerivedShipToCustomerAndDerivedShipToAddressMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();

            var shippingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(shipToAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            internalOrganisation.AddPartyContactMechanism(shippingAddress);
            
            this.DatabaseSession.Derive(true);

            var order = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(shippingAddress.ContactMechanism, order.ShipToAddress);
        }
    }
}