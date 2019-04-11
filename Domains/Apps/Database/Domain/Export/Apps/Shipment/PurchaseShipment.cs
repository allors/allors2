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
            if (!this.ExistReceiver)
            {
                var internalOrganisations = new Organisations(this.Strategy.Session).InternalOrganisations();
                if (internalOrganisations.Length == 1)
                {
                    this.Receiver = internalOrganisations.First();
                }
            }

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

                    foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        derivation.AddDependency(orderShipment.OrderItem, this);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExists(this, this.Meta.ShipFromParty);

            this.ShipToAddress = this.ShipToAddress ?? this.Receiver?.ShippingAddress ?? this.Receiver?.GeneralCorrespondence as PostalAddress;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistReceiver && internalOrganisations.Count() == 1)
            {
                this.Receiver = internalOrganisations.First();
            }

            if (!this.ExistFacility && this.ExistReceiver && this.Receiver?.StoresWhereInternalOrganisation.Count == 1)
            {
                this.Facility = this.Receiver.StoresWhereInternalOrganisation.Single().DefaultFacility;
            }

            if (!this.ExistShipmentNumber && this.ExistReceiver)
            {
                this.ShipmentNumber = this.Receiver.NextShipmentNumber();
            }

            if (!this.ExistShipFromAddress && this.ExistShipFromParty)
            {
                this.ShipFromAddress = this.ShipFromParty.ShippingAddress;
            }

            if (this.ShipmentItems.Any() && this.ShipmentItems.All(v => v.ShipmentReceiptWhereShipmentItem.QuantityAccepted.Equals(v.ShipmentReceiptWhereShipmentItem.OrderItem?.QuantityOrdered)))
            {
                this.PurchaseShipmentState = new PurchaseShipmentStates(this.Strategy.Session).Delivered;
            }
        }
    }
}