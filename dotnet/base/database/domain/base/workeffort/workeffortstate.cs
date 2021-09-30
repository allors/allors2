// <copyright file="WorkEffortState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class WorkEffortState
    {
        public bool IsCreated => this.Equals(new WorkEffortStates(this.Strategy.Session).Created);

        public bool IsInProgress => this.Equals(new WorkEffortStates(this.Strategy.Session).InProgress);

        public bool IsCompleted => this.Equals(new WorkEffortStates(this.Strategy.Session).Completed);

        public bool IsFinished => this.Equals(new WorkEffortStates(this.Strategy.Session).Finished);

        public bool IsCancelled => this.Equals(new WorkEffortStates(this.Strategy.Session).Cancelled);
    }
}
