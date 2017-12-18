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
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            { new TransitionalConfiguration(M.SerialisedInventoryItem, M.SerialisedInventoryItem.SerialisedInventoryItemState), };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSerialisedInventoryItemState)
            {
                this.SerialisedInventoryItemState = new SerialisedInventoryItemStates(this.Strategy.Session).Good;
            }

            if (!this.ExistFacility)
            {
                this.Facility = this.Strategy.Session.GetSingleton().InternalOrganisation.DefaultFacility;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            derivation.AddDependency(this, this.Good);
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.InventoryItem.Good, M.InventoryItem.Part);
            derivation.Validation.AssertExistsAtMostOne(this, M.InventoryItem.Good, M.InventoryItem.Part);

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

            this.AppsOnDeriveProductCategories(derivation);
        }

        public void AppsDelete(DeletableDelete method)
        {
            foreach (InventoryItemVersion version in this.AllVersions)
            {
                version.Delete();
            }
        }
    }
}