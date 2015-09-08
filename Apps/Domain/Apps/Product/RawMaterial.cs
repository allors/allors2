// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawMaterial.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class RawMaterial
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            

            this.InventoryItemKind = new InventoryItemKinds(this.Strategy.Session).NonSerialized;

            if (!this.ExistOwnedByParty)
            {
                this.OwnedByParty = Domain.Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistInventoryItemsWherePart)
                {
                    foreach (InventoryItem inventoryItem in InventoryItemsWherePart)
                    {
                        derivation.AddDependency(inventoryItem, this);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.DeriveInventoryItem(derivation);
        }

        public void AppsOnDeriveInventoryItem(IDerivation derivation)
        {
            if (this.ExistInventoryItemKind && this.InventoryItemKind.Equals(new InventoryItemKinds(this.Strategy.Session).NonSerialized))
            {
                if (!this.ExistInventoryItemsWherePart)
                {
                    new NonSerializedInventoryItemBuilder(this.Strategy.Session)
                        .WithFacility(this.OwnedByParty.DefaultFacility)
                        .WithPart(this)
                        .Build();
                }
            }
        }
    }
}