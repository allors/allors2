// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControlListTests.cs" company="Allors bvba">
// Copyright 2002-2019 Allors bvba.
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

namespace Tests
{
    using Allors;
    using Allors.Meta;

    using global::Allors.Domain;

    using Xunit;

    
    public class AccessControlListTests : DomainTest
    {
        [Fact]
        public void GivenAnAuthenticationPopulatonWhenCreatingAnAccessListForGuestThenPermissionIsDenied()
        {
            var guest = new PersonBuilder(this.Session).WithUserName("guest").WithLastName("Guest").Build();
            var administrator = new PersonBuilder(this.Session).WithUserName("admin").WithLastName("Administrator").Build();
            var user = new PersonBuilder(this.Session).WithUserName("user").WithLastName("User").Build();

            this.Session.GetSingleton().Guest = guest;
            new UserGroups(this.Session).FindBy(M.UserGroup.Name, "Administrators").AddMember(administrator);

            this.Session.Derive(true);
            this.Session.Commit();

            var sessions = new ISession[] { this.Session };
            foreach (var session in sessions)
            {
                session.Commit();

                foreach (AccessControlledObject aco in (IObject[])session.Extent(M.Organisation.ObjectType))
                {
                    // When
                    var accessList = new AccessControlList(aco, guest);

                    // Then
                    Assert.False(accessList.CanExecute(M.Organisation.JustDoIt));
                }

                session.Rollback();
            }
        }

        [Fact]
        public void GivenAUserAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var permission = this.FindPermission(M.Organisation.Name, Operations.Read);
            var role = new RoleBuilder(this.Session).WithName("Role").WithPermission(permission).Build();
            var person = new PersonBuilder(this.Session).WithFirstName("John").WithLastName("Doe").Build();
            new AccessControlBuilder(this.Session).WithSubject(person).WithRole(role).Build();

            this.Session.Derive(true);
            this.Session.Commit();

            var sessions = new ISession[] { this.Session };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var accessControl = (AccessControl)session.Instantiate(role.AccessControlsWhereRole.First);
                token.AddAccessControl(accessControl);

                this.Session.Derive(true);

                Assert.False(this.Session.Derive(false).HasErrors);

                var accessList = new AccessControlList(organisation, person);

                Assert.True(accessList.CanRead(M.Organisation.Name));

                session.Rollback();
            }
        }

        [Fact]
        public void GivenAUserGroupAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var permission = this.FindPermission(M.Organisation.Name, Operations.Read);
            var role = new RoleBuilder(this.Session).WithName("Role").WithPermission(permission).Build();
            var person = new PersonBuilder(this.Session).WithFirstName("John").WithLastName("Doe").Build();
            new UserGroupBuilder(this.Session).WithName("Group").WithMember(person).Build();

            new AccessControlBuilder(this.Session).WithSubject(person).WithRole(role).Build();

            this.Session.Derive(true);
            this.Session.Commit();

            var sessions = new ISession[] { this.Session };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var accessControl = (AccessControl)session.Instantiate(role.AccessControlsWhereRole.First);
                token.AddAccessControl(accessControl);

                Assert.False(this.Session.Derive(false).HasErrors);

                var accessList = new AccessControlList(organisation, person);

                Assert.True(accessList.CanRead(M.Organisation.Name));

                session.Rollback();
            }
        }

        [Fact]
        public void GivenAnotherUserAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var readOrganisationName = this.FindPermission(M.Organisation.Name, Operations.Read);
            var databaseRole = new RoleBuilder(this.Session).WithName("Role").WithPermission(readOrganisationName).Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            var person = new PersonBuilder(this.Session).WithFirstName("John").WithLastName("Doe").Build();
            var anotherPerson = new PersonBuilder(this.Session).WithFirstName("Jane").WithLastName("Doe").Build();

            this.Session.Derive(true);
            this.Session.Commit();

            new AccessControlBuilder(this.Session).WithSubject(anotherPerson).WithRole(databaseRole).Build();
            this.Session.Commit();

            var sessions = new ISession[] { this.Session };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var role = (Role)session.Instantiate(new Roles(this.Session).FindBy(M.Role.Name, "Role"));
                var accessControl = (AccessControl)session.Instantiate(role.AccessControlsWhereRole.First);
                token.AddAccessControl(accessControl);

                Assert.False(this.Session.Derive(false).HasErrors);

                var accessList = new AccessControlList(organisation, person);

                Assert.False(accessList.CanRead(M.Organisation.Name));

                session.Rollback();
            }
        }

        [Fact]
        public void GivenAnotherUserGroupAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var readOrganisationName = this.FindPermission(M.Organisation.Name, Operations.Read);
            var databaseRole = new RoleBuilder(this.Session).WithName("Role").WithPermission(readOrganisationName).Build();

            var person = new PersonBuilder(this.Session).WithFirstName("John").WithLastName("Doe").Build();
            new UserGroupBuilder(this.Session).WithName("Group").WithMember(person).Build();
            var anotherUserGroup = new UserGroupBuilder(this.Session).WithName("AnotherGroup").Build();

            this.Session.Derive(true);
            this.Session.Commit();

            new AccessControlBuilder(this.Session).WithSubjectGroup(anotherUserGroup).WithRole(databaseRole).Build();

            this.Session.Commit();

            var sessions = new ISession[] { this.Session };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var role = (Role)session.Instantiate(new Roles(this.Session).FindBy(M.Role.Name, "Role"));
                var accessControl = (AccessControl)session.Instantiate(role.AccessControlsWhereRole.First);
                token.AddAccessControl(accessControl);

                Assert.False(this.Session.Derive(false).HasErrors);

                var accessList = new AccessControlList(organisation, person);

                Assert.False(accessList.CanRead(M.Organisation.Name));

                session.Rollback();
            }
        }
        
        [Fact]
        public void DeniedPermissions()
        {
            var readOrganisationName = this.FindPermission(M.Organisation.Name, Operations.Read);
            var databaseRole = new RoleBuilder(this.Session).WithName("Role").WithPermission(readOrganisationName).Build();
            var person = new PersonBuilder(this.Session).WithFirstName("John").WithLastName("Doe").Build();
            new AccessControlBuilder(this.Session).WithRole(databaseRole).WithSubject(person).Build();

            this.Session.Derive(true);
            this.Session.Commit();

            var sessions = new ISession[] { this.Session };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var role = (Role)session.Instantiate(new Roles(this.Session).FindBy(M.Role.Name, "Role"));
                var accessControl = (AccessControl)session.Instantiate(role.AccessControlsWhereRole.First);
                token.AddAccessControl(accessControl);

                Assert.False(this.Session.Derive(false).HasErrors);

                var accessList = new AccessControlList(organisation, person);

                Assert.True(accessList.CanRead(M.Organisation.Name));

                organisation.AddDeniedPermission(readOrganisationName);

                accessList = new AccessControlList(organisation, person);

                Assert.False(accessList.CanRead(M.Organisation.Name));

                session.Rollback();
            }
        }

        private Permission FindPermission(ObjectType objectType, RoleType roleType, Operations operation)
        {
            Extent<Permission> permissions = this.Session.Extent<Permission>();
            permissions.Filter.AddEquals(M.Permission.ConcreteClassPointer, objectType.Id);
            permissions.Filter.AddEquals(M.Permission.OperandTypePointer, roleType.Id);
            permissions.Filter.AddEquals(M.Permission.OperationEnum, operation);
            return permissions.First;
        }

        private Permission FindPermission(RoleType roleType, Operations operation)
        {
            return this.FindPermission(roleType.AssociationType.ObjectType, roleType, operation);
        }
    }
}
