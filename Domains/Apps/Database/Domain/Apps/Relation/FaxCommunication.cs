// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaxCommunication.cs" company="Allors bvba">
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
    public partial class FaxCommunication
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            this.AppsOnDeriveFromParties();
            this.AppsOnDeriveToParties();
            this.AppsOnDeriveInvolvedParties();
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            var isNewVersion =
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
                !object.Equals(this.Receiver, this.CurrentVersion.Receiver) ||
                !object.Equals(this.OutgoingFaxNumber, this.CurrentVersion.OutgoingFaxNumber) ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            var isNewStateVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new FaxCommunicationVersionBuilder(this.Strategy.Session).WithFaxCommunication(this).Build();
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
            this.AddFromParty(this.Originator);
        }

        public void AppsOnDeriveToParties()
        {
            this.RemoveToParties();
            this.AddToParty(this.Receiver);
        }

        public void AppsOnDeriveInvolvedParties()
        {
            this.RemoveInvolvedParties();

            if (this.ExistOriginator)
            {
                this.AddInvolvedParty(this.Originator);
            }

            if (this.ExistReceiver)
            {
                this.AddInvolvedParty(this.Receiver);
            }

            //if (this.ExistOwner && !this.InvolvedParties.Contains(this.Owner))
            //{
            //    this.AddInvolvedParty(this.Owner);
            //}

            if (this.ExistPartyRelationshipWhereCommunicationEvent)
            {
                foreach (Party party in this.PartyRelationshipWhereCommunicationEvent.Parties)
                {
                    this.AddInvolvedParty(party);
                }
            }
        }
    }
}
