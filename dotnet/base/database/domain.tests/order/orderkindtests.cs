// <copyright file="OrderKindTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Allors.Domain
{
    using Xunit;

    public class OrderKindTests : DomainTest
    {
        [Fact]
        public void GivenOrderKind_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OrderKindBuilder(this.Session);
            var orderKind = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("orderkind");
            orderKind = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrderKind_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var orderKind = new OrderKindBuilder(this.Session)
                .WithDescription("Pre order summer collections")
                .Build();

            this.Session.Derive();

            Assert.False(orderKind.ScheduleManually);
        }
    }
}
