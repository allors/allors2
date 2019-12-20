// <copyright file="NonUnifiedGoodCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors;
using Allors.Domain.TestPopulation;
using Allors.Meta;

namespace Tests.NonUnifiedGood
{
    using System.Linq;
    using Allors.Domain;
    using Components;
    using src.allors.material.@base.objects.good.list;
    using Xunit;

    [Collection("Test collection")]
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

            var internalOrganisation = new Organisations(this.MemorySession).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new NonUnifiedGoodBuilder(this.MemorySession).WithSerialisedPartDefaults(internalOrganisation).Build();

            this.MemorySession.Derive();

            var nonUnifiedGoodCreate = this.goods.CreateNonUnifiedGood();

            nonUnifiedGoodCreate
                .Name.Set(expected.Name)
                .Description.Set(expected.Description)
                .Part.Set(expected.Part.Name)
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new NonUnifiedGoods(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var good = after.Except(before).First();

            Assert.Equal(expected.Name, good.Name);
            Assert.Equal(expected.Description, good.Description);
            Assert.Equal(expected.Part.Name, good.Part.Name);
        }
    }
}
