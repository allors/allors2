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
    using Xunit;


    public class CashTests : DomainTest
    {
        [Fact]
        public void GivenCashPaymentMethod_WhenDeriving_ThenGeneralLedgerAccountAndJournalAtMostOne()
        {
            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.Session).WithDescription("journal").Build();

            this.Session.Commit();

            var cash = new CashBuilder(this.Session)
                .WithDescription("description")
                .WithGeneralLedgerAccount(internalOrganisationGlAccount)
                .Build();

            var internalOrganisation = this.InternalOrganisation;
            internalOrganisation.DoAccounting = true;
            internalOrganisation.DefaultCollectionMethod = cash;

            Assert.False(this.Session.Derive(false).HasErrors);

            cash.Journal = journal;

            Assert.True(this.Session.Derive(false).HasErrors);

            cash.RemoveGeneralLedgerAccount();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCashPaymentMethodForSingletonThatDoesAccounting_WhenDeriving_ThenEitherGeneralLedgerAccountOrJournalMustExist()
        {
            var internalOrganisation = this.InternalOrganisation;

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.Session)
                .WithDescription("journal")
                .Build();

            this.Session.Commit();

            var cash = new CashBuilder(this.Session)
                .WithDescription("description")
                .Build();

            internalOrganisation.DoAccounting = true;
            internalOrganisation.AddActiveCollectionMethod(cash);

            Assert.True(this.Session.Derive(false).HasErrors);

            cash.Journal = journal;

            Assert.False(this.Session.Derive(false).HasErrors);

            cash.RemoveJournal();
            cash.GeneralLedgerAccount = internalOrganisationGlAccount;

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
