//------------------------------------------------------------------------------------------------- 
// <copyright file="ExternalAccountingTransactionTests.cs" company="Allors bvba">
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
    using System;
    using Xunit;

    
    public class ExternalAccountingTransactionTests : DomainTest
    {
        [Fact]
        public void GivenTaxDue_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var partyFrom = new OrganisationBuilder(this.DatabaseSession).WithName("party from").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var partyTo = new OrganisationBuilder(this.DatabaseSession).WithName("party to").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var builder = new TaxDueBuilder(this.DatabaseSession);
            var taxDue = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("taxdue");
            taxDue = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithEntryDate(DateTime.UtcNow);
            taxDue = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithTransactionDate(DateTime.UtcNow.AddYears(1));
            taxDue = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromParty(partyFrom);
            taxDue = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithToParty(partyTo);
            taxDue = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }
    }
}