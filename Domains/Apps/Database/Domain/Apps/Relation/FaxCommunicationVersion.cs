// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaxCommunicationVersion.cs" company="Allors bvba">
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
    public partial class FaxCommunicationVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (FaxCommunicationVersionBuilder) method.Builder;
            var faxCommunication = builder.FaxCommunication;

            if (faxCommunication != null)
            {
                this.ToParties = faxCommunication.ToParties;
                this.FromParties = faxCommunication.FromParties;
                this.InvolvedParties = faxCommunication.InvolvedParties;
                this.ScheduledStart = faxCommunication.ScheduledStart;
                this.ContactMechanisms = faxCommunication.ContactMechanisms;
                this.InitialScheduledStart = faxCommunication.InitialScheduledStart;
                this.EventPurposes = faxCommunication.EventPurposes;
                this.ScheduledEnd = faxCommunication.ScheduledEnd;
                this.ActualEnd = faxCommunication.ActualEnd;
                this.WorkEfforts = faxCommunication.WorkEfforts;
                this.Description = faxCommunication.Description;
                this.InitialScheduledEnd = faxCommunication.InitialScheduledEnd;
                this.Subject = faxCommunication.Subject;
                this.Documents = faxCommunication.Documents;
                this.Case = faxCommunication.Case;
                this.Priority = faxCommunication.Priority;
                this.Owner = faxCommunication.Owner;
                this.Note = faxCommunication.Note;
                this.ActualStart = faxCommunication.ActualStart;
                this.SendNotification = faxCommunication.SendNotification;
                this.SendReminder = faxCommunication.SendReminder;
                this.RemindAt = faxCommunication.RemindAt;
                this.Originator= faxCommunication.Originator;
                this.Receiver = faxCommunication.Receiver;
                this.OutgoingFaxNumber = faxCommunication.OutgoingFaxNumber;
                this.CurrentObjectState = faxCommunication.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}