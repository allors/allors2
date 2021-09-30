namespace libs.angular.material.@base.src.export.objects.salesorder.create
{
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;

    public static partial class SalesOrderCreateComponentExtensions
    {
        public static SalesOrderCreateComponent BuildForOrganisationInternalDefaults(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
        {
            @this.BillToCustomer.Select(salesOrder.BillToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.BillToContactPerson.Select(salesOrder.BillToContactPerson);
            @this.DerivedBillToContactMechanism.Select(salesOrder.DerivedBillToContactMechanism);
            @this.DerivedShipFromAddress.Select(salesOrder.DerivedShipFromAddress);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.DerivedShipToAddress.Select(salesOrder.DerivedShipToAddress);
            @this.ShipToContactPerson.Select(salesOrder.ShipToContactPerson);
            @this.CustomerReference.Set(salesOrder.CustomerReference);

            return @this;
        }

        public static SalesOrderCreateComponent BuildForOrganisationExternalDefaults(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
        {
            @this.CustomerReference.Set(salesOrder.CustomerReference);
            @this.BillToCustomer.Select(salesOrder.BillToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.BillToContactPerson.Select(salesOrder.BillToContactPerson);
            @this.DerivedBillToContactMechanism.Select(salesOrder.DerivedBillToContactMechanism);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.DerivedShipToAddress.Select(salesOrder.DerivedShipToAddress);
            @this.DerivedShipFromAddress.Select(salesOrder.DerivedShipFromAddress);
            @this.ShipToContactPerson.Select(salesOrder.ShipToContactPerson);

            return @this;
        }

        public static SalesOrderCreateComponent BuildForPersonInternalDefaults(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
        {
            @this.BillToCustomer.Select(salesOrder.BillToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.BillToContactPerson.Select(salesOrder.BillToContactPerson);
            @this.DerivedBillToContactMechanism.Select(salesOrder.DerivedBillToContactMechanism);
            @this.DerivedShipFromAddress.Select(salesOrder.DerivedShipFromAddress);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.DerivedShipToAddress.Select(salesOrder.DerivedShipToAddress);
            @this.ShipToContactPerson.Select(salesOrder.ShipToContactPerson);
            @this.CustomerReference.Set(salesOrder.CustomerReference);

            return @this;
        }

        public static SalesOrderCreateComponent BuildForPersonExternalDefaults(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
        {
            @this.BillToCustomer.Select(salesOrder.BillToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.DerivedBillToContactMechanism.Select(salesOrder.DerivedBillToContactMechanism);
            @this.DerivedShipFromAddress.Select(salesOrder.DerivedShipFromAddress);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.DerivedShipToAddress.Select(salesOrder.DerivedShipToAddress);
            @this.CustomerReference.Set(salesOrder.CustomerReference);

            return @this;
        }
    }
}
