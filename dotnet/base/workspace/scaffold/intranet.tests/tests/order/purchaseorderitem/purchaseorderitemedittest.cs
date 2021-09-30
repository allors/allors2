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
    public class PurchaseOrderItemEditTest : Test
    {
        private readonly PurchaseOrderListComponent purchaseOrderListPage;
        private readonly Organisation internalOrganisation;

        public PurchaseOrderItemEditTest(TestFixture fixture)
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
        public void EditWithNonSerializedPartDefaults()
        {
            var purchaseOrder = new PurchaseOrders(this.Session).Extent().FirstOrDefault();

            var before = new PurchaseOrderItems(this.Session).Extent().ToArray();

            var disposablePurchaseOrder = this.internalOrganisation.CreatePurchaseOrderWithNonSerializedItem(this.Session.Faker());
            var expected = disposablePurchaseOrder.PurchaseOrderItems.First(v => v.InvoiceItemType.IsPartItem);

            var purchaseOrderItem = purchaseOrder.PurchaseOrderItems.First(v => v.InvoiceItemType.IsPartItem);
            var id = purchaseOrderItem.Id;

            this.Session.Derive();

            var expectedDescription = expected.Description;
            var expectedComment = expected.Comment;
            var expectedInternalComment = expected.InternalComment;
            var expectedInvoiceItemType = expected.InvoiceItemType;
            var expectedPart = expected.Part;
            var expectedQuantityOrdered = expected.QuantityOrdered;
            var expectedAssignedUnitPrice = expected.AssignedUnitPrice;
            var expectedMessage = expected.Message;

            this.purchaseOrderListPage.Table.DefaultAction(purchaseOrder);
            var purchaseOrderOverview = new PurchaseOrderOverviewComponent(this.purchaseOrderListPage.Driver);
            var purchaseOrderItemOverviewPanel = purchaseOrderOverview.PurchaseorderitemOverviewPanel.Click();

            purchaseOrderItemOverviewPanel.Table.DefaultAction(purchaseOrderItem);

            var purchaseOrderItemEdit = new PurchaseOrderItemEditComponent(this.Driver);

            purchaseOrderItemEdit.OrderItemDescription_1.Set(expected.Description);
            purchaseOrderItemEdit.Comment.Set(expected.Comment);
            purchaseOrderItemEdit.InternalComment.Set(expected.InternalComment);
            purchaseOrderItemEdit.QuantityOrdered.Set(expected.QuantityOrdered.ToString());
            purchaseOrderItemEdit.AssignedUnitPrice.Set(expected.AssignedUnitPrice.ToString());
            purchaseOrderItemEdit.Message.Set(expected.Message);

            this.Session.Rollback();
            purchaseOrderItemEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PurchaseOrderItems(this.Session).Extent().ToArray();

            var actual = (PurchaseOrderItem)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedDescription, actual.Description);
            Assert.Equal(expectedComment, actual.Comment);
            Assert.Equal(expectedInternalComment, actual.InternalComment);
            Assert.Equal(expectedInvoiceItemType, actual.InvoiceItemType);
            Assert.Equal(expectedQuantityOrdered, actual.QuantityOrdered);
            Assert.Equal(expectedAssignedUnitPrice, actual.AssignedUnitPrice);
            Assert.Equal(expectedMessage, actual.Message);
        }

        /**
         * MinimalWithProductItemDefaults
         **/
        [Fact]
        public void EditWithSerialisedPartDefaults()
        {
            var purchaseOrder = new PurchaseOrders(this.Session).Extent().FirstOrDefault();

            var before = new PurchaseOrderItems(this.Session).Extent().ToArray();

            var disposablePurchaseOrder = this.internalOrganisation.CreatePurchaseOrderWithSerializedItem();
            var expected = disposablePurchaseOrder.PurchaseOrderItems.First(v => v.InvoiceItemType.IsProductItem);

            var purchaseOrderItem = purchaseOrder.PurchaseOrderItems.First(v => v.InvoiceItemType.IsProductItem);
            var id = purchaseOrderItem.Id;

            this.Session.Derive();

            var expectedComment = expected.Comment;
            var expectedInternalComment = expected.InternalComment;
            var expectedInvoiceItemType = expected.InvoiceItemType;
            var expectedPart = expected.Part;
            var expectedQuantityOrdered = expected.QuantityOrdered;
            var expectedAssignedUnitPrice = expected.AssignedUnitPrice;
            var expectedMessage = expected.Message;

            this.purchaseOrderListPage.Table.DefaultAction(purchaseOrder);
            var purchaseOrderOverview = new PurchaseOrderOverviewComponent(this.purchaseOrderListPage.Driver);
            var purchaseOrderItemOverviewPanel = purchaseOrderOverview.PurchaseorderitemOverviewPanel.Click();

            purchaseOrderItemOverviewPanel.Table.DefaultAction(purchaseOrderItem);

            var purchaseOrderItemEdit = new PurchaseOrderItemEditComponent(this.Driver);

            purchaseOrderItemEdit.Comment.Set(expected.Comment);
            purchaseOrderItemEdit.InternalComment.Set(expected.InternalComment);
            purchaseOrderItemEdit.AssignedUnitPrice.Set(expected.AssignedUnitPrice.ToString());
            purchaseOrderItemEdit.Message.Set(expected.Message);

            this.Session.Rollback();
            purchaseOrderItemEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PurchaseOrderItems(this.Session).Extent().ToArray();

            var actual = (PurchaseOrderItem)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedComment, actual.Comment);
            Assert.Equal(expectedInternalComment, actual.InternalComment);
            Assert.Equal(expectedInvoiceItemType, actual.InvoiceItemType);
            Assert.Equal(expectedQuantityOrdered, actual.QuantityOrdered);
            Assert.Equal(expectedAssignedUnitPrice, actual.AssignedUnitPrice);
            Assert.Equal(expectedMessage, actual.Message);
        }
    }
}
