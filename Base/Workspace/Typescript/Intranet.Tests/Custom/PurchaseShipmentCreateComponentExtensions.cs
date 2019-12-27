using Allors.Domain.TestPopulation;

namespace src.allors.material.@base.objects.purchaseshipment.create
{
    using Allors.Domain;
    using Xunit;

    public static partial class PurchaseShipmentCreateComponentExtensions
    {
        public static PurchaseShipmentCreateComponent Build(this PurchaseShipmentCreateComponent @this, PurchaseShipment shipment, bool minimal = false)
        {
            @this.ShipFromParty.Select(shipment.ShipFromParty.PartyName);

            if (!minimal)
            {
                @this.ShipFromContactPerson.Set(shipment.ShipFromContactPerson?.PartyName);
                @this.ShipToAddress.Set(shipment.ShipToAddress?.DisplayName());
                @this.ShipToContactPerson.Set(shipment.ShipToContactPerson?.PartyName);
                @this.ShipToAddress.Set(shipment.ShipToParty?.ShippingAddress.DisplayName());
            }

            return @this;
        }

        public static void AssertFull(this PurchaseShipmentCreateComponent @this, PurchaseShipment shipment)
        {
            Assert.True(shipment.ExistShipFromParty);
            Assert.True(shipment.ExistShipFromContactPerson);
            Assert.True(shipment.ExistShipToParty);
            Assert.True(shipment.ExistShipToContactPerson);
            Assert.True(shipment.ExistShipToAddress);
        }
    }
}
