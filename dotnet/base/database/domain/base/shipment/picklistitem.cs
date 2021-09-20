// <copyright file="PickListItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;
    using Resources;

    public partial class PickListItem
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                foreach (ItemIssuance itemIssuance in this.ItemIssuancesWherePickListItem)
                {
                    iteration.AddDependency(itemIssuance, this);
                    iteration.Mark(itemIssuance);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.Quantity > 0 && this.QuantityPicked > this.Quantity)
            {
                derivation.Validation.AddError(this, M.PickListItem.QuantityPicked, ErrorMessages.PickListItemQuantityMoreThanAllowed);
            }

            if (this.QuantityPicked > 0 && this.ExistPickListWherePickListItem && this.PickListWherePickListItem.PickListState.Equals(new PickListStates(this.Session()).Picked))
            {
                var quantityProcessed = this.ItemIssuancesWherePickListItem.SelectMany(v => v.ShipmentItem.OrderShipmentsWhereShipmentItem).Sum(v => v.Quantity);
                var diff = quantityProcessed - this.QuantityPicked;

                foreach (ItemIssuance itemIssuance in this.ItemIssuancesWherePickListItem)
                {
                    itemIssuance.IssuanceDateTime = this.Session().Now();
                    foreach (OrderShipment orderShipment in itemIssuance.ShipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        if (orderShipment.OrderItem is SalesOrderItem salesOrderItem)
                        {
                            if (diff > 0 && this.QuantityPicked != orderShipment.Quantity)
                            {
                                if (orderShipment.Quantity >= diff)
                                {
                                    new OrderShipmentBuilder(this.Strategy.Session)
                                        .WithOrderItem(salesOrderItem)
                                        .WithShipmentItem(orderShipment.ShipmentItem)
                                        .WithQuantity(diff * -1)
                                        .Build();

                                    diff = 0;
                                }
                                else
                                {
                                    new OrderShipmentBuilder(this.Strategy.Session)
                                        .WithOrderItem(salesOrderItem)
                                        .WithShipmentItem(orderShipment.ShipmentItem)
                                        .WithQuantity(orderShipment.Quantity * -1)
                                        .Build();

                                    diff -= orderShipment.Quantity;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void BaseDelete(PickListItemDelete method)
        {
            foreach (ItemIssuance itemIssuance in this.ItemIssuancesWherePickListItem)
            {
                itemIssuance.Delete();
            }
        }
    }
}
