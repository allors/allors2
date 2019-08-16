// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathTests.cs" company="Allors bvba">
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

namespace Tests
{
    using System.Collections.Generic;

    using Allors;
    using Allors.Data;
    using Allors.Domain;
    using Allors.Meta;

    using Moq;

    using Xunit;

    public class PathTests : DomainTest
    {
        [Fact]
        public void One2ManyWithPropertyTypes()
        {
            var c2A = new C2Builder(this.Session).WithC2AllorsString("c2A").Build();
            var c2B = new C2Builder(this.Session).WithC2AllorsString("c2B").Build();
            var c2C = new C2Builder(this.Session).WithC2AllorsString("c2C").Build();

            var c1a = new C1Builder(this.Session)
                .WithC1AllorsString("c1A")
                .WithC1C2One2Many(c2A)
                .Build();

            var c1b = new C1Builder(this.Session)
                .WithC1AllorsString("c1B")
                .WithC1C2One2Many(c2B)
                .WithC1C2One2Many(c2C)
                .Build();

            this.Session.Derive(true);

            var path = new Fetch(M.C1.C1C2One2Manies, M.C2.C2AllorsString);

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<IPropertyType>())).Returns(true);
            var acls = new AccessControlListFactory(null, (allorsObject, user) => aclMock.Object);

            var result = (ISet<object>)path.Get(c1a, acls);
            Assert.Equal(1, result.Count);
            Assert.True(result.Contains("c2A"));

            result = (ISet<object>)path.Get(c1b, acls);
            Assert.Equal(2, result.Count);
            Assert.True(result.Contains("c2B"));
            Assert.True(result.Contains("c2C"));
        }

        [Fact]
        public void One2ManyWithPropertyTypeIds()
        {
            var c2A = new C2Builder(this.Session).WithC2AllorsString("c2A").Build();
            var c2B = new C2Builder(this.Session).WithC2AllorsString("c2B").Build();
            var c2C = new C2Builder(this.Session).WithC2AllorsString("c2C").Build();

            var c1a = new C1Builder(this.Session)
                .WithC1AllorsString("c1A")
                .WithC1C2One2Many(c2A)
                .Build();

            var c1b = new C1Builder(this.Session)
                .WithC1AllorsString("c1B")
                .WithC1C2One2Many(c2B)
                .WithC1C2One2Many(c2C)
                .Build();

            this.Session.Derive(true);

            var path = new Fetch(MetaC1.Instance.C1C2One2Manies, MetaC2.Instance.C2AllorsString);

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<IPropertyType>())).Returns(true);
            var acls = new AccessControlListFactory(null, (allorsObject, user) => aclMock.Object);

            var result = (ISet<object>)path.Get(c1a, acls);
            Assert.Equal(1, result.Count);
            Assert.True(result.Contains("c2A"));

            result = (ISet<object>)path.Get(c1b, acls);
            Assert.Equal(2, result.Count);
            Assert.True(result.Contains("c2B"));
            Assert.True(result.Contains("c2C"));
        }

        [Fact]
        public void One2ManyWithPropertyNames()
        {
            var c2A = new C2Builder(this.Session).WithC2AllorsString("c2A").Build();
            var c2B = new C2Builder(this.Session).WithC2AllorsString("c2B").Build();
            var c2C = new C2Builder(this.Session).WithC2AllorsString("c2C").Build();

            var c1A = new C1Builder(this.Session)
                .WithC1AllorsString("c1A")
                .WithC1C2One2Many(c2A)
                .Build();

            var c1B = new C1Builder(this.Session)
                .WithC1AllorsString("c1B")
                .WithC1C2One2Many(c2B)
                .WithC1C2One2Many(c2C)
                .Build();

            this.Session.Derive(true);

            Fetch fetch;
            Fetch.TryParse(M.C2.ObjectType, "C1WhereC1C2One2Many", out fetch);

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<IPropertyType>())).Returns(true);
            var acls = new AccessControlListFactory(null, (allorsObject, user) => aclMock.Object);

            var result = (C1)fetch.Get(c2A, acls);
            Assert.Equal(result, c1A);

            result = (C1)fetch.Get(c2B, acls);
            Assert.Equal(result, c1B);
        }
    }
}
