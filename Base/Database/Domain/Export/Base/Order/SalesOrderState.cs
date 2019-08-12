// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderStates.cs" company="Allors bvba">
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