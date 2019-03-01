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

            if (derivation.IsModified(this))
            {
                var salesOrder = this.SalesOrderWhereSalesOrderItem;
                derivation.Mark(salesOrder);
                derivation.AddDependency(salesOrder, this);

                foreach (SalesOrderItem featureItem in this.OrderedWithFeatures)
                {
                    derivation.Mark(featureItem);
                    derivation.AddDependency(this, featureItem);
                }

                if (this.ExistOrderShipmentsWhereOrderItem)
                {
                    var orderShipments = this.OrderShipmentsWhereOrderItem;
                    foreach (OrderShipment orderShipment in orderShipments)
                    {
                        derivation.Mark(orderShipment);
                        derivation.AddDependency(orderShipment, this);
                    }
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
        }

        public void AppsConfirm(OrderItemConfirm method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void AppsReject(OrderItemReject method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Rejected;
        }

        public void AppsApprove(OrderItemApprove method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void AppsContinue(SalesOrderItemContinue method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;
        }

        internal void SetQuantitiesWithInventoryFirstTime(IDerivation derivation)
        {
            this.QuantityReserved = 0;
            this.QuantityRequestsShipping = 0;
            this.QuantityShortFalled = 0;

            this.QuantityRequestsShipping = this.QuantityOrdered > this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise ?
                this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise : this.QuantityOrdered;

            if (this.QuantityRequestsShipping < 0)
            {
                this.QuantityRequestsShipping = 0;
            }

            this.QuantityReserved = this.QuantityOrdered;
            this.QuantityShortFalled = this.QuantityOrdered - this.QuantityRequestsShipping;

            this.ReservedFromNonSerialisedInventoryItem.OnDerive(x => x.WithDerivation(derivation));
        }

        internal void DecreasePendingShipmentQuantity(IDerivation derivation, decimal diff)
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
                            pendingShipment.AppsOnDeriveQuantityDecreased(derivation, shipmentItem, this, diff);
                            break;
                        }
                    }
                }
            }
        }
    }
}