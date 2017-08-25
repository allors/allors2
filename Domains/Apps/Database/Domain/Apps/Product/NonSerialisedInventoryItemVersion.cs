// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonSerialisedInventoryItemVersion.cs" company="Allors bvba">
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

    public partial class NonSerialisedInventoryItemVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (NonSerialisedInventoryItemVersionBuilder) method.Builder;
            var nonSerialisedInventoryItem = builder.NonSerialisedInventoryItem;

            if (nonSerialisedInventoryItem != null)
            {
                this.InventoryItemVariances = nonSerialisedInventoryItem.InventoryItemVariances;
                this.Part = nonSerialisedInventoryItem.Part;
                this.Container = nonSerialisedInventoryItem.Container;
                this.Name = nonSerialisedInventoryItem.Name;
                this.Lot = nonSerialisedInventoryItem.Lot;
                this.Sku = nonSerialisedInventoryItem.Sku;
                this.UnitOfMeasure = nonSerialisedInventoryItem.UnitOfMeasure;
                this.DerivedProductCategories = nonSerialisedInventoryItem.DerivedProductCategories;
                this.Good = nonSerialisedInventoryItem.Good;
                this.Facility = nonSerialisedInventoryItem.Facility;
                this.QuantityCommittedOut = nonSerialisedInventoryItem.QuantityCommittedOut;
                this.QuantityOnHand = nonSerialisedInventoryItem.QuantityOnHand;
                this.AvailableToPromise = nonSerialisedInventoryItem.AvailableToPromise;
                this.QuantityExpectedIn = nonSerialisedInventoryItem.QuantityExpectedIn;
                this.CurrentObjectState = nonSerialisedInventoryItem.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}