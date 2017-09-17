// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LetterCorrespondenceVersion.cs" company="Allors bvba">
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
    public partial class LetterCorrespondenceVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (LetterCorrespondenceVersionBuilder) method.Builder;
            var letterCorrespondence = builder.LetterCorrespondence;

            if (letterCorrespondence != null)
            {
                this.ToParties = letterCorrespondence.ToParties;
                this.FromParties = letterCorrespondence.FromParties;
                this.InvolvedParties = letterCorrespondence.InvolvedParties;
                this.ScheduledStart = letterCorrespondence.ScheduledStart;
                this.ContactMechanisms = letterCorrespondence.ContactMechanisms;
                this.InitialScheduledStart = letterCorrespondence.InitialScheduledStart;
                this.EventPurposes = letterCorrespondence.EventPurposes;
                this.ScheduledEnd = letterCorrespondence.ScheduledEnd;
                this.ActualEnd = letterCorrespondence.ActualEnd;
                this.WorkEfforts = letterCorrespondence.WorkEfforts;
                this.Description = letterCorrespondence.Description;
                this.InitialScheduledEnd = letterCorrespondence.InitialScheduledEnd;
                this.Subject = letterCorrespondence.Subject;
                this.Documents = letterCorrespondence.Documents;
                this.Case = letterCorrespondence.Case;
                this.Priority = letterCorrespondence.Priority;
                this.Owner = letterCorrespondence.Owner;
                this.Note = letterCorrespondence.Note;
                this.ActualStart = letterCorrespondence.ActualStart;
                this.SendNotification = letterCorrespondence.SendNotification;
                this.SendReminder = letterCorrespondence.SendReminder;
                this.RemindAt = letterCorrespondence.RemindAt;
                this.PostalAddresses= letterCorrespondence.PostalAddresses;
                this.Originators = letterCorrespondence.Originators;
                this.Receivers = letterCorrespondence.Receivers;
                this.IncomingLetter = letterCorrespondence.IncomingLetter;
                this.CurrentObjectState = letterCorrespondence.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}