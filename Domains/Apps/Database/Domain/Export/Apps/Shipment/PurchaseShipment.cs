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

using System.Linq;

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;

    using Allors.Meta;

    public partial class PurchaseShipment
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PurchaseShipment, M.PurchaseShipment.PurchaseShipmentState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var internalOrganisation = this.ShipToParty as InternalOrganisation;

            if (!this.ExistPurchaseShipmentState)
            {
                this.PurchaseShipmentState = new PurchaseShipmentStates(this.Strategy.Session).Created;
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

            derivation.Validation.AssertExists(this, this.Meta.ShipFromParty);

            var internalOrganisations = new Organisations(this.strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistReceiver && internalOrganisations.Count() == 1)
            {
                this.Receiver = internalOrganisations.First();
            }

            if (!this.ExistShipToAddress)
            {
                this.ShipToAddress = this.Receiver.ExistShippingAddress ? this.Receiver.ShippingAddress : this.Receiver.GeneralCorrespondence;
            }

            if (!this.ExistFacility && this.ExistReceiver)
            {
                this.Facility = this.Receiver.DefaultFacility;
            }

            if (!this.ExistShipmentNumber && this.ExistReceiver)
            {
                this.ShipmentNumber = this.Receiver.NextShipmentNumber();
            }

            if (!this.ExistShipFromAddress && this.ExistShipFromParty)
            {
                this.ShipFromAddress = this.ShipFromParty.ShippingAddress;
            }

            if (this.ExistPurchaseShipmentState &&
                this.PurchaseShipmentState.Equals(new PurchaseShipmentStates(this.strategy.Session).Completed) &&
                !this.PurchaseShipmentState.Equals(this.LastPurchaseShipmentState))
            {

                this.AppsOnDeriveOrderItemQuantityReceived(derivation);
            }

            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                shipmentItem.OnDerive(x => x.WithDerivation(derivation));
            }
        }

        public void AppsComplete()
        {
            this.PurchaseShipmentState = new PurchaseShipmentStates(this.Strategy.Session).Completed;
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