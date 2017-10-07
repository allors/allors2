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
                .Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            internalOrganisation.DoAccounting = true;
            internalOrganisation.DefaultPaymentMethod = cash;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            cash.Journal = journal;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            cash.RemoveGeneralLedgerAccount();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCashPaymentMethodForSingletonThatDoesAccounting_WhenDeriving_ThenEitherGeneralLedgerAccountOrJournalMustExist()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

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
                .Build();

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
