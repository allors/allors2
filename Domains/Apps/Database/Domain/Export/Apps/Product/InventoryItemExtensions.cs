// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryItemExtensions.cs" company="Allors bvba">
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
    using System.Linq;

    public static partial class InventoryItemExtensions
    {
        public static void AppsOnBuild(this InventoryItem @this, ObjectOnBuild method)
        {
            //TODO: Let Sync set Unit of Measure
            if (!@this.ExistUnitOfMeasure)
            {
                @this.UnitOfMeasure = @this.Part?.UnitOfMeasure;
            }
        }

        public static void AppsOnPreDerive(this InventoryItem @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            derivation.AddDependency(@this.Part, @this);
        }

        public static void AppsOnDerive(this InventoryItem @this, ObjectOnDerive method)
        {
            var session = @this.Strategy.Session;
            var internalOrganisations = new Organisations(session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!@this.ExistInventoryOwnershipsWhereInventoryItem && internalOrganisations.Count() == 1)
            {
                new InventoryOwnershipBuilder(session)
                    .WithInventoryItem(@this)
                    .WithInternalOrganisation(internalOrganisations.First())
                    .Build();
            }

            if (!@this.ExistFacility && internalOrganisations.Count() == 1 && internalOrganisations.Single().StoresWhereInternalOrganisation.Count == 1)
            {
                @this.Facility = internalOrganisations.Single().StoresWhereInternalOrganisation.Single().DefaultFacility;
            }

            //TODO: Let Sync set Unit of Measure
            if (!@this.ExistUnitOfMeasure)
            {
                @this.UnitOfMeasure = @this.Part?.UnitOfMeasure;
            }
        }
    }
}