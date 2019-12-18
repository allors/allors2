using Allors.Domain.TestPopulation;

namespace src.allors.material.@base.objects.customershipment.create
{
    using Allors.Domain;
    using Xunit;

    public static partial class CustomerShipmentCreateComponentExtensions
    {
        public static CustomerShipmentCreateComponent Build(this CustomerShipmentCreateComponent @this, CustomerShipment shipment, bool minimal = false)
        {
            @this.ShipToParty.Select(shipment.ShipToParty.PartyName);

            if (!minimal)
            {
                @this.ShipToAddress.Set(shipment.ShipToAddress?.DisplayName());
                @this.ShipToContactPerson.Set(shipment.ShipToContactPerson?.PartyName);
                @this.ShipFromAddress.Set(shipment.ShipFromParty?.ShippingAddress.DisplayName());
                @this.ShipmentMethod.Set(shipment.ShipmentMethod.Name);
                @this.ShipFromFacility.Set(((Organisation)shipment.ShipFromParty).FacilitiesWhereOwner?.First.Name);
                @this.Carrier.Set(shipment.Carrier.Name);
                @this.EstimatedShipDate.Set(shipment.EstimatedShipDate.Value.Date);
                @this.EstimatedArrivalDate.Set(shipment.EstimatedArrivalDate.Value.Date);
                @this.HandlingInstruction.Set(shipment.HandlingInstruction);
                @this.Comment.Set(shipment.Comment);
            }

            return @this;
        }

        public static void AssertFull(this CustomerShipmentCreateComponent @this, CustomerShipment shipment)
        {
            Assert.True(shipment.ExistShipToParty);
            Assert.True(shipment.ExistShipToAddress);
            Assert.True(shipment.ExistHandlingInstruction);
            Assert.True(shipment.ExistComment);
        }
    }
}
