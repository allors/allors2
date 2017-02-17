//------------------------------------------------------------------------------------------------- 
// <copyright file="InheritanceTest.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the InheritanceTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta.Static
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class InheritanceTest : AbstractTest
    {
        [Test]
        public void Validate()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Domain" };

            var c1 = new ClassBuilder(domain, Guid.NewGuid()).WithSingularName("C1").WithPluralName("C1s").Build();

            var i1 = new InterfaceBuilder(domain, Guid.NewGuid()).WithSingularName("I1").WithPluralName("I1s").Build();
            var i2 = new InterfaceBuilder(domain, Guid.NewGuid()).WithSingularName("I2").WithPluralName("I2s").Build();

            Assert.IsTrue(metaPopulation.IsValid);

            // class with interface
            new InheritanceBuilder(domain, Guid.NewGuid()).WithSubtype(c1).WithSupertype(i1).Build();

            var validation = metaPopulation.Validate();
            Assert.IsFalse(validation.ContainsErrors);

            // interface with interface
            new InheritanceBuilder(domain, Guid.NewGuid()).WithSubtype(i1).WithSupertype(i2).Build();

            validation = metaPopulation.Validate();
            Assert.IsFalse(validation.ContainsErrors);

            // Cyclic
            var cycle = new InheritanceBuilder(domain, Guid.NewGuid()).WithSubtype(i2).WithSupertype(i1).Build();

            validation = metaPopulation.Validate(); 
            Assert.IsTrue(validation.ContainsErrors);
            Assert.AreEqual(i1, validation.Errors[0].Source);
            Assert.AreEqual(1, validation.Errors[0].Members.Length);
            Assert.AreEqual("IComposite.Supertypes", validation.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Cyclic, validation.Errors[0].Kind);
        }
    }

    public class InheritanceTestWithSuperDomains : InheritanceTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}