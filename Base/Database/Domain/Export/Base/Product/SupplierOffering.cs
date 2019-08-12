// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupplierOffering.cs" company="Allors bvba">
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
    using Meta;

    public partial class SupplierOffering
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.Part != null)
            {
                derivation.AddDependency(this.Part, this);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistCurrency)
            {
                this.Currency = this.strategy.Session.GetSingleton().Settings.PreferredCurrency;
            }

            this.BaseOnDeriveInventoryItem(derivation);
        }

        public void BaseOnDeriveInventoryItem(IDerivation derivation)
        {
            if (this.ExistPart && this.Part.ExistInventoryItemKind &&
                this.Part.InventoryItemKind.Equals(new InventoryItemKinds(this.Strategy.Session).NonSerialised))
            {
                var warehouses = this.Strategy.Session.Extent<Facility>();
                warehouses.Filter.AddEquals(M.Facility.FacilityType, new FacilityTypes(this.strategy.Session).Warehouse);

                foreach (Facility facility in warehouses)
                {
                    var inventoryItems = this.Part.InventoryItemsWherePart;
                    inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, facility);
                    var inventoryItem = inventoryItems.First;

                    if (inventoryItem == null)
                    {
                        new NonSerialisedInventoryItemBuilder(this.Strategy.Session).WithPart(this.Part).WithFacility(facility).Build();
                    }
                }
            }
        }
    }
}