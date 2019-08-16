// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateAccessTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Allors;

namespace Tests
{
    using Allors.Domain;
    using Allors.Meta;
    using Xunit;

    public class DelegateAccessTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void DelegateAccessReturnsTokens()
        {
            var administrator = new People(this.Session).FindBy(M.Person.UserName, "Administrator");
            var accessClass = new AccessClassBuilder(this.Session).Build();

            var acl = new AccessControlList(accessClass, administrator);
            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            this.Session.Commit();

            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));
        }

        [Fact]
        public void DelegateAccessReturnsNoTokens()
        {
            var administrator = new People(this.Session).FindBy(M.Person.UserName, "Administrator");
            var accessClass = new AccessClassBuilder(this.Session).WithBlock(true).Build();

            // Use default security from Singleton
            var acl = new AccessControlList(accessClass, administrator);
            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            this.Session.Commit();

            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            this.Session.Commit();
        }
    }
}
