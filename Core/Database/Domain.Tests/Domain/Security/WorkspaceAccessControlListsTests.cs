// <copyright file="AccessControlListTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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

            this.Session.Derive();
            this.Session.Commit();

            var databaseOnlyPermissions = new Permissions(this.Session).Extent().Where(v => v.OperandType.Equals(M.Person.DatabaseOnlyField));
            var databaseOnlyReadPermission = databaseOnlyPermissions.First(v => v.Operation == Operations.Read);

            administrator.AddDeniedPermission(databaseOnlyReadPermission);

            var workspaceAccessControlLists = new WorkspaceAccessControlLists(administrator);
            var acl = workspaceAccessControlLists[administrator];

            var deniedWorkspacePermissions = acl.DeniedPermissionIds;

            Assert.DoesNotContain(databaseOnlyReadPermission.Id, deniedWorkspacePermissions);
        }

        [Fact]
        public void GivenAWorkspaceAccessControlListsThenAWorkspaceDeniedPermissionsIsNotPresent()
        {
            var administrator = new PersonBuilder(this.Session).WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            var workspacePermissions = new Permissions(this.Session).Extent().Where(v => v.OperandType.Equals(M.Person.WorkspaceField));
            var workspaceReadPermission = workspacePermissions.First(v => v.Operation == Operations.Read);

            administrator.AddDeniedPermission(workspaceReadPermission);

            var workspaceAccessControlLists = new WorkspaceAccessControlLists(administrator);
            var acl = workspaceAccessControlLists[administrator];

            var deniedWorkspacePermissions = acl.DeniedPermissionIds;

            Assert.Contains(workspaceReadPermission.Id, deniedWorkspacePermissions);
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
