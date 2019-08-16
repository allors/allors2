// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountryTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using Allors;
    using Allors.Domain;

    using Xunit;

    public class CountryTests : DomainTest
    {
        [Fact]
        public void GivenCountryWhenValidatingThenRequiredRelationsMustExist()
        {
            var builder = new CountryBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder.WithIsoCode("XX").Build();
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("X Country");

            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder = new CountryBuilder(this.Session);
            builder.WithName("X Country");

            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);
        }
    }
}
