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
                this.CreationDate = this.Session().Now();
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
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                foreach (PickListItem pickListItem in this.PickListItems)
                {
                    iteration.AddDependency(this, pickListItem);
                    iteration.Mark(pickListItem);

                    iteration.AddDependency(this, pickListItem.InventoryItem);
                    iteration.Mark(pickListItem.InventoryItem);
                }

                if (this.ExistShipToParty)
                {
                    foreach (var customerShipment in this.ShipToParty.ShipmentsWhereShipToParty
                        .OfType<CustomerShipment>()
                        .Where(shipment =>
                            shipment.ShipmentState.Equals(new ShipmentStates(this.ShipToParty.Strategy.Session).Created)
                            || shipment.ShipmentState.Equals(new ShipmentStates(this.ShipToParty.Strategy.Session).Picking)
                            || shipment.ShipmentState.Equals(new ShipmentStates(this.ShipToParty.Strategy.Session).Picked)
                            || shipment.ShipmentState.Equals(new ShipmentStates(this.ShipToParty.Strategy.Session).OnHold)
                            || shipment.ShipmentState.Equals(new ShipmentStates(this.ShipToParty.Strategy.Session).Packed)
                        ))
                    {
                        if (!changeSet.IsCreated(customerShipment))
                        {
                            iteration.AddDependency(customerShipment, this);
                            iteration.Mark(customerShipment);
                        }
                    }
                }
            }
        }

        public void BaseDelete(PickListDelete method)
        {
            foreach (PickListItem pickListItem in this.PickListItems)
            {
                pickListItem.Delete();
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
                        var package = shipment?.ShipmentPackages.FirstOrDefault();

                        if (this.Store.AutoGenerateShipmentPackage
                            && package != null
                            && package.PackagingContents.FirstOrDefault(v => v.ShipmentItem.Equals(itemIssuance.ShipmentItem)) == null)
                        {
                            package.AddPackagingContent(
                                new PackagingContentBuilder(this.Strategy.Session)
                                    .WithShipmentItem(itemIssuance.ShipmentItem)
                                    .WithQuantity(itemIssuance.Quantity)
                                    .Build());
                        }
                    }
                }
            }
        }

        public void BaseCancel(PickListCancel method) => this.PickListState = new PickListStates(this.Strategy.Session).Cancelled;

        public void BaseHold(PickListHold method) => this.PickListState = new PickListStates(this.Strategy.Session).OnHold;

        public void BaseContinue(PickListContinue method) => this.PickListState = this.PreviousPickListState;

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
