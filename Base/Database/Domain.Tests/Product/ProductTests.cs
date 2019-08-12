//------------------------------------------------------------------------------------------------- 
// <copyright file="ProductTests.cs" company="Allors bvba">
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
    using Xunit;

    public class ProductTests : DomainTest
    {
        [Fact]
        public void GivenDeliverableBasedService_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            this.Session.Derive();
            this.Session.Commit();

            var builder = new DeliverableBasedServiceBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithVatRate(vatRate21);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("service");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenTimeAndMaterialsService_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new TimeAndMaterialsServiceBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("good");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithVatRate(vatRate21);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenGood_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var finishedGood = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new NonUnifiedGoodBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("good");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithVatRate(vatRate21);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPart(finishedGood);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
