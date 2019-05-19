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
                new TransitionalConfiguration(M.PurchaseShipment, M.PurchaseShipment.ShipmentState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistShipFromParty)
            {
                var internalOrganisations = new Organisations(this.Strategy.Session).InternalOrganisations();
                if (internalOrganisations.Length == 1)
                {
                    this.ShipFromParty = internalOrganisations.First();
                }
            }

            if (!this.ExistShipmentState)
            {
                this.ShipmentState = new ShipmentStates(this.Strategy.Session).Created;
            }

            if (!this.ExistEstimatedArrivalDate)
            {
                this.EstimatedArrivalDate = this.strategy.Session.Now().Date;
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

            this.ShipToAddress = this.ShipToAddress ?? this.ShipFromParty?.ShippingAddress ?? this.ShipFromParty?.GeneralCorrespondence as PostalAddress;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            var shipToParty = this.ShipToParty as InternalOrganisation;
            if (!this.ExistShipToParty && internalOrganisations.Count() == 1)
            {
                this.ShipToParty = internalOrganisations.First();
                shipToParty = internalOrganisations.First();
            }

            if (!this.ExistShipToFacility && shipToParty != null)
            {
                this.ShipToFacility = shipToParty.StoresWhereInternalOrganisation.Single().DefaultFacility;
            }

            if (!this.ExistShipmentNumber && shipToParty != null)
            {
                this.ShipmentNumber = shipToParty.NextShipmentNumber(this.Strategy.Session.Now().Year);
            }

            if (!this.ExistShipFromAddress && shipToParty != null)
            {
                this.ShipFromAddress = shipToParty.ShippingAddress;
            }

            if (this.ShipmentItems.Any() && this.ShipmentItems.All(v => v.ShipmentReceiptWhereShipmentItem.QuantityAccepted.Equals(v.ShipmentReceiptWhereShipmentItem.OrderItem?.QuantityOrdered)))
            {
                this.ShipmentState = new ShipmentStates(this.Strategy.Session).Delivered;
            }
        }
    }
}