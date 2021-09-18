// <copyright file="AccessControlListTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System;
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Xunit;

    public class WorkspaceAccessControlListsTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenAWorkspaceAccessControlListsThenADatabaseDeniedPermissionsIsNotPresent()
        {
            var administrator = new PersonBuilder(this.Session).WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            var databaseOnlyPermissions = new Permissions(this.Session).Extent().Where(v => v.OperandType.Equals(M.Person.DatabaseOnlyField));
            var databaseOnlyReadPermission = databaseOnlyPermissions.First(v => v.Operation == Operations.Read);

            var restriction = new RestrictionBuilder(this.Session).WithUniqueId(Guid.NewGuid())
                .WithDeniedPermission(databaseOnlyReadPermission).Build();

            ((PersonDerivedRoles)administrator).AddRestriction(restriction);

            this.Session.Derive();
            this.Session.Commit();

            var workspaceAccessControlLists = new WorkspaceAccessControlLists(administrator);
            var acl = workspaceAccessControlLists[administrator];

            Assert.DoesNotContain(restriction, acl.Restrictions);
        }

        [Fact]
        public void GivenAWorkspaceAccessControlListsThenAWorkspaceDeniedPermissionsIsNotPresent()
        {
            var administrator = new PersonBuilder(this.Session).WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            var workspacePermissions = new Permissions(this.Session).Extent().Where(v => v.OperandType.Equals(M.Person.WorkspaceField));
            var workspaceReadPermission = workspacePermissions.First(v => v.Operation == Operations.Read);

            var restriction = new RestrictionBuilder(this.Session).WithUniqueId(Guid.NewGuid())
                .WithDeniedPermission(workspaceReadPermission).Build();

            this.Session.Derive();
            this.Session.Commit();

            ((PersonDerivedRoles)administrator).AddRestriction(restriction);

            var workspaceAccessControlLists = new WorkspaceAccessControlLists(administrator);
            var acl = workspaceAccessControlLists[administrator];

            Assert.Contains(restriction, acl.Restrictions);
        }

        private Permission FindPermission(ObjectType objectType, RoleType roleType, Operations operation)
        {
            var permissions = this.Session.Extent<Permission>();
            permissions.Filter.AddEquals(M.Permission.ConcreteClassPointer, objectType.Id);
            permissions.Filter.AddEquals(M.Permission.OperandTypePointer, roleType.Id);
            permissions.Filter.AddEquals(M.Permission.OperationEnum, operation);
            return permissions.First;
        }

        private Permission FindPermission(RoleType roleType, Operations operation) => this.FindPermission(roleType.AssociationType.ObjectType, roleType, operation);
    }
}
