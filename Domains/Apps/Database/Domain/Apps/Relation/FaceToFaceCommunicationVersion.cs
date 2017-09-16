// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaceToFaceCommunicationVersion.cs" company="Allors bvba">
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
    public partial class FaceToFaceCommunicationVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (FaceToFaceCommunicationVersionBuilder) method.Builder;
            var faceToFaceCommunication = builder.FaceToFaceCommunication;

            if (faceToFaceCommunication != null)
            {
                this.ScheduledStart = faceToFaceCommunication.ScheduledStart;
                this.ContactMechanisms = faceToFaceCommunication.ContactMechanisms;
                this.InitialScheduledStart = faceToFaceCommunication.InitialScheduledStart;
                this.EventPurposes = faceToFaceCommunication.EventPurposes;
                this.ScheduledEnd = faceToFaceCommunication.ScheduledEnd;
                this.ActualEnd = faceToFaceCommunication.ActualEnd;
                this.WorkEfforts = faceToFaceCommunication.WorkEfforts;
                this.Description = faceToFaceCommunication.Description;
                this.InitialScheduledEnd = faceToFaceCommunication.InitialScheduledEnd;
                this.Subject = faceToFaceCommunication.Subject;
                this.Documents = faceToFaceCommunication.Documents;
                this.Case = faceToFaceCommunication.Case;
                this.Priority = faceToFaceCommunication.Priority;
                this.Owner = faceToFaceCommunication.Owner;
                this.Note = faceToFaceCommunication.Note;
                this.ActualStart = faceToFaceCommunication.ActualStart;
                this.SendNotification = faceToFaceCommunication.SendNotification;
                this.SendReminder = faceToFaceCommunication.SendReminder;
                this.RemindAt = faceToFaceCommunication.RemindAt;
                this.Participants= faceToFaceCommunication.Participants;
                this.Location = faceToFaceCommunication.Location;
                this.CurrentObjectState = faceToFaceCommunication.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}