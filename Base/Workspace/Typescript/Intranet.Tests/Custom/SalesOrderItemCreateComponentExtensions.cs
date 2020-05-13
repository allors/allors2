namespace src.allors.material.@base.objects.salesorderitem.edit
{
    using Allors.Domain;

    public static partial class SalesOrderItemEditComponentExtensions
    {
        public static SalesOrderItemEditComponent BuildForDefaults(this SalesOrderItemEditComponent @this, SalesOrderItem salesOrderItem)
        {
            @this.SalesOrderItemInvoiceItemType_1.Select(salesOrderItem.InvoiceItemType);
            @this.PriceableAssignedUnitPrice_2.Set(salesOrderItem.AssignedUnitPrice.ToString());
            @this.Description.Set(salesOrderItem.Description);
            @this.Comment.Set(salesOrderItem.Comment);
            @this.InternalComment.Set(salesOrderItem.InternalComment);

            return @this;
        }

        public static SalesOrderItemEditComponent BuildForProductItemDefaults(this SalesOrderItemEditComponent @this, SalesOrderItem salesOrderItem)
        {
            @this.SalesOrderItemInvoiceItemType_2.Select(salesOrderItem.InvoiceItemType);
            @this.Product.Select(salesOrderItem.Product.Name);
            @this.QuantityOrdered.Set(salesOrderItem.QuantityOrdered.ToString());
            @this.SerialisedItem.Select(salesOrderItem.SerialisedItem);
            @this.PriceableAssignedUnitPrice_2.Set(salesOrderItem.AssignedUnitPrice.ToString());
            @this.Description.Set(salesOrderItem.Description);
            @this.Comment.Set(salesOrderItem.Comment);
            @this.InternalComment.Set(salesOrderItem.InternalComment);

            return @this;
        }

        public static SalesOrderItemEditComponent BuildForPartItemDefaults(this SalesOrderItemEditComponent @this, SalesOrderItem salesOrderItem)
        {
            @this.SalesOrderItemInvoiceItemType_2.Select(salesOrderItem.InvoiceItemType);
            @this.Product.Select(salesOrderItem.Product.Name);
            @this.QuantityOrdered.Set(salesOrderItem.QuantityOrdered.ToString());
            @this.SerialisedItem.Select(salesOrderItem.SerialisedItem);
            @this.PriceableAssignedUnitPrice_2.Set(salesOrderItem.AssignedUnitPrice.ToString());
            @this.Description.Set(salesOrderItem.Description);
            @this.Comment.Set(salesOrderItem.Comment);
            @this.InternalComment.Set(salesOrderItem.InternalComment);

            return @this;
        }
    }
}
