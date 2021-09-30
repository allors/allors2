// <copyright file="CountryTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Xunit;

    public class CountryTests : DomainTest
    {
        [Fact]
        public void GivenCountryWhenValidatingThenRequiredRelationsMustExist()
        {
            var builder = new CountryBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("name");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithIsoCode("nm");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
