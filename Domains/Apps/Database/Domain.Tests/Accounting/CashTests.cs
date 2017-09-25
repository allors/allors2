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
    using Meta;
    using Xunit;

    
    public class CashTests : DomainTest
    {
        [Fact]
        public void GivenCashPaymentMethodForInternalOrganisationThatDoesAccounting_WhenDeriving_ThenCreditorIsRequired()
        {
            var cash = new CashBuilder(this.DatabaseSession)
                .WithDescription("description")
                .Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;             
            
            internalOrganisation.RemovePaymentMethods();
            internalOrganisation.AddPaymentMethod(cash);
            internalOrganisation.DoAccounting = false;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            internalOrganisation.DoAccounting = true;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCashPaymentMethod_WhenDeriving_ThenGeneralLedgerAccountAndJournalCannotExistBoth()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");

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

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            cash.Journal = journal;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            cash.RemoveGeneralLedgerAccount();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCashPaymentMethodForInternalOrganisationThatDoesAccounting_WhenDeriving_ThenEitherGeneralLedgerAccountOrJournalMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
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

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            cash.Journal = journal;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            cash.RemoveJournal();
            cash.GeneralLedgerAccount = internalOrganisationGlAccount;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }
    }
}
