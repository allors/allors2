// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialisedInventoryItemVersion.cs" company="Allors bvba">
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

    public partial class SerialisedInventoryItemVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (SerialisedInventoryItemVersionBuilder) method.Builder;
            var serialisedInventoryItem = builder.SerialisedInventoryItem;

            if (serialisedInventoryItem != null)
            {
                this.SerialNumber = serialisedInventoryItem.SerialNumber;
                this.InventoryItemVariances = serialisedInventoryItem.InventoryItemVariances;
                this.Part = serialisedInventoryItem.Part;
                this.Container = serialisedInventoryItem.Container;
                this.Name = serialisedInventoryItem.Name;
                this.Lot = serialisedInventoryItem.Lot;
                this.Sku = serialisedInventoryItem.Sku;
                this.UnitOfMeasure = serialisedInventoryItem.UnitOfMeasure;
                this.DerivedProductCategories = serialisedInventoryItem.DerivedProductCategories;
                this.Good = serialisedInventoryItem.Good;
                this.Facility = serialisedInventoryItem.Facility;
                this.Ownership = serialisedInventoryItem.Ownership;
                this.Owner = serialisedInventoryItem.Owner;
                this.AcquisitionYear = serialisedInventoryItem.AcquisitionYear;
                this.ManufacturingYear = serialisedInventoryItem.ManufacturingYear;
                this.ReplacementValue = serialisedInventoryItem.ReplacementValue;
                this.LifeTime = serialisedInventoryItem.LifeTime;
                this.DepreciationYears = serialisedInventoryItem.DepreciationYears;
                this.PurchasePrice = serialisedInventoryItem.PurchasePrice;
                this.ExpectedSalesPrice = serialisedInventoryItem.ExpectedSalesPrice;
                this.RefurbishCost = serialisedInventoryItem.RefurbishCost;
                this.TransportCost = serialisedInventoryItem.TransportCost;
                this.CurrentObjectState = serialisedInventoryItem.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}