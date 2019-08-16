
// <copyright file="SalesOrderState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesOrderState
    {
        public bool Provisional => Equals(this.UniqueId, SalesOrderStates.ProvisionalId);

        public bool RequestsApproval => Equals(this.UniqueId, SalesOrderStates.RequestsApprovalId);

        public bool Cancelled => Equals(this.UniqueId, SalesOrderStates.CancelledId);

        public bool Completed => Equals(this.UniqueId, SalesOrderStates.CompletedId);

        public bool Rejected => Equals(this.UniqueId, SalesOrderStates.RejectedId);

        public bool Finished => Equals(this.UniqueId, SalesOrderStates.FinishedId);

        public bool OnHold => Equals(this.UniqueId, SalesOrderStates.OnHoldId);

        public bool InProcess => Equals(this.UniqueId, SalesOrderStates.InProcessId);
    }
}
