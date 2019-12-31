// <copyright file="NonUnifiedPart.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Text;

namespace Allors.Domain
{
    using System.Linq;

    using Allors.Meta;

    public partial class NonUnifiedPart
    {
        public void BaseOnBuild(ObjectOnBuild method)
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

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.HasChangedRoles(this, this.Meta.SearchString))
            {
                if (this.ExistInventoryItemsWherePart)
                {
                    foreach (InventoryItem inventoryItem in this.InventoryItemsWherePart)
                    {
                        derivation.AddDependency(inventoryItem, this);
                    }
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
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
                if (supplierOffering.FromDate <= this.strategy.Session.Now()
                    && (!supplierOffering.ExistThroughDate || supplierOffering.ThroughDate >= this.strategy.Session.Now()))
                {
                    this.AddSuppliedBy(supplierOffering.Supplier);
                }

                if (supplierOffering.FromDate > this.strategy.Session.Now()
                    || (supplierOffering.ExistThroughDate && supplierOffering.ThroughDate < this.strategy.Session.Now()))
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

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var builder = new StringBuilder();
            if (this.ExistProductIdentifications)
            {
                builder.Append(string.Join(" ", this.ProductIdentifications.Select(v => v.Identification)));
            }

            if (this.ExistProductCategoriesWhereAllPart)
            {
                builder.Append(string.Join(" ", this.ProductCategoriesWhereAllPart.Select(v => v.Name)));
            }

            if (this.ExistSupplierOfferingsWherePart)
            {
                builder.Append(string.Join(" ", this.SupplierOfferingsWherePart.Select(v => v.Supplier.PartyName)));
                builder.Append(string.Join(" ", this.SupplierOfferingsWherePart.Select(v => v.SupplierProductId)));
                builder.Append(string.Join(" ", this.SupplierOfferingsWherePart.Select(v => v.SupplierProductName)));
            }

            if (this.ExistSerialisedItems)
            {
                builder.Append(string.Join(" ", this.SerialisedItems.Select(v => v.SerialNumber)));
            }

            if (this.ExistProductType)
            {
                builder.Append(string.Join(" ", this.ProductType.Name));
            }

            if (this.ExistBrand)
            {
                builder.Append(string.Join(" ", this.Brand.Name));
            }

            if (this.ExistModel)
            {
                builder.Append(string.Join(" ", this.Model.Name));
            }

            builder.Append(string.Join(" ", this.Keywords));

            this.SearchString = builder.ToString();
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
            if (this.InventoryItemKind.NonSerialised)
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

            foreach (var characteristic in characteristicsToDelete)
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
