// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class Singletons
    {
        protected override void CustomSecure(Security config)
        {
            var defaultSecurityToken = this.Instance.DefaultSecurityToken;

            if (!this.Instance.ExistSalesAccessControl)
            {
                this.Instance.SalesAccessControl = new AccessControlBuilder(this.Session)
                .WithRole(new Roles(this.Session).Sales)
                .WithSubjectGroup(new UserGroups(this.Session).Sales)
                .Build();

                defaultSecurityToken.AddAccessControl(this.Instance.SalesAccessControl);
            }

            if (!this.Instance.ExistOperationsAccessControl)
            {
                this.Instance.OperationsAccessControl = new AccessControlBuilder(this.Session)
                .WithRole(new Roles(this.Session).Operations)
                .WithSubjectGroup(new UserGroups(this.Session).Operations)
                .Build();

                defaultSecurityToken.AddAccessControl(this.Instance.OperationsAccessControl);
            }

            if (!this.Instance.ExistProcurementAccessControl)
            {
                this.Instance.ProcurementAccessControl = new AccessControlBuilder(this.Session)
                .WithRole(new Roles(this.Session).Procurement)
                .WithSubjectGroup(new UserGroups(this.Session).Procurement)
                .Build();

                defaultSecurityToken.AddAccessControl(this.Instance.ProcurementAccessControl);
            }
        }
    }
}
