// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserGroupTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using Allors;
    using Allors.Meta;
    using global::Allors.Domain;
    using Xunit;

    public class UserGroupTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenNoUserGroupWhenCreatingAUserGroupWithoutANameThenUserGroupIsInvalid()
        {
            new UserGroupBuilder(this.Session).Build();

            var validation = this.Session.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Equal(1, validation.Errors.Length);

            var derivationError = validation.Errors[0];

            Assert.Equal(1, derivationError.Relations.Length);
            Assert.Equal(typeof(DerivationErrorRequired), derivationError.GetType());
            Assert.Equal((RoleType)M.UserGroup.Name, derivationError.Relations[0].RoleType);
        }

        [Fact]
        public void GivenAUserGroupWhenCreatingAUserGroupWithTheSameNameThenUserGroupIsInvalid()
        {
            new UserGroupBuilder(this.Session).WithName("Same").Build();
            new UserGroupBuilder(this.Session).WithName("Same").Build();

            var validation = this.Session.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Equal(2, validation.Errors.Length);

            foreach (var derivationError in validation.Errors)
            {
                Assert.Equal(1, derivationError.Relations.Length);
                Assert.Equal(typeof(DerivationErrorUnique), derivationError.GetType());
                Assert.Equal((RoleType)M.UserGroup.Name, derivationError.Relations[0].RoleType);
            }
        }
    }
}
