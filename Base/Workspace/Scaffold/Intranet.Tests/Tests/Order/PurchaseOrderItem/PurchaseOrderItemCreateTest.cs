// <copyright file="PurchaseOrderItemCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PurchaseOrderItemTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using libs.angular.material.@base.src.export.objects.purchaseorder.list;
    using libs.angular.material.@base.src.export.objects.purchaseorder.overview;
    using libs.angular.material.@base.src.export.objects.purchaseorderitem.edit;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Order")]
    public class PurchaseOrderItemCreateTest : Test
    {
        private readonly PurchaseOrderListComponent purchaseOrderListPage;
        private readonly Organisation internalOrganisation;

        public PurchaseOrderItemCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            this.Login();
            this.purchaseOrderListPage = this.Sidenav.NavigateToPurchaseOrders();
        }

        /**
         * MinimalWithDefaults except Product/Part Item
         **/
        [Fact]
        public void CreateWithNonSerializedPartDefaults()
        {
            var purchaseOrder = new PurchaseOrders(this.Session).Extent().FirstOrDefault();

            var before = new PurchaseOrderItems(this.Session).Extent().ToArray();

            var expected = new PurchaseOrderItemBuilder(this.Session).WithNonSerializedPartDefaults(this.internalOrganisation, purchaseOrder).Build();
            purchaseOrder.AddPurchaseOrderItem(expected);

            this.Session.Derive();

            Assert.True(expected.ExistDescription);
            Assert.True(expected.ExistComment);
            Assert.True(expected.ExistInternalComment);
            Assert.True(expected.ExistInvoiceItemType);
            Assert.True(expected.ExistPart);
            Assert.True(expected.ExistQuantityOrdered);
            Assert.True(expected.ExistAssignedUnitPrice);
            Assert.True(expected.ExistAssignedDeliveryDate);
            Assert.True(expected.ExistShippingInstruction);
            Assert.True(expected.ExistMessage);

            var expectedDescription = expected.Description;
            var expectedComment = expected.Comment;
            var expectedInternalComment = expected.InternalComment;
            var expectedInvoiceItemType = expected.InvoiceItemType;
            var expectedPart = expected.Part;
            var expectedQuantityOrdered = expected.QuantityOrdered;
            var expectedQuoteItem = expected.QuoteItem;
            var expectedAssignedUnitPrice = expected.AssignedUnitPrice;
            var expectedAssignedDeliveryDate = expected.AssignedDeliveryDate;
            var expectedShippingInstruction = expected.ShippingInstruction;
            var expectedMessage = expected.Message;

            this.purchaseOrderListPage.Table.DefaultAction(purchaseOrder);
            var purchaseOrderOverview = new PurchaseOrderOverviewComponent(this.purchaseOrderListPage.Driver);
            var purchaseOrderItemOverviewPanel = purchaseOrderOverview.PurchaseorderitemOverviewPanel.Click();

            var purchaseOrderItemCreate = purchaseOrderItemOverviewPanel.CreatePurchaseOrderItem();

            purchaseOrderItemCreate.InvoiceItemType.Select(expected.InvoiceItemType);
            purchaseOrderItemCreate.OrderItemDescription_1.Set(expected.Description);
            purchaseOrderItemCreate.Comment.Set(expected.Comment);
            purchaseOrderItemCreate.InternalComment.Set(expected.InternalComment);
            purchaseOrderItemCreate.PurchaseOrderItemPart_2.Select(expected.Part.Name);
            purchaseOrderItemCreate.QuantityOrdered.Set(expected.QuantityOrdered.ToString());
            purchaseOrderItemCreate.AssignedUnitPrice.Set(expected.AssignedUnitPrice.ToString());
            purchaseOrderItemCreate.Message.Set(expected.Message);


            this.Session.Rollback();
            purchaseOrderItemCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PurchaseOrderItems(this.Session).Extent().ToArray();

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
        public void CreateWithSerializedPartDefaults()
        {
            var purchaseOrder = new PurchaseOrders(this.Session).Extent().FirstOrDefault();

            var before = new PurchaseOrderItems(this.Session).Extent().ToArray();

            var expected = new PurchaseOrderItemBuilder(this.Session).WithSerializedPartDefaults(this.internalOrganisation).Build();
            purchaseOrder.AddPurchaseOrderItem(expected);

            this.Session.Derive();

            Assert.True(expected.ExistDescription);
            Assert.True(expected.ExistComment);
            Assert.True(expected.ExistInternalComment);
            Assert.True(expected.ExistInvoiceItemType);
            Assert.True(expected.ExistPart);
            Assert.True(expected.ExistSerialisedItem);
            Assert.True(expected.ExistQuantityOrdered);
            Assert.True(expected.ExistAssignedUnitPrice);

            var expectedDescription = expected.Description;
            var expectedComment = expected.Comment;
            var expectedInternalComment = expected.InternalComment;
            var expectedInvoiceItemType = expected.InvoiceItemType;
            var expectedPart = expected.Part;
            var expectedSerialisedItem = expected.SerialisedItem;
            var expectedQuantityOrdered = expected.QuantityOrdered;
            var expectedAssignedUnitPrice = expected.AssignedUnitPrice;

            this.purchaseOrderListPage.Table.DefaultAction(purchaseOrder);
            var purchaseOrderOverview = new PurchaseOrderOverviewComponent(this.purchaseOrderListPage.Driver);
            var purchaseOrderItemOverviewPanel = purchaseOrderOverview.PurchaseorderitemOverviewPanel.Click();

            var purchaseOrderItemCreate = purchaseOrderItemOverviewPanel.CreatePurchaseOrderItem();
            purchaseOrderItemCreate.OrderItemDescription_1.Set(expected.Description);
            purchaseOrderItemCreate.Comment.Set(expected.Comment);
            purchaseOrderItemCreate.InternalComment.Set(expected.InternalComment);
            purchaseOrderItemCreate.Message.Set(expected.Message);

            this.Session.Rollback();
            purchaseOrderItemCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PurchaseOrderItems(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedDescription, actual.Description);
            Assert.Equal(expectedComment, actual.Comment);
            Assert.Equal(expectedInternalComment, actual.InternalComment);
            Assert.Equal(expectedInvoiceItemType, actual.InvoiceItemType);
            Assert.Equal(expectedPart, actual.Part);
            Assert.Equal(expectedSerialisedItem, actual.SerialisedItem);
            Assert.Equal(expectedQuantityOrdered, actual.QuantityOrdered);
            Assert.Equal(expectedAssignedUnitPrice, actual.AssignedUnitPrice);
        }
    }
}
