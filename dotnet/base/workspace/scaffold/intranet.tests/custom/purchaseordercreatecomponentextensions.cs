namespace libs.angular.material.@base.src.export.objects.purchaseorder.create
{
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;

    public static partial class PurchaseOrderCreateComponentExtensions
    {
        public static PurchaseOrderCreateComponent BuildForDefaults(this PurchaseOrderCreateComponent @this, PurchaseOrder purchaseOrder)
        {
            @this.TakenViaSupplier.Select(purchaseOrder.TakenViaSupplier.DisplayName());
            @this.DerivedTakenViaContactMechanism.Select(purchaseOrder.DerivedTakenViaContactMechanism);
            @this.TakenViaContactPerson.Select(purchaseOrder.TakenViaContactPerson);
            @this.BillToContactPerson.Select(purchaseOrder.BillToContactPerson);
            @this.DerivedBillToContactMechanism.Select(purchaseOrder.DerivedBillToContactMechanism);

            @this.Driver.WaitForAngular();

            @this.DerivedShipToAddress.Select(purchaseOrder.DerivedShipToAddress);
            @this.ShipToContactPerson.Select(purchaseOrder.ShipToContactPerson);
            @this.CustomerReference.Set(purchaseOrder.CustomerReference);
            @this.StoredInFacility.Select(purchaseOrder.StoredInFacility);
            @this.Description.Set(purchaseOrder.Description);
            @this.InternalComment.Set(purchaseOrder.InternalComment);

            return @this;
        }
    }
}
