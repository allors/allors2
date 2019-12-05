// <copyright file="Requirements.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Requirements
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.RequirementState);

        protected override void BaseSecure(Security config)
        {
            var createdState = new WorkEffortStates(this.Session).Created;
            var cancelledState = new WorkEffortStates(this.Session).Cancelled;
            var finishedState = new WorkEffortStates(this.Session).Completed;

            config.Deny(this.ObjectType, createdState, M.WorkEffort.Reopen);

            config.Deny(this.ObjectType, cancelledState, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, finishedState, Operations.Execute, Operations.Read);
        }
    }
}
