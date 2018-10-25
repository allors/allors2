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

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using System.Text;

    public partial class Person
    {
        public new string ToString() => this.PartyName;

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

            foreach (Employment relationship in this.EmploymentsWhereEmployee)
            {
                if (relationship.FromDate.Date <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public bool AppsIsActiveContact(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            foreach (OrganisationContactRelationship relationship in this.OrganisationContactRelationshipsWhereContact)
            {
                if (relationship.FromDate.Date <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public bool AppsIsActiveSalesRep(DateTime? date)
        {
            if (date == DateTime.MinValue)
            {
                return false;
            }

            foreach (SalesRepRelationship relationship in this.SalesRepRelationshipsWhereSalesRepresentative)
            {
                if (relationship.FromDate.Date <= date &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= date))
                {
                    return true;
                }
            }

            return false;
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.PartyName = this.DerivePartyName();

            this.AppsOnDeriveCurrentContacts(derivation);
            this.AppsOnDeriveInactiveContacts(derivation);
            this.AppsOnDeriveCurrentOrganisationContactRelationships(derivation);
            this.AppsOnDeriveInactiveOrganisationContactRelationships(derivation);
            this.AppsOnDeriveCurrentPartyContactMechanisms(derivation);
            this.AppsOnDeriveInactivePartyContactMechanisms(derivation);
            this.AppsOnDeriveCommission();
            this.SyncTimeSheet();

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

        public void AppsOnDeriveCurrentContacts(IDerivation derivation)
        {
            this.RemoveCurrentContacts();
        }

        public void AppsOnDeriveInactiveContacts(IDerivation derivation)
        {
            this.RemoveInactiveContacts();
        }

        public void AppsOnDeriveCurrentOrganisationContactRelationships(IDerivation derivation)
        {
            this.RemoveCurrentOrganisationContactRelationships();

            var contactRelationships = this.OrganisationContactRelationshipsWhereContact;
            foreach (OrganisationContactRelationship relationship in contactRelationships)
            {
                if (relationship.FromDate <= DateTime.UtcNow &&
                    (!relationship.ExistThroughDate || relationship.ThroughDate >= DateTime.UtcNow))
                {
                    this.AddCurrentOrganisationContactRelationship(relationship);
                }
            }
        }

        public void AppsOnDeriveInactiveOrganisationContactRelationships(IDerivation derivation)
        {
            this.RemoveInactiveOrganisationContactRelationships();

            var contactRelationships = this.OrganisationContactRelationshipsWhereContact;
            foreach (OrganisationContactRelationship relationship in contactRelationships)
            {
                if (relationship.FromDate > DateTime.UtcNow ||
                    (relationship.ExistThroughDate && relationship.ThroughDate < DateTime.UtcNow))
                {
                    this.AddInactiveOrganisationContactRelationship(relationship);
                }
            }
        }

        public void AppsOnDeriveInactivePartyContactMechanisms(IDerivation derivation)
        {
            this.RemoveInactivePartyContactMechanisms();

            foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanisms)
            {
                if (partyContactMechanism.FromDate > DateTime.UtcNow ||
                    (partyContactMechanism.ExistThroughDate && partyContactMechanism.ThroughDate < DateTime.UtcNow))
                {
                    this.AddInactivePartyContactMechanism(partyContactMechanism);
                }
            }
        }

        public void AppsOnDeriveCommission()
        {
            this.YTDCommission = 0;
            this.LastYearsCommission = 0;

            foreach (SalesRepCommission salesRepCommission in this.SalesRepCommissionsWhereSalesRep)
            {
                if (salesRepCommission.Commission.HasValue)
                {
                    if (salesRepCommission.Year == DateTime.UtcNow.Year)
                    {
                        this.YTDCommission += salesRepCommission.Commission.Value;
                    }

                    if (salesRepCommission.Year == DateTime.UtcNow.AddYears(-1).Year)
                    {
                        this.LastYearsCommission += salesRepCommission.Commission.Value;
                    }
                }
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

        private void SyncTimeSheet()
        {
            if (!this.ExistTimeSheetWhereWorker
                && (this.ExistEmploymentsWhereEmployee
                || this.ExistSubContractorRelationshipsWhereContractor
                || this.ExistSubContractorRelationshipsWhereSubContractor))
            {
                new TimeSheetBuilder(this.strategy.Session).WithWorker(this).Build();
            }
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
                if (communicationEvent is WebSiteCommunication website)
                {
                    if (Equals(website.Receiver, this))
                    {
                        website.RemoveReceiver();
                    }

                    if (Equals(website.Originator, this))
                    {
                        website.RemoveOriginator();
                    }

                    if (!website.ExistReceiver || !website.ExistOriginator)
                    {
                        communicationEvent.Delete();
                    }
                }

                else if (communicationEvent is FaxCommunication fax)
                {
                    if (Equals(fax.Receiver, this))
                    {
                        fax.RemoveReceiver();
                    }

                    if (Equals(fax.Originator, this))
                    {
                        fax.RemoveOriginator();
                    }

                    if (!fax.ExistReceiver || !fax.ExistOriginator)
                    {
                        communicationEvent.Delete();
                    }
                }

                else if (communicationEvent is LetterCorrespondence letter)
                {
                    if (letter.Originators.Contains(this))
                    {
                        letter.RemoveOriginator(this);
                    }

                    if (letter.Receivers.Contains(this))
                    {
                        letter.RemoveReceiver(this);
                    }

                    if (letter.Receivers.Count == 0 || letter.Originators.Count == 0)
                    {
                        communicationEvent.Delete();
                    }
                }

                else if (communicationEvent is PhoneCommunication phone)
                {
                    if (phone.Callers.Contains(this))
                    {
                        phone.RemoveCaller(this);
                    }

                    if (phone.Receivers.Contains(this))
                    {
                        phone.RemoveReceiver(this);
                    }

                    if (phone.Receivers.Count == 0 || phone.Callers.Count == 0)
                    {
                        communicationEvent.Delete();
                    }
                }

                else if (communicationEvent is EmailCommunication email)
                {
                    if (email.Addressees.Count == 0 || !email.ExistOriginator)
                    {
                        communicationEvent.Delete();
                    }
                }

                else if (communicationEvent is FaceToFaceCommunication faceToFace)
                {
                    if (faceToFace.Participants.Contains(this))
                    {
                        faceToFace.RemoveParticipant(this);
                    }

                    if (faceToFace.Participants.Count <= 1)
                    {
                        communicationEvent.Delete();
                    }
                }
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
    }
}