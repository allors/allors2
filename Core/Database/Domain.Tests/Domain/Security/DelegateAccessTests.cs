// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateAccessTests.cs" Organisation="Allors bvba">
//   Copyright 2002-2011 Allors bvba.
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
            Assert.False(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            this.Session.Commit();

            Assert.False(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            this.Session.Commit();
        }
    }
}
