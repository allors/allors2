// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickList.cs" company="Allors bvba">
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

    public partial class PickList
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PickList, M.PickList.PickListState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public bool IsNegativePickList => this.ExistPickListItems && this.PickListItems.First.Quantity < 0;

        public bool IsComplete
        {
            get
            {
                foreach (PickListItem pickListItem in this.PickListItems)
                {
                    if (pickListItem.Quantity != pickListItem.QuantityPicked)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCreationDate)
            {
                this.CreationDate = DateTime.UtcNow;
            }

            if (!this.ExistPickListState)
            {
                this.PickListState = new PickListStates(this.Strategy.Session).Created;
            }

            if (!this.ExistStore)
            {
                this.Store = this.Strategy.Session.Extent<Store>().First;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (PickListItem pickListItem in this.PickListItems)
            {
                derivation.AddDependency(this, pickListItem);

                derivation.AddDependency(this, pickListItem.InventoryItem);
            }

            if (this.ExistShipToParty)
            {
                foreach (var customerShipment in this.ShipToParty.ShipmentsWhereShipToParty
                    .OfType<CustomerShipment>()
                    .Where(shipment =>
                        shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(this.ShipToParty.Strategy.Session).Created)
                        || shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(this.ShipToParty.Strategy.Session).Picked)
                        || shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(this.ShipToParty.Strategy.Session).OnHold)
                        || shipment.CustomerShipmentState.Equals(new CustomerShipmentStates(this.ShipToParty.Strategy.Session).Packed)
                    ))
                {
                    if (!derivation.IsCreated(customerShipment))
                    {
                        derivation.AddDependency(customerShipment, this);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.Store.IsImmediatelyPicked)
            {
                this.SetPicked();

                foreach (PickListItem pickListItem in this.PickListItems)
                {
                    foreach (ItemIssuance itemIssuance in pickListItem.ItemIssuancesWherePickListItem)
                    {
                        var shipment = itemIssuance.ShipmentItem.ShipmentWhereShipmentItem as CustomerShipment;

                        if (shipment?.ShipmentPackages[0].PackagingContents.Count == 0)
                        {
                            shipment?.ShipmentPackages[0].AddPackagingContent(
                                new PackagingContentBuilder(this.Strategy.Session)
                                    .WithShipmentItem(itemIssuance.ShipmentItem).WithQuantity(itemIssuance.Quantity)
                                    .Build());
                        }
                    }
                }
            }
        }

        public void AppsCancel(PickListCancel method)
        {
            this.PickListState = new PickListStates(this.Strategy.Session).Cancelled;
        }

        public void AppsHold(PickListHold method)
        {
            this.PickListState = new PickListStates(this.Strategy.Session).OnHold;
        }

        public void AppsContinue(PickListContinue method)
        {
            this.PickListState = new PickListStates(this.Strategy.Session).Created;
        }

        public void AppsSetPicked(PickListSetPicked method)
        {
            this.PickListState = new PickListStates(this.Strategy.Session).Picked;

            foreach (PickListItem pickListItem in this.PickListItems)
            {
                pickListItem.QuantityPicked = pickListItem.Quantity;
            }
        }
    }
}