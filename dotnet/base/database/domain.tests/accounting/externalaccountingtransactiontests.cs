// <copyright file="ExternalAccountingTransactionTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class ExternalAccountingTransactionTests : DomainTest
    {
        [Fact]
        public void GivenTaxDue_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var partyFrom = new OrganisationBuilder(this.Session).WithName("party from").Build();
            var partyTo = new OrganisationBuilder(this.Session).WithName("party to").Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new TaxDueBuilder(this.Session);
            var taxDue = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("taxdue");
            taxDue = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithEntryDate(this.Session.Now());
            taxDue = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithTransactionDate(this.Session.Now().AddYears(1));
            taxDue = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromParty(partyFrom);
            taxDue = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithToParty(partyTo);
            taxDue = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
