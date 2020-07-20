namespace src.allors.material.@base.objects.salesorder.create
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
            @this.BillToContactMechanism.Select(salesOrder.BillToContactMechanism);
            @this.ShipFromAddress.Select(salesOrder.ShipFromAddress);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.ShipToAddress.Select(salesOrder.ShipToAddress);
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
            @this.BillToContactMechanism.Select(salesOrder.BillToContactMechanism);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.ShipToAddress.Select(salesOrder.ShipToAddress);
            @this.ShipFromAddress.Select(salesOrder.ShipFromAddress);
            @this.ShipToContactPerson.Select(salesOrder.ShipToContactPerson);

            return @this;
        }

        public static SalesOrderCreateComponent BuildForPersonInternalDefaults(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
        {
            @this.BillToCustomer.Select(salesOrder.BillToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.BillToContactPerson.Select(salesOrder.BillToContactPerson);
            @this.BillToContactMechanism.Select(salesOrder.BillToContactMechanism);
            @this.ShipFromAddress.Select(salesOrder.ShipFromAddress);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.ShipToAddress.Select(salesOrder.ShipToAddress);
            @this.ShipToContactPerson.Select(salesOrder.ShipToContactPerson);
            @this.CustomerReference.Set(salesOrder.CustomerReference);

            return @this;
        }

        public static SalesOrderCreateComponent BuildForPersonExternalDefaults(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
        {
            @this.BillToCustomer.Select(salesOrder.BillToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.BillToContactMechanism.Select(salesOrder.BillToContactMechanism);
            @this.ShipFromAddress.Select(salesOrder.ShipFromAddress);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());

            @this.Driver.WaitForAngular();

            @this.ShipToAddress.Select(salesOrder.ShipToAddress);
            @this.CustomerReference.Set(salesOrder.CustomerReference);

            return @this;
        }
    }
}
