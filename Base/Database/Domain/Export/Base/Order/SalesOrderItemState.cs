// <copyright file="SalesOrderItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesOrderItemState
    {
        public bool Created => Equals(this.UniqueId, SalesOrderItemStates.CreatedId);

        public bool Cancelled => Equals(this.UniqueId, SalesOrderItemStates.CancelledId);

        public bool Completed => Equals(this.UniqueId, SalesOrderItemStates.CompletedId);

        public bool Rejected => Equals(this.UniqueId, SalesOrderItemStates.RejectedId);

        public bool Finished => Equals(this.UniqueId, SalesOrderItemStates.FinishedId);

        public bool OnHold => Equals(this.UniqueId, SalesOrderItemStates.OnHoldId);

        public bool InProcess => Equals(this.UniqueId, SalesOrderItemStates.InProcessId);
    }
}
