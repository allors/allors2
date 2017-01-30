//------------------------------------------------------------------------------------------------- 
// <copyright file="PurchaseOrderTests.cs" company="Allors bvba">
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

using Resources;

namespace Allors.Domain
{
    using System;
    using System.Security.Principal;
    using System.Threading;
    using Meta;
    using NUnit.Framework;

    [TestFixture]
    public class PurchaseOrderTests : DomainTest
    {
        [Test]
        public void GivenPurchaseOrderBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession).WithTakenViaSupplier(supplier).Build();

            Assert.AreEqual(new PurchaseOrderObjectStates(this.DatabaseSession).Provisional, order.CurrentObjectState);
            Assert.AreEqual(DateTime.UtcNow.Date, order.OrderDate.Date);
            Assert.AreEqual(DateTime.UtcNow.Date, order.EntryDate.Date);
            Assert.AreEqual(order.PreviousTakenViaSupplier, order.TakenViaSupplier);
            Assert.AreEqual(internalOrganisation, order.ShipToBuyer);
            Assert.AreEqual(internalOrganisation, order.BillToPurchaser);
            Assert.AreEqual(order.ShipToBuyer.PreferredCurrency, order.CustomerCurrency);
            Assert.IsTrue(order.ExistUniqueId);
        }

        [Test]
        public void GivenOrder_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            ContactMechanism takenViaContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var supplierContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(takenViaContactMechanism)
                .WithUseAsDefault(true)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).OrderAddress)
                .Build();
            supplier.AddPartyContactMechanism(supplierContactMechanism);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new PurchaseOrderBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithTakenViaSupplier(supplier);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            builder.WithTakenViaContactMechanism(takenViaContactMechanism);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenPurchaseOrder_WhenDeriving_ThenTakenViaSupplierMustBeInSupplierRelationship()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");

            new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithBillToPurchaser(internalOrganisation)
                .Build();

            var expectedError = ErrorMessages.PartyIsNotASupplier;
            Assert.AreEqual(expectedError, this.DatabaseSession.Derive().Errors[0].Message);

            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenOrder_WhenDeriving_ThenLocaleMustExist()
        {
            var englischLocale = new Locales(this.DatabaseSession).EnglishGreatBritain;

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            ContactMechanism takenViaContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var supplierContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(takenViaContactMechanism)
                .WithUseAsDefault(true)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).OrderAddress)
                .Build();
            supplier.AddPartyContactMechanism(supplierContactMechanism);

            var order = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithShipToBuyer(internalOrganisation)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(englischLocale, order.Locale);
        }

        [Test]
        public void GivenPurchaseOrder_WhenGettingOrderNumberWithoutFormat_ThenOrderNumberShouldBeReturned()
        {
            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession).Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();

            var order1 = new PurchaseOrderBuilder(this.DatabaseSession).WithTakenViaSupplier(supplier).WithBillToPurchaser(internalOrganisation).Build();
            Assert.AreEqual("1", order1.OrderNumber);

            var order2 = new PurchaseOrderBuilder(this.DatabaseSession).WithTakenViaSupplier(supplier).WithBillToPurchaser(internalOrganisation).Build();
            Assert.AreEqual("2", order2.OrderNumber);
        }

        [Test]
        public void GivenPurchaseOrder_WhenGettingOrderNumberWithFormat_ThenFormattedOrderNumberShouldBeReturned()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession).Build();
            internalOrganisation.PurchaseOrderNumberPrefix = "the format is ";

            var order1 = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithBillToPurchaser(internalOrganisation)
                .Build();

            Assert.AreEqual("the format is 1", order1.OrderNumber);

            var order2 = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithBillToPurchaser(internalOrganisation)
                .Build();

            Assert.AreEqual("the format is 2", order2.OrderNumber);
        }

        [Test]
        public void GivenPurchaseOrder_WhenObjectStateIsApproved_ThenCheckTransitions()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithBillToPurchaser(internalOrganisation)
                .Build();

            order.Confirm();

            this.DatabaseSession.Derive(true); 
            
            order.Approve();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderObjectStates(this.DatabaseSession).RequestsApproval, order.CurrentObjectState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Confirm));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Reject));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Approve));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Continue));
        }

        [Test]
        public void GivenPurchaseOrder_WhenObjectStateIsInProcess_ThenCheckTransitions()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithBillToPurchaser(internalOrganisation)
                .Build();

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderObjectStates(this.DatabaseSession).InProcess, order.CurrentObjectState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsTrue(acl.CanExecute(M.PurchaseOrder.Cancel));
            Assert.IsTrue(acl.CanExecute(M.PurchaseOrder.Hold));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Confirm));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Reject));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Approve));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Continue));
        }

        [Test]
        public void GivenPurchaseOrder_WhenObjectStateIsOnHold_ThenCheckTransitions()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithBillToPurchaser(internalOrganisation)
                .Build();

            order.Confirm();

            this.DatabaseSession.Derive(true); 
            
            order.Hold();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderObjectStates(this.DatabaseSession).OnHold, order.CurrentObjectState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsTrue(acl.CanExecute(M.PurchaseOrder.Cancel));
            Assert.IsTrue(acl.CanExecute(M.PurchaseOrder.Continue));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Confirm));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Reject));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Approve));
            Assert.IsFalse(acl.CanExecute(M.PurchaseOrder.Hold));
        }

        [Test]
        public void GivenPurchaseOrder_WhenConfirming_ThenAllValidItemsAreInConfirmedState()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var part = new RawMaterialBuilder(this.DatabaseSession).WithName("RawMaterial").Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Exempt)
                .Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(1).Build();
            var item2 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(2).Build();
            var item3 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(3).Build();
            var item4 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(4).Build();
            order.AddPurchaseOrderItem(item1);
            order.AddPurchaseOrderItem(item2);
            order.AddPurchaseOrderItem(item3);
            order.AddPurchaseOrderItem(item4);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            item4.Cancel();

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).InProcess, item1.CurrentObjectState);
            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).InProcess, item2.CurrentObjectState);
            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).InProcess, item3.CurrentObjectState);
            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).Cancelled, item4.CurrentObjectState);
        }

        [Test]
        public void GivenPurchaseOrder_WhenOrdering_ThenAllValidItemsAreInInProcessState()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var part = new RawMaterialBuilder(this.DatabaseSession).WithName("RawMaterial").Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Exempt)
                .Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(1).Build();
            var item2 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(2).Build();
            var item3 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(3).Build();
            order.AddPurchaseOrderItem(item1);
            order.AddPurchaseOrderItem(item2);
            order.AddPurchaseOrderItem(item3);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).InProcess, item1.CurrentObjectState);
            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).InProcess, item2.CurrentObjectState);
            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).InProcess, item3.CurrentObjectState);
        }

        [Test]
        public void GivenPurchaseOrder_WhenShipmentIsReceived_ThenCurrenShipmentStatusIsUpdated()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var part = new RawMaterialBuilder(this.DatabaseSession).WithName("RawMaterial").Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithBillToPurchaser(internalOrganisation)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Exempt)
                .Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(1).Build();
            var item2 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(2).Build();
            var item3 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(3).Build();
            order.AddPurchaseOrderItem(item1);
            order.AddPurchaseOrderItem(item2);
            order.AddPurchaseOrderItem(item3);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment1 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithPart(part).Build();
            shipment1.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(1)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item1)
                .Build();

            shipment1.AppsComplete();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).Received, item1.CurrentShipmentStatus.PurchaseOrderItemObjectState);
            Assert.AreEqual(new PurchaseOrderObjectStates(this.DatabaseSession).PartiallyReceived, order.CurrentShipmentStatus.PurchaseOrderObjectState);
            Assert.AreEqual(new PurchaseOrderObjectStates(this.DatabaseSession).InProcess, order.CurrentOrderStatus.PurchaseOrderObjectState);

            var shipment2 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();
            shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithPart(part).Build();
            shipment2.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(2)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item2)
                .Build();

            shipment2.AppsComplete();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).Received, item2.CurrentShipmentStatus.PurchaseOrderItemObjectState);
            Assert.AreEqual(new PurchaseOrderObjectStates(this.DatabaseSession).PartiallyReceived, order.CurrentShipmentStatus.PurchaseOrderObjectState);
            Assert.AreEqual(new PurchaseOrderObjectStates(this.DatabaseSession).InProcess, order.CurrentOrderStatus.PurchaseOrderObjectState);

            var shipment3 = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();
            shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithPart(part).Build();
            shipment3.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(3)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item3)
                .Build();

            shipment3.AppsComplete();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).Received, item3.CurrentShipmentStatus.PurchaseOrderItemObjectState);
            Assert.AreEqual(new PurchaseOrderObjectStates(this.DatabaseSession).Received, order.CurrentShipmentStatus.PurchaseOrderObjectState);
            Assert.AreEqual(new PurchaseOrderObjectStates(this.DatabaseSession).Completed, order.CurrentOrderStatus.PurchaseOrderObjectState);
        }
    }
}