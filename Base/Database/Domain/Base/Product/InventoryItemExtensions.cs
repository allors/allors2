// <copyright file="InventoryItemExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using System.Text;

    public static partial class InventoryItemExtensions
    {
        public static void BaseOnBuild(this InventoryItem @this, ObjectOnBuild method)
        {
            // TODO: Let Sync set Unit of Measure
            if (!@this.ExistUnitOfMeasure)
            {
                @this.UnitOfMeasure = @this.Part?.UnitOfMeasure;
            }
        }

        public static void BaseOnPreDerive(this InventoryItem @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(@this) || changeSet.IsCreated(@this) || changeSet.HasChangedRoles(@this))
            {
                iteration.AddDependency(@this.Part, @this);
                iteration.Mark(@this.Part);
            }
        }

        public static void BaseOnDerive(this InventoryItem @this, ObjectOnDerive method)
        {
            var session = @this.Strategy.Session;
            var now = session.Now();

            if (!@this.ExistFacility && @this.ExistPart && @this.Part.ExistDefaultFacility)
            {
                @this.Facility = @this.Part.DefaultFacility;
            }

            Party owner = (Organisation)@this.Facility.Owner;
            if (@this is SerialisedInventoryItem item)
            {
                owner = item.SerialisedItem?.ReportingUnit;
            }

            foreach (var inventoryOwnership in @this.InventoryOwnershipsWhereInventoryItem.Where(v => v.FromDate < now && (!v.ExistThroughDate || v.ThroughDate >= now)))
            {
                if (!Equals(inventoryOwnership.Owner, owner))
                {
                    inventoryOwnership.ThroughDate = now;
                }
            }

            var ownerInventoryOwnership = @this.InventoryOwnershipsWhereInventoryItem.FirstOrDefault(v => v.Owner.Equals(owner) && v.FromDate < now && (!v.ExistThroughDate || v.ThroughDate >= now));
            if (ownerInventoryOwnership == null && owner != null)
            {
                new InventoryOwnershipBuilder(session)
                    .WithInventoryItem(@this)
                    .WithOwner(owner)
                    .Build();
            }

            // TODO: Let Sync set Unit of Measure
            if (!@this.ExistUnitOfMeasure)
            {
                @this.UnitOfMeasure = @this.Part?.UnitOfMeasure;
            }
        }

        public static void BaseOnPostDerive(this InventoryItem @this, ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;
            var part = @this.Part;

            var builder = new StringBuilder();
            if (part.ExistProductIdentifications)
            {
                builder.Append(string.Join(" ", part.ProductIdentifications.Select(v => v.Identification)));
            }

            if (part.ExistProductCategoriesWhereAllPart)
            {
                builder.Append(string.Join(" ", part.ProductCategoriesWhereAllPart.Select(v => v.Name)));
            }

            if (part.ExistSupplierOfferingsWherePart)
            {
                builder.Append(string.Join(" ", part.SupplierOfferingsWherePart.Select(v => v.Supplier.PartyName)));
                builder.Append(string.Join(" ", part.SupplierOfferingsWherePart.Select(v => v.SupplierProductId)));
                builder.Append(string.Join(" ", part.SupplierOfferingsWherePart.Select(v => v.SupplierProductName)));
            }

            if (part.ExistSerialisedItems)
            {
                builder.Append(string.Join(" ", part.SerialisedItems.Select(v => v.SerialNumber)));
            }

            if (part.ExistProductType)
            {
                builder.Append(string.Join(" ", part.ProductType.Name));
            }

            if (part.ExistBrand)
            {
                builder.Append(string.Join(" ", part.Brand.Name));
            }

            if (part.ExistModel)
            {
                builder.Append(string.Join(" ", part.Model.Name));
            }

            builder.Append(string.Join(" ", part.Keywords));

            @this.SearchString = builder.ToString();
        }
    }
}
