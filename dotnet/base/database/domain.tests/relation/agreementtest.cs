// <copyright file="AgreementTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class AgreementTest : DomainTest
    {
        [Fact]
        public void GivenClientAgreement_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ClientAgreementBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(DateTimeFactory.CreateDate(2010, 12, 31));
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("client agreement");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenEmploymentAgreement_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new EmploymentAgreementBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(DateTimeFactory.CreateDate(2010, 12, 31));
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            this.Session.Rollback();

            builder.WithDescription("employment agreement");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPurchaseAgreement_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new PurchaseAgreementBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(DateTimeFactory.CreateDate(2010, 12, 31));
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("purchase agreement");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSalesAgreement_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new SalesAgreementBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(DateTimeFactory.CreateDate(2010, 12, 31));
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("sales agreement");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSubContractorAgreement_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new SubContractorAgreementBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(DateTimeFactory.CreateDate(2010, 12, 31));
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("subContractor agreement");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
