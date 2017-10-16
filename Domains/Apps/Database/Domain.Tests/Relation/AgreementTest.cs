//------------------------------------------------------------------------------------------------- 
// <copyright file="AgreementTest.cs" company="Allors bvba">
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
