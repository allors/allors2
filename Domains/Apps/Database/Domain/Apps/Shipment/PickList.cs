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

        public bool IsNegativePickList => this.ExistPickListItems && this.PickListItems.First.RequestedQuantity < 0;

        public bool IsComplete
        {
            get
            {
                foreach (PickListItem pickListItem in this.PickListItems)
                {
                    if (!pickListItem.ExistActualQuantity || pickListItem.RequestedQuantity != pickListItem.ActualQuantity)
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
                this.Store = this.strategy.Session.Extent<Store>().First;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                foreach (PickListItem pickListItem in this.PickListItems)
                {
                    derivation.AddDependency(this, pickListItem);

                    var inventoryItem = pickListItem.InventoryItem as NonSerialisedInventoryItem;
                    if (inventoryItem != null)
                    {
                        derivation.AddDependency(this, inventoryItem);
                    }
                }

                if (this.ExistShipToParty)
                {
                    foreach (var customerShipment in this.ShipToParty.AppsGetPendingCustomerShipments())
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

                        shipment?.ShipmentPackages[0].AddPackagingContent(
                            new PackagingContentBuilder(this.strategy.Session)
                                .WithShipmentItem(itemIssuance.ShipmentItem).WithQuantity(itemIssuance.Quantity)
                                .Build());
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
                if (!pickListItem.ExistActualQuantity)
                {
                    pickListItem.ActualQuantity = pickListItem.RequestedQuantity;
                }
            }
        }
    }
}