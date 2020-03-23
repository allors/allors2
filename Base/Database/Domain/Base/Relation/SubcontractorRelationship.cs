// <copyright file="SubcontractorRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;

namespace Allors.Domain
{
    public partial class SubContractorRelationship
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistSubContractor)
                {
                    iteration.AddDependency(this.SubContractor, this);
                    iteration.Mark(this.SubContractor);

                    foreach (OrganisationContactRelationship contactRelationship in this.SubContractor.OrganisationContactRelationshipsWhereOrganisation)
                    {
                        iteration.AddDependency(this, contactRelationship);
                        iteration.Mark(contactRelationship);
                    }
                }

                if (this.ExistContractor)
                {
                    iteration.AddDependency(this.Contractor, this);
                    iteration.Mark(this.Contractor);
                }
            }
        }

        public void BaseOnInit(ObjectOnInit method)
        {
            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistContractor && internalOrganisations.Count() == 1)
            {
                this.Contractor = internalOrganisations.First();
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            this.Parties = new[] { this.Contractor, this.SubContractor };

            if (this.ExistContractor && this.ExistSubContractor)
            {
                // HACK: DerivedRoles
                var internalOrganisationDerivedRoles = (OrganisationDerivedRoles)this.Contractor;

                if (this.FromDate <= this.Session().Now() && (!this.ExistThroughDate || this.ThroughDate >= this.Session().Now()))
                {
                    internalOrganisationDerivedRoles.AddActiveSubContractor(this.SubContractor);
                }

                if (this.FromDate > this.Session().Now() || (this.ExistThroughDate && this.ThroughDate < this.Session().Now()))
                {
                    internalOrganisationDerivedRoles.RemoveActiveSubContractor(this.SubContractor);
                }

                if (this.SubContractor?.ContactsUserGroup != null)
                {
                    foreach (OrganisationContactRelationship contactRelationship in this.SubContractor.OrganisationContactRelationshipsWhereOrganisation)
                    {
                        if (contactRelationship.FromDate <= this.Session().Now() &&
                            (!contactRelationship.ExistThroughDate || this.ThroughDate >= this.Session().Now()))
                        {
                            if (!this.SubContractor.ContactsUserGroup.Members.Contains(contactRelationship.Contact))
                            {
                                this.SubContractor.ContactsUserGroup.AddMember(contactRelationship.Contact);
                            }
                        }
                        else
                        {
                            if (this.SubContractor.ContactsUserGroup.Members.Contains(contactRelationship.Contact))
                            {
                                this.SubContractor.ContactsUserGroup.RemoveMember(contactRelationship.Contact);
                            }
                        }
                    }
                }
            }
        }
    }
}
