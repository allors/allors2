namespace src.allors.material.@base.objects.salesorderitem.edit
{
    using Allors.Domain;
    using Allors.Domain.TestPopulation;

    public static partial class SalesOrderItemEditComponentExtensions
    {
        public static SalesOrderItemEditComponent BuildForDefaults(this SalesOrderItemEditComponent @this, SalesOrderItem salesOrderItem)
        {
            @this.Description.Set(salesOrderItem.Description);
            @this.Comment.Set(salesOrderItem.Comment);
            @this.InternalComment.Set(salesOrderItem.InternalComment);
            @this.SalesOrderItemInvoiceItemType_1.Select(salesOrderItem.InvoiceItemType);
            @this.Product.Select(salesOrderItem.Product.Name);
            @this.SerialisedItem.Select(salesOrderItem.SerialisedItem);
            @this.QuantityOrdered.Set(salesOrderItem.QuantityOrdered.ToString());
            @this.PriceableAssignedUnitPrice_2.Set(salesOrderItem.AssignedUnitPrice.ToString());


            return @this;
        }

        public static SalesOrderItemEditComponent BuildForGSEDefaults(this SalesOrderItemEditComponent @this, SalesOrderItem salesOrderItem)
        {
            @this.Description.Set(salesOrderItem.Description);
            @this.Comment.Set(salesOrderItem.Comment);
            @this.InternalComment.Set(salesOrderItem.InternalComment);
            @this.SalesOrderItemInvoiceItemType_2.Select(salesOrderItem.InvoiceItemType);
            @this.SerialisedItem.Select(salesOrderItem.SerialisedItem);
            @this.QuantityOrdered.Set(salesOrderItem.QuantityOrdered.ToString());
            @this.PriceableAssignedUnitPrice_2.Set(salesOrderItem.AssignedUnitPrice.ToString());

            return @this;
        }
    }
}
