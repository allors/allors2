// <copyright file="ShipmentReceipt.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    using Allors.Meta;

    public partial class ShipmentReceipt
    {
        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistReceivedDateTime)
            {
                this.ReceivedDateTime = this.Session().Now();
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.ReceivedDateTime = this.ReceivedDateTime.Date;

            if (this.ExistOrderItem && this.ExistShipmentItem)
            {
                var orderShipmentsWhereShipmentItem = this.ShipmentItem.OrderShipmentsWhereShipmentItem;
                orderShipmentsWhereShipmentItem.Filter.AddEquals(M.OrderShipment.OrderItem, this.OrderItem);

                if (orderShipmentsWhereShipmentItem.First == null)
                {
                    new OrderShipmentBuilder(this.Strategy.Session)
                        .WithOrderItem(this.OrderItem)
                        .WithShipmentItem(this.ShipmentItem)
                        .WithQuantity(this.QuantityAccepted)
                        .Build();
                }
                else
                {
                    orderShipmentsWhereShipmentItem.First.Quantity += this.QuantityAccepted;
                }
            }

            this.BaseOnDeriveInventoryItem(derivation);
        }

        public void BaseOnDeriveInventoryItem(IDerivation derivation)
        {
            if (this.ExistShipmentItem && this.ExistFacility)
            { 
                if (this.ShipmentItem.ExistSerialisedItem)
                {
                    var inventoryItems = this.ShipmentItem.SerialisedItem.SerialisedInventoryItemsWhereSerialisedItem;
                    inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, this.Facility);
                    inventoryItems.Filter.AddEquals(M.SerialisedInventoryItem.SerialisedInventoryItemState, new SerialisedInventoryItemStates(this.Session()).Good);
                    this.InventoryItem = inventoryItems.First;

                    if (!this.ExistInventoryItem)
                    {
                        this.InventoryItem = new SerialisedInventoryItemBuilder(this.Strategy.Session)
                                            .WithPart(this.ShipmentItem.Part)
                                            .WithSerialisedItem(this.ShipmentItem.SerialisedItem)
                                            .WithFacility(this.Facility)
                                            .WithUnitOfMeasure(this.ShipmentItem.Part.UnitOfMeasure)
                                            .Build();
                    }
                }
                else
                {
                    var inventoryItems = this.ShipmentItem.Part.InventoryItemsWherePart;
                    inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, this.Facility);
                    //inventoryItems.Filter.AddEquals(M.NonSerialisedInventoryItem.NonSerialisedInventoryItemState, new NonSerialisedInventoryItemStates(this.Session()).Good);
                    this.InventoryItem = inventoryItems.First;

                    if (!this.ExistInventoryItem)
                    {
                        this.InventoryItem = new NonSerialisedInventoryItemBuilder(this.Strategy.Session)
                                            .WithPart(this.ShipmentItem.Part)
                                            .WithFacility(this.Facility)
                                            .WithUnitOfMeasure(this.ShipmentItem.Part.UnitOfMeasure)
                                            .Build();
                    }
                }
            }
        }
    }
}
