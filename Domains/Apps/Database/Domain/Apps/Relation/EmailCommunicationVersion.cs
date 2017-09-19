// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailCommunicationVersion.cs" company="Allors bvba">
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
    public partial class EmailCommunicationVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (EmailCommunicationVersionBuilder) method.Builder;
            var emailCommunication = builder.EmailCommunication;

            if (emailCommunication != null)
            {
                this.ToParties = emailCommunication.ToParties;
                this.FromParties = emailCommunication.FromParties;
                this.InvolvedParties = emailCommunication.InvolvedParties;
                this.ScheduledStart = emailCommunication.ScheduledStart;
                this.ContactMechanisms = emailCommunication.ContactMechanisms;
                this.InitialScheduledStart = emailCommunication.InitialScheduledStart;
                this.EventPurposes = emailCommunication.EventPurposes;
                this.ScheduledEnd = emailCommunication.ScheduledEnd;
                this.ActualEnd = emailCommunication.ActualEnd;
                this.WorkEfforts = emailCommunication.WorkEfforts;
                this.Description = emailCommunication.Description;
                this.InitialScheduledEnd = emailCommunication.InitialScheduledEnd;
                this.Subject = emailCommunication.Subject;
                this.Documents = emailCommunication.Documents;
                this.Case = emailCommunication.Case;
                this.Priority = emailCommunication.Priority;
                this.Owner = emailCommunication.Owner;
                this.Note = emailCommunication.Note;
                this.ActualStart = emailCommunication.ActualStart;
                this.SendNotification = emailCommunication.SendNotification;
                this.SendReminder = emailCommunication.SendReminder;
                this.RemindAt = emailCommunication.RemindAt;
                this.Originator = emailCommunication.Originator;
                this.Addressees = emailCommunication.Addressees;
                this.CarbonCopies= emailCommunication.CarbonCopies;
                this.BlindCopies = emailCommunication.BlindCopies;
                this.EmailTemplate= emailCommunication.EmailTemplate;
                this.IncomingMail = emailCommunication.IncomingMail;
                this.CurrentObjectState = emailCommunication.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}