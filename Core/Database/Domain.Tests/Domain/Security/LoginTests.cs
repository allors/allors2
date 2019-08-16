
// <copyright file="LoginTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Xunit;

    public class LoginTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void WhenDeletingUserThenLoginShouldAlsoBeDeleted()
        {
            var user = new PersonBuilder(this.Session).WithUserName("User").WithLastName("User").Build();
            var login = new LoginBuilder(this.Session).WithUser(user).WithProvider("MyProvider").WithKey("XXXYYYZZZ").Build();

            this.Session.Derive();

            user.Delete();

            this.Session.Derive();

            Assert.True(login.Strategy.IsDeleted);
        }
    }
}
