//------------------------------------------------------------------------------------------------- 
// <copyright file="PurchaseInvoiceTests.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using Meta;
    using Xunit;
    using Resources;

    
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

            builder.WithBilledFromParty(new Organisations(this.Session).FindBy(M.Organisation.Name, "supplier"));
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPurchaseInvoice_WhenDeriving_ThenBilledFromPartyMustBeInSupplierRelationship()
        {
            var supplier2 = new OrganisationBuilder(this.Session).WithName("supplier2").WithOrganisationRole(new OrganisationRoles(this.Session).Supplier).Build();

            var invoice = new PurchaseInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .WithBilledFromParty(supplier2)
                .Build();

            Assert.Equal(ErrorMessages.PartyIsNotASupplier, this.Session.Derive(false).Errors[0].Message);

            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier2).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPurchaseInvoice_WhenGettingInvoiceNumberWithoutFormat_ThenInvoiceNumberShouldBeReturned()
        {
            var invoice1 = new PurchaseInvoiceBuilder(this.Session)
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .Build();

            Assert.Equal("incoming invoiceno: 1", invoice1.InvoiceNumber);

            var invoice2 = new PurchaseInvoiceBuilder(this.Session)
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .Build();

            Assert.Equal("incoming invoiceno: 2", invoice2.InvoiceNumber);
        }
    }
}