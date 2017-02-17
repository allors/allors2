//------------------------------------------------------------------------------------------------- 
// <copyright file="MethodTypeTest.cs" company="Allors bvba">
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
// <summary>Defines the MethodTypeTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta.Static
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class MethodTypeTest : AbstractTest
    {
        [Test]
        public void ValidateDuplicateMethod()
        {
            this.Populate();

            var methodType = new MethodTypeBuilder(this.Domain, Guid.NewGuid()).Build();
            methodType.ObjectType = this.Population.C1;
            methodType.Name = "MyName";

            Assert.IsTrue(this.MetaPopulation.IsValid);

            var otherMethodType = new MethodTypeBuilder(this.Domain, Guid.NewGuid()).Build();
            methodType.ObjectType = this.Population.C1;
            methodType.Name = "MyName";

            Assert.IsFalse(this.MetaPopulation.IsValid);
        }
    }

    public class MethodTypeTestWithSuperDomains : MethodTypeTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}