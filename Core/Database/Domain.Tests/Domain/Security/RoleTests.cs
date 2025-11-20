// <copyright file="RoleTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Allors.Domain.Derivations.Errors;
    using Allors.Meta;
    using Xunit;

    public class RoleTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

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
