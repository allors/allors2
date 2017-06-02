// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleTests.cs" company="Allors bvba">
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

namespace Domain
{
    using Allors;
    using Allors.Meta;

    using global::Allors.Domain;

    using Xunit;

    
    public class RoleTests : DomainTest
    {
        [Fact]
        public void GivenNoRolesWhenCreatingARoleWithoutANameThenRoleIsInvalid()
        {
            new RoleBuilder(this.Session).Build();

            var validation = this.Session.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Equal(1, validation.Errors.Length);

            var derivationError = validation.Errors[0];

            Assert.Equal(1, derivationError.Relations.Length);
            Assert.Equal(typeof(DerivationErrorRequired), derivationError.GetType());
            Assert.Equal((RoleType)M.Role.Name, derivationError.Relations[0].RoleType);
        }

        [Fact]
        public void GivenARoleWhenCreatingARoleWithTheSameNameThenRoleIsInvalid()
        {
            new RoleBuilder(this.Session)
                .WithName("Same")
                .Build();

            new RoleBuilder(this.Session)
                .WithName("Same")
                .Build();

            var validation = this.Session.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Equal(2, validation.Errors.Length);

            foreach (var derivationError in validation.Errors)
            {
                Assert.Equal(1, derivationError.Relations.Length);
                Assert.Equal(typeof(DerivationErrorUnique), derivationError.GetType());
                Assert.Equal((RoleType)M.Role.Name, derivationError.Relations[0].RoleType);
            }
        }

        [Fact]
        public void GivenNoRolesWhenCreatingARoleWithoutAUniqueIdThenRoleIsValid()
        {
            var role = new RoleBuilder(this.Session)
                .WithName("Role")
                .Build();

            Assert.True(role.ExistUniqueId);

            var validation = this.Session.Derive(false);

            Assert.False(validation.HasErrors);
        }
    }
}
