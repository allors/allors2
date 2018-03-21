// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartExtensions.v.cs" company="Allors bvba">
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
    public static partial class PartExtensions
    {
        public static void AppsOnBuild(this Part @this, ObjectOnBuild method)
        {
            @this.InventoryItemKind = new InventoryItemKinds(@this.Strategy.Session).NonSerialised;
        }

        public static void AppsOnPreDerive(this Part @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.ChangeSet.Associations.Contains(@this.Id))
            {
                if (@this.ExistInventoryItemsWherePart)
                {
                    foreach (InventoryItem inventoryItem in @this.InventoryItemsWherePart)
                    {
                        derivation.AddDependency(inventoryItem, @this);
                    }
                }
            }
        }

        public static void AppsOnDerive(this Part @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(@this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!@this.ExistInternalOrganisation && internalOrganisations.Count() == 1)
            {
                @this.InternalOrganisation = internalOrganisations.First();
            }

            @this.AppsOnDeriveInventoryItem(derivation);
        }

        public static void AppsOnDeriveInventoryItem(this Part @this, IDerivation derivation)
        {
            if (@this.ExistInventoryItemKind && @this.InventoryItemKind.Equals(new InventoryItemKinds(@this.Strategy.Session).NonSerialised))
            {
                if (!@this.ExistInventoryItemsWherePart )
                {
                    foreach (Facility facility in @this.InternalOrganisation.FacilitiesWhereOwner)
                    {
                        new NonSerialisedInventoryItemBuilder(@this.Strategy.Session)
                            .WithPart(@this)
                            .WithFacility(facility)
                            .Build();
                    }
                }
            }
        }
    }
}