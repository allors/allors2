// <copyright file="PurchaseOrders.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using Allors.Meta;

    public partial class PurchaseOrders
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.PurchaseOrderState);

        protected override void BaseSecure(Security config)
        {
            var states = new PurchaseOrderStates(this.Session);
            var created = states.Created;
            var onHold = states.OnHold;
            var cancelled = states.Cancelled;
            var rejected = states.Rejected;
            var awaitingApprovalLevel1 = states.AwaitingApprovalLevel1;
            var awaitingApprovalLevel2 = states.AwaitingApprovalLevel2;
            var inProcess = states.InProcess;
            var sent = states.Sent;
            var completed = states.Completed;
            var finished = states.Finished;

            var approve = this.Meta.Approve;
            var reject = this.Meta.Reject;
            var hold = this.Meta.Hold;
            var @continue = this.Meta.Continue;
            var setReadyForProcessing = this.Meta.SetReadyForProcessing;
            var cancel = this.Meta.Cancel;
            var reopen = this.Meta.Reopen;
            var send = this.Meta.Send;
            var revise = this.Meta.Revise;
            var quickReceive = this.Meta.QuickReceive;
            var invoice = this.Meta.Invoice;

            config.Deny(this.ObjectType, created, approve, cancel, reject, @continue, reopen, send, quickReceive, invoice, revise);
            config.Deny(this.ObjectType, onHold, approve, hold, setReadyForProcessing, reopen, send, quickReceive, invoice, revise);
            config.Deny(this.ObjectType, cancelled, approve, reject, hold, @continue, setReadyForProcessing, cancel, send, quickReceive, invoice, revise);
            config.Deny(this.ObjectType, rejected, approve, reject, hold, @continue, setReadyForProcessing, cancel, send, quickReceive, invoice, revise);
            config.Deny(this.ObjectType, awaitingApprovalLevel1, hold, @continue, setReadyForProcessing, cancel, reopen, send, quickReceive, @continue, revise);
            config.Deny(this.ObjectType, awaitingApprovalLevel2, hold, @continue, setReadyForProcessing, cancel, reopen, send, quickReceive, @continue, revise);
            config.Deny(this.ObjectType, inProcess, approve, reject, @continue, setReadyForProcessing, quickReceive);
            config.Deny(this.ObjectType, inProcess, approve, reject, hold, @continue, setReadyForProcessing, reopen);
            config.Deny(this.ObjectType, sent, approve, reject, hold, @continue, setReadyForProcessing, reopen, send);
            config.Deny(this.ObjectType, completed, approve, reject, hold, @continue, setReadyForProcessing, cancel, reopen, send, quickReceive);

            var except = new HashSet<IOperandType>
            {
                this.Meta.ElectronicDocuments.RoleType,
                this.Meta.Print,
            };

            config.DenyExcept(this.ObjectType, inProcess, except, Operations.Write);
            config.DenyExcept(this.ObjectType, cancelled, except, Operations.Write);
            config.DenyExcept(this.ObjectType, rejected, except, Operations.Write, Operations.Execute);
            config.DenyExcept(this.ObjectType, completed, except, Operations.Write);
            config.DenyExcept(this.ObjectType, finished, except, Operations.Execute, Operations.Write);
        }
    }
}
