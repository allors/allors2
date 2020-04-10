namespace src.allors.material.@base.objects.salesorder.create
{
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Xunit;

    public static partial class SalesOrderCreateComponentExtensions
    {
        public static SalesOrderCreateComponent Build(this SalesOrderCreateComponent @this, SalesOrder salesOrder, bool minimal = false)
        {
            if (!minimal)
            {
                @this.ShipFromAddress.Select(salesOrder.ShipFromAddress);
                @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());
                @this.ShipToAddress.Select(salesOrder.ShipToAddress);
                @this.BillToCustomer.Select(salesOrder.BillToCustomer.DisplayName());
                @this.BillToContactMechanism.Select(salesOrder.BillToContactMechanism);
                @this.ShipToEndCustomer.Select(salesOrder.ShipToEndCustomer.DisplayName());
                @this.ShipToEndCustomerAddress.Select(salesOrder.ShipToEndCustomerAddress);
                @this.BillToEndCustomer.Select(salesOrder.BillToEndCustomer.DisplayName());
                @this.BillToEndCustomerContactMechanism.Select(salesOrder.BillToEndCustomerContactMechanism);
                @this.CustomerReference.Set(salesOrder.CustomerReference);
                @this.Description.Set(salesOrder.Description);
                @this.InternalComment.Set(salesOrder.InternalComment);

                if (salesOrder.ExistBillToContactPerson)
                {
                    @this.BillToContactPerson.Select(salesOrder.BillToContactPerson);
                }

                if (salesOrder.ExistShipToEndCustomerContactPerson)
                {
                    @this.ShipToEndCustomerContactPerson.Select(salesOrder.ShipToEndCustomerContactPerson);
                }

                if (salesOrder.ExistShipToContactPerson)
                {
                    @this.ShipToContactPerson.Select(salesOrder.ShipToContactPerson);
                }
            }

            return @this;
        }

        public static void AssertFull(this SalesOrderCreateComponent @this, SalesOrder shipment)
        {
            Assert.True(shipment.ExistShipToAddress);
            Assert.True(shipment.ExistComment);
        }
    }
}
