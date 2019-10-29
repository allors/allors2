// <copyright file="DelegateAccessTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System.Linq;
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
            var administrator = new PersonBuilder(this.Session).WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);
            var accessClass = new AccessClassBuilder(this.Session).Build();

            this.Session.Derive();
            this.Session.Commit();

            var defaultSecurityToken = this.Session.GetSingleton().DefaultSecurityToken;
            var dstAcs = defaultSecurityToken.AccessControls.Where(v => v.EffectiveUsers.Contains(administrator));
            var dstAcs2 = defaultSecurityToken.AccessControls.Where(v => v.SubjectGroups.Contains(administrators));

            var acs = new AccessControls(this.Session).Extent().Where(v => v.EffectiveUsers.Contains(administrator));
            var acs2 = new AccessControls(this.Session).Extent().Where(v => v.SubjectGroups.Contains(administrators));

            var acl = new AccessControlLists(administrator)[accessClass];
            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));
        }

        [Fact]
        public void DelegateAccessReturnsNoTokens()
        {
            var administrator = new PersonBuilder(this.Session).WithUserName("administrator").Build();
            new UserGroups(this.Session).Administrators.AddMember(administrator);
            var accessClass = new AccessClassBuilder(this.Session).WithBlock(true).Build();

            accessClass.Block = true;

            this.Session.Derive();
            this.Session.Commit();

            // Use default security from Singleton
            var acl = new AccessControlLists(administrator)[accessClass];
            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));
        }
    }
}
