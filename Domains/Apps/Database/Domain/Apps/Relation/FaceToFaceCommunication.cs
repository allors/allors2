// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaceToFaceCommunication.cs" company="Allors bvba">
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
    using Allors.Meta;
    
    public partial class FaceToFaceCommunication
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.Participants.Count <= 1)
            {
                derivation.Validation.AddError(this, this.Meta.Participants, "minimum 2 participants");
            }

            this.FromParties = this.Participants;
            this.ToParties = this.Participants;
            this.AppsOnDeriveInvolvedParties(derivation);
        }

        public void AppsOnDeriveInvolvedParties(IDerivation derivation)
        {
            this.InvolvedParties = this.Participants;

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
                !object.Equals(this.Participants, this.CurrentVersion.Participants) ||
                !object.Equals(this.Location, this.CurrentVersion.Location) ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new FaceToFaceCommunicationVersionBuilder(this.Strategy.Session).WithFaceToFaceCommunication(this).Build();
                this.AddAllVersion(this.CurrentVersion);
            }

            if (isNewStateVersion)
            {
                this.CurrentStateVersion = CurrentVersion;
                this.AddAllStateVersion(this.CurrentStateVersion);
            }

            this.RemoveSecurityTokens();
            this.AddSecurityToken(Singleton.Instance(this.Strategy.Session).DefaultSecurityToken);

            if (this.ExistOwner)
            {
                this.AddSecurityToken(this.Owner.OwnerSecurityToken);
            }
        }
    }
}
