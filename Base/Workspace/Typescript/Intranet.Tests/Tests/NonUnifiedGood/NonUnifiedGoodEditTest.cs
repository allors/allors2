// <copyright file="NonUnifiedGoodEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.NonUnifiedGood
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using src.allors.material.@base.objects.good.list;
    using src.allors.material.@base.objects.nonunifiedgood.overview;
    using Xunit;

    [Collection("Test collection")]
    public class NonUnifiedGoodEditTest : Test
    {
        private readonly GoodListComponent goods;

        public NonUnifiedGoodEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.goods = this.Sidenav.NavigateToGoods();
        }

        [Fact]
        public void Edit()
        {
            var internalOrganisation = new OrganisationBuilder(this.MemorySession).WithInternalOrganisationDefaults().Build();

            var newGood = new NonUnifiedGoodBuilder(this.MemorySession).WithNonSerialisedPartDefaults(internalOrganisation).Build();
            var before = new NonUnifiedGoods(this.Session).Extent().ToArray();

            var nonUnifiedGood = before.First();
            var id = nonUnifiedGood.Id;

            this.goods.Table.DefaultAction(nonUnifiedGood);
            var goodDetails = new NonUnifiedGoodOverviewComponent(this.goods.Driver);
            var goodOverviewDetail = goodDetails.NonunifiedgoodOverviewDetail.Click();

            goodOverviewDetail
                .Name.Set(newGood.Name)
                .Description.Set(newGood.Description)
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new NonUnifiedGoods(this.Session).Extent().ToArray();

            var good = after.First(v => v.Id.Equals(id));

            Assert.Equal(after.Length, before.Length);
            Assert.Equal(newGood.Name, good.Name);
            Assert.Equal(newGood.Description, good.Description);
        }
    }
}
