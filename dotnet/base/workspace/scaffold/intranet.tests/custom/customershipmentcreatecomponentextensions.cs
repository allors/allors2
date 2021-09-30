namespace libs.angular.material.@base.src.export.objects.customershipment.create
{
    using Allors.Domain.TestPopulation;
    using Allors.Domain;
    using Components;
    using Xunit;

    public static partial class CustomerShipmentCreateComponentExtensions
    {
        public static CustomerShipmentCreateComponent Build(this CustomerShipmentCreateComponent @this, CustomerShipment shipment, bool minimal = false)
        {
            @this.ShipToParty.Select(shipment.ShipToParty.DisplayName());

            @this.Driver.WaitForAngular();

            if (!minimal)
            {
                @this.ShipToAddress.Select(shipment.ShipToAddress);
                @this.ShipFromAddress.Select(shipment.ShipFromParty?.ShippingAddress);
                @this.ShipmentMethod.Select(shipment.ShipmentMethod);
                @this.ShipFromFacility.Select(((Organisation)shipment.ShipFromParty)?.FacilitiesWhereOwner?.First);
                @this.Carrier.Select(shipment.Carrier);
                @this.EstimatedShipDate.Set(shipment.EstimatedShipDate.Value.Date);
                @this.EstimatedArrivalDate.Set(shipment.EstimatedArrivalDate.Value.Date);
                @this.HandlingInstruction.Set(shipment.HandlingInstruction);
                @this.Comment.Set(shipment.Comment);

                if (shipment.ExistShipToContactPerson)
                {
                    @this.ShipToContactPerson.Select(shipment.ShipToContactPerson);
                }
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
