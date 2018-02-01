//------------------------------------------------------------------------------------------------- 
// <copyright file="SerialisedInventoryItemTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Meta;
    using Xunit;

    
    public class SerialisedInventoryItemTests : DomainTest
    {
        [Fact]
        public void GivenInventoryItem_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var part = new FinishedGoodBuilder(this.Session).WithName("part")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithManufacturerId("10101")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithSku("sku")
                .Build();

            this.Session.Commit();

            var builder = new SerialisedInventoryItemBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPart(part);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithSerialNumber("1");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithGood(new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithName("good")
                .Build());

            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenInventoryItem_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var item = new SerialisedInventoryItemBuilder(this.Session)
                .WithSerialNumber("1")
                .WithPart(new FinishedGoodBuilder(this.Session)
                            .WithInternalOrganisation(this.InternalOrganisation)
                            .WithName("part")
                            .WithManufacturerId("10101")
                            .WithSku("1")
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SerialisedInventoryItemStates(this.Session).Good, item.SerialisedInventoryItemState);
            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), item.Facility);
        }
    }
}
