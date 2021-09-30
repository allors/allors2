using Allors.Domain.TestPopulation;

namespace libs.angular.material.@base.src.export.objects.purchaseshipment.create
{
    using Allors.Domain;
    using Xunit;

    public static partial class PurchaseShipmentCreateComponentExtensions
    {
        public static PurchaseShipmentCreateComponent Build(this PurchaseShipmentCreateComponent @this, PurchaseShipment shipment, bool minimal = false)
        {
            @this.ShipFromParty.Select(shipment.ShipFromParty.DisplayName());

            if (!minimal)
            {
                @this.ShipToAddress.Select(shipment.ShipToAddress);
                @this.ShipToAddress.Select(shipment.ShipToParty?.ShippingAddress);

                if (shipment.ExistShipFromContactPerson)
                {
                    @this.ShipFromContactPerson.Select(shipment.ShipFromContactPerson);
                }

                if (shipment.ExistShipToContactPerson)
                {
                    @this.ShipToContactPerson.Select(shipment.ShipToContactPerson);
                }
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
