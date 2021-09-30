// <copyright file="ProductFeatureTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class ProductFeatureTests : DomainTest
    {
        [Fact]
        public void GivenDimension_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new DimensionBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("name");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenModel_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ModelBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("Mt");

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenServiceFeature_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ServiceFeatureBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("Mt");

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSizeConstant_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new SizeBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("Mt");

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSoftwareFeature_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new SoftwareFeatureBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("Mt");

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenProductQualityConstant_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ProductQualityBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("Mt");

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
