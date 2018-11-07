// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialisedInventoryItem.cs" company="Allors bvba">
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
    using System.Linq;
    using System.Text;

    using Meta;

    public partial class SerialisedItem
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistItemNumber)
            {
                this.ItemNumber = this.strategy.Session.GetSingleton().Settings.NextSerialisedItemNumber();
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistName && this.ExistPartWhereSerialisedItem)
            {
                this.Name = this.PartWhereSerialisedItem.Name;
            }

            this.DeriveProductCharacteristics(derivation);

            if (derivation.IsCreated(this))
            {
                this.Details = this.DeriveDetails();
            }
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
                        this.AddSerialisedItemCharacteristic(
                            new SerialisedItemCharacteristicBuilder(this.strategy.Session)
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

        public void AppsDelete(DeletableDelete method)
        {
            //TODO: Restrit Delete?
        }

        public string DeriveDetails()
        {
            var builder = new StringBuilder();
            var part = this.PartWhereSerialisedItem;

            if (part != null && part.ExistManufacturedBy)
            {
                builder.Append($", Manufacturer: {part.ManufacturedBy.PartyName}");
            }

            if (part != null && part.ExistManufacturedBy)
            {
                builder.Append($", Brand: {part.Brand.Name}");
            }

            if (part != null && part.ExistManufacturedBy)
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
    }
}