// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCommunicationVersion.cs" company="Allors bvba">
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
    public partial class PhoneCommunicationVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (PhoneCommunicationVersionBuilder) method.Builder;
            var phoneCommunication = builder.PhoneCommunication;

            if (phoneCommunication != null)
            {
                this.ToParties = phoneCommunication.ToParties;
                this.FromParties = phoneCommunication.FromParties;
                this.InvolvedParties = phoneCommunication.InvolvedParties;
                this.ScheduledStart = phoneCommunication.ScheduledStart;
                this.ContactMechanisms = phoneCommunication.ContactMechanisms;
                this.InitialScheduledStart = phoneCommunication.InitialScheduledStart;
                this.EventPurposes = phoneCommunication.EventPurposes;
                this.ScheduledEnd = phoneCommunication.ScheduledEnd;
                this.ActualEnd = phoneCommunication.ActualEnd;
                this.WorkEfforts = phoneCommunication.WorkEfforts;
                this.Description = phoneCommunication.Description;
                this.InitialScheduledEnd = phoneCommunication.InitialScheduledEnd;
                this.Subject = phoneCommunication.Subject;
                this.Documents = phoneCommunication.Documents;
                this.Case = phoneCommunication.Case;
                this.Priority = phoneCommunication.Priority;
                this.Owner = phoneCommunication.Owner;
                this.Note = phoneCommunication.Note;
                this.ActualStart = phoneCommunication.ActualStart;
                this.SendNotification = phoneCommunication.SendNotification;
                this.SendReminder = phoneCommunication.SendReminder;
                this.RemindAt = phoneCommunication.RemindAt;
                this.LeftVoiceMail = phoneCommunication.LeftVoiceMail;
                this.IncomingCall = phoneCommunication.IncomingCall;
                this.Receivers = phoneCommunication.Receivers;
                this.Callers = phoneCommunication.Callers;
                this.CurrentObjectState = phoneCommunication.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}