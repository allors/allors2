// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailCommunication.cs" company="Allors bvba">
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

using System;

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class EmailCommunication
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.EmailCommunication, M.EmailCommunication.CommunicationEventState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistSubject && this.ExistEmailTemplate && this.EmailTemplate.ExistSubjectTemplate)
            {
                this.Subject = this.EmailTemplate.SubjectTemplate;
            }

            this.AppsOnDeriveFromParties();
            this.AppsOnDeriveToParties();
            this.AppsOnDeriveInvolvedParties();

            if (!this.ExistOriginator || this.Addressees.Count == 0)
            {
                this.Delete();
            }
        }

        public void AppsOnDeriveFromParties()
        {
            this.RemoveFromParties();

            if (this.ExistOriginator)
            {
                foreach (PartyContactMechanism partyContactMechanism in this.Originator.PartyContactMechanismsWhereContactMechanism)
                {
                    if (partyContactMechanism.FromDate <= DateTime.UtcNow &&
                        (!partyContactMechanism.ExistThroughDate || partyContactMechanism.ThroughDate >= DateTime.UtcNow))
                    {
                        this.AddFromParty(partyContactMechanism.PartyWherePartyContactMechanism);
                    }
                }
            }
        }

        public void AppsOnDeriveToParties()
        {
            this.RemoveToParties();

            foreach (EmailAddress addressee in this.Addressees)
            {
                foreach (PartyContactMechanism partyContactMechanism in addressee.PartyContactMechanismsWhereContactMechanism)
                {
                    if (partyContactMechanism.FromDate <= DateTime.UtcNow &&
                        (!partyContactMechanism.ExistThroughDate || partyContactMechanism.ThroughDate >= DateTime.UtcNow))
                    {
                        this.AddToParty(partyContactMechanism.PartyWherePartyContactMechanism);
                    }
                }
            }

            foreach (EmailAddress carbonCopy in this.CarbonCopies)
            {
                foreach (PartyContactMechanism partyContactMechanism in carbonCopy.PartyContactMechanismsWhereContactMechanism)
                {
                    if (partyContactMechanism.FromDate <= DateTime.UtcNow &&
                        (!partyContactMechanism.ExistThroughDate || partyContactMechanism.ThroughDate >= DateTime.UtcNow))
                    {
                        this.AddToParty(partyContactMechanism.PartyWherePartyContactMechanism);
                    }
                }
            }

            foreach (EmailAddress blindCopy in this.BlindCopies)
            {
                foreach (PartyContactMechanism partyContactMechanism in blindCopy.PartyContactMechanismsWhereContactMechanism)
                {
                    if (partyContactMechanism.FromDate <= DateTime.UtcNow &&
                        (!partyContactMechanism.ExistThroughDate || partyContactMechanism.ThroughDate >= DateTime.UtcNow))
                    {
                        this.AddToParty(partyContactMechanism.PartyWherePartyContactMechanism);
                    }
                }
            }

            foreach (Party party in this.PartiesWhereCommunicationEvent)
            {
                this.AddToParty(party);
            }
        }

        public void AppsOnDeriveInvolvedParties()
        {
            this.RemoveInvolvedParties();

            foreach (Party party in this.FromParties)
            {
                this.AddInvolvedParty(party);
            }

            foreach (Party party in this.ToParties)
            {
                this.AddInvolvedParty(party);
            }

            if (this.ExistOwner && !this.InvolvedParties.Contains(this.Owner))
            {
                this.AddInvolvedParty(this.Owner);
            }

            foreach (Party party in this.InvolvedParties)
            {
                if (party is Person person)
                {
                    foreach (OrganisationContactRelationship organisationContactRelationship in person.OrganisationContactRelationshipsWhereContact)
                    {
                        if (organisationContactRelationship.FromDate <= DateTime.UtcNow &&
                            (!organisationContactRelationship.ExistThroughDate || organisationContactRelationship.ThroughDate >= DateTime.UtcNow))
                        {
                            this.AddInvolvedParty(organisationContactRelationship.Organisation);
                        }
                    }
                }
            }
        }
    }
}
