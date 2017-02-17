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

    using NUnit.Framework;

    [TestFixture]
    public class RoleTests : DomainTest
    {
        [Test]
        public void GivenNoRolesWhenCreatingARoleWithoutANameThenRoleIsInvalid()
        {
            new RoleBuilder(this.Session).Build();

            var validation = this.Session.Derive();

            Assert.IsTrue(validation.HasErrors);
            Assert.AreEqual(1, validation.Errors.Length);

            var derivationError = validation.Errors[0];

            Assert.AreEqual(1, derivationError.Relations.Length);
            Assert.AreEqual(typeof(DerivationErrorRequired), derivationError.GetType());
            Assert.AreEqual((RoleType)M.Role.Name, derivationError.Relations[0].RoleType);
        }

        [Test]
        public void GivenARoleWhenCreatingARoleWithTheSameNameThenRoleIsInvalid()
        {
            new RoleBuilder(this.Session)
                .WithName("Same")
                .Build();

            new RoleBuilder(this.Session)
                .WithName("Same")
                .Build();

            var validation = this.Session.Derive();

            Assert.IsTrue(validation.HasErrors);
            Assert.AreEqual(2, validation.Errors.Length);

            foreach (var derivationError in validation.Errors)
            {
                Assert.AreEqual(1, derivationError.Relations.Length);
                Assert.AreEqual(typeof(DerivationErrorUnique), derivationError.GetType());
                Assert.AreEqual((RoleType)M.Role.Name, derivationError.Relations[0].RoleType);
            }
        }

        [Test]
        public void GivenNoRolesWhenCreatingARoleWithoutAUniqueIdThenRoleIsValid()
        {
            var role = new RoleBuilder(this.Session)
                .WithName("Role")
                .Build();

            Assert.IsTrue(role.ExistUniqueId);

            var validation = this.Session.Derive();

            Assert.IsFalse(validation.HasErrors);
        }
    }
}
