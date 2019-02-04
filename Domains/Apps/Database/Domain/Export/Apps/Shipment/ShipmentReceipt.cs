// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentReceipt.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;
    using System.Linq;

    using Meta;

    public partial class ShipmentReceipt
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistReceivedDateTime)
            {
                this.ReceivedDateTime = DateTime.UtcNow;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
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

            this.AppsOnDeriveInventoryItem(derivation);
        }

        public void AppsOnDeriveInventoryItem(IDerivation derivation)
        {
            if (this.ExistShipmentItem && this.ShipmentItem.ExistOrderShipmentsWhereShipmentItem)
            {
                var purchaseShipment = (PurchaseShipment)this.ShipmentItem.ShipmentWhereShipmentItem;
                var internalOrganisation = purchaseShipment.Receiver;
                var purchaseOrderItem = this.ShipmentItem.OrderShipmentsWhereShipmentItem[0].OrderItem as PurchaseOrderItem;

                var defaultFacility = internalOrganisation?.StoresWhereInternalOrganisation.Count == 1 ? internalOrganisation.StoresWhereInternalOrganisation.Single().DefaultFacility : null;

                if (purchaseOrderItem != null && purchaseOrderItem.ExistPart)
                {
                    if (!this.ExistInventoryItem || !this.InventoryItem.Part.Equals(purchaseOrderItem.Part))
                    {
                        var inventoryItems = purchaseOrderItem.Part.InventoryItemsWherePart;
                        inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, defaultFacility);
                        this.InventoryItem = inventoryItems.First as NonSerialisedInventoryItem;
                    }
                }
            }
        }
    }
}