// <copyright file="AddendumTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class AddendumTest : DomainTest
    {
        [Fact]
        public void GivenAddendum_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new AddendumBuilder(this.Session);
            var addendum = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("addendum");
            addendum = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
