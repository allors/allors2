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

namespace Domain
{
    using System.Collections;
    using System.Collections.Generic;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class PathTests : DomainTest
    {
        [Test]
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

            var path = new Path(M.C1.C1C2One2Manies, M.C2.C2AllorsString);

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<PropertyType>())).Returns(true);
            var acls = new AccessControlListCache(null, (allorsObject, user) => aclMock.Object);

            var result = (ISet<object>)path.Get(c1a, acls);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains("c2A"));

            result = (ISet<object>)path.Get(c1b, acls);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("c2B"));
            Assert.IsTrue(result.Contains("c2C"));
        }

        [Test]
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

            var path = new Path(MetaC1.Instance.C1C2One2Manies, MetaC2.Instance.C2AllorsString);

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<PropertyType>())).Returns(true);
            var acls = new AccessControlListCache(null, (allorsObject, user) => aclMock.Object);

            var result = (ISet<object>)path.Get(c1a, acls);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains("c2A"));

            result = (ISet<object>)path.Get(c1b, acls);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("c2B"));
            Assert.IsTrue(result.Contains("c2C"));
        }

        [Test]
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

            Path path;
            Path.TryParse(M.C2.ObjectType, "C1WhereC1C2One2Many", out path);

            var aclMock = new Mock<IAccessControlList>();
            aclMock.Setup(acl => acl.CanRead(It.IsAny<PropertyType>())).Returns(true);
            var acls = new AccessControlListCache(null, (allorsObject, user) => aclMock.Object);

            var result = (C1)path.Get(c2A, acls);
            Assert.AreEqual(result, c1A);
            
            result = (C1)path.Get(c2B, acls);
            Assert.AreEqual(result, c1B);
        }
    }
}