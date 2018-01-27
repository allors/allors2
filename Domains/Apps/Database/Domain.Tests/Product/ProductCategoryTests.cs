 //------------------------------------------------------------------------------------------------- 
// <copyright file="ProductCategoryTests.cs" company="Allors bvba">
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

    
    public class ProductCategoryTests : DomainTest
    {
        [Fact]
        public void GivenProductCategory_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ProductCategoryBuilder(this.Session);
            var productCategory = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("category");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenLeafeProductCategory_WhenPackageIsDefined_ThenValidationHasNoErrors()
        {
            var package = new PackageBuilder(this.Session).WithName("package").Build();

            new ProductCategoryBuilder(this.Session)
                .WithName("category")
                .WithPackage(package)
                .Build();

            Assert.False(new NonLogging.Derivation(this.Session).Derive().HasErrors);

            this.Session.Rollback();
        }

        [Fact]
        public void GivenParentProductCategory_WhenPackageIsDefined_ThenValidationHasErrors()
        {
            var package = new PackageBuilder(this.Session).WithName("package").Build();

            var parentProductCategory = new ProductCategoryBuilder(this.Session)
                .WithDescription("parent")
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.Session)
                .WithDescription("child")
                .WithParent(parentProductCategory).
                Build();

            Assert.True(new NonLogging.Derivation(this.Session).Derive().HasErrors);
        }

        [Fact]
        public void GivenProductCategory_WhenDeriving_ThenSuperJacentAreSet()
        {
            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithParent(productCategory1)
                .WithParent(productCategory2)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithParent(productCategory1)
                .WithParent(productCategory2)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithParent(productCategory11)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithParent(productCategory12)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithParent(productCategory12)
                .Build();

            this.Session.Derive(); 

            Assert.False(productCategory1.ExistSuperJacent);
            Assert.False(productCategory2.ExistSuperJacent);

            Assert.Equal(2, productCategory11.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory11.SuperJacent);
            Assert.Contains(productCategory2, productCategory11.SuperJacent);

            Assert.Equal(2, productCategory12.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory12.SuperJacent);
            Assert.Contains(productCategory2, productCategory12.SuperJacent);

            Assert.Equal(3, productCategory111.SuperJacent.Count);
            Assert.Contains(productCategory11, productCategory111.SuperJacent);
            Assert.Contains(productCategory1, productCategory111.SuperJacent);
            Assert.Contains(productCategory2, productCategory111.SuperJacent);

            Assert.Equal(3, productCategory121.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory121.SuperJacent);
            Assert.Contains(productCategory1, productCategory121.SuperJacent);
            Assert.Contains(productCategory2, productCategory121.SuperJacent);

            Assert.Equal(3, productCategory122.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory122.SuperJacent);
            Assert.Contains(productCategory1, productCategory122.SuperJacent);
            Assert.Contains(productCategory2, productCategory122.SuperJacent);
        }

        [Fact]
        public void GivenProductCategory_WhenNewParentsAreInserted_ThenSuperJacentAreRecalculated()
        {
            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithParent(productCategory1)
                .WithParent(productCategory2)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithParent(productCategory1)
                .WithParent(productCategory2)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithParent(productCategory11)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithParent(productCategory12)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithParent(productCategory12)
                .Build();

            this.Session.Derive(); 

            Assert.False(productCategory1.ExistSuperJacent);
            Assert.False(productCategory2.ExistSuperJacent);

            Assert.Equal(2, productCategory11.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory11.SuperJacent);
            Assert.Contains(productCategory2, productCategory11.SuperJacent);

            Assert.Equal(2, productCategory12.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory12.SuperJacent);
            Assert.Contains(productCategory2, productCategory12.SuperJacent);

            Assert.Equal(3, productCategory111.SuperJacent.Count);
            Assert.Contains(productCategory11, productCategory111.SuperJacent);
            Assert.Contains(productCategory1, productCategory111.SuperJacent);
            Assert.Contains(productCategory2, productCategory111.SuperJacent);

            Assert.Equal(3, productCategory121.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory121.SuperJacent);
            Assert.Contains(productCategory1, productCategory121.SuperJacent);
            Assert.Contains(productCategory2, productCategory121.SuperJacent);

            Assert.Equal(3, productCategory122.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory122.SuperJacent);
            Assert.Contains(productCategory1, productCategory122.SuperJacent);
            Assert.Contains(productCategory2, productCategory122.SuperJacent);

            var productCategory3 = new ProductCategoryBuilder(this.Session)
                .WithName("3")
                .Build();
            productCategory11.AddParent(productCategory3);

            this.Session.Derive();

            Assert.False(productCategory1.ExistSuperJacent);
            Assert.False(productCategory2.ExistSuperJacent);
            Assert.False(productCategory3.ExistSuperJacent);

            Assert.Equal(3, productCategory11.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory11.SuperJacent);
            Assert.Contains(productCategory2, productCategory11.SuperJacent);
            Assert.Contains(productCategory3, productCategory11.SuperJacent);

            Assert.Equal(2, productCategory12.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory12.SuperJacent);
            Assert.Contains(productCategory2, productCategory12.SuperJacent);

            Assert.Equal(4, productCategory111.SuperJacent.Count);
            Assert.Contains(productCategory11, productCategory111.SuperJacent);
            Assert.Contains(productCategory1, productCategory111.SuperJacent);
            Assert.Contains(productCategory2, productCategory111.SuperJacent);
            Assert.Contains(productCategory3, productCategory111.SuperJacent);

            Assert.Equal(3, productCategory121.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory121.SuperJacent);
            Assert.Contains(productCategory1, productCategory121.SuperJacent);
            Assert.Contains(productCategory2, productCategory121.SuperJacent);

            Assert.Equal(3, productCategory122.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory122.SuperJacent);
            Assert.Contains(productCategory1, productCategory122.SuperJacent);
            Assert.Contains(productCategory2, productCategory122.SuperJacent);

            var productCategory13 = new ProductCategoryBuilder(this.Session)
                .WithName("1.3")
                .WithParent(productCategory1)
                .Build();
            productCategory122.AddParent(productCategory13);

            this.Session.Derive();

            Assert.False(productCategory1.ExistSuperJacent);
            Assert.False(productCategory2.ExistSuperJacent);
            Assert.False(productCategory3.ExistSuperJacent);

            Assert.Equal(3, productCategory11.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory11.SuperJacent);
            Assert.Contains(productCategory2, productCategory11.SuperJacent);
            Assert.Contains(productCategory3, productCategory11.SuperJacent);

            Assert.Equal(2, productCategory12.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory12.SuperJacent);
            Assert.Contains(productCategory2, productCategory12.SuperJacent);

            Assert.Equal(1, productCategory13.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory13.SuperJacent);

            Assert.Equal(4, productCategory111.SuperJacent.Count);
            Assert.Contains(productCategory11, productCategory111.SuperJacent);
            Assert.Contains(productCategory1, productCategory111.SuperJacent);
            Assert.Contains(productCategory2, productCategory111.SuperJacent);
            Assert.Contains(productCategory3, productCategory111.SuperJacent);

            Assert.Equal(3, productCategory121.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory121.SuperJacent);
            Assert.Contains(productCategory1, productCategory121.SuperJacent);
            Assert.Contains(productCategory2, productCategory121.SuperJacent);

            Assert.Equal(4, productCategory122.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory122.SuperJacent);
            Assert.Contains(productCategory13, productCategory122.SuperJacent);
            Assert.Contains(productCategory1, productCategory122.SuperJacent);
            Assert.Contains(productCategory2, productCategory122.SuperJacent);
        }

        [Fact]
        public void GivenProductCategory_WhenNewParentsAreRemoved_ThenSuperJacentAreRecalculated()
        {
            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithParent(productCategory1)
                .WithParent(productCategory2)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithParent(productCategory1)
                .WithParent(productCategory2)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithParent(productCategory11)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithParent(productCategory12)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithParent(productCategory12)
                .Build();

            this.Session.Derive(); 

            Assert.False(productCategory1.ExistSuperJacent);
            Assert.False(productCategory2.ExistSuperJacent);

            Assert.Equal(2, productCategory11.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory11.SuperJacent);
            Assert.Contains(productCategory2, productCategory11.SuperJacent);

            Assert.Equal(2, productCategory12.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory12.SuperJacent);
            Assert.Contains(productCategory2, productCategory12.SuperJacent);

            Assert.Equal(3, productCategory111.SuperJacent.Count);
            Assert.Contains(productCategory11, productCategory111.SuperJacent);
            Assert.Contains(productCategory1, productCategory111.SuperJacent);
            Assert.Contains(productCategory2, productCategory111.SuperJacent);

            Assert.Equal(3, productCategory121.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory121.SuperJacent);
            Assert.Contains(productCategory1, productCategory121.SuperJacent);
            Assert.Contains(productCategory2, productCategory121.SuperJacent);

            Assert.Equal(3, productCategory122.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory122.SuperJacent);
            Assert.Contains(productCategory1, productCategory122.SuperJacent);
            Assert.Contains(productCategory2, productCategory122.SuperJacent);

            productCategory11.RemoveParent(productCategory2);

            this.Session.Derive();

            Assert.False(productCategory1.ExistSuperJacent);
            Assert.False(productCategory2.ExistSuperJacent);

            Assert.Equal(1, productCategory11.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory11.SuperJacent);

            Assert.Equal(2, productCategory12.SuperJacent.Count);
            Assert.Contains(productCategory1, productCategory12.SuperJacent);
            Assert.Contains(productCategory2, productCategory12.SuperJacent);

            Assert.Equal(2, productCategory111.SuperJacent.Count);
            Assert.Contains(productCategory11, productCategory111.SuperJacent);
            Assert.Contains(productCategory1, productCategory111.SuperJacent);

            Assert.Equal(3, productCategory121.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory121.SuperJacent);
            Assert.Contains(productCategory1, productCategory121.SuperJacent);
            Assert.Contains(productCategory2, productCategory121.SuperJacent);

            Assert.Equal(3, productCategory122.SuperJacent.Count);
            Assert.Contains(productCategory12, productCategory122.SuperJacent);
            Assert.Contains(productCategory1, productCategory122.SuperJacent);
            Assert.Contains(productCategory2, productCategory122.SuperJacent);
        }

        [Fact]
        public void GivenProductCategory_WhenDeriving_ThenChildrenAreSet()
        {
            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithParent(productCategory1)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithParent(productCategory1)
                .WithParent(productCategory2)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithParent(productCategory11)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithParent(productCategory12)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithParent(productCategory12)
                .Build();

            this.Session.Derive(); 

            Assert.Equal(5, productCategory1.Children.Count);
            Assert.Contains(productCategory11, productCategory1.Children);
            Assert.Contains(productCategory12, productCategory1.Children);
            Assert.Contains(productCategory111, productCategory1.Children);
            Assert.Contains(productCategory121, productCategory1.Children);
            Assert.Contains(productCategory122, productCategory1.Children);

            Assert.Equal(3, productCategory2.Children.Count);
            Assert.Contains(productCategory12, productCategory2.Children);
            Assert.Contains(productCategory121, productCategory2.Children);
            Assert.Contains(productCategory122, productCategory2.Children);

            Assert.Equal(1, productCategory11.Children.Count);
            Assert.Contains(productCategory111, productCategory11.Children);

            Assert.Equal(2, productCategory12.Children.Count);
            Assert.Contains(productCategory121, productCategory12.Children);
            Assert.Contains(productCategory122, productCategory12.Children);

            Assert.False(productCategory111.ExistChildren);
            Assert.False(productCategory121.ExistChildren);
            Assert.False(productCategory122.ExistChildren);
        }

        [Fact]
        public void GivenProductCategoryHierarchy_WhenDeriving_ThenAllProductAreSet()
        {
            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithParent(productCategory1)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithParent(productCategory1)
                .WithParent(productCategory2)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithParent(productCategory11)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithParent(productCategory12)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithParent(productCategory12)
                .Build();

            this.Session.Derive();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithName("good1")
                .WithSku("good1")
                .WithPrimaryProductCategory(productCategory1)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithVatRate(vatRate21)
                .Build();
            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithName("good2")
                .WithSku("good2")
                .WithPrimaryProductCategory(productCategory2)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithVatRate(vatRate21)
                .Build();
            var good11 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithName("good11")
                .WithSku("good11")
                .WithPrimaryProductCategory(productCategory11)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithVatRate(vatRate21)
                .Build();
            var good12 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithName("good12")
                .WithSku("good12")
                .WithPrimaryProductCategory(productCategory12)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithVatRate(vatRate21)
                .Build();
            var good111 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithName("good111")
                .WithSku("good111")
                .WithPrimaryProductCategory(productCategory111)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithVatRate(vatRate21)
                .Build();
            var good121 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithName("good121")
                .WithSku("good121")
                .WithPrimaryProductCategory(productCategory121)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithVatRate(vatRate21)
                .Build();
            var good122 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithName("good122")
                .WithSku("good122")
                .WithPrimaryProductCategory(productCategory122)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithVatRate(vatRate21)
                .Build();

            this.Session.Derive();

            Assert.Equal(6, productCategory1.AllProducts.Count);
            Assert.Contains(good1, productCategory1.AllProducts);
            Assert.Contains(good11, productCategory1.AllProducts);
            Assert.Contains(good12, productCategory1.AllProducts);
            Assert.Contains(good111, productCategory1.AllProducts);
            Assert.Contains(good121, productCategory1.AllProducts);
            Assert.Contains(good122, productCategory1.AllProducts);

            Assert.Equal(4, productCategory2.AllProducts.Count);
            Assert.Contains(good2, productCategory2.AllProducts);
            Assert.Contains(good12, productCategory2.AllProducts);
            Assert.Contains(good121, productCategory2.AllProducts);
            Assert.Contains(good122, productCategory2.AllProducts);

            Assert.Equal(2, productCategory11.AllProducts.Count);
            Assert.Contains(good11, productCategory11.AllProducts);
            Assert.Contains(good111, productCategory11.AllProducts);

            Assert.Equal(3, productCategory12.AllProducts.Count);
            Assert.Contains(good12, productCategory12.AllProducts);
            Assert.Contains(good121, productCategory12.AllProducts);
            Assert.Contains(good122, productCategory12.AllProducts);

            Assert.Equal(1, productCategory111.AllProducts.Count);
            Assert.Contains(good111, productCategory111.AllProducts);

            Assert.Equal(1, productCategory121.AllProducts.Count);
            Assert.Contains(good121, productCategory121.AllProducts);

            Assert.Equal(1, productCategory122.AllProducts.Count);
            Assert.Contains(good122, productCategory122.AllProducts);
        }
    }
}
