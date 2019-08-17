//-------------------------------------------------------------------------------------------------
// <copyright file="SalesRepRelationshipTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
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
