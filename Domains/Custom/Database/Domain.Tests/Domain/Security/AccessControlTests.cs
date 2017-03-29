//------------------------------------------------------------------------------------------------- 
// <copyright file="AccessControlTests.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
// <summary>Defines the AccessControlTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Domain
{
    using System.Collections;

    using Allors;
    using Allors.Meta;

    using global::Allors.Domain;

    using Xunit;

    
    public class AccessControlTests : DomainTest
    {
        [Fact]
        public void GivenNoAccessControlWhenCreatingAAccessControlWithoutARoleThenAccessControlIsInvalid()
        {
            var userGroup = new UserGroupBuilder(this.Session).WithName("UserGroup").Build();
            var securityToken = new SecurityTokenBuilder(this.Session).Build();

            securityToken.AddAccessControl(new AccessControlBuilder(this.Session)
                .WithSubjectGroup(userGroup)
                .Build());

            var validation = this.Session.Derive();

            Assert.True(validation.HasErrors);
            Assert.Equal(1, validation.Errors.Length);

            var derivationError = validation.Errors[0];

            Assert.Equal(1, derivationError.Relations.Length);
            Assert.Equal(typeof(DerivationErrorRequired), derivationError.GetType());
            Assert.Equal(M.AccessControl.Role, derivationError.Relations[0].RoleType);
        }

        [Fact]
        public void GivenNoAccessControlWhenCreatingAAccessControlWithoutAUserOrUserGroupThenAccessControlIsInvalid()
        {
            var securityToken = new SecurityTokenBuilder(this.Session).Build();
            var role = new RoleBuilder(this.Session).WithName("Role").Build();

            securityToken.AddAccessControl(
            new AccessControlBuilder(this.Session)
                .WithRole(role)
                .Build());

            var validation = this.Session.Derive();

            Assert.True(validation.HasErrors);
            Assert.Equal(1, validation.Errors.Length);

            var derivationError = validation.Errors[0];

            Assert.Equal(2, derivationError.Relations.Length);
            Assert.Equal(typeof(DerivationErrorAtLeastOne), derivationError.GetType());
            Assert.True(new ArrayList(derivationError.RoleTypes).Contains((RoleType)M.AccessControl.Subjects));
            Assert.True(new ArrayList(derivationError.RoleTypes).Contains((RoleType)M.AccessControl.SubjectGroups));
        }
    }
}
