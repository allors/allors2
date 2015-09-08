//------------------------------------------------------------------------------------------------- 
// <copyright file="CreditCardTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using NUnit.Framework;

    [TestFixture]
    public class CreditCardTests : DomainTest
    {
        [Test]
        public void GivenCreditCard_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new CreditCardBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCardNumber("4012888888881881");
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithExpirationYear(2016);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithExpirationMonth(03);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithNameOnCard("M.E. van Knippenberg");
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCreditCardCompany(new CreditCardCompanyBuilder(this.DatabaseSession).WithName("Visa").Build());
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenCreditCard_WhenDeriving_ThenCardNumberMustBeUnique()
        {
            new CreditCardBuilder(this.DatabaseSession)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(2016)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.DatabaseSession).WithName("Visa").Build())
                .Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            new CreditCardBuilder(this.DatabaseSession)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(2016)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.DatabaseSession).WithName("Visa").Build())
                .Build();
            
            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
