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
    using NUnit.Framework;

    [TestFixture]
    public class PurchaseShipmentTests : DomainTest
    {
        [Test]
        public void GivenPurchaseShipmentBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseShipmentObjectStates(this.DatabaseSession).Created, shipment.CurrentObjectState);
            Assert.AreEqual(internalOrganisation, shipment.ShipToParty);
            Assert.AreEqual(internalOrganisation.ShippingAddress, shipment.ShipToAddress);
            Assert.AreEqual(shipment.ShipToParty, shipment.ShipToParty);
        }

        [Test]
        public void GivenPurchaseShipment_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();

            this.DatabaseSession.Commit();

            var builder = new PurchaseShipmentBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithShipFromParty(supplier);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenPurchaseShipment_WhenGettingShipmentNumberWithoutFormat_ThenShipmentNumberShouldBeReturned()
        {
            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession).Build();

            var shipment1 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipToParty(internalOrganisation).Build();

            Assert.AreEqual("1", shipment1.ShipmentNumber);

            var shipment2 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipToParty(internalOrganisation).Build();

            Assert.AreEqual("2", shipment2.ShipmentNumber);
        }

        [Test]
        public void GivenPurchaseShipment_WhenGettingShipmentNumberWithFormat_ThenFormattedShipmentNumberShouldBeReturned()
        {
            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession).Build();
            internalOrganisation.IncomingShipmentNumberPrefix = "the format is ";

            var shipment1 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipToParty(internalOrganisation).Build();

            Assert.AreEqual("the format is 1", shipment1.ShipmentNumber);

            var shipment2 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipToParty(internalOrganisation).Build();

            Assert.AreEqual("the format is 2", shipment2.ShipmentNumber);
        }

        [Test]
        public void GivenPurchaseShipmentWithShipToAddress_WhenDeriving_ThenDerivedShipToAddressMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            PostalAddress postalAddress = null;
            foreach (PartyContactMechanism partyContactMechanism in internalOrganisation.PartyContactMechanisms)
            {
                if (partyContactMechanism.ContactPurpose.Equals(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress))
                {
                    postalAddress = partyContactMechanism.ContactMechanism as PostalAddress;
                }
            }

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();
            
            this.DatabaseSession.Derive(true);

            Assert.AreEqual(postalAddress, shipment.ShipToAddress);
        }

        [Test]
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

            Assert.AreEqual(shippingAddress.ContactMechanism, order.ShipToAddress);
        }
    }
}