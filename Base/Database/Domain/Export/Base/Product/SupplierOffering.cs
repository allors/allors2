// <copyright file="SupplierOffering.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;

namespace Allors.Domain
{
    using Allors.Meta;

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
