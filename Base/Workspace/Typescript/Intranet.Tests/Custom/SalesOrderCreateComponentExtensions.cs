namespace src.allors.material.@base.objects.salesorder.create
{
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Xunit;

    public static partial class SalesOrderCreateComponentExtensions
    {
        public static SalesOrderCreateComponent Build(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
        {
            @this.BillToCustomer.Select(salesOrder.BillToCustomer.DisplayName());
            @this.BillToContactPerson.Select(salesOrder.BillToContactPerson);
            @this.BillToContactMechanism.Select(salesOrder.BillToContactMechanism);
            @this.ShipFromAddress.Select(salesOrder.ShipFromAddress);
            @this.ShipToAddress.Select(salesOrder.ShipToAddress);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());
            @this.ShipToContactPerson.Select(salesOrder.ShipToContactPerson);
            @this.CustomerReference.Set(salesOrder.CustomerReference);
            @this.Description.Set(salesOrder.Description);
            @this.InternalComment.Set(salesOrder.InternalComment);

            return @this;
        }

        public static void AssertFull(this SalesOrderCreateComponent @this, SalesOrder shipment)
        {
            Assert.True(shipment.ExistShipToAddress);
            Assert.True(shipment.ExistComment);
        }
    }
}
