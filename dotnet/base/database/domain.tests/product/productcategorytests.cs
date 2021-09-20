// <copyright file="ProductCategoryTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
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
        public void GivenProductCategory_WhenDeriving_ThenProductCategoriesWhereDescendantAreSet()
        {
            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithPrimaryParent(productCategory11)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithPrimaryParent(productCategory12)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithPrimaryParent(productCategory12)
                .Build();

            this.Session.Derive();

            Assert.False(productCategory1.ExistProductCategoriesWhereDescendant);
            Assert.False(productCategory2.ExistProductCategoriesWhereDescendant);

            Assert.Equal(2, productCategory11.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory11.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory11.ProductCategoriesWhereDescendant);

            Assert.Equal(2, productCategory12.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory12.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory12.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory111.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory11, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory111.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory121.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory121.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory122.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory122.ProductCategoriesWhereDescendant);
        }

        [Fact]
        public void GivenProductCategory_WhenDeriving_ThenDisplayNameIsSet()
        {
            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithPrimaryParent(productCategory11)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithPrimaryParent(productCategory12)
                .Build();

            this.Session.Derive();

            Assert.Equal("1", productCategory1.DisplayName);
            Assert.Equal("2", productCategory2.DisplayName);
            Assert.Equal("1/1.1", productCategory11.DisplayName);
            Assert.Equal("1/1.2", productCategory12.DisplayName);
            Assert.Equal("1/1.1/1.1.1", productCategory111.DisplayName);
            Assert.Equal("1/1.2/1.2.1", productCategory121.DisplayName);
        }

        [Fact]
        public void GivenProductCategory_WhenNewParentsAreInserted_ThenProductCategoriesWhereDescendantAreRecalculated()
        {
            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithPrimaryParent(productCategory11)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithPrimaryParent(productCategory12)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithPrimaryParent(productCategory12)
                .Build();

            this.Session.Derive();

            Assert.False(productCategory1.ExistProductCategoriesWhereDescendant);
            Assert.False(productCategory2.ExistProductCategoriesWhereDescendant);

            Assert.Equal(2, productCategory11.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory11.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory11.ProductCategoriesWhereDescendant);

            Assert.Equal(2, productCategory12.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory12.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory12.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory111.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory11, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory111.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory121.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory121.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory122.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory122.ProductCategoriesWhereDescendant);

            var productCategory3 = new ProductCategoryBuilder(this.Session)
                .WithName("3")
                .Build();
            productCategory11.AddSecondaryParent(productCategory3);

            this.Session.Derive();

            Assert.False(productCategory1.ExistProductCategoriesWhereDescendant);
            Assert.False(productCategory2.ExistProductCategoriesWhereDescendant);
            Assert.False(productCategory3.ExistProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory11.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory11.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory11.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory3, productCategory11.ProductCategoriesWhereDescendant);

            Assert.Equal(2, productCategory12.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory12.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory12.ProductCategoriesWhereDescendant);

            Assert.Equal(4, productCategory111.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory11, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory3, productCategory111.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory121.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory121.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory122.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory122.ProductCategoriesWhereDescendant);

            var productCategory13 = new ProductCategoryBuilder(this.Session)
                .WithName("1.3")
                .WithPrimaryParent(productCategory1)
                .Build();
            productCategory122.AddSecondaryParent(productCategory13);

            this.Session.Derive();

            Assert.False(productCategory1.ExistProductCategoriesWhereDescendant);
            Assert.False(productCategory2.ExistProductCategoriesWhereDescendant);
            Assert.False(productCategory3.ExistProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory11.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory11.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory11.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory3, productCategory11.ProductCategoriesWhereDescendant);

            Assert.Equal(2, productCategory12.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory12.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory12.ProductCategoriesWhereDescendant);

            Assert.Single(productCategory13.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory13.ProductCategoriesWhereDescendant);

            Assert.Equal(4, productCategory111.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory11, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory3, productCategory111.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory121.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory121.ProductCategoriesWhereDescendant);

            Assert.Equal(4, productCategory122.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory13, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory122.ProductCategoriesWhereDescendant);
        }

        [Fact]
        public void GivenProductCategory_WhenNewParentsAreRemoved_ThenProductCategoriesWhereDescendantAreRecalculated()
        {
            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithPrimaryParent(productCategory11)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithPrimaryParent(productCategory12)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithPrimaryParent(productCategory12)
                .Build();

            this.Session.Derive();

            Assert.False(productCategory1.ExistProductCategoriesWhereDescendant);
            Assert.False(productCategory2.ExistProductCategoriesWhereDescendant);

            Assert.Equal(2, productCategory11.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory11.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory11.ProductCategoriesWhereDescendant);

            Assert.Equal(2, productCategory12.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory12.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory12.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory111.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory11, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory111.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory121.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory121.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory122.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory122.ProductCategoriesWhereDescendant);

            productCategory11.RemoveSecondaryParent(productCategory2);

            this.Session.Derive();

            Assert.False(productCategory1.ExistProductCategoriesWhereDescendant);
            Assert.False(productCategory2.ExistProductCategoriesWhereDescendant);

            Assert.Single(productCategory11.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory11.ProductCategoriesWhereDescendant);

            Assert.Equal(2, productCategory12.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory1, productCategory12.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory12.ProductCategoriesWhereDescendant);

            Assert.Equal(2, productCategory111.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory11, productCategory111.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory111.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory121.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory121.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory121.ProductCategoriesWhereDescendant);

            Assert.Equal(3, productCategory122.ProductCategoriesWhereDescendant.Count);
            Assert.Contains(productCategory12, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory1, productCategory122.ProductCategoriesWhereDescendant);
            Assert.Contains(productCategory2, productCategory122.ProductCategoriesWhereDescendant);
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
                .WithPrimaryParent(productCategory1)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithPrimaryParent(productCategory11)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithPrimaryParent(productCategory12)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithPrimaryParent(productCategory12)
                .Build();

            this.Session.Derive();

            Assert.Equal(2, productCategory1.Children.Count);
            Assert.Contains(productCategory11, productCategory1.Children);
            Assert.Contains(productCategory12, productCategory1.Children);

            Assert.Equal(1, productCategory2.Children.Count);
            Assert.Contains(productCategory12, productCategory2.Children);

            Assert.Single(productCategory11.Children);
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
            var good1 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good1")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("1")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            var good2 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good2")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good2")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("2")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            var good11 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good11")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good11")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("3")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            var good12 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good12")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good12")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("4")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            var good111 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good111")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good111")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("5")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            var good121 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good121")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good121")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("6")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            var good122 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good122")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good122")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("7")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .WithProduct(good1)
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .WithProduct(good2)
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithPrimaryParent(productCategory1)
                .WithProduct(good11)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .WithProduct(good12)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithPrimaryParent(productCategory11)
                .WithProduct(good111)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithPrimaryParent(productCategory12)
                .WithProduct(good121)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithPrimaryParent(productCategory12)
                .WithProduct(good122)
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

            Assert.Single(productCategory111.AllProducts);
            Assert.Contains(good111, productCategory111.AllProducts);

            Assert.Single(productCategory121.AllProducts);
            Assert.Contains(good121, productCategory121.AllProducts);

            Assert.Single(productCategory122.AllProducts);
            Assert.Contains(good122, productCategory122.AllProducts);
        }

        [Fact]
        public void GivenProductCategoryHierarchy_WhenDeriving_ThenAllSerialisedItemsForSaleAreSet()
        {
            var good1 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good1")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("1")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                            .Build())
                .Build();

            var serialisedItem1 = new SerialisedItemBuilder(this.Session).WithSerialNumber("1").WithAvailableForSale(true).Build();
            var serialisedItem1Not = new SerialisedItemBuilder(this.Session).WithSerialNumber("1Not").Build();  // This one must be excluded
            good1.Part.AddSerialisedItem(serialisedItem1);
            good1.Part.AddSerialisedItem(serialisedItem1Not);

            var good2 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good2")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good2")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("2")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                            .Build())
                .Build();

            var serialisedItem2a = new SerialisedItemBuilder(this.Session).WithSerialNumber("2a").WithAvailableForSale(true).Build();
            var serialisedItem2b = new SerialisedItemBuilder(this.Session).WithSerialNumber("2b").WithAvailableForSale(true).Build();
            good2.Part.AddSerialisedItem(serialisedItem2a);
            good2.Part.AddSerialisedItem(serialisedItem2b);

            var good11 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good11")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good11")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("3")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                            .Build())
                .Build();

            var serialisedItem11 = new SerialisedItemBuilder(this.Session).WithSerialNumber("11").WithAvailableForSale(true).Build();
            good11.Part.AddSerialisedItem(serialisedItem11);

            var good12 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good12")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good12")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("4")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                            .Build())
                .Build();

            var serialisedItem12 = new SerialisedItemBuilder(this.Session).WithSerialNumber("12").WithAvailableForSale(true).Build();
            good12.Part.AddSerialisedItem(serialisedItem12);

            var good111 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good111")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good111")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("5")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                            .Build())
                .Build();

            var serialisedItem111 = new SerialisedItemBuilder(this.Session).WithSerialNumber("111").WithAvailableForSale(true).Build();
            good111.Part.AddSerialisedItem(serialisedItem111);

            var good121 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good121")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good121")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("6")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                            .Build())
                .Build();

            var serialisedItem121 = new SerialisedItemBuilder(this.Session).WithSerialNumber("121").WithAvailableForSale(true).Build();
            good121.Part.AddSerialisedItem(serialisedItem121);

            var good122 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good122")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good122")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("7")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                            .Build())
                .Build();

            var serialisedItem122 = new SerialisedItemBuilder(this.Session).WithSerialNumber("122").WithAvailableForSale(true).Build();
            good122.Part.AddSerialisedItem(serialisedItem122);

            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .WithProduct(good1)
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .WithProduct(good2)
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithPrimaryParent(productCategory1)
                .WithProduct(good11)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .WithProduct(good12)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithPrimaryParent(productCategory11)
                .WithProduct(good111)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithPrimaryParent(productCategory12)
                .WithProduct(good121)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithPrimaryParent(productCategory12)
                .WithProduct(good122)
                .Build();

            this.Session.Derive();

            Assert.Equal(6, productCategory1.AllSerialisedItemsForSale.Count);
            Assert.Contains(serialisedItem1, productCategory1.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem11, productCategory1.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem12, productCategory1.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem111, productCategory1.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem121, productCategory1.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem122, productCategory1.AllSerialisedItemsForSale);

            Assert.Equal(5, productCategory2.AllSerialisedItemsForSale.Count);
            Assert.Contains(serialisedItem2a, productCategory2.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem2b, productCategory2.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem12, productCategory2.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem121, productCategory2.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem122, productCategory2.AllSerialisedItemsForSale);

            Assert.Equal(2, productCategory11.AllSerialisedItemsForSale.Count);
            Assert.Contains(serialisedItem11, productCategory11.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem111, productCategory11.AllSerialisedItemsForSale);

            Assert.Equal(3, productCategory12.AllSerialisedItemsForSale.Count);
            Assert.Contains(serialisedItem12, productCategory12.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem121, productCategory12.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem122, productCategory12.AllSerialisedItemsForSale);

            Assert.Single(productCategory111.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem111, productCategory111.AllSerialisedItemsForSale);

            Assert.Single(productCategory121.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem121, productCategory121.AllSerialisedItemsForSale);

            Assert.Single(productCategory122.AllSerialisedItemsForSale);
            Assert.Contains(serialisedItem122, productCategory122.AllSerialisedItemsForSale);
        }

        [Fact]
        public void GivenProductCategoryHierarchy_WhenDeriving_ThenAllNonSerialisedInventoryItemsForSaleAreSet()
        {
            var good1 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good1")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("1")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .WithDefaultFacility(this.InternalOrganisation.FacilitiesWhereOwner.First)
                            .Build())
                .Build();

            var item1 = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(good1.Part).WithNonSerialisedInventoryItemState(new NonSerialisedInventoryItemStates(this.Session).Good).Build();

            this.Session.Derive();

            var item1Not = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(good1.Part).WithNonSerialisedInventoryItemState(new NonSerialisedInventoryItemStates(this.Session).Scrap).Build();

            var good2 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good2")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good2")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("2")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .WithDefaultFacility(this.InternalOrganisation.FacilitiesWhereOwner.First)
                            .Build())
                .Build();

            var item2a = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(good2.Part).WithNonSerialisedInventoryItemState(new NonSerialisedInventoryItemStates(this.Session).Good).Build();
            var item2b = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(good2.Part).WithNonSerialisedInventoryItemState(new NonSerialisedInventoryItemStates(this.Session).SlightlyDamaged).Build();

            var good11 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good11")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good11")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("3")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .WithDefaultFacility(this.InternalOrganisation.FacilitiesWhereOwner.First)
                            .Build())
                .Build();

            var item11 = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(good11.Part).WithNonSerialisedInventoryItemState(new NonSerialisedInventoryItemStates(this.Session).Good).Build();

            var good12 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good12")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good12")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("4")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .WithDefaultFacility(this.InternalOrganisation.FacilitiesWhereOwner.First)
                            .Build())
                .Build();

            var item12 = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(good12.Part).WithNonSerialisedInventoryItemState(new NonSerialisedInventoryItemStates(this.Session).Good).Build();

            var good111 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good111")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good111")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("5")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .WithDefaultFacility(this.InternalOrganisation.FacilitiesWhereOwner.First)
                            .Build())
                .Build();

            var item111 = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(good111.Part).WithNonSerialisedInventoryItemState(new NonSerialisedInventoryItemStates(this.Session).Good).Build();

            var good121 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good121")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good121")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("6")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .WithDefaultFacility(this.InternalOrganisation.FacilitiesWhereOwner.First)
                            .Build())
                .Build();

            var item121 = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(good121.Part).WithNonSerialisedInventoryItemState(new NonSerialisedInventoryItemStates(this.Session).Good).Build();

            var good122 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good122")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good122")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("7")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .WithDefaultFacility(this.InternalOrganisation.FacilitiesWhereOwner.First)
                            .Build())
                .Build();

            var item122 = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(good122.Part).WithNonSerialisedInventoryItemState(new NonSerialisedInventoryItemStates(this.Session).Good).Build();

            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("1")
                .WithProduct(good1)
                .Build();
            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("2")
                .WithProduct(good2)
                .Build();
            var productCategory11 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1")
                .WithPrimaryParent(productCategory1)
                .WithProduct(good11)
                .Build();
            var productCategory12 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2")
                .WithPrimaryParent(productCategory1)
                .WithSecondaryParent(productCategory2)
                .WithProduct(good12)
                .Build();
            var productCategory111 = new ProductCategoryBuilder(this.Session)
                .WithName("1.1.1")
                .WithPrimaryParent(productCategory11)
                .WithProduct(good111)
                .Build();
            var productCategory121 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.1")
                .WithPrimaryParent(productCategory12)
                .WithProduct(good121)
                .Build();
            var productCategory122 = new ProductCategoryBuilder(this.Session)
                .WithName("1.2.2")
                .WithPrimaryParent(productCategory12)
                .WithProduct(good122)
                .Build();

            this.Session.Derive();

            Assert.Equal(6, productCategory1.AllNonSerialisedInventoryItemsForSale.Count);
            Assert.Contains(item1, productCategory1.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item11, productCategory1.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item12, productCategory1.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item111, productCategory1.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item121, productCategory1.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item122, productCategory1.AllNonSerialisedInventoryItemsForSale);

            Assert.Equal(5, productCategory2.AllNonSerialisedInventoryItemsForSale.Count);
            Assert.Contains(item2a, productCategory2.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item2b, productCategory2.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item12, productCategory2.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item121, productCategory2.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item122, productCategory2.AllNonSerialisedInventoryItemsForSale);

            Assert.Equal(2, productCategory11.AllNonSerialisedInventoryItemsForSale.Count);
            Assert.Contains(item11, productCategory11.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item111, productCategory11.AllNonSerialisedInventoryItemsForSale);

            Assert.Equal(3, productCategory12.AllNonSerialisedInventoryItemsForSale.Count);
            Assert.Contains(item12, productCategory12.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item121, productCategory12.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item122, productCategory12.AllNonSerialisedInventoryItemsForSale);

            Assert.Single(productCategory111.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item111, productCategory111.AllNonSerialisedInventoryItemsForSale);

            Assert.Single(productCategory121.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item121, productCategory121.AllNonSerialisedInventoryItemsForSale);

            Assert.Single(productCategory122.AllNonSerialisedInventoryItemsForSale);
            Assert.Contains(item122, productCategory122.AllNonSerialisedInventoryItemsForSale);
        }
    }
}
