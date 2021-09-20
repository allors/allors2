// <copyright file="DesiredProductFeatureTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class DesiredProductFeatureTests : DomainTest
    {
        [Fact]
        public void GivenDesiredProductFeature_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var softwareFeature = new SoftwareFeatureBuilder(this.Session)
                .WithVatRegime(new VatRegimes(this.Session).ZeroRated)
                .WithName("Tutorial")
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new DesiredProductFeatureBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithRequired(false);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithProductFeature(softwareFeature);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
