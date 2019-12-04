// <copyright file="OrderShipment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;
    using Resources;

    public partial class OrderShipment
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.IsModified(this))
            {
                derivation.AddDependency(this.OrderItem, this);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ShipmentItem.ShipmentWhereShipmentItem is CustomerShipment customerShipment && this.OrderItem is SalesOrderItem salesOrderItem)
            {
                var quantityPendingShipment = this.OrderItem?.OrderShipmentsWhereOrderItem?.Where(v => v.ExistShipmentItem && !((CustomerShipment)v.ShipmentItem.ShipmentWhereShipmentItem).ShipmentState.Equals(new ShipmentStates(this.strategy.Session).Shipped)).Sum(v => v.Quantity);

                if (salesOrderItem.QuantityPendingShipment > quantityPendingShipment)
                {
                    var diff = this.Quantity * -1;
                    salesOrderItem.QuantityPendingShipment -= diff;
                    salesOrderItem.QuantityRequestsShipping -= diff;
                    customerShipment.BaseOnDeriveQuantityDecreased(this.ShipmentItem, salesOrderItem, diff);
                }

                if (this.strategy.IsNewInSession)
                {
                    var quantityPicked = this.OrderItem.OrderShipmentsWhereOrderItem.Select(v => v.ShipmentItem?.ItemIssuancesWhereShipmentItem.Sum(z => z.PickListItem.Quantity)).Sum();

                    if (salesOrderItem.ExistReservedFromNonSerialisedInventoryItem && this.Quantity > salesOrderItem.ReservedFromNonSerialisedInventoryItem.QuantityOnHand + quantityPicked)
                    {
                        derivation.Validation.AddError(this, M.OrderShipment.Quantity, ErrorMessages.SalesOrderItemQuantityToShipNowNotAvailable);
                    }
                    else if (this.Quantity > salesOrderItem.QuantityOrdered)
                    {
                        derivation.Validation.AddError(this, M.OrderShipment.Quantity, ErrorMessages.SalesOrderItemQuantityToShipNowIsLargerThanQuantityOrdered);
                    }
                    else
                    {
                        if (this.Quantity > salesOrderItem.QuantityOrdered - salesOrderItem.QuantityShipped - salesOrderItem.QuantityPendingShipment + salesOrderItem.QuantityReturned + quantityPicked)
                        {
                            derivation.Validation.AddError(this, M.OrderShipment.Quantity, ErrorMessages.SalesOrderItemQuantityToShipNowIsLargerThanQuantityRemaining);
                        }
                    }
                }
            }
        }
    }
}
