// <copyright file="Security.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using Allors.Meta;

    public partial class Security
    {
        public void GrantBlueCollarWorker(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.BlueCollarWorkerId, objectType, operations);

        public void GrantExceptBlueCollarWorker(ObjectType objectType, ICollection<OperandType> excepts, params Operations[] operations) => this.GrantExcept(Roles.BlueCollarWorkerId, objectType, excepts, operations);

        public void GrantExceptEmployee(ObjectType objectType, ICollection<OperandType> excepts, params Operations[] operations) => this.GrantExcept(Roles.EmployeeId, objectType, excepts, operations);

        public void GrantProductQuoteApprover(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.ProductQuoteApproverId, objectType, operations);

        public void GrantPurchaseOrderApproverLevel1(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.PurchaseOrderApproverLevel1Id, objectType, operations);

        public void GrantPurchaseOrderApproverLevel2(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.PurchaseOrderApproverLevel2Id, objectType, operations);

        public void GrantLocalAdministrator(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.LocalAdministratorId, objectType, operations);

        public void GrantSalesAccountManager(ObjectType objectType, params Operations[] operations) => this.Grant(Roles.SalesAccountManagerId, objectType, operations);

        private void BaseOnPreSetup()
        {
        }

        private void BaseOnPostSetup()
        {
        }
    }
}
