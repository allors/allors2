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
    public class SalesOrderItemEditTest : Test
    {
        private readonly SalesOrderListComponent salesOrderListPage;
        private readonly Organisation internalOrganisation;

        public SalesOrderItemEditTest(TestFixture fixture)
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
        public void EditWithDefaults()
        {
            var salesOrder = new SalesOrders(this.Session).Extent().FirstOrDefault();

            var before = new SalesOrderItems(this.Session).Extent().ToArray();

            var disposableSalesOrder = new SalesOrderBuilder(this.Session).WithOrganisationInternalDefaults(this.internalOrganisation).Build();
            var expected = disposableSalesOrder.SalesOrderItems.First(v => !(v.InvoiceItemType.IsProductItem || v.InvoiceItemType.IsPartItem));

            var salesOrderItem = salesOrder.SalesOrderItems.First(v => !(v.InvoiceItemType.IsProductItem || v.InvoiceItemType.IsPartItem));
            var id = salesOrderItem.Id;

            this.Session.Derive();

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

            salesOrderItemOverviewPanel.Table.DefaultAction(salesOrderItem);

            var salesOrderItemEdit = new SalesOrderItemEditComponent(this.Driver);

            salesOrderItemEdit.Description.Set(expected.Description);
            salesOrderItemEdit.Comment.Set(expected.Comment);
            salesOrderItemEdit.InternalComment.Set(expected.InternalComment);
            salesOrderItemEdit.PriceableAssignedUnitPrice_1.Set(expected.AssignedUnitPrice.ToString());

            this.Session.Rollback();
            salesOrderItemEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrderItems(this.Session).Extent().ToArray();

            var actual = (SalesOrderItem)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedDescription, actual.Description);
            Assert.Equal(expectedComment, actual.Comment);
            Assert.Equal(expectedInternalComment, actual.InternalComment);
            Assert.Equal(expectedAssignedUnitPrice, actual.AssignedUnitPrice);
        }

        /**
         * MinimalWithSerializedProductItemDefaults
         **/
        [Fact]
        public void EditWithSerialisedProductItemDefaults()
        {
            var salesOrder = new SalesOrders(this.Session).Extent().FirstOrDefault();

            var before = new SalesOrderItems(this.Session).Extent().ToArray();

            var disposableSalesOrder = new SalesOrderBuilder(this.Session).WithOrganisationInternalDefaults(this.internalOrganisation).Build();
            var expected = disposableSalesOrder.SalesOrderItems.First(v => v.InvoiceItemType.IsProductItem);

            var salesOrderItem = salesOrder.SalesOrderItems.First(v => v.InvoiceItemType.IsProductItem);
            var id = salesOrderItem.Id;

            this.Session.Derive();

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

            salesOrderItemOverviewPanel.Table.DefaultAction(salesOrderItem);

            var salesOrderItemEdit = new SalesOrderItemEditComponent(this.Driver);

            salesOrderItemEdit.Description.Set(expected.Description);
            salesOrderItemEdit.Comment.Set(expected.Comment);
            salesOrderItemEdit.InternalComment.Set(expected.InternalComment);
            salesOrderItemEdit.PriceableAssignedUnitPrice_2.Set(expected.AssignedUnitPrice.ToString());

            this.Session.Rollback();
            salesOrderItemEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrderItems(this.Session).Extent().ToArray();

            var actual = (SalesOrderItem)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedDescription, actual.Description);
            Assert.Equal(expectedComment, actual.Comment);
            Assert.Equal(expectedInternalComment, actual.InternalComment);
            Assert.Equal(expectedInvoiceItemType, actual.InvoiceItemType);
            Assert.Equal(expectedAssignedUnitPrice, actual.AssignedUnitPrice);
        }

        [Fact]
        public void EditWithNonSerialisedProductItemDefaults()
        {
            var salesOrder = new SalesOrders(this.Session).Extent().FirstOrDefault();

            var before = new SalesOrderItems(this.Session).Extent().ToArray();

            var disposableSalesOrder = new SalesOrderBuilder(this.Session).WithOrganisationInternalDefaults(this.internalOrganisation).Build();
            var expected = disposableSalesOrder.SalesOrderItems.First(v => v.InvoiceItemType.IsProductItem);

            var salesOrderItem = salesOrder.SalesOrderItems.First(v => v.InvoiceItemType.IsProductItem);
            var id = salesOrderItem.Id;

            this.Session.Derive();

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

            salesOrderItemOverviewPanel.Table.DefaultAction(salesOrderItem);

            var salesOrderItemEdit = new SalesOrderItemEditComponent(this.Driver);

            salesOrderItemEdit.Description.Set(expected.Description);
            salesOrderItemEdit.Comment.Set(expected.Comment);
            salesOrderItemEdit.InternalComment.Set(expected.InternalComment);
            salesOrderItemEdit.PriceableAssignedUnitPrice_2.Set(expected.AssignedUnitPrice.ToString());

            this.Session.Rollback();
            salesOrderItemEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrderItems(this.Session).Extent().ToArray();

            var actual = (SalesOrderItem)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedDescription, actual.Description);
            Assert.Equal(expectedComment, actual.Comment);
            Assert.Equal(expectedInternalComment, actual.InternalComment);
            Assert.Equal(expectedInvoiceItemType, actual.InvoiceItemType);
            Assert.Equal(expectedAssignedUnitPrice, actual.AssignedUnitPrice);
        }

        [Fact]
        public void EditWithNonSerialisedPartItemDefaults()
        {
        }
    }
}
