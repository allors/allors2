// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialisedInventoryItem.cs" company="Allors bvba">
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
    using Meta;

    public partial class SerialisedInventoryItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            { new TransitionalConfiguration(M.SerialisedInventoryItem, M.SerialisedInventoryItem.SerialisedInventoryItemState), };

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
                this.SerialisedInventoryItemState = new SerialisedInventoryItemStates(this.Strategy.Session).Available;
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
                    this.Quantity -= (int)pickListItem.QuantityPicked;
                }
            }

            foreach (ShipmentReceipt shipmentReceipt in this.ShipmentReceiptsWhereInventoryItem)
            {
                // serialised items are handled via InventoryItemTransactions
                if (shipmentReceipt.ExistShipmentItem && !shipmentReceipt.ShipmentItem.Part.InventoryItemKind.Serialised)
                {
                    var purchaseShipment = (PurchaseShipment)shipmentReceipt.ShipmentItem.ShipmentWhereShipmentItem;
                    if (purchaseShipment.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Delivered))
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
