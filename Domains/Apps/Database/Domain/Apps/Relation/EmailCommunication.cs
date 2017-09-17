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
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            var isNewVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.ActualStart, this.CurrentVersion.ActualStart);

            var isNewStateVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.ScheduledStart, this.CurrentVersion.ScheduledStart) ||
                !object.Equals(this.ContactMechanisms, this.CurrentVersion.ContactMechanisms) ||
                !object.Equals(this.InitialScheduledStart, this.CurrentVersion.InitialScheduledStart) ||
                !object.Equals(this.EventPurposes, this.CurrentVersion.EventPurposes) ||
                !object.Equals(this.ScheduledEnd, this.CurrentVersion.ScheduledEnd) ||
                !object.Equals(this.ActualEnd, this.CurrentVersion.ActualEnd) ||
                !object.Equals(this.WorkEfforts, this.CurrentVersion.WorkEfforts) ||
                !object.Equals(this.Description, this.CurrentVersion.Description) ||
                !object.Equals(this.InitialScheduledEnd, this.CurrentVersion.InitialScheduledEnd) ||
                !object.Equals(this.Subject, this.CurrentVersion.Subject) ||
                !object.Equals(this.Documents, this.CurrentVersion.Documents) ||
                !object.Equals(this.Case, this.CurrentVersion.Case) ||
                !object.Equals(this.Priority, this.CurrentVersion.Priority) ||
                !object.Equals(this.Owner, this.CurrentVersion.Owner) ||
                !object.Equals(this.Note, this.CurrentVersion.Note) ||
                !object.Equals(this.ActualStart, this.CurrentVersion.ActualStart) ||
                !object.Equals(this.SendNotification, this.CurrentVersion.SendNotification) ||
                !object.Equals(this.SendReminder, this.CurrentVersion.SendReminder) ||
                !object.Equals(this.RemindAt, this.CurrentVersion.RemindAt) ||
                !object.Equals(this.Originator, this.CurrentVersion.Originator) ||
                !object.Equals(this.Addressees, this.CurrentVersion.Addressees) ||
                !object.Equals(this.CarbonCopies, this.CurrentVersion.CarbonCopies) ||
                !object.Equals(this.BlindCopies, this.CurrentVersion.BlindCopies) ||
                !object.Equals(this.EmailTemplate, this.CurrentVersion.EmailTemplate) ||
                !object.Equals(this.IncomingMail, this.CurrentVersion.IncomingMail) ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new EmailCommunicationVersionBuilder(this.Strategy.Session).WithEmailCommunication(this).Build();
                this.AddAllVersion(this.CurrentVersion);
            }

            if (isNewStateVersion)
            {
                this.CurrentStateVersion = CurrentVersion;
                this.AddAllStateVersion(this.CurrentStateVersion);
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

            //if (this.ExistOwner && !this.InvolvedParties.Contains(this.Owner))
            //{
            //    this.AddInvolvedParty(this.Owner);
            //}
        }
    }
}
