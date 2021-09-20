// <copyright file="CustomerRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public partial class CustomerRelationship
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistCustomer)
                {
                    iteration.AddDependency(this.Customer, this);
                    iteration.Mark(this.Customer);

                    if (this.Customer is Organisation customer)
                    {
                        foreach (OrganisationContactRelationship contactRelationship in customer.OrganisationContactRelationshipsWhereOrganisation)
                        {
                            iteration.AddDependency(this, contactRelationship);
                            iteration.Mark(contactRelationship);
                        }
                    }
                }

                if (this.ExistInternalOrganisation)
                {
                    iteration.AddDependency(this.InternalOrganisation, this);
                    iteration.Mark(this.InternalOrganisation);
                }
            }
        }

        public void BaseOnPostBuild(ObjectOnPostBuild method)
        {
            // TODO: Don't extent for InternalOrganisations
            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistInternalOrganisation && internalOrganisations.Length == 1)
            {
                this.InternalOrganisation = internalOrganisations.First();
            }
        }

        public void BaseOnDerive(ObjectOnDerive method) => this.Parties = new Party[] { this.Customer, this.InternalOrganisation };
    }
}
