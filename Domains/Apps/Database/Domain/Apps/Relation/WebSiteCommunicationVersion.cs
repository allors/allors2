// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebSiteCommunicationVersion.cs" company="Allors bvba">
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
    public partial class WebSiteCommunicationVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (WebSiteCommunicationVersionBuilder) method.Builder;
            var webSiteCommunication = builder.WebSiteCommunication;

            if (webSiteCommunication != null)
            {
                this.ToParties = webSiteCommunication.ToParties;
                this.FromParties = webSiteCommunication.FromParties;
                this.InvolvedParties = webSiteCommunication.InvolvedParties;
                this.ScheduledStart = webSiteCommunication.ScheduledStart;
                this.ContactMechanisms = webSiteCommunication.ContactMechanisms;
                this.InitialScheduledStart = webSiteCommunication.InitialScheduledStart;
                this.EventPurposes = webSiteCommunication.EventPurposes;
                this.ScheduledEnd = webSiteCommunication.ScheduledEnd;
                this.ActualEnd = webSiteCommunication.ActualEnd;
                this.WorkEfforts = webSiteCommunication.WorkEfforts;
                this.Description = webSiteCommunication.Description;
                this.InitialScheduledEnd = webSiteCommunication.InitialScheduledEnd;
                this.Subject = webSiteCommunication.Subject;
                this.Documents = webSiteCommunication.Documents;
                this.Case = webSiteCommunication.Case;
                this.Priority = webSiteCommunication.Priority;
                this.Owner = webSiteCommunication.Owner;
                this.Note = webSiteCommunication.Note;
                this.ActualStart = webSiteCommunication.ActualStart;
                this.SendNotification = webSiteCommunication.SendNotification;
                this.SendReminder = webSiteCommunication.SendReminder;
                this.RemindAt = webSiteCommunication.RemindAt;
                this.Originator = webSiteCommunication.Originator;
                this.Receiver = webSiteCommunication.Receiver;
                this.CurrentObjectState = webSiteCommunication.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}