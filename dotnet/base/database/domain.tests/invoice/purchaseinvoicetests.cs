// <copyright file="PurchaseInvoiceTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;
    using Resources;
    using Xunit;

    public class PurchaseInvoiceTests : DomainTest
    {
        [Fact]
        public void GivenPurchaseInvoice_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new PurchaseInvoiceBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithBilledFrom(this.InternalOrganisation.ActiveSuppliers.First);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPurchaseInvoice_WhenDeriving_ThenBilledFromPartyMustBeInSupplierRelationship()
        {
            var supplier2 = new OrganisationBuilder(this.Session).WithName("supplier2").Build();

            new PurchaseInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .WithBilledFrom(supplier2)
                .Build();

            Assert.Equal(ErrorMessages.PartyIsNotASupplier, this.Session.Derive(false).Errors[0].Message);

            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier2).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPurchaseInvoice_WhenGettingInvoiceNumberWithoutFormat_ThenInvoiceNumberShouldBeReturned()
        {
            this.InternalOrganisation.InvoiceSequence = new InvoiceSequences(this.Session).EnforcedSequence;
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();

            var invoice1 = new PurchaseInvoiceBuilder(this.Session)
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .WithBilledFrom(supplier)
                .Build();

            this.Session.Derive();

            Assert.Equal("incoming invoiceno: 1", invoice1.InvoiceNumber);

            var invoice2 = new PurchaseInvoiceBuilder(this.Session)
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .WithBilledFrom(supplier)
                .Build();

            this.Session.Derive();

            Assert.Equal("incoming invoiceno: 2", invoice2.InvoiceNumber);
        }

        [Fact]
        public void GivenBilledToWithoutInvoiceNumberPrefix_WhenDeriving_ThenSortableInvoiceNumberIsSet()
        {
            this.InternalOrganisation.RemovePurchaseInvoiceNumberPrefix();
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();

            var invoice = new PurchaseInvoiceBuilder(this.Session)
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .WithBilledFrom(supplier)
                .Build();

            this.Session.Derive();

            invoice.Confirm();
            this.Session.Derive();

            Assert.Equal(int.Parse(invoice.InvoiceNumber), invoice.SortableInvoiceNumber);
        }

        [Fact]
        public void GivenBilledToWithInvoiceNumberPrefix_WhenDeriving_ThenSortableInvoiceNumberIsSet()
        {
            this.InternalOrganisation.InvoiceSequence = new InvoiceSequences(this.Session).EnforcedSequence;
            this.InternalOrganisation.PurchaseInvoiceNumberPrefix = "prefix-";
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();

            var invoice = new PurchaseInvoiceBuilder(this.Session)
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .WithBilledFrom(supplier)
                .Build();

            this.Session.Derive();

            invoice.Confirm();
            this.Session.Derive();

            Assert.Equal(int.Parse(invoice.InvoiceNumber.Split('-')[1]), invoice.SortableInvoiceNumber);
        }

        [Fact]
        public void GivenBilledToWithParametrizedInvoiceNumberPrefix_WhenDeriving_ThenSortableInvoiceNumberIsSet()
        {
            this.InternalOrganisation.InvoiceSequence = new InvoiceSequences(this.Session).EnforcedSequence;
            this.InternalOrganisation.PurchaseInvoiceNumberPrefix = "prefix-{year}-";
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();

            var invoice = new PurchaseInvoiceBuilder(this.Session)
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .WithBilledFrom(supplier)
                .Build();

            this.Session.Derive();

            invoice.Confirm();
            this.Session.Derive();

            var number = int.Parse(invoice.InvoiceNumber.Split('-').Last()).ToString("000000");
            Assert.Equal(int.Parse(string.Concat(this.Session.Now().Date.Year.ToString(), number)), invoice.SortableInvoiceNumber);
        }
    }


    [Trait("Category", "Security")]
    public class PurchaseInvoiceSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenPurchaseInvoice_WhenObjectStateIsCreated_ThenCheckTransitions()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            this.Session.Derive();
            this.Session.Commit();

            User user = this.Administrator;
            this.Session.SetUser(user);

            var invoice = new PurchaseInvoiceBuilder(this.Session)
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .WithBilledFrom(supplier)
                .Build();

            this.Session.Derive();

            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[invoice];

            Assert.Equal(new PurchaseInvoiceStates(this.Session).Created, invoice.PurchaseInvoiceState);
            Assert.False(acl.CanExecute(M.PurchaseInvoice.Approve));
            Assert.False(acl.CanExecute(M.PurchaseInvoice.Reject));
            Assert.False(acl.CanExecute(M.PurchaseInvoice.Reopen));
            Assert.False(acl.CanExecute(M.PurchaseInvoice.SetPaid));
            Assert.False(acl.CanExecute(M.PurchaseInvoice.Revise));
            Assert.False(acl.CanExecute(M.PurchaseInvoice.FinishRevising));
            Assert.False(acl.CanExecute(M.PurchaseInvoice.CreateSalesInvoice));
        }
    }
}
