//------------------------------------------------------------------------------------------------- 
// <copyright file="AccountingPeriodTests.cs" company="Allors bvba">
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
    using Xunit;

    
    public class AccountingPeriodTests : DomainTest
    {
        [Fact]
        public void GivenAccountingPeriod_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new AccountingPeriodBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPeriodNumber(1);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithTimeFrequency(new TimeFrequencies(this.DatabaseSession).Day);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTimeFactory.CreateDate(2010, 12, 31));
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithThroughDate(DateTimeFactory.CreateDate(2011, 12, 31));
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenAccountingPeriod_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var accountingPeriod = new AccountingPeriodBuilder(this.DatabaseSession).Build();

            Assert.True(accountingPeriod.Active);
        }

        [Fact]
        public void GivenAccountingPeriod_WhenMonthInSameQuarterIsAdded_ThenOnlyPeriodForMonthIsAdded()
        {
            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(euro)
                .WithDefaultPaymentMethod(ownBankAccount)
                .Build();

            organisation.AppsStartNewFiscalYear();

            Assert.Equal(4, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            var nextMonth = organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(5, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            Assert.Equal(organisation.ActualAccountingPeriod.PeriodNumber + 1, nextMonth.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Month, nextMonth.TimeFrequency);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(1).Date, nextMonth.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(2).AddSeconds(-1).Date, nextMonth.ThroughDate);
            Assert.True(nextMonth.ExistParent);
            Assert.Contains(nextMonth, organisation.AccountingPeriods);
        }

        [Fact]
        public void GivenAccountingPeriod_WhenMonthInNextQuarterIsAdded_ThenPeriodsForMonthAndForQuarterAreAdded()
        {
            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(euro)
                .WithDefaultPaymentMethod(ownBankAccount)
                .Build();

            organisation.AppsStartNewFiscalYear();

            Assert.Equal(4, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(5, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(6, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            var fourthMonth = organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(8, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            Assert.Equal(organisation.ActualAccountingPeriod.PeriodNumber + 3, fourthMonth.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Month, fourthMonth.TimeFrequency);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(3).Date, fourthMonth.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(4).AddSeconds(-1).Date, fourthMonth.ThroughDate);
            Assert.True(fourthMonth.ExistParent);
            Assert.Contains(fourthMonth, organisation.AccountingPeriods);

            var secondQuarter = fourthMonth.Parent;

            Assert.Equal(organisation.ActualAccountingPeriod.Parent.PeriodNumber + 1, secondQuarter.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Trimester, secondQuarter.TimeFrequency);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(3).Date, secondQuarter.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(6).AddSeconds(-1).Date, secondQuarter.ThroughDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.Parent, secondQuarter.Parent);
            Assert.Contains(secondQuarter, organisation.AccountingPeriods);
        }

        [Fact]
        public void GivenAccountingPeriod_WhenMonthInNextSemesterIsAdded_ThenPeriodsForMonthForQuarterAndForSemesterAreAdded()
        {
            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(euro)
                .WithDefaultPaymentMethod(ownBankAccount)
                .Build();

            organisation.AppsStartNewFiscalYear();

            Assert.Equal(4, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(5, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(6, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(8, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(9, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(10, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            var seventhMonth = organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(13, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            var thirdQuarter = seventhMonth.Parent;

            Assert.Equal(organisation.ActualAccountingPeriod.Parent.PeriodNumber + 2, thirdQuarter.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Trimester, thirdQuarter.TimeFrequency);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(6).Date, thirdQuarter.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(9).AddSeconds(-1).Date, thirdQuarter.ThroughDate);
            Assert.Contains(thirdQuarter, organisation.AccountingPeriods);

            var secondSemester = thirdQuarter.Parent;

            Assert.Equal(organisation.ActualAccountingPeriod.Parent.Parent.PeriodNumber + 1, secondSemester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Semester, secondSemester.TimeFrequency);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.Parent.FromDate.AddMonths(6).Date, secondSemester.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.Parent.FromDate.AddMonths(12).AddSeconds(-1).Date, secondSemester.ThroughDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.Parent.Parent, secondSemester.Parent);
            Assert.Contains(secondSemester, organisation.AccountingPeriods);
        }

        [Fact]
        public void GivenAccountingPeriod_WhenPeriod13IsAdded_ThenOnlyMonthPeriodIsAdded()
        {
            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(euro)
                .WithDefaultPaymentMethod(ownBankAccount)
                .Build();

            organisation.AppsStartNewFiscalYear();

            organisation.ActualAccountingPeriod.AddNextMonth();
            organisation.ActualAccountingPeriod.AddNextMonth();
            organisation.ActualAccountingPeriod.AddNextMonth();
            organisation.ActualAccountingPeriod.AddNextMonth();
            organisation.ActualAccountingPeriod.AddNextMonth();
            organisation.ActualAccountingPeriod.AddNextMonth();
            organisation.ActualAccountingPeriod.AddNextMonth();
            organisation.ActualAccountingPeriod.AddNextMonth();
            organisation.ActualAccountingPeriod.AddNextMonth();
            organisation.ActualAccountingPeriod.AddNextMonth();
            organisation.ActualAccountingPeriod.AddNextMonth();

            var period13 = organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(20, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            Assert.Equal(13, period13.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Month, period13.TimeFrequency);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(11).Date, period13.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(12).AddSeconds(-1).Date, period13.ThroughDate);
            Assert.True(period13.ExistParent);
            Assert.Contains(period13, organisation.AccountingPeriods);

            var fourthQuarter = period13.Parent;

            Assert.Equal(4, fourthQuarter.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Trimester, fourthQuarter.TimeFrequency);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(9).Date, fourthQuarter.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(12).AddSeconds(-1).Date, fourthQuarter.ThroughDate);
            Assert.Contains(fourthQuarter, organisation.AccountingPeriods);
        }
    }
}
