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
    using NUnit.Framework;

    [TestFixture]
    public class ProductTests : DomainTest
    {
        [Test]
        public void GivenDeliverableCoredService_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new DeliverableBasedServiceBuilder(this.DatabaseSession);
            var deliverableBasedService = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithVatRate(vatRate21);
            deliverableBasedService = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("DeliverableCoredService");
            deliverableBasedService = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenDeliverableCoredServiceWithPrimaryProductCategoryWithoutProductCategory_WhenDeriving_ThenFirstProductCategoryIsCopiedFromPrimaryCategory()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var productCategory = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

            var deliverableBasedService = new DeliverableBasedServiceBuilder(this.DatabaseSession)
                .WithName("DeliverableCoredService")
                .WithPrimaryProductCategory(productCategory)
                .WithVatRate(vatRate21)
                .Build();

            this.DatabaseSession.Derive(true); 
            
            Assert.Contains(productCategory, deliverableBasedService.ProductCategories);
        }

        [Test]
        public void GivenDeliverableCoredServiceWithoutPrimaryProductCategoryWithOneProductCategory_WhenDeriving_ThenPrimaryProductCategoryIsCopiedFromCategory()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var productCategory = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

            var deliverableBasedService = new DeliverableBasedServiceBuilder(this.DatabaseSession)
                .WithName("DeliverableCoredService")
                .WithProductCategory(productCategory)
                .WithVatRate(vatRate21)
                .Build();

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(productCategory, deliverableBasedService.PrimaryProductCategory);
        }

        [Test]
        public void GivenGood_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var finishedGood = new FinishedGoodBuilder(this.DatabaseSession).WithName("finishedGood").Build();
            
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();
            
            var builder = new GoodBuilder(this.DatabaseSession);
            var good = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();
            
            builder.WithName("Good");
            good = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece);
            good = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithVatRate(vatRate21);
            good = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFinishedGood(finishedGood);
            good = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            builder.WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized);
            good = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            good.RemoveFinishedGood();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenGoodWithPrimaryProductCategoryWithoutProductCategory_WhenDeriving_ThenFirstProductCategoryIsCopiedFromPrimaryCategory()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var productCategory = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithPrimaryProductCategory(productCategory)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true); 

            Assert.Contains(productCategory, good.ProductCategories);
        }

        [Test]
        public void GivenGoodWithoutPrimaryProductCategoryWithOneProductCategory_WhenDeriving_ThenPrimaryProductCategoryIsCopiedFromCategory()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var productCategory = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithProductCategory(productCategory)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(productCategory, good.PrimaryProductCategory);
        }

        [Test]
        public void GivenGoodWithProductCategory_WhenDeriving_ThenProductCategoriesExpandedIsSet()
        {
            var productCategory1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1").Build();
            var productCategory2 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("2").Build();
            var productCategory11 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory12 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory111 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1.1").WithParent(productCategory11).Build();
            var productCategory121 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.1").WithParent(productCategory12).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithPrimaryProductCategory(productCategory111)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithVatRate(vatRate21)
                .Build();

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(4, good.ProductCategoriesExpanded.Count);
            Assert.Contains(productCategory111, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory11, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory1, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory2, good.ProductCategoriesExpanded);

            good.AddProductCategory(productCategory121);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(6, good.ProductCategoriesExpanded.Count);
            Assert.Contains(productCategory111, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory121, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory11, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory12, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory1, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory2, good.ProductCategoriesExpanded);
        }

        [Test]
        public void GivenGood_WhenProductCategoryParentsAreInserted_ThenProductCategoriesExpandedAreRecalculated()
        {
            var productCategory1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1").Build();
            var productCategory2 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("2").Build();
            var productCategory11 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory12 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory111 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1.1").WithParent(productCategory11).Build();
            var productCategory121 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.1").WithParent(productCategory12).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithPrimaryProductCategory(productCategory111)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithVatRate(vatRate21)
                .Build();

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(4, good.ProductCategoriesExpanded.Count);
            Assert.Contains(productCategory111, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory11, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory1, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory2, good.ProductCategoriesExpanded);

            var productCategory3 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("3").Build();
            productCategory11.AddParent(productCategory3);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(5, good.ProductCategoriesExpanded.Count);
            Assert.Contains(productCategory111, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory11, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory1, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory2, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory3, good.ProductCategoriesExpanded);

            good.AddProductCategory(productCategory121);

            this.DatabaseSession.Derive(true);

            var productCategory13 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.3").WithParent(productCategory1).Build();
            productCategory121.AddParent(productCategory13);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(8, good.ProductCategoriesExpanded.Count);
            Assert.Contains(productCategory111, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory121, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory11, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory12, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory13, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory1, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory2, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory3, good.ProductCategoriesExpanded);
        }

        [Test]
        public void GivenGood_WhenProductCategoryParentsAreRemoved_ThenProductCategoriesExpandedAreRecalculated()
        {
            var productCategory1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1").Build();
            var productCategory2 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("2").Build();
            var productCategory11 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory12 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory111 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1.1").WithParent(productCategory11).Build();
            var productCategory121 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.1").WithParent(productCategory12).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithPrimaryProductCategory(productCategory111)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithVatRate(vatRate21)
                .Build();

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(4, good.ProductCategoriesExpanded.Count);
            Assert.Contains(productCategory111, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory11, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory1, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory2, good.ProductCategoriesExpanded);

            productCategory11.RemoveParent(productCategory2);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, good.ProductCategoriesExpanded.Count);
            Assert.Contains(productCategory111, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory11, good.ProductCategoriesExpanded);
            Assert.Contains(productCategory1, good.ProductCategoriesExpanded);
        }

        [Test]
        public void GivenTimeAndMaterialsService_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new TimeAndMaterialsServiceBuilder(this.DatabaseSession);
            var good = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("TimeAndMaterialsService");
            good = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithVatRate(vatRate21);
            good = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenTimeAndMaterialsServiceWithPrimaryProductCategoryWithoutProductCategory_WhenDeriving_ThenFirstProductCategoryIsCopiedFromPrimaryCategory()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var productCategory = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

            var timeAndMaterialsService = new TimeAndMaterialsServiceBuilder(this.DatabaseSession)
                .WithName("TimeAndMaterialsService")
                .WithPrimaryProductCategory(productCategory)
                .WithVatRate(vatRate21)
                .Build();

            this.DatabaseSession.Derive(true); 

            Assert.Contains(productCategory, timeAndMaterialsService.ProductCategories);
        }

        [Test]
        public void GivenTimeAndMaterialsServiceWithoutPrimaryProductCategoryWithOneProductCategory_WhenDeriving_ThenPrimaryProductCategoryIsCopiedFromCategory()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var productCategory = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

            var timeAndMaterialsService = new TimeAndMaterialsServiceBuilder(this.DatabaseSession)
                .WithName("TimeAndMaterialsService")
                .WithProductCategory(productCategory)
                .WithVatRate(vatRate21)
                .Build();

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(productCategory, timeAndMaterialsService.PrimaryProductCategory);
        }
    }
}
