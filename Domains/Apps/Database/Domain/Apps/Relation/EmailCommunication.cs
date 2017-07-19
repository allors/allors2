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
using Allors.Meta;

namespace Allors.Domain
{
    public partial class EmailCommunication
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

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
                this.AddFromParty(this.Originator.PartyWherePersonalEmailAddress);
            }
        }

        public void AppsOnDeriveToParties()
        {
            this.RemoveToParties();

            foreach (EmailAddress addressee in this.Addressees)
            {
                if (addressee.ExistPartyWherePersonalEmailAddress && !this.ToParties.Contains(addressee.PartyWherePersonalEmailAddress))
                {
                    this.AddToParty(addressee.PartyWherePersonalEmailAddress);
                }
            }

            foreach (EmailAddress carbonCopy in this.CarbonCopies)
            {
                if (carbonCopy.ExistPartyWherePersonalEmailAddress && !this.ToParties.Contains(carbonCopy.PartyWherePersonalEmailAddress))
                {
                    this.AddToParty(carbonCopy.PartyWherePersonalEmailAddress);
                }
            }

            foreach (EmailAddress blindCopy in this.BlindCopies)
            {
                if (blindCopy.ExistPartyWherePersonalEmailAddress && !this.ToParties.Contains(blindCopy.PartyWherePersonalEmailAddress))
                {
                    this.AddToParty(blindCopy.PartyWherePersonalEmailAddress);
                }
            }

            if (this.ExistPartyRelationshipWhereCommunicationEvent)
            {
                foreach (Party party in this.PartyRelationshipWhereCommunicationEvent.Parties)
                {
                    this.AddToParty(party);
                }
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
        }
    }
}
