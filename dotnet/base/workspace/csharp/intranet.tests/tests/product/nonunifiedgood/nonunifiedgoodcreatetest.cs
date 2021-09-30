// <copyright file="NonUnifiedGoodCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.NonUnifiedGood
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using src.allors.material.@base.objects.good.list;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Product")]
    public class NonUnifiedGoodCreateTest : Test
    {
        private readonly GoodListComponent goods;

        public NonUnifiedGoodCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.goods = this.Sidenav.NavigateToGoods();
        }

        [Fact]
        public void Create()
        {
            var before = new NonUnifiedGoods(this.Session).Extent().ToArray();

            var internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new NonUnifiedGoodBuilder(this.Session).WithSerialisedPartDefaults(internalOrganisation).Build();

            var expectedPart = new NonUnifiedParts(this.Session).Extent().First;

            this.Session.Derive();

            var expectedName = expected.Name;
            var expectedDescription = expected.Description;
            var expectedPartName = expectedPart.Name;

            var nonUnifiedGoodCreate = this.goods.CreateNonUnifiedGood();

            nonUnifiedGoodCreate
                .Name.Set(expected.Name)
                .Description.Set(expected.Description)
                .Part.Select(expectedPart.Name);

            this.Session.Rollback();
            nonUnifiedGoodCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new NonUnifiedGoods(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var good = after.Except(before).First();

            Assert.Equal(expectedName, good.Name);
            Assert.Equal(expectedDescription, good.Description);
            Assert.Equal(expectedPartName, good.Part.Name);
        }
    }
}
