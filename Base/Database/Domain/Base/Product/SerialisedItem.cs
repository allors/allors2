// <copyright file="SerialisedItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using System.Text;

    using Allors.Meta;

    public partial class SerialisedItem
    {
        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSerialisedItemState)
            {
                this.SerialisedItemState = new SerialisedItemStates(this.Strategy.Session).NA;
            }

            if (!this.ExistItemNumber)
            {
                this.ItemNumber = this.Strategy.Session.GetSingleton().Settings.NextSerialisedItemNumber();
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (changeSet.HasChangedRole(this, this.Meta.OwnedBy) && this.ExistSerialisedInventoryItemsWhereSerialisedItem)
            {
                foreach (InventoryItem inventoryItem in this.SerialisedInventoryItemsWhereSerialisedItem)
                {
                    iteration.AddDependency(inventoryItem, this);
                    iteration.Mark(inventoryItem);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExistsAtMostOne(this, this.Meta.AcquiredDate, this.Meta.AcquisitionYear);

            if (!this.ExistName && this.ExistPartWhereSerialisedItem)
            {
                this.Name = this.PartWhereSerialisedItem.Name;
            }

            var purchaseOrderItem = this.PurchaseOrderItemsWhereSerialisedItem.OrderByDescending(v => v.PurchaseOrderWherePurchaseOrderItem.OrderDate).FirstOrDefault();
            this.PurchasePrice = purchaseOrderItem?.TotalExVat ?? this.AssignedPurchasePrice;

            this.DeriveProductCharacteristics(derivation);

            if (derivation.ChangeSet.IsCreated(this))
            {
                this.Details = this.DeriveDetails();
            }
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var builder = new StringBuilder();

            builder.Append(this.ItemNumber);
            builder.Append(string.Join(" ", this.SerialNumber));
            builder.Append(string.Join(" ", this.Name));

            if (this.ExistOwnedBy)
            {
                builder.Append(string.Join(" ", this.OwnedBy.PartyName));
            }

            if (this.ExistReportingUnit)
            {
                builder.Append(string.Join(" ", this.ReportingUnit.PartyName));
            }

            if (this.ExistPartWhereSerialisedItem)
            {
                builder.Append(string.Join(" ", this.PartWhereSerialisedItem?.Brand?.Name));
                builder.Append(string.Join(" ", this.PartWhereSerialisedItem?.Model?.Name));
            }

            builder.Append(string.Join(" ", this.Keywords));

            this.SearchString = builder.ToString();
        }

        public void BaseDelete(DeletableDelete method)
        {
            // TODO: Restrit Delete?
            foreach (SerialisedItemVersion version in this.AllVersions)
            {
                version.Delete();
            }
        }

        public string DeriveDetails()
        {
            var builder = new StringBuilder();
            var part = this.PartWhereSerialisedItem;

            if (part != null && part.ExistManufacturedBy)
            {
                builder.Append($", Manufacturer: {part.ManufacturedBy.PartyName}");
            }

            if (part != null && part.ExistBrand)
            {
                builder.Append($", Brand: {part.Brand.Name}");
            }

            if (part != null && part.ExistModel)
            {
                builder.Append($", Model: {part.Model.Name}");
            }

            builder.Append($", SN: {this.SerialNumber}");

            if (this.ExistManufacturingYear)
            {
                builder.Append($", YOM: {this.ManufacturingYear}");
            }

            foreach (SerialisedItemCharacteristic characteristic in this.SerialisedItemCharacteristics)
            {
                if (characteristic.ExistValue)
                {
                    var characteristicType = characteristic.SerialisedItemCharacteristicType;
                    if (characteristicType.ExistUnitOfMeasure)
                    {
                        var uom = characteristicType.UnitOfMeasure.ExistAbbreviation
                                        ? characteristicType.UnitOfMeasure.Abbreviation
                                        : characteristicType.UnitOfMeasure.Name;
                        builder.Append(
                            $", {characteristicType.Name}: {characteristic.Value} {uom}");
                    }
                    else
                    {
                        builder.Append($", {characteristicType.Name}: {characteristic.Value}");
                    }
                }
            }

            var details = builder.ToString();

            if (details.StartsWith(","))
            {
                details = details.Substring(2);
            }

            return details;
        }

        private void DeriveProductCharacteristics(IDerivation derivation)
        {
            var characteristicsToDelete = this.SerialisedItemCharacteristics.ToList();
            var part = this.PartWhereSerialisedItem;

            if (this.ExistPartWhereSerialisedItem && part.ExistProductType)
            {
                foreach (SerialisedItemCharacteristicType characteristicType in part.ProductType.SerialisedItemCharacteristicTypes)
                {
                    var characteristic = this.SerialisedItemCharacteristics.FirstOrDefault(v => Equals(v.SerialisedItemCharacteristicType, characteristicType));
                    if (characteristic == null)
                    {
                        var newCharacteristic = new SerialisedItemCharacteristicBuilder(this.Strategy.Session)
                            .WithSerialisedItemCharacteristicType(characteristicType).Build();

                        this.AddSerialisedItemCharacteristic(newCharacteristic);

                        var partCharacteristics = part.SerialisedItemCharacteristics;
                        partCharacteristics.Filter.AddEquals(M.SerialisedItemCharacteristic.SerialisedItemCharacteristicType, characteristicType);
                        var fromPart = partCharacteristics.FirstOrDefault();

                        if (fromPart != null)
                        {
                            newCharacteristic.Value = fromPart.Value;
                        }
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
    }
}
