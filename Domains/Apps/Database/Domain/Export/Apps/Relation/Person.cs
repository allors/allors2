// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="Allors bvba">
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

using Allors.Meta;

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using System.Text;

    public partial class Person
    {
        public new string ToString() => $"Person {this.Id} {this.PartyName}";

        private bool IsDeletable =>
            !this.ExistCurrentOrganisationContactRelationships
            && !this.ExistEmploymentsWhereEmployee
            && (!this.ExistTimeSheetWhereWorker || !this.TimeSheetWhereWorker.ExistTimeEntries);

        public bool AppsIsActiveEmployee(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistEmploymentsWhereEmployee 
                   && this.EmploymentsWhereEmployee
                       .Any(v => v.FromDate.Date <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
        }

        public bool AppsIsActiveContact(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistOrganisationContactRelationshipsWhereContact
                   && this.OrganisationContactRelationshipsWhereContact
                       .Any(v => v.FromDate.Date <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
        }

        public bool AppsIsActiveSalesRep(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return this.ExistSalesRepRelationshipsWhereSalesRepresentative
                   && this.SalesRepRelationshipsWhereSalesRepresentative
                       .Any(v => v.FromDate.Date <= date && (!v.ExistThroughDate || v.ThroughDate >= date));
          
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.Strategy.Session.Prefetch(this.PrefetchPolicy);

            this.PartyName = this.DerivePartyName();
            
            var allOrganisationContactRelationships = this.OrganisationContactRelationshipsWhereContact;

            this.CurrentOrganisationContactRelationships = allOrganisationContactRelationships
                .Where(v => v.FromDate <= DateTime.UtcNow && (!v.ExistThroughDate || v.ThroughDate >= DateTime.UtcNow))
                .ToArray();

            this.InactiveOrganisationContactRelationships = allOrganisationContactRelationships
                .Except(this.CurrentOrganisationContactRelationships)
                .ToArray();
            
            this.CurrentPartyContactMechanisms = this.PartyContactMechanisms
                .Where(v => v.FromDate > DateTime.UtcNow || (v.ExistThroughDate && v.ThroughDate < DateTime.UtcNow))
                .ToArray();
            
            this.InactivePartyContactMechanisms = this.PartyContactMechanisms
                    .Except(this.CurrentPartyContactMechanisms).ToArray();

            if (!this.ExistTimeSheetWhereWorker
                && (this.ExistEmploymentsWhereEmployee
                    || this.ExistSubContractorRelationshipsWhereContractor
                    || this.ExistSubContractorRelationshipsWhereSubContractor))
            {
                new TimeSheetBuilder(this.strategy.Session).WithWorker(this).Build();
            }

            var deletePermission = new Permissions(this.strategy.Session).Get(this.Meta.ObjectType, this.Meta.Delete, Operations.Execute);
            if (this.IsDeletable)
            {
                this.RemoveDeniedPermission(deletePermission);
            }
            else
            {
                this.AddDeniedPermission(deletePermission);
            }
        }

        public PrefetchPolicy PrefetchPolicy
        {
            get
            {
                var organisationContactRelationshipPrefetch = new PrefetchPolicyBuilder()
                    .WithRule(M.OrganisationContactRelationship.Contact)
                    .Build();

             
                return new PrefetchPolicyBuilder()
                    .WithRule(M.Person.OrganisationContactRelationshipsWhereContact)
                    .WithRule(M.Person.PartyContactMechanisms.RoleType)
                    .WithRule(M.Person.TimeSheetWhereWorker)
                    .Build();
            }
        }

        private string DerivePartyName()
        {
            var partyName = new StringBuilder();

            if (this.ExistFirstName)
            {
                partyName.Append(this.FirstName);
            }

            if (this.ExistMiddleName)
            {
                if (partyName.Length > 0)
                {
                    partyName.Append(" ");
                }

                partyName.Append(this.MiddleName);
            }

            if (this.ExistLastName)
            {
                if (partyName.Length > 0)
                {
                    partyName.Append(" ");
                }

                partyName.Append(this.LastName);
            }

            return partyName.Length > 0 ? partyName.ToString() : $"[{this.UserName}]";
        }

        public void AppsDelete(DeletableDelete method)
        {
            if (!this.IsDeletable)
            {
                return;
            }

            foreach (PartyFinancialRelationship partyFinancialRelationship in this.PartyFinancialRelationshipsWhereParty)
            {
                partyFinancialRelationship.Delete();
            }

            foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanisms)
            {
                partyContactMechanism.ContactMechanism.Delete();
            }

            foreach (CommunicationEvent communicationEvent in this.CommunicationEventsWhereInvolvedParty)
            {
                communicationEvent.Delete();
            }

            foreach (OrganisationContactRelationship contactRelationship in this.OrganisationContactRelationshipsWhereContact)
            {
                contactRelationship.Delete();
            }

            if (this.ExistTimeSheetWhereWorker)
            {
                this.TimeSheetWhereWorker.Delete();
            }

            if (this.ExistOwnerAccessControl)
            {
                this.OwnerAccessControl.Delete();
            }

            if (this.ExistOwnerSecurityToken)
            {
                this.OwnerSecurityToken.Delete();
            }
        }

        public void Sync(PartyContactMechanism[] organisationContactMechanisms)
        {
            foreach (PartyContactMechanism partyContactMechanism in organisationContactMechanisms)
            {
                this.RemoveCurrentOrganisationContactMechanism(partyContactMechanism.ContactMechanism);

                if (partyContactMechanism.FromDate <= DateTime.UtcNow &&
                    (!partyContactMechanism.ExistThroughDate || partyContactMechanism.ThroughDate >= DateTime.UtcNow))
                {
                    this.AddCurrentOrganisationContactMechanism(partyContactMechanism.ContactMechanism);
                }
            }
        }
    }
}