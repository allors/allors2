//------------------------------------------------------------------------------------------------- 
// <copyright file="Cash.cs" company="Allors bvba">
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
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class CashTests : DomainTest
    {
        [Test]
        public void GivenCashPaymentMethodForInternalOrganisationThatDoesAccounting_WhenDeriving_ThenCreditorIsRequired()
        {
            var cash = new CashBuilder(this.DatabaseSession)
                .WithDescription("description")
                .Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;             
            
            internalOrganisation.RemovePaymentMethods();
            internalOrganisation.AddPaymentMethod(cash);
            internalOrganisation.DoAccounting = false;

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            internalOrganisation.DoAccounting = true;

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenCashPaymentMethod_WhenDeriving_ThenGeneralLedgerAccountAndJournalCannotExistBoth()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var supplierRelationship = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession).WithDescription("journal").Build();

            this.DatabaseSession.Commit();

            var cash = new CashBuilder(this.DatabaseSession)
                .WithDescription("description")
                .WithGeneralLedgerAccount(internalOrganisationGlAccount)
                .WithCreditor(supplierRelationship)
                .Build();

            internalOrganisation.RemovePaymentMethods();
            internalOrganisation.AddPaymentMethod(cash);
            internalOrganisation.DoAccounting = true;

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            cash.Journal = journal;

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            cash.RemoveGeneralLedgerAccount();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenCashPaymentMethodForInternalOrganisationThatDoesAccounting_WhenDeriving_ThenEitherGeneralLedgerAccountOrJournalMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var supplierRelationship = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession).WithDescription("journal").Build();

            this.DatabaseSession.Commit();

            var cash = new CashBuilder(this.DatabaseSession)
                .WithDescription("description")
                .WithCreditor(supplierRelationship)
                .Build();

            internalOrganisation.RemovePaymentMethods();
            internalOrganisation.AddPaymentMethod(cash);
            internalOrganisation.DoAccounting = true;

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            cash.Journal = journal;

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            cash.RemoveJournal();
            cash.GeneralLedgerAccount = internalOrganisationGlAccount;

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
