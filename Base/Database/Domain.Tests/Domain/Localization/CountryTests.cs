// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountryTests.cs" company="Allors bvba">
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
    using Allors;
    using Allors.Domain;

    using NUnit.Framework;

    [TestFixture]
    public class CountryTests : DomainTest
    {
        [Test]
        public void GivenCountryWhenValidatingThenRequiredRelationsMustExist()
        {
            var builder = new CountryBuilder(this.Session);
            builder.Build();
            
            Assert.IsTrue(this.Session.Derive().HasErrors);

            builder.WithIsoCode("XX").Build();
            builder.Build();

            Assert.IsTrue(this.Session.Derive().HasErrors);

            this.Session.Rollback();

            builder.WithName("X Country");

            builder.Build();

            Assert.IsFalse(this.Session.Derive().HasErrors);

            this.Session.Rollback();

            builder = new CountryBuilder(this.Session);
            builder.WithName("X Country");

            builder.Build();

            Assert.IsTrue(this.Session.Derive().HasErrors);
        }
    }
}