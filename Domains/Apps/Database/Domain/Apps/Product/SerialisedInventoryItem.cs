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
    using Meta;

    public partial class SerialisedInventoryItem
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new SerialisedInventoryItemObjectStates(this.Strategy.Session).Good;
            }

            if (!this.ExistFacility)
            {
                if (Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation != null)
                {
                    this.Facility = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation.DefaultFacility;
                }
            }

            if (!this.ExistSku && this.ExistGood && this.Good.ExistSku)
            {
                this.Sku = this.Good.Sku;
            }

            if (!this.ExistSku && this.ExistPart && this.Part.ExistSku)
            {
                this.Sku = this.Part.Sku;
            }

            if (!this.ExistName && this.ExistGood && this.Good.ExistName)
            {
                this.Name = this.Good.Name;
            }

            if (!this.ExistName && this.ExistPart && this.Part.ExistName)
            {
                this.Name = this.Part.Name;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.InventoryItemVersioned.Good, M.InventoryItemVersioned.Part);
            derivation.Validation.AssertExistsAtMostOne(this, M.InventoryItemVersioned.Good, M.InventoryItemVersioned.Part);

            this.AppsOnDeriveProductCategories(derivation);
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            var isNewVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.SerialNumber, this.CurrentVersion.SerialNumber) ||
                !object.Equals(this.InventoryItemVariances, this.CurrentVersion.InventoryItemVariances) ||
                !object.Equals(this.Part, this.CurrentVersion.Part) ||
                !object.Equals(this.Container, this.CurrentVersion.Container) ||
                !object.Equals(this.Name, this.CurrentVersion.Name) ||
                !object.Equals(this.Lot, this.CurrentVersion.Lot) ||
                !object.Equals(this.Sku, this.CurrentVersion.Sku) ||
                !object.Equals(this.UnitOfMeasure, this.CurrentVersion.UnitOfMeasure) ||
                !object.Equals(this.DerivedProductCategories, this.CurrentVersion.DerivedProductCategories) ||
                !object.Equals(this.Good, this.CurrentVersion.Good) ||
                !object.Equals(this.Facility, this.CurrentVersion.Facility) ||
                !object.Equals(this.Ownership, this.CurrentVersion.Ownership) ||
                !object.Equals(this.Owner, this.CurrentVersion.Owner) ||
                !object.Equals(this.AcquisitionYear, this.CurrentVersion.AcquisitionYear) ||
                !object.Equals(this.ManufacturingYear, this.CurrentVersion.ManufacturingYear) ||
                !object.Equals(this.ReplacementValue, this.CurrentVersion.ReplacementValue) ||
                !object.Equals(this.LifeTime, this.CurrentVersion.LifeTime) ||
                !object.Equals(this.DepreciationYears, this.CurrentVersion.DepreciationYears) ||
                !object.Equals(this.PurchasePrice, this.CurrentVersion.PurchasePrice) ||
                !object.Equals(this.ExpectedSalesPrice, this.CurrentVersion.ExpectedSalesPrice) ||
                !object.Equals(this.RefurbishCost, this.CurrentVersion.RefurbishCost) ||
                !object.Equals(this.TransportCost, this.CurrentVersion.TransportCost) ||

                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (!this.ExistCurrentVersion || isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new SerialisedInventoryItemVersionBuilder(this.Strategy.Session).WithSerialisedInventoryItem(this).Build();
                this.AddAllVersion(this.CurrentVersion);
            }
        }
    }
}