// <copyright file="ShipmentItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;

namespace Allors.Domain
{
    public partial class ShipmentItem
    {
        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (method.SecurityTokens == null)
            {
                method.SecurityTokens = this.SyncedShipment?.SecurityTokens.ToArray();
            }

            if (method.DeniedPermissions == null)
            {
                method.DeniedPermissions = this.SyncedShipment?.DeniedPermissions.ToArray();
            }
        }

        public void BaseDelete(DeletableDelete method)
        {
            if (this.ExistItemIssuancesWhereShipmentItem)
            {
                foreach (ItemIssuance itemIssuance in this.ItemIssuancesWhereShipmentItem)
                {
                    itemIssuance.Delete();
                }
            }
        }

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistShipmentItemState)
            {
                this.ShipmentItemState= new ShipmentItemStates(this.Strategy.Session).Created;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            derivation.AddDependency(this.ShipmentWhereShipmentItem, this);
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ShipmentWhereShipmentItem.GetType().Name != typeof(CustomerShipment).Name)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Ship, Operations.Execute));
            }

            this.BaseOnDeriveCustomerShipmentItem(derivation);

            this.BaseOnDerivePurchaseShipmentItem(derivation);
        }

        public void BaseOnDerivePurchaseShipmentItem(IDerivation derivation)
        {
            if (this.ShipmentWhereShipmentItem is PurchaseShipment)
            {
                this.Quantity = 0;
                var shipmentReceipt = this.ShipmentReceiptWhereShipmentItem;
                this.Quantity += shipmentReceipt.QuantityAccepted + shipmentReceipt.QuantityRejected;
            }
        }

        public void BaseOnDeriveCustomerShipmentItem(IDerivation derivation)
        {
            if (this.ShipmentWhereShipmentItem is CustomerShipment && Equals(this.ShipmentItemState, new ShipmentItemStates(this.Strategy.Session).Shipped))
            {
                this.QuantityShipped = 0;
                foreach (PackagingContent packagingContent in this.PackagingContentsWhereShipmentItem)
                {
                    this.QuantityShipped += packagingContent.Quantity;
                }
            }
        }

        public void BaseShip(ShipmentItemShip method)
        {
            this.ShipmentItemState = new ShipmentItemStates(this.Strategy.Session).Shipped;
            foreach (OrderShipment orderShipment in this.OrderShipmentsWhereShipmentItem)
            {
                var inventoryAssignment = ((SalesOrderItem)orderShipment.OrderItem).SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault();
                if (inventoryAssignment != null)
                {
                    inventoryAssignment.Quantity -= orderShipment.Quantity;
                }
            }
        }

        public void Sync(Shipment shipment) => this.SyncedShipment = shipment;
    }
}
