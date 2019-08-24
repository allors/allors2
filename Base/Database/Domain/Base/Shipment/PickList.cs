// <copyright file="PickList.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

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

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCreationDate)
            {
                this.CreationDate = this.strategy.Session.Now();
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

        public void BaseOnPreDerive(ObjectOnPreDerive method)
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
                        shipment.ShipmentState.Equals(new ShipmentStates(this.ShipToParty.Strategy.Session).Created)
                        || shipment.ShipmentState.Equals(new ShipmentStates(this.ShipToParty.Strategy.Session).Picked)
                        || shipment.ShipmentState.Equals(new ShipmentStates(this.ShipToParty.Strategy.Session).OnHold)
                        || shipment.ShipmentState.Equals(new ShipmentStates(this.ShipToParty.Strategy.Session).Packed)
                    ))
                {
                    if (!derivation.IsCreated(customerShipment))
                    {
                        derivation.AddDependency(customerShipment, this);
                    }
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
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

        public void BaseCancel(PickListCancel method) => this.PickListState = new PickListStates(this.Strategy.Session).Cancelled;

        public void BaseHold(PickListHold method) => this.PickListState = new PickListStates(this.Strategy.Session).OnHold;

        public void BaseContinue(PickListContinue method) => this.PickListState = new PickListStates(this.Strategy.Session).Created;

        public void BaseSetPicked(PickListSetPicked method)
        {
            foreach (PickListItem pickListItem in this.PickListItems)
            {
                if (pickListItem.QuantityPicked == 0)
                {
                    pickListItem.QuantityPicked = pickListItem.Quantity;
                }
            }

            this.PickListState = new PickListStates(this.Strategy.Session).Picked;
        }
    }
}
