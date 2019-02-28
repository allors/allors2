// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonUnifiedPart.v.cs" company="Allors bvba">
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

using Allors.Meta;

namespace Allors.Domain
{
    using System;

    public partial class NonUnifiedPart
    {
        public new string ToString() => this.Name ?? this.Id.ToString();

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistInventoryItemKind)
            {
                this.InventoryItemKind = new InventoryItemKinds(this.Strategy.Session).NonSerialised;
            }

            if (!this.ExistUnitOfMeasure)
            {
                this.UnitOfMeasure = new UnitsOfMeasure(this.Strategy.Session).Piece;
            }

            if (!this.ExistDefaultFacility)
            {
                this.DefaultFacility = this.Strategy.Session.GetSingleton().Settings.DefaultFacility;
            }

            this.DeriveName();
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

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var setings = this.Strategy.Session.GetSingleton().Settings;

            if (derivation.HasChangedRoles(this, new RoleType[] { this.Meta.UnitOfMeasure, this.Meta.DefaultFacility }))
            {
                this.SyncDefaultInventoryItem();
            }

            this.DeriveName();

            var identifications = this.ProductIdentifications;
            identifications.Filter.AddEquals(M.ProductIdentification.ProductIdentificationType, new ProductIdentificationTypes(this.Strategy.Session).Part);
            var partNumber = identifications.FirstOrDefault();

            if (partNumber == null && setings.UsePartNumberCounter)
            {
                this.AddProductIdentification(new PartNumberBuilder(this.Strategy.Session)
                    .WithIdentification(setings.NextPartNumber())
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Strategy.Session).Part).Build());
            }

            foreach (SupplierOffering supplierOffering in this.SupplierOfferingsWherePart)
            {
                if (supplierOffering.FromDate <= DateTime.UtcNow
                    && (!supplierOffering.ExistThroughDate || supplierOffering.ThroughDate >= DateTime.UtcNow))
                {
                    this.AddSuppliedBy(supplierOffering.Supplier);
                }

                if (supplierOffering.FromDate > DateTime.UtcNow
                    || (supplierOffering.ExistThroughDate && supplierOffering.ThroughDate < DateTime.UtcNow))
                {
                    this.RemoveSuppliedBy(supplierOffering.Supplier);
                }
            }

            this.DeriveProductCharacteristics(derivation);
            this.DeriveQuantityOnHand();
            this.DeriveAvailableToPromise();
            this.DeriveQuantityCommittedOut();
            this.DeriveQuantityExpectedIn();
        }

        private void DeriveName()
        {
            if (!this.ExistName)
            {
                this.Name = "Part " + (this.PartIdentification() ?? this.UniqueId.ToString());
            }
        }

        private void SyncDefaultInventoryItem()
        {
            if (this.InventoryItemKind.IsNonSerialized)
            {
                var inventoryItems = this.InventoryItemsWherePart;

                if (!inventoryItems.Any(i => i.Facility.Equals(this.DefaultFacility) && i.UnitOfMeasure.Equals(this.UnitOfMeasure)))
                {
                    var inventoryItem = (InventoryItem)new NonSerialisedInventoryItemBuilder(this.Strategy.Session)
                      .WithFacility(this.DefaultFacility)
                      .WithUnitOfMeasure(this.UnitOfMeasure)
                      .WithPart(this)
                      .Build();
                }
            }
        }

        private void DeriveProductCharacteristics(IDerivation derivation)
        {
            var characteristicsToDelete = this.SerialisedItemCharacteristics.ToList();

            if (this.ExistProductType)
            {
                foreach (SerialisedItemCharacteristicType characteristicType in this.ProductType.SerialisedItemCharacteristicTypes)
                {
                    var characteristic = this.SerialisedItemCharacteristics.FirstOrDefault(v => Equals(v.SerialisedItemCharacteristicType, characteristicType));
                    if (characteristic == null)
                    {
                        this.AddSerialisedItemCharacteristic(
                            new SerialisedItemCharacteristicBuilder(this.Strategy.Session)
                                .WithSerialisedItemCharacteristicType(characteristicType)
                                .Build());
                    }
                    else
                    {
                        characteristicsToDelete.Remove(characteristic);
                    }
                }
            }

            foreach (SerialisedItemCharacteristic characteristic in characteristicsToDelete)
            {
                this.RemoveSerialisedItemCharacteristic(characteristic);
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