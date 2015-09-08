// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrders.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class PurchaseOrders
    {
        public static readonly Guid PurchaseOrderTemplateEnId = new Guid("3A442D2F-4D8D-46EC-983F-BD003F8ABB3D");
        public static readonly Guid PurchaseOrderTemplateNlId = new Guid("F8D92C09-A45C-4618-9320-B398FFE979E0");

        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, TemplatePurposes.Meta.ObjectType);
            setup.AddDependency(this.ObjectType, PurchaseOrderObjectStates.Meta.ObjectType);
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new StringTemplateBuilder(Session)
                .WithName("PurchaseOrder " + englishLocale.Name)
                .WithBody(PurchaseOrderTemplateEn)
                .WithUniqueId(PurchaseOrderTemplateEnId)
                .WithLocale(englishLocale)
                .WithTemplatePurpose(new TemplatePurposes(this.Session).PurchaseOrder)
                .Build();

            new StringTemplateBuilder(Session)
                .WithName("PurchaseOrder " + dutchLocale.Name)
                .WithBody(PurchaseOrderTemplateNl)
                .WithUniqueId(PurchaseOrderTemplateNlId)
                .WithLocale(dutchLocale)
                .WithTemplatePurpose(new TemplatePurposes(this.Session).PurchaseOrder)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);

            config.GrantProcurement(this.ObjectType, full);
            config.GrantOperations(this.ObjectType, full);

            config.GrantSupplier(this.ObjectType, Meta.OrderNumber, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.OrderDate, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.CurrentOrderStatus, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.CurrentPaymentStatus, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.CurrentShipmentStatus, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.OrderStatuses, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.PaymentStatuses, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.ShipmentStatuses, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalBasePrice, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalDiscount, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalSurcharge, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalExVat, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalVat, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.TotalIncVat, Operation.Read);

            config.GrantProcurement(this.ObjectType, Meta.OrderNumber, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.OrderDate, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.CurrentOrderStatus, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.CurrentPaymentStatus, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.CurrentShipmentStatus, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.OrderStatuses, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.PaymentStatuses, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.ShipmentStatuses, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalBasePrice, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalDiscount, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalSurcharge, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalExVat, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalVat, Operation.Read);
            config.GrantProcurement(this.ObjectType, Meta.TotalIncVat, Operation.Read);

            var created = new PurchaseOrderObjectStates(Session).Provisional;
            var onHold = new PurchaseOrderObjectStates(Session).OnHold;
            var requestsApproval = new PurchaseOrderObjectStates(Session).RequestsApproval;
            var inProcess = new PurchaseOrderObjectStates(Session).InProcess;
            var cancelled = new PurchaseOrderObjectStates(Session).Cancelled;
            var rejected = new PurchaseOrderObjectStates(Session).Rejected;
            var completed = new PurchaseOrderObjectStates(Session).Completed;
            var finished = new PurchaseOrderObjectStates(Session).Finished;

            var approve = Meta.Approve;
            var reject = Meta.Reject;
            var hold = Meta.Hold;
            var @continue = Meta.Continue;
            var confirm = Meta.Confirm;

            config.Deny(this.ObjectType, created, reject, approve, hold, @continue);
            config.Deny(this.ObjectType, requestsApproval, confirm, reject, approve, @continue);
            config.Deny(this.ObjectType, inProcess, confirm, reject, approve, @continue);
            config.Deny(this.ObjectType, onHold, confirm, reject, approve, hold);

            config.Deny(this.ObjectType, cancelled, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, rejected, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, completed, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, finished, Operation.Execute, Operation.Write);
        }
    }
}