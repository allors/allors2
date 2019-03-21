using System.Linq;

namespace Allors.Domain
{
    using Meta;

    using Resources;

    public partial class SalesOrderItem
    {
        #region Transitional
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
        {
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemState),
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemShipmentState),
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemInvoiceState),
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemPaymentState),
        };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;
        #endregion

        public bool IsValid => !(this.SalesOrderItemState.Cancelled || this.SalesOrderItemState.Rejected);

        public Part Part
        {
            get
            {
                if (this.ExistProduct)
                {
                    var nonUnifiedGood = this.Product as NonUnifiedGood;
                    var unifiedGood = this.Product as UnifiedGood;
                    return unifiedGood ?? nonUnifiedGood?.Part;
                }

                return null;
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSalesOrderItemState)
            {
                this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Created;
            }

            if (this.ExistProduct && !this.ExistInvoiceItemType)
            {
                this.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).ProductItem;
            }

            if (!this.ExistSalesOrderItemShipmentState)
            {
                this.SalesOrderItemShipmentState = new SalesOrderItemShipmentStates(this.Strategy.Session).NotShipped;
            }

            if (!this.ExistSalesOrderItemInvoiceState)
            {
                this.SalesOrderItemInvoiceState = new SalesOrderItemInvoiceStates(this.Strategy.Session).NotInvoiced;
            }

            if (!this.ExistSalesOrderItemPaymentState)
            {
                this.SalesOrderItemPaymentState = new SalesOrderItemPaymentStates(this.Strategy.Session).NotPaid;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            var salesOrder = this.SalesOrderWhereSalesOrderItem;
            derivation.AddDependency(salesOrder, this);

            foreach (SalesOrderItem featureItem in this.OrderedWithFeatures)
            {
                derivation.AddDependency(this, featureItem);
            }

            if (this.ExistReservedFromNonSerialisedInventoryItem)
            {
                derivation.AddDependency(this, this.ReservedFromNonSerialisedInventoryItem);

                if (!this.ReservedFromNonSerialisedInventoryItem.Equals(this.PreviousReservedFromNonSerialisedInventoryItem))
                {
                    derivation.AddDependency(this,  this.PreviousReservedFromNonSerialisedInventoryItem);
                }
            }
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            if (this.ExistSalesOrderItemInvoiceState && this.SalesOrderItemInvoiceState.Equals(new SalesOrderItemInvoiceStates(this.Strategy.Session).Invoiced))
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.QuantityOrdered, Operations.Write));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.AssignedUnitPrice, Operations.Write));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.InvoiceItemType, Operations.Write));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Product, Operations.Write));
            }
        }

        public void AppsCancel(OrderItemCancel method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Cancelled;
            this.OnCancelOrReject();
        }

        public void AppsConfirm(OrderItemConfirm method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void AppsReject(OrderItemReject method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Rejected;
            this.OnCancelOrReject();
        }

        public void AppsApprove(OrderItemApprove method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void AppsContinue(SalesOrderItemContinue method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;
        }

        internal void DecreasePendingShipmentQuantity(decimal diff)
        {
            var pendingShipment = this.ShipToParty.AppsGetPendingCustomerShipmentForStore(this.ShipToAddress, this.SalesOrderWhereSalesOrderItem.Store, this.SalesOrderWhereSalesOrderItem.ShipmentMethod);

            if (pendingShipment != null)
            {
                foreach (ShipmentItem shipmentItem in pendingShipment.ShipmentItems)
                {
                    foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        if (orderShipment.OrderItem.Equals(this))
                        {
                            this.QuantityPendingShipment -= diff;
                            pendingShipment.AppsOnDeriveQuantityDecreased(shipmentItem, this, diff);
                            break;
                        }
                    }
                }
            }
        }

        private void OnCancelOrReject()
        {
            if (this.ExistReservedFromNonSerialisedInventoryItem && this.ExistQuantityCommittedOut)
            {
                var inventoryAssignment = this.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault();
                if (inventoryAssignment != null)
                {
                    inventoryAssignment.Quantity = 0 - this.QuantityCommittedOut;
                }

                this.DecreasePendingShipmentQuantity(this.QuantityPendingShipment);
            }

            if (this.ExistReservedFromSerialisedInventoryItem)
            {
                this.ReservedFromSerialisedInventoryItem.SerialisedInventoryItemState = new SerialisedInventoryItemStates(this.Strategy.Session).Available;
            }
        }
    }
}