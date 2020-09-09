namespace libs.angular.material.@base.src.export.objects.purchaseorderitem.edit
{
    using Allors.Domain;

    public static partial class PurchaseOrderItemEditComponentExtensions
    {
        public static PurchaseOrderItemEditComponent BuildForNonSerializedPartDefaults(this PurchaseOrderItemEditComponent @this, PurchaseOrderItem purchaseOrderItem)
        {
            @this.OrderItemDescription_1.Set(purchaseOrderItem.Description);
            @this.Comment.Set(purchaseOrderItem.Comment);
            @this.InternalComment.Set(purchaseOrderItem.InternalComment);

            return @this;
        }

        public static PurchaseOrderItemEditComponent BuildForSerializedPartDefaults(this PurchaseOrderItemEditComponent @this, PurchaseOrderItem purchaseOrderItem)
        {
            @this.QuantityOrdered.Set(purchaseOrderItem.QuantityOrdered.ToString());
            @this.OrderItemDescription_1.Set(purchaseOrderItem.Description);
            @this.Comment.Set(purchaseOrderItem.Comment);
            @this.InternalComment.Set(purchaseOrderItem.InternalComment);

            return @this;
        }
    }
}
