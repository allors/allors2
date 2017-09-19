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
    using System.Collections.Generic;
    using System.Text;

    public partial class Person
    {
        private bool IsDeletable => !this.ExistCurrentOrganisationContactRelationships;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.PartyName = this.DerivePartyName();
            this.AppsOnDeriveCurrentContacts(derivation);
            this.AppsOnDeriveInactiveContacts(derivation);
            this.AppsOnDeriveCurrentOrganisationContactRelationships(derivation);
            this.AppsOnDeriveInactiveOrganisationContactRelationships(derivation);
            this.AppsOnDeriveCurrentEmployment(derivation);
            this.AppsOnDeriveCurrentPartyContactMechanisms(derivation);
            this.AppsOnDeriveCurrentPartyRelationships(derivation);
            this.AppsOnDeriveInactivePartyContactMechanisms(derivation);
            this.AppsOnDeriveCommission();

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

        public void AppsOnDeriveCurrentEmployment(IDerivation derivation)
        {
            this.RemoveCurrentEmployment();

            foreach (Employment employment in this.EmploymentsWhereEmployee)
            {
                if (employment.FromDate <= DateTime.UtcNow &&
                    (!employment.ExistThroughDate || employment.ThroughDate >= DateTime.UtcNow))
                {
                    this.CurrentEmployment = employment;
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

            return partyName.ToString();
        }

        public void AppsDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
                foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanisms)
                {
                    partyContactMechanism.ContactMechanism.Delete();
                }

                foreach (CommunicationEvent communicationEvent in this.CommunicationEventsWhereInvolvedParty)
                {
                    if (communicationEvent.GetType().Name == typeof(WebSiteCommunication).Name)
                    {
                        var communication = (WebSiteCommunication)communicationEvent;
                        if (Equals(communication.Receiver, this))
                        {
                            communication.RemoveReceiver();
                        }

                        if (Equals(communication.Originator, this))
                        {
                            communication.RemoveOriginator();
                        }

                        if (!communication.ExistReceiver || !communication.ExistOriginator)
                        {
                            communicationEvent.Delete();
                        }
                    }

                    if (communicationEvent.GetType().Name == typeof(FaxCommunication).Name)
                    {
                        var communication = (FaxCommunication)communicationEvent;
                        if (Equals(communication.Receiver, this))
                        {
                            communication.RemoveReceiver();
                        }

                        if (Equals(communication.Originator, this))
                        {
                            communication.RemoveOriginator();
                        }

                        if (!communication.ExistReceiver || !communication.ExistOriginator)
                        {
                            communicationEvent.Delete();
                        }
                    }

                    if (communicationEvent.GetType().Name == typeof(LetterCorrespondence).Name)
                    {
                        var communication = (LetterCorrespondence)communicationEvent;
                        if (communication.Originators.Contains(this))
                        {
                            communication.RemoveOriginator(this);
                        }

                        if (communication.Receivers.Contains(this))
                        {
                            communication.RemoveReceiver(this);
                        }

                        if (communication.Receivers.Count == 0 || communication.Originators.Count == 0)
                        {
                            communicationEvent.Delete();
                        }
                    }

                    if (communicationEvent.GetType().Name == typeof(PhoneCommunication).Name)
                    {
                        var communication = (PhoneCommunication)communicationEvent;
                        if (communication.Callers.Contains(this))
                        {
                            communication.RemoveCaller(this);
                        }

                        if (communication.Receivers.Contains(this))
                        {
                            communication.RemoveReceiver(this);
                        }

                        if (communication.Receivers.Count == 0 || communication.Callers.Count == 0)
                        {
                            communicationEvent.Delete();
                        }
                    }

                    if (communicationEvent.GetType().Name == typeof(EmailCommunication).Name)
                    {
                        var communication = (EmailCommunication)communicationEvent;

                        if (communication.Addressees.Count == 0 || !communication.ExistOriginator)
                        {
                            communicationEvent.Delete();
                        }
                    }

                    if (communicationEvent.GetType().Name == typeof(FaceToFaceCommunication).Name)
                    {
                        var communication = (FaceToFaceCommunication)communicationEvent;
                        if (communication.Participants.Contains(this))
                        {
                            communication.RemoveParticipant(this);
                        }

                        if (communication.Participants.Count <= 1)
                        {
                            communicationEvent.Delete();
                        }
                    }
                }

                foreach (OrganisationContactRelationship contactRelationship in this.OrganisationContactRelationshipsWhereContact)
                {
                    contactRelationship.Delete();
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
}