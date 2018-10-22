// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkEffortType.cs" company="Allors bvba">
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
    using Meta;

    public partial class WorkEffortState
    {
        public bool IsNeedsAction => this.Equals(new WorkEffortStates(this.strategy.Session).NeedsAction);

        public bool IsConfirmed => this.Equals(new WorkEffortStates(this.strategy.Session).Confirmed);

        public bool IsDeclined => this.Equals(new WorkEffortStates(this.strategy.Session).Declined);

        public bool IsInProgress => this.Equals(new WorkEffortStates(this.strategy.Session).InProgress);

        public bool IsCompleted => this.Equals(new WorkEffortStates(this.strategy.Session).Completed);

        public bool IsFinished => this.Equals(new WorkEffortStates(this.strategy.Session).Finished);

        public bool IsCancelled => this.Equals(new WorkEffortStates(this.strategy.Session).Cancelled);

        public bool IsDelegated => this.Equals(new WorkEffortStates(this.strategy.Session).Delagated);

        public bool IsInPlanning => this.Equals(new WorkEffortStates(this.strategy.Session).InPlanning);

        public bool IsPlanned => this.Equals(new WorkEffortStates(this.strategy.Session).Planned);

        public bool IsSent => this.Equals(new WorkEffortStates(this.strategy.Session).Sent);

        public bool IsAccepted => this.Equals(new WorkEffortStates(this.strategy.Session).Accepted);

        public bool IsTentative => this.Equals(new WorkEffortStates(this.strategy.Session).Tentative);
    }
}