// <copyright file="RequestItems.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class RequestItems
    {
        protected override void BaseSecure(Security config)
        {
            var draft = new RequestItemStates(this.Session).Draft;
            var cancelled = new RequestItemStates(this.Session).Cancelled;
            var rejected = new RequestItemStates(this.Session).Rejected;
            var submitted = new RequestItemStates(this.Session).Submitted;
            var quoted = new RequestItemStates(this.Session).Quoted;

            var cancel = this.Meta.Cancel;
            var hold = this.Meta.Hold;
            var submit = this.Meta.Submit;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, submitted, submit);
            config.Deny(this.ObjectType, cancelled, cancel, submit, hold);
            config.Deny(this.ObjectType, rejected, cancel, submit, hold);
            config.Deny(this.ObjectType, quoted, cancel, submit, hold, delete);

            config.Deny(this.ObjectType, cancelled, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Write);
            config.Deny(this.ObjectType, quoted, Operations.Write);
        }
    }
}
