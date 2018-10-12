// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartExtensions.v.cs" company="Allors bvba">
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
    public  partial class Part
    {
        public InventoryStrategy GetInventoryStrategy
            => this.InventoryStrategy ?? (this.InternalOrganisation?.InventoryStrategy ?? new InventoryStrategies(this.strategy.Session).Standard);

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistInventoryItemKind)
            {
                this.InventoryItemKind = new InventoryItemKinds(this.Strategy.Session).NonSerialised;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistInventoryItemsWherePart)
                {
                    foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
                    {
                        derivation.AddDependency(this, inventoryItem);
                    }
                }
            }
        }

        public  void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistInternalOrganisation && internalOrganisations.Count() == 1)
            {
                this.InternalOrganisation = internalOrganisations.First();
            }

            this.DeriveInventoryItem(derivation);
            this.DeriveQuantityOnHand();
            this.DeriveAvailableToPromise();
            this.DeriveQuantityCommittedOut();
            this.DeriveQuantityExpectedIn();
        }

        private void DeriveInventoryItem(IDerivation derivation)
        {
            if (this.ExistInternalOrganisation && this.ExistInventoryItemKind && this.InventoryItemKind.IsNonSerialized)
            //if (this.ExistInternalOrganisation && !this.ExistInventoryItemsWherePart && this.ExistInventoryItemKind)
            {
                var inventoryItems = this.InventoryItemsWherePart;

                foreach (Facility facility in this.InternalOrganisation.FacilitiesWhereOwner)
                {
                    if (!inventoryItems.Any(i => i.ExistFacility && i.Facility.Equals(facility)))
                    {
                        new NonSerialisedInventoryItemBuilder(this.Strategy.Session)
                          .WithPart(this)
                          .WithFacility(facility)
                          .Build();
                    }
                }
            }
        }

        private void DeriveQuantityOnHand()
        {
            this.QuantityOnHand = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
            {
                if (inventoryItem is NonSerialisedInventoryItem nonSerialisedItem)
                {
                    this.QuantityOnHand += nonSerialisedItem.QuantityOnHand;
                }
                else if (inventoryItem is SerialisedInventoryItem serialisedItem)
                {
                    this.QuantityOnHand += serialisedItem.QuantityOnHand;
                }
            }
        }

        private void DeriveAvailableToPromise()
        {
            this.AvailableToPromise = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
            {
                if (inventoryItem is NonSerialisedInventoryItem nonSerialisedItem)
                {
                    this.AvailableToPromise += nonSerialisedItem.AvailableToPromise;
                }
                else if (inventoryItem is SerialisedInventoryItem serialisedItem)
                {
                    this.AvailableToPromise += serialisedItem.AvailableToPromise;
                }
            }
        }

        private void DeriveQuantityCommittedOut()
        {
            this.QuantityCommittedOut = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
            {
                if (inventoryItem is NonSerialisedInventoryItem nonSerialised)
                {
                    this.QuantityCommittedOut += nonSerialised.QuantityCommittedOut;
                }
            }
        }

        private void DeriveQuantityExpectedIn()
        {
            this.QuantityExpectedIn = 0;

            foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
            {
                if (inventoryItem is NonSerialisedInventoryItem nonSerialised)
                {
                    this.QuantityExpectedIn += nonSerialised.QuantityExpectedIn;
                }
            }
        }
    }
}