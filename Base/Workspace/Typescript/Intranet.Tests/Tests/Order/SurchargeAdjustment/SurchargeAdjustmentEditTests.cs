// <copyright file="SurchargeAdjustmentEditTests.cs" company="Allors bvba">
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
    using src.allors.material.@base.objects.orderadjustment.edit;
    using src.allors.material.@base.objects.productquote.list;
    using src.allors.material.@base.objects.productquote.overview;
    using src.allors.material.@base.objects.purchaseinvoice.list;
    using src.allors.material.@base.objects.purchaseinvoice.overview;
    using src.allors.material.@base.objects.salesinvoice.list;
    using src.allors.material.@base.objects.salesinvoice.overview;
    using src.allors.material.@base.objects.salesorder.list;
    using src.allors.material.@base.objects.salesorder.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Order")]
    public class SurchargeAdjustmentEditTests : Test
    {
        private ProductQuoteListComponent quoteListPage;
        private SalesOrderListComponent salesOrderListPage;
        private SalesInvoiceListComponent salesInvoiceListPage;
        private PurchaseInvoiceListComponent purchaseInvoiceListPage;

        public SurchargeAdjustmentEditTests(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
        }

        [Fact]
        public void EditForProductQuote()
        {
            this.quoteListPage = this.Sidenav.NavigateToProductQuotes();

            var quote = new ProductQuotes(this.Session).Extent().First;
            quote.AddOrderAdjustment(new SurchargeAdjustmentBuilder(this.Session).WithAmountDefaults().Build());

            this.Session.Derive();
            this.Session.Commit();

            var before = new OrderAdjustments(this.Session).Extent().ToArray();

            var expected = new SurchargeAdjustmentBuilder(this.Session).WithAmountDefaults().Build();

            var surchargeAdjustment = quote.OrderAdjustments.First();
            var id = surchargeAdjustment.Id;

            this.Session.Derive();

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.quoteListPage.Table.DefaultAction(quote);
            var quoteOverview = new ProductQuoteOverviewComponent(this.quoteListPage.Driver);
            var adjustmentOverviewPanel = quoteOverview.OrderadjustmentOverviewPanel.Click();

            adjustmentOverviewPanel.Table.DefaultAction(surchargeAdjustment);

            var adjustmentEdit = new OrderAdjustmentEditComponent(this.Driver);

            adjustmentEdit.Amount.Set(expected.Amount.ToString());
            adjustmentEdit.Description.Set(expected.Description);

            this.Session.Rollback();
            adjustmentEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new OrderAdjustments(this.Session).Extent().ToArray();

            var actual = (SurchargeAdjustment)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void EditForSalesOrder()
        {
            this.salesOrderListPage = this.Sidenav.NavigateToSalesOrders();

            var salesOrder = new SalesOrders(this.Session).Extent().First;
            salesOrder.AddOrderAdjustment(new SurchargeAdjustmentBuilder(this.Session).WithAmountDefaults().Build());

            this.Session.Derive();
            this.Session.Commit();

            var before = new OrderAdjustments(this.Session).Extent().ToArray();

            var expected = new SurchargeAdjustmentBuilder(this.Session).WithAmountDefaults().Build();

            var surchargeAdjustment = salesOrder.OrderAdjustments.First();
            var id = surchargeAdjustment.Id;

            this.Session.Derive();

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.salesOrderListPage.Table.DefaultAction(salesOrder);
            var salesOrderOverview = new SalesOrderOverviewComponent(this.salesOrderListPage.Driver);
            var adjustmentOverviewPanel = salesOrderOverview.OrderadjustmentOverviewPanel.Click();

            adjustmentOverviewPanel.Table.DefaultAction(surchargeAdjustment);

            var adjustmentEdit = new OrderAdjustmentEditComponent(this.Driver);

            adjustmentEdit.Amount.Set(expected.Amount.ToString());
            adjustmentEdit.Description.Set(expected.Description);

            this.Session.Rollback();
            adjustmentEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new OrderAdjustments(this.Session).Extent().ToArray();

            var actual = (SurchargeAdjustment)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void EditForSalesInvoice()
        {
            this.salesInvoiceListPage = this.Sidenav.NavigateToSalesInvoices();

            var salesInvoice = new SalesInvoices(this.Session).Extent().First;
            salesInvoice.AddOrderAdjustment(new SurchargeAdjustmentBuilder(this.Session).WithAmountDefaults().Build());

            this.Session.Derive();
            this.Session.Commit();

            var before = new OrderAdjustments(this.Session).Extent().ToArray();

            var expected = new SurchargeAdjustmentBuilder(this.Session).WithAmountDefaults().Build();

            var surchargeAdjustment = salesInvoice.OrderAdjustments.First();
            var id = surchargeAdjustment.Id;

            this.Session.Derive();

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.salesInvoiceListPage.Table.DefaultAction(salesInvoice);
            var salesInvoiceOverview = new SalesInvoiceOverviewComponent(this.salesInvoiceListPage.Driver);
            var adjustmentOverviewPanel = salesInvoiceOverview.OrderadjustmentOverviewPanel.Click();

            adjustmentOverviewPanel.Table.DefaultAction(surchargeAdjustment);

            var adjustmentEdit = new OrderAdjustmentEditComponent(this.Driver);

            adjustmentEdit.Amount.Set(expected.Amount.ToString());
            adjustmentEdit.Description.Set(expected.Description);

            this.Session.Rollback();
            adjustmentEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new OrderAdjustments(this.Session).Extent().ToArray();

            var actual = (SurchargeAdjustment)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void EditForPurchaseInvoice()
        {
            this.purchaseInvoiceListPage = this.Sidenav.NavigateToPurchaseInvoices();

            var purchaseInvoice = new PurchaseInvoices(this.Session).Extent().First;
            purchaseInvoice.AddOrderAdjustment(new SurchargeAdjustmentBuilder(this.Session).WithAmountDefaults().Build());

            this.Session.Derive();
            this.Session.Commit();

            var before = new OrderAdjustments(this.Session).Extent().ToArray();

            var expected = new SurchargeAdjustmentBuilder(this.Session).WithAmountDefaults().Build();

            var surchargeAdjustment = purchaseInvoice.OrderAdjustments.First();
            var id = surchargeAdjustment.Id;

            this.Session.Derive();

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.purchaseInvoiceListPage.Table.DefaultAction(purchaseInvoice);
            var purchaseInvoiceOverview = new PurchaseInvoiceOverviewComponent(this.purchaseInvoiceListPage.Driver);
            var adjustmentOverviewPanel = purchaseInvoiceOverview.OrderadjustmentOverviewPanel.Click();

            adjustmentOverviewPanel.Table.DefaultAction(surchargeAdjustment);

            var adjustmentEdit = new OrderAdjustmentEditComponent(this.Driver);

            adjustmentEdit.Amount.Set(expected.Amount.ToString());
            adjustmentEdit.Description.Set(expected.Description);

            this.Session.Rollback();
            adjustmentEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new OrderAdjustments(this.Session).Extent().ToArray();

            var actual = (SurchargeAdjustment)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }
    }
}
