// <copyright file="FeeEditTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.OrderAdjustmentTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using libs.angular.material.@base.src.export.objects.orderadjustment.edit;
    using libs.angular.material.@base.src.export.objects.productquote.list;
    using libs.angular.material.@base.src.export.objects.productquote.overview;
    using libs.angular.material.@base.src.export.objects.purchaseinvoice.list;
    using libs.angular.material.@base.src.export.objects.purchaseinvoice.overview;
    using libs.angular.material.@base.src.export.objects.salesinvoice.list;
    using libs.angular.material.@base.src.export.objects.salesinvoice.overview;
    using libs.angular.material.@base.src.export.objects.salesorder.list;
    using libs.angular.material.@base.src.export.objects.salesorder.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Order")]
    public class FeeEditTests : Test
    {
        private ProductQuoteListComponent quoteListPage;
        private SalesOrderListComponent salesOrderListPage;
        private SalesInvoiceListComponent salesInvoiceListPage;
        private PurchaseInvoiceListComponent purchaseInvoiceListPage;

        public FeeEditTests(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
        }

        [Fact]
        public void EditForProductQuote()
        {
            this.quoteListPage = this.Sidenav.NavigateToProductQuotes();

            var quote = new ProductQuotes(this.Session).Extent().First;
            quote.AddOrderAdjustment(new FeeBuilder(this.Session).WithAmountDefaults().Build());

            this.Session.Derive();
            this.Session.Commit();

            var before = new OrderAdjustments(this.Session).Extent().ToArray();

            var expected = new FeeBuilder(this.Session).WithAmountDefaults().Build();

            var fee = quote.OrderAdjustments.First();
            var id = fee.Id;

            this.Session.Derive();

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.quoteListPage.Table.DefaultAction(quote);
            var quoteOverview = new ProductQuoteOverviewComponent(this.quoteListPage.Driver);
            var adjustmentOverviewPanel = quoteOverview.OrderadjustmentOverviewPanel.Click();

            adjustmentOverviewPanel.Table.DefaultAction(fee);

            var adjustmentEdit = new OrderAdjustmentEditComponent(this.Driver);

            adjustmentEdit.Amount.Set(expected.Amount.ToString());
            adjustmentEdit.Description.Set(expected.Description);

            this.Session.Rollback();
            adjustmentEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new OrderAdjustments(this.Session).Extent().ToArray();

            var actual = (Fee)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void EditForSalesOrder()
        {
            this.salesOrderListPage = this.Sidenav.NavigateToSalesOrders();

            var salesOrder = new SalesOrders(this.Session).Extent().First;
            salesOrder.AddOrderAdjustment(new FeeBuilder(this.Session).WithAmountDefaults().Build());

            this.Session.Derive();
            this.Session.Commit();

            var before = new OrderAdjustments(this.Session).Extent().ToArray();

            var expected = new FeeBuilder(this.Session).WithAmountDefaults().Build();

            var fee = salesOrder.OrderAdjustments.First();
            var id = fee.Id;

            this.Session.Derive();

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.salesOrderListPage.Table.DefaultAction(salesOrder);
            var salesOrderOverview = new SalesOrderOverviewComponent(this.salesOrderListPage.Driver);
            var adjustmentOverviewPanel = salesOrderOverview.OrderadjustmentOverviewPanel.Click();

            adjustmentOverviewPanel.Table.DefaultAction(fee);

            var adjustmentEdit = new OrderAdjustmentEditComponent(this.Driver);

            adjustmentEdit.Amount.Set(expected.Amount.ToString());
            adjustmentEdit.Description.Set(expected.Description);

            this.Session.Rollback();
            adjustmentEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new OrderAdjustments(this.Session).Extent().ToArray();

            var actual = (Fee)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void EditForSalesInvoice()
        {
            this.salesInvoiceListPage = this.Sidenav.NavigateToSalesInvoices();

            var salesInvoice = new SalesInvoices(this.Session).Extent().First;
            salesInvoice.AddOrderAdjustment(new FeeBuilder(this.Session).WithAmountDefaults().Build());

            this.Session.Derive();
            this.Session.Commit();

            var before = new OrderAdjustments(this.Session).Extent().ToArray();

            var expected = new FeeBuilder(this.Session).WithAmountDefaults().Build();

            var fee = salesInvoice.OrderAdjustments.First();
            var id = fee.Id;

            this.Session.Derive();

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.salesInvoiceListPage.Table.DefaultAction(salesInvoice);
            var salesInvoiceOverview = new SalesInvoiceOverviewComponent(this.salesInvoiceListPage.Driver);
            var adjustmentOverviewPanel = salesInvoiceOverview.OrderadjustmentOverviewPanel.Click();

            adjustmentOverviewPanel.Table.DefaultAction(fee);

            var adjustmentEdit = new OrderAdjustmentEditComponent(this.Driver);

            adjustmentEdit.Amount.Set(expected.Amount.ToString());
            adjustmentEdit.Description.Set(expected.Description);

            this.Session.Rollback();
            adjustmentEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new OrderAdjustments(this.Session).Extent().ToArray();

            var actual = (Fee)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void EditForPurchaseInvoice()
        {
            this.purchaseInvoiceListPage = this.Sidenav.NavigateToPurchaseInvoices();

            var purchaseInvoice = new PurchaseInvoices(this.Session).Extent().First;
            purchaseInvoice.AddOrderAdjustment(new FeeBuilder(this.Session).WithAmountDefaults().Build());

            this.Session.Derive();
            this.Session.Commit();

            var before = new OrderAdjustments(this.Session).Extent().ToArray();

            var expected = new FeeBuilder(this.Session).WithAmountDefaults().Build();

            var fee = purchaseInvoice.OrderAdjustments.First();
            var id = fee.Id;

            this.Session.Derive();

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.purchaseInvoiceListPage.Table.DefaultAction(purchaseInvoice);
            var purchaseInvoiceOverview = new PurchaseInvoiceOverviewComponent(this.purchaseInvoiceListPage.Driver);
            var adjustmentOverviewPanel = purchaseInvoiceOverview.OrderadjustmentOverviewPanel.Click();

            adjustmentOverviewPanel.Table.DefaultAction(fee);

            var adjustmentEdit = new OrderAdjustmentEditComponent(this.Driver);

            adjustmentEdit.Amount.Set(expected.Amount.ToString());
            adjustmentEdit.Description.Set(expected.Description);

            this.Session.Rollback();
            adjustmentEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new OrderAdjustments(this.Session).Extent().ToArray();

            var actual = (Fee)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }
    }
}
