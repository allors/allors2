// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseShipment.cs" company="Allors bvba">
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
    using System.Collections.Generic;

    public partial class PurchaseShipment
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new PurchaseShipmentObjectStates(this.Strategy.Session).Created;
            }

            if (!this.ExistShipToParty)
            {
                this.ShipToParty = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }

            if (!this.ExistFacility && this.ExistShipToParty)
            {
                var toParty = this.ShipToParty as InternalOrganisation;
                if (toParty != null)
                {
                    this.Facility = toParty.DefaultFacility;
                }
            }

            if (!this.ExistShipmentNumber && this.ExistShipToParty)
            {
                var internalOrganisation = this.ShipToParty as InternalOrganisation;

                if (internalOrganisation != null)
                {
                    this.ShipmentNumber = internalOrganisation.DeriveNextShipmentNumber();
                }
            }

            if (!this.ExistEstimatedArrivalDate)
            {
                this.EstimatedArrivalDate = DateTime.UtcNow.Date;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                foreach (ShipmentItem shipmentItem in this.ShipmentItems)
                {
                    if (shipmentItem.ShipmentReceiptWhereShipmentItem.ExistInventoryItem)
                    {
                        derivation.AddDependency(shipmentItem.ShipmentReceiptWhereShipmentItem.InventoryItem, this);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistShipToAddress && this.ExistShipToParty)
            {
                this.ShipToAddress = this.ShipToParty.ShippingAddress;
            }

            if (!this.ExistShipFromAddress && this.ExistShipFromParty)
            {
                this.ShipFromAddress = this.ShipFromParty.ShippingAddress;
            }

            if (this.ExistCurrentObjectState && 
                this.CurrentObjectState.UniqueId.Equals(new PurchaseShipmentObjectStates(this.strategy.Session).Completed) &&
                !this.CurrentObjectState.Equals(this.LastObjectState))
            {

                this.AppsOnDeriveOrderItemQuantityReceived(derivation);
            }

            this.AppsOnDeriveCurrentObjectState(derivation);

            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                shipmentItem.OnDerive(x => x.WithDerivation(derivation));
            }
        }

        public void AppsOnDeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.LastObjectState))
            {
                var currentStatus = new PurchaseShipmentStatusBuilder(this.Strategy.Session).WithPurchaseShipmentObjectState(this.CurrentObjectState).Build();
                this.AddShipmentStatus(currentStatus);
                this.CurrentShipmentStatus = currentStatus;
            }
        }

        public void AppsComplete()
        {
            this.CurrentObjectState = new PurchaseShipmentObjectStates(this.Strategy.Session).Completed;
        }

        public void AppsOnDeriveOrderItemQuantityReceived(IDerivation derivation)
        {
            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                var receipt = shipmentItem.ShipmentReceiptWhereShipmentItem;
                var orderItem = (PurchaseOrderItem)receipt.OrderItem;

                if (orderItem != null)
                {
                    orderItem.AppsOnDeriveCurrentShipmentStatus(derivation);
                }
            }
        }
    }
}