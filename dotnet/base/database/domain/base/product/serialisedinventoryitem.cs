// <copyright file="SerialisedInventoryItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class SerialisedInventoryItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.SerialisedInventoryItem, M.SerialisedInventoryItem.SerialisedInventoryItemState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public InventoryStrategy InventoryStrategy
            => this.Strategy.Session.GetSingleton().Settings.InventoryStrategy;

        public int QuantityOnHand
            => this.InventoryStrategy.OnHandSerialisedStates.Contains(this.SerialisedInventoryItemState) ? this.Quantity : 0;

        public int AvailableToPromise
            => this.InventoryStrategy.AvailableToPromiseSerialisedStates.Contains(this.SerialisedInventoryItemState) ? this.Quantity : 0;

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSerialisedInventoryItemState)
            {
                this.SerialisedInventoryItemState = new SerialisedInventoryItemStates(this.Strategy.Session).Good;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRole(this, this.Meta.Quantity))
            {
                iteration.AddDependency(this.SerialisedItem, this);
                iteration.Mark(this.SerialisedItem);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistName)
            {
                this.Name = $"{this.Part?.Name} at {this.Facility?.Name} with state {this.SerialisedInventoryItemState?.Name}";
            }

            this.BaseOnDeriveQuantity(derivation);

            if (this.Quantity < 0 || this.Quantity > 1)
            {
                var message = "Invalid transaction";
                derivation.Validation.AddError(this, this.Meta.Quantity, message);
            }

            // TODO: Remove OnDerive
            this.Part.OnDerive(x => x.WithDerivation(derivation));
        }

        public void BaseOnDeriveQuantity(IDerivation derivation)
        {
            this.Quantity = 0;

            foreach (InventoryItemTransaction inventoryTransaction in this.InventoryItemTransactionsWhereInventoryItem)
            {
                var reason = inventoryTransaction.Reason;

                if (reason.IncreasesQuantityOnHand == true)
                {
                    this.Quantity += (int)inventoryTransaction.Quantity;
                }
                else if (reason.IncreasesQuantityOnHand == false)
                {
                    this.Quantity -= (int)inventoryTransaction.Quantity;
                }
            }

            foreach (PickListItem pickListItem in this.PickListItemsWhereInventoryItem)
            {
                if (pickListItem.PickListWherePickListItem.PickListState.Equals(new PickListStates(this.Strategy.Session).Picked))
                {
                    foreach (ItemIssuance itemIssuance in pickListItem.ItemIssuancesWherePickListItem)
                    {
                        if (!itemIssuance.ShipmentItem.ShipmentItemState.Shipped)
                        {
                            this.Quantity -= (int)pickListItem.QuantityPicked;
                        }
                    }
                }
            }

            foreach (ShipmentReceipt shipmentReceipt in this.ShipmentReceiptsWhereInventoryItem)
            {
                // serialised items are handled via InventoryItemTransactions
                if (shipmentReceipt.ExistShipmentItem && !shipmentReceipt.ShipmentItem.Part.InventoryItemKind.IsSerialised)
                {
                    var purchaseShipment = (PurchaseShipment)shipmentReceipt.ShipmentItem.ShipmentWhereShipmentItem;
                    if (purchaseShipment.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Received))
                    {
                        this.Quantity += (int)shipmentReceipt.QuantityAccepted;
                    }
                }
            }
        }

        public void BaseDelete(DeletableDelete method)
        {
            foreach (InventoryItemVersion version in this.AllVersions)
            {
                version.Delete();
            }
        }
    }
}
