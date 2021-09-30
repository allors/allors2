// <copyright file="ShipmentItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using Allors.Meta;
using Resources;

namespace Allors.Domain
{
    public partial class ShipmentItem
    {
        #region Transitional

        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
        {
            new TransitionalConfiguration(M.ShipmentItem, M.ShipmentItem.ShipmentItemState),
        };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        #endregion Transitional

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (method.SecurityTokens == null)
            {
                method.SecurityTokens = this.SyncedShipment?.SecurityTokens.ToArray();
            }

            if (method.Restrictions == null)
            {
                method.Restrictions = this.SyncedShipment?.Restrictions.ToArray();
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
                this.ShipmentItemState = new ShipmentItemStates(this.Strategy.Session).Created;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                iteration.AddDependency(this.ShipmentWhereShipmentItem, this);
                iteration.Mark(this.ShipmentWhereShipmentItem, this);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistDerivationTrigger)
            {
                this.DerivationTrigger = Guid.NewGuid();
            }

            if ((this.ShipmentWhereShipmentItem.GetType().Name.Equals(typeof(CustomerShipment).Name) || this.ShipmentWhereShipmentItem.GetType().Name.Equals(typeof(PurchaseReturn).Name))
                && this.ExistSerialisedItem
                && !this.ExistNextSerialisedItemAvailability)
            {
                derivation.Validation.AssertExists(this, this.Meta.NextSerialisedItemAvailability);
            }

            if (this.ExistSerialisedItem && this.Quantity != 1)
            {
                derivation.Validation.AddError(this, this.Meta.Quantity, ErrorMessages.SerializedItemQuantity);
            }

            this.BaseOnDeriveCustomerShipmentItem(derivation);

            this.BaseOnDerivePurchaseShipmentItem(derivation);
        }

        public void BaseOnDerivePurchaseShipmentItem(IDerivation derivation)
        {
            if (this.ExistShipmentWhereShipmentItem
                && this.ShipmentWhereShipmentItem is PurchaseShipment
                && this.ExistPart
                && this.Part.InventoryItemKind.IsNonSerialised
                && !this.ExistUnitPurchasePrice)
            {
                derivation.Validation.AssertExists(this, this.Meta.UnitPurchasePrice);
            }

            if (this.ExistShipmentWhereShipmentItem
                && this.ShipmentWhereShipmentItem is PurchaseShipment
                && !this.ExistStoredInFacility
                && this.ShipmentWhereShipmentItem.ExistShipToFacility)
            {
                this.StoredInFacility = this.ShipmentWhereShipmentItem.ShipToFacility;
            }

            if (this.ExistShipmentWhereShipmentItem
                && this.ShipmentWhereShipmentItem is PurchaseShipment
                && this.ExistShipmentReceiptWhereShipmentItem)
            {
                this.Quantity = 0;
                var shipmentReceipt = this.ShipmentReceiptWhereShipmentItem;
                this.Quantity += shipmentReceipt.QuantityAccepted + shipmentReceipt.QuantityRejected;
            }
        }

        public void BaseOnDeriveCustomerShipmentItem(IDerivation derivation)
        {
            if (this.ShipmentWhereShipmentItem is CustomerShipment)
            {
                this.QuantityPicked = 0;
                foreach (ItemIssuance itemIssuance in this.ItemIssuancesWhereShipmentItem)
                {
                    if (itemIssuance.PickListItem.PickListWherePickListItem.PickListState.Equals(new PickListStates(this.Strategy.Session).Picked))
                    {
                        this.QuantityPicked += itemIssuance.Quantity;
                    }
                }

                if (Equals(this.ShipmentWhereShipmentItem.ShipmentState, new ShipmentStates(this.Strategy.Session).Shipped))
                {
                    this.QuantityShipped = 0;
                    foreach (ItemIssuance itemIssuance in this.ItemIssuancesWhereShipmentItem)
                    {
                        this.QuantityShipped += itemIssuance.Quantity;
                    }
                }
            }
        }

        public void Sync(Shipment shipment) => this.SyncedShipment = shipment;
    }
}
