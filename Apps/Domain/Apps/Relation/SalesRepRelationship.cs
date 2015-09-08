// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepRelationship.cs" company="Allors bvba">
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
    using System;

    public partial class SalesRepRelationship
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            

            if (!this.ExistFromDate)
            {
                this.FromDate = DateTime.UtcNow;
            }

            if (!this.ExistInternalOrganisation)
            {
                this.InternalOrganisation = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.DeriveMembership();
            this.DeriveCustomerContacts();

            if (this.ExistCustomer && this.ExistSalesRepresentative)
            {
                this.Customer.DeriveCurrentSalesReps(derivation);
                this.SalesRepresentative.OnDerive(x => x.WithDerivation(derivation));
            }
        }

        public void AppsCustomerContacts()
        {
            if (this.ExistSalesRepresentative && this.SalesRepresentative.ExistCurrentEmployment && this.ExistCustomer)
            {
                var customer = this.Customer as Organisation;
                if (customer != null)
                {
                    foreach (OrganisationContactRelationship contactRelationship in customer.OrganisationContactRelationshipsWhereOrganisation)
                    {
                        contactRelationship.Contact.OnPostDerive();

                        foreach (CustomerRelationship customerRelationship in contactRelationship.Organisation.CustomerRelationshipsWhereCustomer)
                        {
                            this.AddSecurityToken(customerRelationship.InternalOrganisation.OwnerSecurityToken);
                        }
                    }
                }
            }
        }

        public void AppsOnDeriveMembership()
        {
            var usergroups = this.InternalOrganisation.UserGroupsWhereParty;
            usergroups.Filter.AddEquals(UserGroups.Meta.Parent, new Roles(this.Strategy.Session).Sales.UserGroupWhereRole);
            var salesRepUserGroup = usergroups.First;

            if (this.ExistSalesRepresentative && this.SalesRepresentative.ExistCurrentEmployment
                && this.ExistInternalOrganisation)
            {
                if (salesRepUserGroup != null)
                {
                    if (this.FromDate <= DateTime.UtcNow && (!this.ExistThroughDate || this.ThroughDate >= DateTime.UtcNow))
                    {
                        if (!salesRepUserGroup.ContainsMember(this.SalesRepresentative))
                        {
                            salesRepUserGroup.AddMember(this.SalesRepresentative);
                        }
                    }
                    else
                    {
                        if (salesRepUserGroup.ContainsMember(this.SalesRepresentative))
                        {
                            salesRepUserGroup.RemoveMember(this.SalesRepresentative);
                        }
                    }
                }
            }
            else if (this.ExistSalesRepresentative && this.ExistInternalOrganisation)
            {
                salesRepUserGroup.RemoveMember(this.SalesRepresentative);
            }
        }

        public void AppsOnDeriveCommission()
        {
            this.YTDCommission = 0;
            this.LastYearsCommission = 0;

            foreach (SalesRepCommission salesRepCommission in this.SalesRepresentative.SalesRepCommissionsWhereSalesRep)
            {
                if (salesRepCommission.InternalOrganisation.Equals(this.InternalOrganisation))
                {
                    if (salesRepCommission.Year == DateTime.UtcNow.Year)
                    {
                        this.YTDCommission += salesRepCommission.Year;
                    }

                    if (salesRepCommission.Year == DateTime.UtcNow.AddYears(-1).Year)
                    {
                        this.LastYearsCommission += salesRepCommission.Year;
                    }
                }
            }
        }
    }
}