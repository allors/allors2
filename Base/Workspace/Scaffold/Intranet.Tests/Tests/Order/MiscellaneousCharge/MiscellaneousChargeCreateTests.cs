// <copyright file="MiscellaneousChargeCreateTests.cs" company="Allors bvba">
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
    public class MiscellaneousChargeCreateTests : Test
    {
        private ProductQuoteListComponent quoteListPage;
        private SalesOrderListComponent salesOrderListPage;
        private SalesInvoiceListComponent salesInvoiceListPage;
        private PurchaseInvoiceListComponent purchaseInvoiceListPage;

        public MiscellaneousChargeCreateTests(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
        }

        [Fact]
        public void CreateAmountForProductQuote()
        {
            this.quoteListPage = this.Sidenav.NavigateToProductQuotes();

            var quote = new ProductQuotes(this.Session).Extent().First;

            var before = new MiscellaneousCharges(this.Session).Extent().ToArray();

            var expected = new MiscellaneousChargeBuilder(this.Session).WithAmountDefaults().Build();
            quote.AddOrderAdjustment(expected);

            this.Session.Derive();

            Assert.True(expected.ExistAmount);
            Assert.True(expected.ExistDescription);

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.quoteListPage.Table.DefaultAction(quote);
            var miscellaneousChargeCreate = new ProductQuoteOverviewComponent(this.quoteListPage.Driver).OrderadjustmentOverviewPanel.Click().CreateMiscellaneousCharge();

            miscellaneousChargeCreate
                .Amount.Set(expectedAmount.ToString())
                .Description.Set(expectedDescription);

            this.Session.Rollback();
            miscellaneousChargeCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new MiscellaneousCharges(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void CreatePercentageForProductQuote()
        {
            this.quoteListPage = this.Sidenav.NavigateToProductQuotes();

            var quote = new ProductQuotes(this.Session).Extent().First;

            var before = new MiscellaneousCharges(this.Session).Extent().ToArray();

            var expected = new MiscellaneousChargeBuilder(this.Session).WithPercentageDefaults().Build();
            quote.AddOrderAdjustment(expected);

            this.Session.Derive();

            Assert.True(expected.ExistPercentage);
            Assert.True(expected.ExistDescription);

            var expectedPercentage = expected.Percentage;
            var expectedDescription = expected.Description;

            this.quoteListPage.Table.DefaultAction(quote);
            var miscellaneousChargeCreate = new ProductQuoteOverviewComponent(this.quoteListPage.Driver).OrderadjustmentOverviewPanel.Click().CreateMiscellaneousCharge();

            miscellaneousChargeCreate
                .Percentage.Set(expectedPercentage.ToString())
                .Description.Set(expectedDescription);

            this.Session.Rollback();
            miscellaneousChargeCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new MiscellaneousCharges(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedPercentage, actual.Percentage);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void CreateAmountForSalesOrder()
        {
            this.salesOrderListPage = this.Sidenav.NavigateToSalesOrders();

            var salesOrder = new SalesOrders(this.Session).Extent().First;

            var before = new MiscellaneousCharges(this.Session).Extent().ToArray();

            var expected = new MiscellaneousChargeBuilder(this.Session).WithAmountDefaults().Build();
            salesOrder.AddOrderAdjustment(expected);

            this.Session.Derive();

            Assert.True(expected.ExistAmount);
            Assert.True(expected.ExistDescription);

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.salesOrderListPage.Table.DefaultAction(salesOrder);
            var miscellaneousChargeCreate = new SalesOrderOverviewComponent(this.salesOrderListPage.Driver).OrderadjustmentOverviewPanel.Click().CreateMiscellaneousCharge();

            miscellaneousChargeCreate
                .Amount.Set(expectedAmount.ToString())
                .Description.Set(expectedDescription);

            this.Session.Rollback();
            miscellaneousChargeCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new MiscellaneousCharges(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void CreatePercentageForSalesOrder()
        {
            this.salesOrderListPage = this.Sidenav.NavigateToSalesOrders();

            var salesOrder = new SalesOrders(this.Session).Extent().First;

            var before = new MiscellaneousCharges(this.Session).Extent().ToArray();

            var expected = new MiscellaneousChargeBuilder(this.Session).WithPercentageDefaults().Build();
            salesOrder.AddOrderAdjustment(expected);

            this.Session.Derive();

            Assert.True(expected.ExistPercentage);
            Assert.True(expected.ExistDescription);

            var expectedPercentage = expected.Percentage;
            var expectedDescription = expected.Description;

            this.salesOrderListPage.Table.DefaultAction(salesOrder);
            var miscellaneousChargeCreate = new SalesOrderOverviewComponent(this.salesOrderListPage.Driver).OrderadjustmentOverviewPanel.Click().CreateMiscellaneousCharge();

            miscellaneousChargeCreate
                .Percentage.Set(expectedPercentage.ToString())
                .Description.Set(expectedDescription);

            this.Session.Rollback();
            miscellaneousChargeCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new MiscellaneousCharges(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedPercentage, actual.Percentage);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void CreateAmountForSalesInvoice()
        {
            this.salesInvoiceListPage = this.Sidenav.NavigateToSalesInvoices();

            var salesInvoice = new SalesInvoices(this.Session).Extent().First;

            var before = new MiscellaneousCharges(this.Session).Extent().ToArray();

            var expected = new MiscellaneousChargeBuilder(this.Session).WithAmountDefaults().Build();
            salesInvoice.AddOrderAdjustment(expected);

            this.Session.Derive();

            Assert.True(expected.ExistAmount);
            Assert.True(expected.ExistDescription);

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.salesInvoiceListPage.Table.DefaultAction(salesInvoice);
            var miscellaneousChargeCreate = new SalesInvoiceOverviewComponent(this.salesInvoiceListPage.Driver).OrderadjustmentOverviewPanel.Click().CreateMiscellaneousCharge();

            miscellaneousChargeCreate
                .Amount.Set(expectedAmount.ToString())
                .Description.Set(expectedDescription);

            this.Session.Rollback();
            miscellaneousChargeCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new MiscellaneousCharges(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void CreatePercentageForSalesInvoice()
        {
            this.salesInvoiceListPage = this.Sidenav.NavigateToSalesInvoices();

            var salesInvoice = new SalesInvoices(this.Session).Extent().First;

            var before = new MiscellaneousCharges(this.Session).Extent().ToArray();

            var expected = new MiscellaneousChargeBuilder(this.Session).WithPercentageDefaults().Build();
            salesInvoice.AddOrderAdjustment(expected);

            this.Session.Derive();

            Assert.True(expected.ExistPercentage);
            Assert.True(expected.ExistDescription);

            var expectedPercentage = expected.Percentage;
            var expectedDescription = expected.Description;

            this.salesInvoiceListPage.Table.DefaultAction(salesInvoice);
            var miscellaneousChargeCreate = new SalesInvoiceOverviewComponent(this.salesInvoiceListPage.Driver).OrderadjustmentOverviewPanel.Click().CreateMiscellaneousCharge();

            miscellaneousChargeCreate
                .Percentage.Set(expectedPercentage.ToString())
                .Description.Set(expectedDescription);

            this.Session.Rollback();
            miscellaneousChargeCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new MiscellaneousCharges(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedPercentage, actual.Percentage);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void CreateAmountForPurchaseInvoice()
        {
            this.purchaseInvoiceListPage = this.Sidenav.NavigateToPurchaseInvoices();

            var purchaseInvoice = new PurchaseInvoices(this.Session).Extent().First;

            var before = new MiscellaneousCharges(this.Session).Extent().ToArray();

            var expected = new MiscellaneousChargeBuilder(this.Session).WithAmountDefaults().Build();
            purchaseInvoice.AddOrderAdjustment(expected);

            this.Session.Derive();

            Assert.True(expected.ExistAmount);
            Assert.True(expected.ExistDescription);

            var expectedAmount = expected.Amount;
            var expectedDescription = expected.Description;

            this.purchaseInvoiceListPage.Table.DefaultAction(purchaseInvoice);
            var miscellaneousChargeCreate = new PurchaseInvoiceOverviewComponent(this.purchaseInvoiceListPage.Driver).OrderadjustmentOverviewPanel.Click().CreateMiscellaneousCharge();

            miscellaneousChargeCreate
                .Amount.Set(expectedAmount.ToString())
                .Description.Set(expectedDescription);

            this.Session.Rollback();
            miscellaneousChargeCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new MiscellaneousCharges(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedDescription, actual.Description);
        }

        [Fact]
        public void CreatePercentageForPurchaseInvoice()
        {
            this.purchaseInvoiceListPage = this.Sidenav.NavigateToPurchaseInvoices();

            var purchaseInvoice = new PurchaseInvoices(this.Session).Extent().First;

            var before = new MiscellaneousCharges(this.Session).Extent().ToArray();

            var expected = new MiscellaneousChargeBuilder(this.Session).WithPercentageDefaults().Build();
            purchaseInvoice.AddOrderAdjustment(expected);

            this.Session.Derive();

            Assert.True(expected.ExistPercentage);
            Assert.True(expected.ExistDescription);

            var expectedPercentage = expected.Percentage;
            var expectedDescription = expected.Description;

            this.purchaseInvoiceListPage.Table.DefaultAction(purchaseInvoice);
            var miscellaneousChargeCreate = new PurchaseInvoiceOverviewComponent(this.purchaseInvoiceListPage.Driver).OrderadjustmentOverviewPanel.Click().CreateMiscellaneousCharge();

            miscellaneousChargeCreate
                .Percentage.Set(expectedPercentage.ToString())
                .Description.Set(expectedDescription);

            this.Session.Rollback();
            miscellaneousChargeCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new MiscellaneousCharges(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedPercentage, actual.Percentage);
            Assert.Equal(expectedDescription, actual.Description);
        }
    }
}
