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

    public partial class SerialisedInventoryItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            { new TransitionalConfiguration(M.SerialisedInventoryItem, M.SerialisedInventoryItem.SerialisedInventoryItemState), };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public int QuantityOnHand()
        {
            var state = this.SerialisedInventoryItemState;

            if (state.IsScrap || state.IsSold || state.IsInRent)
            {
                return 0;
            }

            return 1;
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSerialisedInventoryItemState)
            {
                this.SerialisedInventoryItemState = new SerialisedInventoryItemStates(this.Strategy.Session).Available;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistName && this.ExistPart && this.Part.ExistName)
            {
                this.Name = this.Part.Name;
            }

            this.AppsOnDeriveProductCharacteristics(derivation);

            if (derivation.IsCreated(this))
            {
                this.Details = this.DeriveDetails();
            }
        }

        private void AppsOnDeriveProductCharacteristics(IDerivation derivation)
        {
            var characteristicsToDelete = this.SerialisedInventoryItemCharacteristics.ToList();
            if (this.ExistPart && this.Part.ExistProductType)
            {
                foreach (SerialisedInventoryItemCharacteristicType characteristicType in this.Part.ProductType.SerialisedInventoryItemCharacteristicTypes)
                {
                    var characteristic = this.SerialisedInventoryItemCharacteristics.FirstOrDefault(v => Equals(v.SerialisedInventoryItemCharacteristicType, characteristicType));
                    if (characteristic == null)
                    {
                        this.AddSerialisedInventoryItemCharacteristic(
                            new SerialisedInventoryItemCharacteristicBuilder(this.strategy.Session)
                            .WithSerialisedInventoryItemCharacteristicType(characteristicType)
                            .Build());
                    }
                    else
                    {
                        characteristicsToDelete.Remove(characteristic);
                    }
                }
            }

            foreach (SerialisedInventoryItemCharacteristic characteristic in characteristicsToDelete)
            {
                this.RemoveSerialisedInventoryItemCharacteristic(characteristic);
            }
        }

        public void AppsDelete(DeletableDelete method)
        {
            foreach (InventoryItemVersion version in this.AllVersions)
            {
                version.Delete();
            }
        }

        public string DeriveDetails()
        {
            var builder = new StringBuilder();

            if (this.ExistPart && this.Part.ExistManufacturedBy)
            {
                builder.Append($", Manufacturer: {this.Part.ManufacturedBy.PartyName}");
            }

            //foreach (ProductFeature feature in this.ProductFeatureApplicabilitiesWhereAvailableFor)
            //{
            //    if (feature is Brand)
            //    {
            //        var brand = (Brand)feature;
            //        builder.Append($", Brand: {brand.Name}");
            //    }
            //    if (feature is Model)
            //    {
            //        var model = (Model)feature;
            //        builder.Append($", Model: {model.Name}");
            //    }
            //}

            builder.Append($", SN: {this.SerialNumber}");

            if (this.ExistManufacturingYear)
            {
                builder.Append($", YOM: {this.ManufacturingYear}");
            }

            foreach (SerialisedInventoryItemCharacteristic characteristic in this.SerialisedInventoryItemCharacteristics)
            {
                if (characteristic.ExistValue)
                {
                    var characteristicType = characteristic.SerialisedInventoryItemCharacteristicType;
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