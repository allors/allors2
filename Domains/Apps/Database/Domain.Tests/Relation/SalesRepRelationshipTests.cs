//------------------------------------------------------------------------------------------------- 
// <copyright file="SalesRepRelationshipTests.cs" company="Allors bvba">
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
    using Meta;
    using Xunit;

    
    public class SalesRepRelationshipTests : DomainTest
    {
        [Fact]
        public void GivenSalesRepRelationship_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain).Build();
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new SalesRepRelationshipBuilder(this.DatabaseSession);
            var relationship = builder.Build();

            this.DatabaseSession.Derive();
            Assert.True(relationship.Strategy.IsDeleted);

            this.DatabaseSession.Rollback();

            builder.WithCustomer(customer);
            relationship = builder.Build();

            this.DatabaseSession.Derive();
            Assert.True(relationship.Strategy.IsDeleted);

            this.DatabaseSession.Rollback();

            builder.WithSalesRepresentative(new PersonBuilder(this.DatabaseSession).WithLastName("salesrep.").Build());
            builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
