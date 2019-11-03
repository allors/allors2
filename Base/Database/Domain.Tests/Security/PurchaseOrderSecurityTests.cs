// <copyright file="PurchaseOrderSecurityTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

using Allors;
using Allors.Domain;
using Allors.Meta;
using Xunit;

public class PurchaseOrderSecurityTests : DomainTest
{
    public override Config Config => new Config { SetupSecurity = true };

    [Fact]
    public void GivenPurchaseOrder_WhenObjectStateIsApproved_ThenCheckTransitions()
    {
        User user = this.Administrator;
        this.Session.SetUser(user);

        var supplier = new OrganisationBuilder(this.Session).WithName("customer2").Build();
        var internalOrganisation = this.InternalOrganisation;
        new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

        var order = new PurchaseOrderBuilder(this.Session)
            .WithTakenViaSupplier(supplier)
            .Build();

        order.Confirm();

        this.Session.Derive();

        order.Approve();

        this.Session.Derive();

        Assert.Equal(new PurchaseOrderStates(this.Session).InProcess, order.PurchaseOrderState);
        var acl = new AccessControlLists(this.Session.GetUser())[order];
        Assert.False(acl.CanExecute(M.PurchaseOrder.Approve));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Reject));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Continue));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Confirm));
        Assert.True(acl.CanExecute(M.PurchaseOrder.Reopen));
        Assert.False(acl.CanExecute(M.PurchaseOrder.QuickReceive));
    }

    [Fact]
    public void GivenPurchaseOrder_WhenObjectStateIsSent_ThenCheckTransitions()
    {
        User user = this.Administrator;
        this.Session.SetUser(user);

        var supplier = new OrganisationBuilder(this.Session).WithName("customer2").Build();
        var internalOrganisation = this.InternalOrganisation;
        new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

        var order = new PurchaseOrderBuilder(this.Session)
            .WithTakenViaSupplier(supplier)
            .Build();

        order.Confirm();

        this.Session.Derive();

        order.Approve();

        this.Session.Derive();

        order.Send();

        this.Session.Derive();

        Assert.Equal(new PurchaseOrderStates(this.Session).Sent, order.PurchaseOrderState);
        var acl = new AccessControlLists(this.Session.GetUser())[order];
        Assert.False(acl.CanExecute(M.PurchaseOrder.Approve));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Reject));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Continue));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Confirm));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Reopen));
        Assert.True(acl.CanExecute(M.PurchaseOrder.QuickReceive));
    }

    [Fact]
    public void GivenPurchaseOrder_WhenObjectStateIsInProcess_ThenCheckTransitions()
    {
        User user = this.Administrator;
        this.Session.SetUser(user);

        var supplier = new OrganisationBuilder(this.Session).WithName("customer2").Build();
        var internalOrganisation = this.InternalOrganisation;
        new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

        var order = new PurchaseOrderBuilder(this.Session)
            .WithTakenViaSupplier(supplier)
            .Build();

        order.Confirm();

        this.Session.Derive();

        Assert.Equal(new PurchaseOrderStates(this.Session).InProcess, order.PurchaseOrderState);
        var acl = new AccessControlLists(this.Session.GetUser())[order];
        Assert.True(acl.CanExecute(M.PurchaseOrder.Cancel));
        Assert.True(acl.CanExecute(M.PurchaseOrder.Hold));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Confirm));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Reject));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Approve));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Continue));
    }

    [Fact]
    public void GivenPurchaseOrder_WhenObjectStateIsOnHold_ThenCheckTransitions()
    {
        User user = this.Administrator;
        this.Session.SetUser(user);

        var supplier = new OrganisationBuilder(this.Session).WithName("customer2").Build();
        new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

        var order = new PurchaseOrderBuilder(this.Session)
            .WithTakenViaSupplier(supplier)
            .Build();

        order.Confirm();

        this.Session.Derive();

        order.Hold();

        this.Session.Derive();

        Assert.Equal(new PurchaseOrderStates(this.Session).OnHold, order.PurchaseOrderState);
        var acl = new AccessControlLists(this.Session.GetUser())[order];
        Assert.False(acl.CanExecute(M.PurchaseOrder.Approve));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Hold));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Confirm));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Reopen));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Send));
        Assert.False(acl.CanExecute(M.PurchaseOrder.QuickReceive));
        Assert.False(acl.CanExecute(M.PurchaseOrder.Invoice));

        Assert.True(acl.CanExecute(M.PurchaseOrder.Reject));
        Assert.True(acl.CanExecute(M.PurchaseOrder.Continue));
        Assert.True(acl.CanExecute(M.PurchaseOrder.Cancel));
    }
}
