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
    using NUnit.Framework;

    using Resources;

    [TestFixture]
    public class PurchaseInvoiceTests : DomainTest
    {
        [Test]
        public void GivenPurchaseInvoice_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new PurchaseInvoiceBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.DatabaseSession).PurchaseInvoice);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBilledToInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"));
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBilledFromParty(new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "supplier"));
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenPurchaseInvoice_WhenDeriving_ThenBilledFromPartyMustBeInSupplierRelationship()
        {
            var supplier2 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier2").Build();

            var invoice = new PurchaseInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.DatabaseSession).PurchaseInvoice)
                .WithBilledFromParty(supplier2)
                .WithBilledToInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            Assert.AreEqual(ErrorMessages.PartyIsNotASupplier, this.DatabaseSession.Derive().Errors[0].Message);

            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier2).WithInternalOrganisation(invoice.BilledToInternalOrganisation).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenPurchaseInvoice_WhenGettingInvoiceNumberWithoutFormat_ThenInvoiceNumberShouldBeReturned()
        {
            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("org")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithDefaultPaymentMethod(ownBankAccount)
                .WithPreferredCurrency(euro)
                .Build();

            var invoice1 = new PurchaseInvoiceBuilder(this.DatabaseSession)
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.DatabaseSession).PurchaseInvoice)
                .WithBilledToInternalOrganisation(internalOrganisation)
                .Build();

            Assert.AreEqual("1", invoice1.InvoiceNumber);

            var invoice2 = new PurchaseInvoiceBuilder(this.DatabaseSession)
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.DatabaseSession).PurchaseInvoice)
                .WithBilledToInternalOrganisation(internalOrganisation)
                .Build();

            Assert.AreEqual("2", invoice2.InvoiceNumber);
        }
    }
}