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
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.Part != null)
            {
                derivation.AddDependency(this.Part, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsOnDeriveInventoryItem(derivation);
        }

        public void AppsOnDeriveInventoryItem(IDerivation derivation)
        {
            if (this.ExistPart && this.Part.ExistInventoryItemKind &&
                this.Part.InventoryItemKind.Equals(new InventoryItemKinds(this.Strategy.Session).NonSerialised))
            {
                var internalOrganisation = this.Part.InternalOrganisation;

                if (internalOrganisation != null)
                {
                    foreach (Facility facility in internalOrganisation.FacilitiesWhereOwner)
                    {
                        var inventoryItems = this.Part.InventoryItemsWherePart;
                        inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, facility);
                        var inventoryItem = inventoryItems.First;

                        if (inventoryItem == null)
                        {
                            new NonSerialisedInventoryItemBuilder(this.Strategy.Session).WithFacility(facility).WithPart(this.Part).Build();
                        }
                    }
                }
            }
        }
    }
}