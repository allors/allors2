// <copyright file="SupplierRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public partial class SupplierRelationship
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistSupplier)
            {
                derivation.AddDependency(this.Supplier, this);

                foreach (OrganisationContactRelationship contactRelationship in this.Supplier.OrganisationContactRelationshipsWhereOrganisation)
                {
                    derivation.AddDependency(this, contactRelationship);
                }
            }

            if (this.ExistInternalOrganisation)
            {
                derivation.AddDependency(this.InternalOrganisation, this);
            }
        }

        public void BaseOnInit(ObjectOnInit method)
        {
            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistInternalOrganisation && internalOrganisations.Count() == 1)
            {
                this.InternalOrganisation = internalOrganisations.First();
            }

            if (!this.ExistNeedsApproval)
            {
                this.NeedsApproval = this.InternalOrganisation.PurchaseOrderNeedsApproval;
            }

            if (!this.ApprovalThresholdLevel1.HasValue)
            {
                this.ApprovalThresholdLevel1 = this.InternalOrganisation.PurchaseOrderApprovalThresholdLevel1;
            }

            if (!this.ApprovalThresholdLevel2.HasValue)
            {
                this.ApprovalThresholdLevel2 = this.InternalOrganisation.PurchaseOrderApprovalThresholdLevel2;
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.BaseOnDeriveInternalOrganisationSupplier(derivation);
            this.BaseOnDeriveMembership(derivation);

            this.Parties = new Party[] { this.Supplier, this.InternalOrganisation };
        }

        public void BaseOnDeriveInternalOrganisationSupplier(IDerivation derivation)
        {
            if (this.ExistSupplier)
            {
                if (this.FromDate <= this.strategy.Session.Now() && (!this.ExistThroughDate || this.ThroughDate >= this.strategy.Session.Now()))
                {
                    this.InternalOrganisation.AddActiveSupplier(this.Supplier);
                }

                if (this.FromDate > this.strategy.Session.Now() || (this.ExistThroughDate && this.ThroughDate < this.strategy.Session.Now()))
                {
                    this.InternalOrganisation.RemoveActiveSupplier(this.Supplier);
                }
            }
        }

        public void BaseOnDeriveMembership(IDerivation derivation)
        {
            if (this.ExistSupplier)
            {
                if (this.Supplier.ContactsUserGroup != null)
                {
                    foreach (OrganisationContactRelationship contactRelationship in this.Supplier.OrganisationContactRelationshipsWhereOrganisation)
                    {
                        if (contactRelationship.FromDate <= this.strategy.Session.Now() &&
                            (!contactRelationship.ExistThroughDate || this.ThroughDate >= this.strategy.Session.Now()))
                        {
                            if (!this.Supplier.ContactsUserGroup.Members.Contains(contactRelationship.Contact))
                            {
                                this.Supplier.ContactsUserGroup.AddMember(contactRelationship.Contact);
                            }
                        }
                        else
                        {
                            if (this.Supplier.ContactsUserGroup.Members.Contains(contactRelationship.Contact))
                            {
                                this.Supplier.ContactsUserGroup.RemoveMember(contactRelationship.Contact);
                            }
                        }
                    }
                }
            }
        }
    }
}
