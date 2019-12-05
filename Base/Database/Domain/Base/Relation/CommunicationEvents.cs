// <copyright file="CommunicationEvents.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class CommunicationEvents
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.CommunicationEventState);

        protected override void BaseSecure(Security config)
        {
            ObjectState scheduled = new CommunicationEventStates(this.Session).Scheduled;
            ObjectState cancelled = new CommunicationEventStates(this.Session).Cancelled;
            ObjectState closed = new CommunicationEventStates(this.Session).Completed;

            var reopenId = M.CommunicationEvent.Reopen;
            var closeId = M.CommunicationEvent.Close;
            var cancelId = M.CommunicationEvent.Cancel;

            config.Deny(this.ObjectType, scheduled, reopenId);
            config.Deny(this.ObjectType, closed, closeId, cancelId);
            config.Deny(this.ObjectType, cancelled, cancelId);

            config.Deny(this.ObjectType, closed, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Write);
        }
    }
}
