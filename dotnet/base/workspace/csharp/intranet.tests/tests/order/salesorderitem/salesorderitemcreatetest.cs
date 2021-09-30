// <copyright file="SalesOrderItemCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.SalesOrderItemTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using src.allors.material.@base.objects.salesorder.list;
    using src.allors.material.@base.objects.salesorder.overview;
    using src.allors.material.@base.objects.salesorderitem.edit;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Order")]
    public class SalesOrderItemCreateTest : Test
    {
        private readonly SalesOrderListComponent salesOrderListPage;
        private readonly Organisation internalOrganisation;

        public SalesOrderItemCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            this.Login();
            this.salesOrderListPage = this.Sidenav.NavigateToSalesOrders();
        }

        /**
         * MinimalWithDefaults except Product/Part Item
         **/
        [Fact]
        public void CreateWithDefaults()
        {
            var salesOrder = new SalesOrders(this.Session).Extent().FirstOrDefault();

            var before = new SalesOrderItems(this.Session).Extent().ToArray();

            var expected = new SalesOrderItemBuilder(this.Session).WithDefaults().Build();
            salesOrder.AddSalesOrderItem(expected);

            this.Session.Derive();

            Assert.True(expected.ExistDescription);
            Assert.True(expected.ExistComment);
            Assert.True(expected.ExistInternalComment);
            Assert.True(expected.ExistInvoiceItemType);
            Assert.True(expected.ExistAssignedUnitPrice);

            var expectedDescription = expected.Description;
            var expectedComment = expected.Comment;
            var expectedInternalComment = expected.InternalComment;
            var expectedInvoiceItemType = expected.InvoiceItemType;
            var expectedAssignedUnitPrice = expected.AssignedUnitPrice;

            this.salesOrderListPage.Table.DefaultAction(salesOrder);
            var salesOrderOverview = new SalesOrderOverviewComponent(this.salesOrderListPage.Driver);
            var salesOrderItemOverviewPanel = salesOrderOverview.SalesorderitemOverviewPanel.Click();

            var salesOrderItemCreate = salesOrderItemOverviewPanel
                .CreateSalesOrderItem()
                .BuildForDefaults(expected);

            this.Session.Rollback();
            salesOrderItemCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrderItems(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedDescription, actual.Description);
            Assert.Equal(expectedComment, actual.Comment);
            Assert.Equal(expectedInternalComment, actual.InternalComment);
            Assert.Equal(expectedInvoiceItemType, actual.InvoiceItemType);
            Assert.Equal(expectedAssignedUnitPrice, actual.AssignedUnitPrice);
        }

        /**
         * MinimalWithProductItemDefaults
         **/
        [Fact]
        public void CreateWithProductItemDefaults()
        {
            var salesOrder = new SalesOrders(this.Session).Extent().FirstOrDefault();

            var before = new SalesOrderItems(this.Session).Extent().ToArray();

            var expected = new SalesOrderItemBuilder(this.Session).WithProductItemDefaults().Build();
            salesOrder.AddSalesOrderItem(expected);

            this.Session.Derive();

            Assert.True(expected.ExistDescription);
            Assert.True(expected.ExistComment);
            Assert.True(expected.ExistInternalComment);
            Assert.True(expected.ExistInvoiceItemType);
            Assert.True(expected.ExistProduct);
            Assert.True(expected.ExistSerialisedItem);
            Assert.True(expected.ExistQuantityOrdered);
            Assert.True(expected.ExistAssignedUnitPrice);

            var expectedDescription = expected.Description;
            var expectedComment = expected.Comment;
            var expectedInternalComment = expected.InternalComment;
            var expectedInvoiceItemType = expected.InvoiceItemType;
            var expectedProduct = expected.Product;
            var expectedSerialisedItem = expected.SerialisedItem;
            var expectedQuantityOrdered = expected.QuantityOrdered;
            var expectedAssignedUnitPrice = expected.AssignedUnitPrice;

            this.salesOrderListPage.Table.DefaultAction(salesOrder);
            var salesOrderOverview = new SalesOrderOverviewComponent(this.salesOrderListPage.Driver);
            var salesOrderItemOverviewPanel = salesOrderOverview.SalesorderitemOverviewPanel.Click();

            var salesOrderItemCreate = salesOrderItemOverviewPanel
                .CreateSalesOrderItem()
                .BuildForProductItemDefaults(expected);

            this.Session.Rollback();
            salesOrderItemCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrderItems(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedDescription, actual.Description);
            Assert.Equal(expectedComment, actual.Comment);
            Assert.Equal(expectedInternalComment, actual.InternalComment);
            Assert.Equal(expectedInvoiceItemType, actual.InvoiceItemType);
            Assert.Equal(expectedProduct, actual.Product);
            Assert.Equal(expectedSerialisedItem, actual.SerialisedItem);
            Assert.Equal(expectedQuantityOrdered, actual.QuantityOrdered);
            Assert.Equal(expectedAssignedUnitPrice, actual.AssignedUnitPrice);
        }

        /**
         * MinimalWithPartItemDefaults
         **/
        [Fact]
        public void CreateWithPartItemDefaults()
        {
            var salesOrder = new SalesOrders(this.Session).Extent().FirstOrDefault();

            var before = new SalesOrderItems(this.Session).Extent().ToArray();

            var expected = new SalesOrderItemBuilder(this.Session).WithPartItemDefaults().Build();
            salesOrder.AddSalesOrderItem(expected);

            this.Session.Derive();

            Assert.True(expected.ExistDescription);
            Assert.True(expected.ExistComment);
            Assert.True(expected.ExistInternalComment);
            Assert.True(expected.ExistInvoiceItemType);
            Assert.True(expected.ExistProduct);
            Assert.True(expected.ExistSerialisedItem);
            Assert.True(expected.ExistQuantityOrdered);
            Assert.True(expected.ExistAssignedUnitPrice);

            var expectedDescription = expected.Description;
            var expectedComment = expected.Comment;
            var expectedInternalComment = expected.InternalComment;
            var expectedInvoiceItemType = expected.InvoiceItemType;
            var expectedProduct = expected.Product;
            var expectedSerialisedItem = expected.SerialisedItem;
            var expectedQuantityOrdered = expected.QuantityOrdered;
            var expectedAssignedUnitPrice = expected.AssignedUnitPrice;

            this.salesOrderListPage.Table.DefaultAction(salesOrder);
            var salesOrderOverview = new SalesOrderOverviewComponent(this.salesOrderListPage.Driver);
            var salesOrderItemOverviewPanel = salesOrderOverview.SalesorderitemOverviewPanel.Click();

            var salesOrderItemCreate = salesOrderItemOverviewPanel
                .CreateSalesOrderItem()
                .BuildForProductItemDefaults(expected);

            this.Session.Rollback();
            salesOrderItemCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrderItems(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedDescription, actual.Description);
            Assert.Equal(expectedComment, actual.Comment);
            Assert.Equal(expectedInternalComment, actual.InternalComment);
            Assert.Equal(expectedInvoiceItemType, actual.InvoiceItemType);
            Assert.Equal(expectedProduct, actual.Product);
            Assert.Equal(expectedSerialisedItem, actual.SerialisedItem);
            Assert.Equal(expectedQuantityOrdered, actual.QuantityOrdered);
            Assert.Equal(expectedAssignedUnitPrice, actual.AssignedUnitPrice);
        }
    }
}
