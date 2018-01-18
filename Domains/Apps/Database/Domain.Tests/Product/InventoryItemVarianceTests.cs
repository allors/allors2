//------------------------------------------------------------------------------------------------- 
// <copyright file="NonSerialisedInventoryItemTests.cs" company="Allors bvba">
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
    using System;
    using Meta;
    using Xunit;

    
    public class InventoryItemVarianceTests : DomainTest
    {

        [Fact]
        public void GivenInventoryItem_WhenPositiveVariance_ThenQuantityOnHandIsRaised()
        {
            var good = new GoodBuilder(this.Session)
                .WithName("good")
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithGood(good)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(0, good.QuantityOnHand);
            Assert.Equal(0, inventoryItem.QuantityOnHand);

            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(10).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();
            Assert.Equal(10, good.QuantityOnHand);
            Assert.Equal(10, inventoryItem.QuantityOnHand);
        }
    }
}
