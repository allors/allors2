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
namespace Allors.Domain
{
    using Meta;

    public partial class SupplierOffering
    {
        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.SupplierOffering.Part, M.SupplierOffering.Product);
            derivation.Validation.AssertExistsAtMostOne(this, M.SupplierOffering.Part, M.SupplierOffering.Product);

            this.AppsOnDeriveInventoryItem(derivation);
        }

        public void AppsOnDeriveInventoryItem(IDerivation derivation)
        {
            Good good = null;
            if (this.ExistProduct)
            {
                good = this.Product as Good;
            }

            var supplier = this.Supplier as Organisation;
            if (supplier != null && good != null)
            {
                if (good.ExistInventoryItemKind && good.InventoryItemKind.Equals(new InventoryItemKinds(this.Strategy.Session).NonSerialised))
                {
                    foreach (SupplierRelationship supplierRelationship in supplier.SupplierRelationshipsWhereSupplier)
                    {
                        foreach (Facility facility in supplierRelationship.InternalOrganisation.FacilitiesWhereOwner)
                        {
                            var inventoryItems = good.InventoryItemVersionedsWhereGood;
                            inventoryItems.Filter.AddEquals(M.InventoryItemVersioned.Facility, facility);
                            var inventoryItem = inventoryItems.First;

                            if (inventoryItem == null)
                            {
                                new NonSerialisedInventoryItemBuilder(this.Strategy.Session).WithFacility(facility).WithGood(good).Build();
                            }
                        }
                    }
                }
                else
                {
                    if (good.ExistFinishedGood &&
                        good.FinishedGood.ExistInventoryItemKind && 
                        good.FinishedGood.InventoryItemKind.Equals(new InventoryItemKinds(this.Strategy.Session).NonSerialised))
                    {
                        foreach (SupplierRelationship supplierRelationship in supplier.SupplierRelationshipsWhereSupplier)
                        {
                            foreach (Facility facility in supplierRelationship.InternalOrganisation.FacilitiesWhereOwner)
                            {
                                var inventoryItems = good.FinishedGood.InventoryItemVersionedsWherePart;
                                inventoryItems.Filter.AddEquals(M.InventoryItemVersioned.Facility, facility);
                                var inventoryItem = inventoryItems.First;

                                if (inventoryItem == null)
                                {
                                    new NonSerialisedInventoryItemBuilder(this.Strategy.Session).WithFacility(facility).WithPart(good.FinishedGood).Build();
                                }
                            }
                        }
                    }                   
                }
            }
        }
    }
}