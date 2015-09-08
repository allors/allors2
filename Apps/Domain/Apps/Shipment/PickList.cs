// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickList.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class PickList
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

        public bool IsNegativePickList
        {
            get
            {
                //// Negative PickList only has 1 item.
                return this.ExistPickListItems && this.PickListItems.First.RequestedQuantity < 0;
            }
        }

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

            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new PickListObjectStates(this.Strategy.Session).Created;
            }

            if (!this.ExistStore)
            {
                var internalOrganisation = Allors.Domain.Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
                if (internalOrganisation.StoresWhereOwner.Count == 1)
                {
                    this.Store = internalOrganisation.StoresWhereOwner.First;
                }
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

                    var inventoryItem = pickListItem.InventoryItem as Allors.Domain.NonSerializedInventoryItem;
                    if (inventoryItem != null)
                    {
                        derivation.AddDependency(this, inventoryItem);
                    }
                }

                if (this.ExistShipToParty)
                {
                    foreach (var customerShipment in this.ShipToParty.PendingCustomerShipments)
                    {
                        derivation.AddDependency(customerShipment, this);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.DeriveCurrentObjectState();

            if (this.ExistPickListItems)
            {
                this.AppsOnDeriveTemplate(derivation);
            }

            this.AppsOnDeriveTemplate(derivation);
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            this.RemoveSecurityTokens();
            this.AddSecurityToken(Allors.Domain.Singleton.Instance(this.Strategy.Session).AdministratorSecurityToken);

            if (this.ExistShipToParty)
            {
                this.AddSecurityToken(this.ShipToParty.OwnerSecurityToken);
            }

            if (this.ExistStore && this.Store.ExistOwner)
            {
                if (this.Store.Owner.ExistOwnerSecurityToken && !this.SecurityTokens.Contains(this.Store.Owner.OwnerSecurityToken))
                {
                    this.AddSecurityToken(Store.Owner.OwnerSecurityToken);
                }
            }
        }

        private void DeriveCurrentObjectState()
        {
            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.PreviousObjectState))
            {
                var currentStatus = new PickListStatusBuilder(this.Strategy.Session).WithPickListObjectState(this.CurrentObjectState).Build();
                this.AddPickListStatus(currentStatus);
                this.CurrentPickListStatus = currentStatus;
            }

            if (this.ExistCurrentObjectState)
            {
                this.CurrentObjectState.Process(this);
            }
        }

        public void AppsCancel(PickListCancel method)
        {
            this.CurrentObjectState = new PickListObjectStates(this.Strategy.Session).Cancelled;
        }

        public void AppsHold(PickListHold method)
        {
            this.CurrentObjectState = new PickListObjectStates(this.Strategy.Session).OnHold;
        }

        public void AppsContinue(PickListContinue method)
        {
            this.CurrentObjectState = new PickListObjectStates(this.Strategy.Session).Created;
        }

        public void AppsSetPicked(PickListSetPicked method)
        {
            this.CurrentObjectState = new PickListObjectStates(this.Strategy.Session).Picked;

            foreach (PickListItem pickListItem in this.PickListItems)
            {
                if (!pickListItem.ExistActualQuantity)
                {
                    pickListItem.ActualQuantity = pickListItem.RequestedQuantity;
                }
            }
        }

        public void AppsOnDeriveTemplate(IDerivation derivation)
        {
            var internalOrganisation = Singleton.Instance(this.strategy.Session).DefaultInternalOrganisation;
            
            if (this.ExistPickListItems)
            {
                internalOrganisation = this.PickListItems[0].InventoryItem.Facility.Owner;
            }

            StringTemplate template = null;

            if (internalOrganisation.ExistLocale)
            {
                var templates = internalOrganisation.PickListTemplates;
                templates.Filter.AddEquals(StringTemplates.Meta.Locale, internalOrganisation.Locale);
                template = templates.First;
            }

            if (template == null)
            {
                var templates = internalOrganisation.PickListTemplates;
                templates.Filter.AddEquals(StringTemplates.Meta.Locale,
                    Singleton.Instance(this.Strategy.Session).DefaultLocale);
                template = templates.First;
            }

            if (template != null)
            {
                this.PrintContent = template.Apply(new Dictionary<string, object> { { "this", this } });
            }
        }
    }
}