// <copyright file="DelegateAccessTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Xunit;

    public class DelegateAccessTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void DelegateAccessReturnsTokens()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Jane").WithLastName("Doe").WithUserName("jane@example.com").Build();
            new UserGroups(this.Session).Administrators.AddMember(administrator);
            var accessClass = new AccessClassBuilder(this.Session).Build();

            var acl = new AccessControlLists(administrator)[accessClass];
            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            this.Session.Commit();

            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));
        }

        [Fact]
        public void DelegateAccessReturnsNoTokens()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Jane").WithLastName("Doe").WithUserName("jane@example.com").Build();
            new UserGroups(this.Session).Administrators.AddMember(administrator);
            var accessClass = new AccessClassBuilder(this.Session).WithBlock(true).Build();

            // Use default security from Singleton
            var acl = new AccessControlLists(administrator)[accessClass];
            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            this.Session.Commit();

            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            this.Session.Commit();
        }
    }
}
