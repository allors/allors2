//------------------------------------------------------------------------------------------------- 
// <copyright file="QuoteTests.cs" company="Allors bvba">
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

    public class QuoteTests : DomainTest
    {
        [Fact]
        public void GivenProductQuote_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new ProductQuoteBuilder(this.Session);
            var productQuote = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("ProductQuote");
            productQuote = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenProposal_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new ProposalBuilder(this.Session);
            var requirement = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("Proposal");
            requirement = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenStatementOfWork_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new StatementOfWorkBuilder(this.Session);
            var statementOfWork = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("StatementOfWork");
            statementOfWork = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}