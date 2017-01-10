// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationTest.cs" company="Allors bvba">
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
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using NUnit.Framework;

    [TestFixture]
    public class DerivationTest : DomainTest
    {
        [Test]
        public void Next()
        {
            var first = new FirstBuilder(this.Session).Build();

            this.Session.Derive(true);

            Assert.IsTrue(first.ExistIsDerived);

            Assert.IsTrue(first.Second.ExistIsDerived);

            Assert.IsTrue(first.Second.Third.ExistIsDerived);
        }

        [Test]
        public void Dependency()
        {
            var dependent = new DependentBuilder(this.Session).Build();
            var dependee = new DependeeBuilder(this.Session).Build();

            dependent.Dependee = dependee;
            
            this.Session.Commit();

            dependee.Counter = 10;

            this.Session.Derive(true);

            Assert.AreEqual(11, dependent.Counter);
            Assert.AreEqual(11, dependee.Counter);
        }

        [Test]
        public void Subdependency()
        {
            var dependent = new DependentBuilder(this.Session).Build();
            var dependee = new DependeeBuilder(this.Session).Build();
            var subdependee = new SubdependeeBuilder(this.Session).Build();

            dependent.Dependee = dependee;
            dependee.Subdependee = subdependee;

            this.Session.Commit();

            subdependee.Subcounter = 10;

            this.Session.Derive(true);

            Assert.AreEqual(1, dependent.Counter);
            Assert.AreEqual(1, dependee.Counter);

            Assert.AreEqual(11, dependent.Subcounter);
            Assert.AreEqual(11, dependee.Subcounter);
            Assert.AreEqual(11, subdependee.Subcounter);
        }



        [Test]
        public void Deleted()
        {
            var dependent = new DependentBuilder(this.Session).Build();
            var dependee = new DependeeBuilder(this.Session).Build();

            dependent.Dependee = dependee;

            this.Session.Commit();

            dependee.DeleteDependent = true;

            this.Session.Derive(true);

            Assert.IsTrue(dependent.Strategy.IsDeleted);
            Assert.AreEqual(1, dependee.Counter);
        }
    }
}