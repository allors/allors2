// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizedTextTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using Allors;
    using Allors.Domain;

    using Xunit;

    public class LocalisedTextTests : DomainTest
    {
        [Fact]
        public void GivenLocalisedTextWhenValidatingThenRequiredRelationsMustExist()
        {
            var builder = new LocalisedTextBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder.WithText("description");

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
