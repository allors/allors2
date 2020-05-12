namespace src.allors.material.@base.objects.salesorder.create
{
    using Allors.Domain;
    using Allors.Domain.TestPopulation;

    public static partial class SalesOrderCreateComponentExtensions
    {
        public static SalesOrderCreateComponent BuildForOrganisationInternalDefaults(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
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

        public static SalesOrderCreateComponent BuildForOrganisationExternalDefaults(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
        {
            @this.CustomerReference.Set(salesOrder.CustomerReference);
            @this.Description.Set(salesOrder.Description);
            @this.InternalComment.Set(salesOrder.InternalComment);
            @this.BillToCustomer.Select(salesOrder.BillToCustomer.DisplayName());
            @this.BillToContactPerson.Select(salesOrder.BillToContactPerson);
            @this.BillToContactMechanism.Select(salesOrder.BillToContactMechanism);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());
            @this.ShipToAddress.Select(salesOrder.ShipToAddress);
            @this.ShipFromAddress.Select(salesOrder.ShipFromAddress);
            @this.ShipToContactPerson.Select(salesOrder.ShipToContactPerson);

            return @this;
        }

        public static SalesOrderCreateComponent BuildForPersonInternalDefaults(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
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

        public static SalesOrderCreateComponent BuildForPersonExternalDefaults(this SalesOrderCreateComponent @this, SalesOrder salesOrder)
        {
            @this.BillToCustomer.Select(salesOrder.BillToCustomer.DisplayName());
            @this.BillToContactMechanism.Select(salesOrder.BillToContactMechanism);
            @this.ShipFromAddress.Select(salesOrder.ShipFromAddress);
            @this.ShipToAddress.Select(salesOrder.ShipToAddress);
            @this.ShipToCustomer.Select(salesOrder.ShipToCustomer.DisplayName());
            @this.CustomerReference.Set(salesOrder.CustomerReference);
            @this.Description.Set(salesOrder.Description);
            @this.InternalComment.Set(salesOrder.InternalComment);

            return @this;
        }
    }
}
