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
namespace Allors.Domain
{
    public static partial class PartExtensions
    {
        public static void AppsOnBuild(this Part @this, ObjectOnBuild method)
        {
            @this.InventoryItemKind = new InventoryItemKinds(@this.Strategy.Session).NonSerialised;

            if (!@this.ExistOwnedByParty)
            {
                @this.OwnedByParty = Singleton.Instance(@this.Strategy.Session).DefaultInternalOrganisation;
            }
        }

        public static void AppsOnPreDerive(this Part @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.ChangeSet.Associations.Contains(@this.Id))
            {
                if (@this.ExistInventoryItemVersionedsWherePart)
                {
                    foreach (InventoryItem inventoryItem in @this.InventoryItemVersionedsWherePart)
                    {
                        derivation.AddDependency(inventoryItem, @this);
                    }
                }
            }
        }

        public static void AppsOnDerive(this Part @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            @this.AppsOnDeriveInventoryItem(derivation);
        }

        public static void AppsOnDeriveInventoryItem(this Part @this, IDerivation derivation)
        {
            if (@this.ExistInventoryItemKind && @this.InventoryItemKind.Equals(new InventoryItemKinds(@this.Strategy.Session).NonSerialised))
            {
                if (!@this.ExistInventoryItemVersionedsWherePart)
                {
                    new NonSerialisedInventoryItemBuilder(@this.Strategy.Session)
                        .WithFacility(@this.OwnedByParty.DefaultFacility)
                        .WithPart(@this)
                        .Build();
                }
            }
        }
    }
}