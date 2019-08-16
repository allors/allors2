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
            var customer = new OrganisationBuilder(this.Session).WithName("customer").WithLocale(new Locales(this.Session).EnglishGreatBritain).Build();
            this.Session.Derive();
            this.Session.Commit();

            var builder = new SalesRepRelationshipBuilder(this.Session);
            var relationship = builder.Build();

            this.Session.Derive();
            Assert.True(relationship.Strategy.IsDeleted);

            this.Session.Rollback();

            builder.WithCustomer(customer);
            relationship = builder.Build();

            this.Session.Derive();
            Assert.True(relationship.Strategy.IsDeleted);

            this.Session.Rollback();

            builder.WithSalesRepresentative(new PersonBuilder(this.Session).WithLastName("salesrep.").Build());
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
