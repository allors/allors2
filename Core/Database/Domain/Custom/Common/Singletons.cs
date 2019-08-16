// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Singletons.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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
