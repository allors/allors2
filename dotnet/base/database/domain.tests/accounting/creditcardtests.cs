// <copyright file="CreditCardTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class CreditCardTests : DomainTest
    {
        [Fact]
        public void GivenCreditCard_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new CreditCardBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCardNumber("4012888888881881");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithExpirationYear(2016);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithExpirationMonth(03);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithNameOnCard("M.E. van Knippenberg");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCreditCardCompany(new CreditCardCompanyBuilder(this.Session).WithName("Visa").Build());
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCreditCard_WhenDeriving_ThenCardNumberMustBeUnique()
        {
            new CreditCardBuilder(this.Session)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(2016)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.Session).WithName("Visa").Build())
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            new CreditCardBuilder(this.Session)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(2016)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.Session).WithName("Visa").Build())
                .Build();

            Assert.True(this.Session.Derive(false).HasErrors);
        }
    }
}
