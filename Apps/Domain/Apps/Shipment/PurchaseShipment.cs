// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseShipment.cs" company="Allors bvba">
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

    public partial class PurchaseShipment
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

        public bool CanEdit
        {
            get
            {
                if (this.CurrentObjectState.Equals(new PurchaseShipmentObjectStates(this.Strategy.Session).Created))
                {
                    return true;
                }

                return false;
            }
        }

        public string DateString
        {
            get { return this.EstimatedArrivalDate.ToShortDateString(); }
        }

        public void DeriveTemplate(IDerivation derivation)
        {
            var internalOrganisation = this.ShipToParty as Domain.InternalOrganisation;
            if (internalOrganisation != null && internalOrganisation.PurchaseShipmentTemplates.Count > 0)
            {
                this.PrintContent =
                    internalOrganisation.PurchaseShipmentTemplates.First.Apply(new Dictionary<string, object> { { "this", this } });
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new PurchaseShipmentObjectStates(this.Strategy.Session).Created;
            }

            if (!this.ExistShipToParty)
            {
                this.ShipToParty = Domain.Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }

            if (!this.ExistFacility && this.ExistShipToParty)
            {
                var toParty = this.ShipToParty as Domain.InternalOrganisation;
                if (toParty != null)
                {
                    this.Facility = toParty.DefaultFacility;
                }
            }

            if (!this.ExistShipmentNumber && this.ExistShipToParty)
            {
                var internalOrganisation = this.ShipToParty as Domain.InternalOrganisation;

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
                this.CurrentObjectState.UniqueId.Equals(PurchaseShipmentObjectStates.CompletedId) &&
                !this.CurrentObjectState.Equals(this.PreviousObjectState))
            {

                this.AppsOnDeriveOrderItemQuantityReceived(derivation);
            }

            this.DeriveCurrentObjectState(derivation);

            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                shipmentItem.OnDerive(x => x.WithDerivation(derivation));
            }

            this.DeriveTemplate(derivation);
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            this.RemoveSecurityTokens();
            this.AddSecurityToken(Domain.Singleton.Instance(this.Strategy.Session).AdministratorSecurityToken);

            if (this.ExistShipToParty)
            {
                this.AddSecurityToken(this.ShipToParty.OwnerSecurityToken);
            }
        }

        public void AppsOnDeriveCurrentObjectState(IDerivation derivation)
        {
            

            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.PreviousObjectState))
            {
                var currentStatus = new PurchaseShipmentStatusBuilder(this.Strategy.Session).WithPurchaseShipmentObjectState(this.CurrentObjectState).Build();
                this.AddShipmentStatus(currentStatus);
                this.CurrentShipmentStatus = currentStatus;
            }

            if (this.ExistCurrentObjectState)
            {
                this.CurrentObjectState.Process(this);
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
                var orderItem = (Allors.Domain.PurchaseOrderItem)receipt.OrderItem;

                if (orderItem != null)
                {
                    orderItem.DeriveCurrentShipmentStatus(derivation);
                    orderItem.PurchaseOrderWherePurchaseOrderItem.DeriveTemplate(derivation);
                }
            }
        }
    }
}