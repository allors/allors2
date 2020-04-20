

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

            ((InventoryItemDerivedRoles)@this).PartDisplayName = @this.Part?.DisplayName;

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

            builder.Append(part.SearchString);

            @this.SearchString = builder.ToString();
        }

        public static void BaseDelete(this InventoryItem @this, DeletableDelete method)
        {
            foreach (InventoryOwnership inventoryOwnership in @this.InventoryOwnershipsWhereInventoryItem)
            {
                inventoryOwnership.Delete();
            }
        }
    }
}
