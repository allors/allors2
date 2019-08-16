
// <copyright file="LocalizedTextTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
