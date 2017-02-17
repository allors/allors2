// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationGraphTest.cs" company="Allors bvba">
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
    using System.Collections.Generic;

    using Allors;
    using Allors.Domain;
    using Allors.Domain.NonLogging;

    using NUnit.Framework;

    [TestFixture]
    public class DerivationGraphTest : DomainTest
    {
        [Test]
        public void Sort()
        {
            var x = new C1Builder(this.Session).Build();
            var y = new C1Builder(this.Session).Build();
            var z = new C1Builder(this.Session).Build();

            x.AddDependency(y);
            y.AddDependency(z);

            var derivation = new Derivation(this.Session);

            var sequence = new List<IObject>();
            derivation["sequence"] = sequence;

            derivation.Derive();

            Assert.AreEqual(z, sequence[0]);
            Assert.AreEqual(y, sequence[1]);
            Assert.AreEqual(x, sequence[2]);
        }

        [Test]
        public void SortDiamond()
        {
            var a = new C1Builder(this.Session).Build();
            var b = new C1Builder(this.Session).Build();
            var c = new C1Builder(this.Session).Build();
            var d = new C1Builder(this.Session).Build();

            a.AddDependency(b);
            a.AddDependency(c);

            b.AddDependency(d);
            c.AddDependency(d);

            var derivation = new Derivation(this.Session);

            var sequence = new List<IObject>();
            derivation["sequence"] = sequence;

            derivation.Derive();

            Assert.AreEqual(d, sequence[0]);
            Assert.AreEqual(a, sequence[3]);
        }
    }
}