// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControls.cs" company="Allors bvba">
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
    public partial class AccessControls
    {
        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var defaultSecurityToken = Domain.Singleton.Instance(this.Session).DefaultSecurityToken;

            new AccessControlBuilder(this.Session)
                .WithRole(new Roles(this.Session).Customer)
                .WithSubjectGroup(new UserGroups(this.Session).Customers)
                .WithObject(defaultSecurityToken)
                .Build();

            new AccessControlBuilder(this.Session)
                .WithRole(new Roles(this.Session).Supplier)
                .WithSubjectGroup(new UserGroups(this.Session).Suppliers)
                .WithObject(defaultSecurityToken)
                .Build();

            new AccessControlBuilder(this.Session)
                .WithRole(new Roles(this.Session).Partner)
                .WithSubjectGroup(new UserGroups(this.Session).Partners)
                .WithObject(defaultSecurityToken)
                .Build();

            new AccessControlBuilder(this.Session)
                .WithRole(new Roles(this.Session).Sales)
                .WithSubjectGroup(new UserGroups(this.Session).Sales)
                .WithObject(defaultSecurityToken)
                .Build();

            new AccessControlBuilder(this.Session)
                .WithRole(new Roles(this.Session).Operations)
                .WithSubjectGroup(new UserGroups(this.Session).Operations)
                .WithObject(defaultSecurityToken)
                .Build();

            new AccessControlBuilder(this.Session)
                .WithRole(new Roles(this.Session).Procurement)
                .WithSubjectGroup(new UserGroups(this.Session).Procurement)
                .WithObject(defaultSecurityToken)
                .Build();
        }
    }
}