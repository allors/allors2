// <copyright file="AccountingPeriodTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class AccountingPeriodTests : DomainTest
    {
        [Fact]
        public void GivenAccountingPeriod_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new AccountingPeriodBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPeriodNumber(1);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFrequency(new TimeFrequencies(this.Session).Day);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(DateTimeFactory.CreateDate(2010, 12, 31));
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithThroughDate(DateTimeFactory.CreateDate(2011, 12, 31));
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenAccountingPeriod_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var accountingPeriod = new AccountingPeriodBuilder(this.Session).Build();

            Assert.True(accountingPeriod.Active);
        }

        [Fact]
        public void GivenAccountingPeriod_WhenMonthInSameQuarterIsAdded_ThenOnlyPeriodForMonthIsAdded()
        {
            var belgium = new Countries(this.Session).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.Session)
                .WithDescription("own account")
                .WithBankAccount(new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var organisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Internal")
                .WithDefaultCollectionMethod(ownBankAccount)
                .Build();

            this.Session.Derive();

            organisation.StartNewFiscalYear();

            Assert.Equal(4, this.Session.Extent<AccountingPeriod>().Count);

            var nextMonth = organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(5, this.Session.Extent<AccountingPeriod>().Count);

            Assert.Equal(organisation.ActualAccountingPeriod.PeriodNumber + 1, nextMonth.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Month, nextMonth.Frequency);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(1).Date, nextMonth.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(2).AddSeconds(-1).Date, nextMonth.ThroughDate);
            Assert.True(nextMonth.ExistParent);
        }

        [Fact]
        public void GivenAccountingPeriod_WhenMonthInNextQuarterIsAdded_ThenPeriodsForMonthAndForQuarterAreAdded()
        {
            var belgium = new Countries(this.Session).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.Session)
                .WithDescription("own account")
                .WithBankAccount(new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var organisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Internal")
                .WithDefaultCollectionMethod(ownBankAccount)
                .Build();

            this.Session.Derive();

            organisation.StartNewFiscalYear();

            Assert.Equal(4, this.Session.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(5, this.Session.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(6, this.Session.Extent<AccountingPeriod>().Count);

            var fourthMonth = organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(8, this.Session.Extent<AccountingPeriod>().Count);

            Assert.Equal(organisation.ActualAccountingPeriod.PeriodNumber + 3, fourthMonth.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Month, fourthMonth.Frequency);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(3).Date, fourthMonth.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(4).AddSeconds(-1).Date, fourthMonth.ThroughDate);
            Assert.True(fourthMonth.ExistParent);

            var secondQuarter = fourthMonth.Parent;

            Assert.Equal(organisation.ActualAccountingPeriod.Parent.PeriodNumber + 1, secondQuarter.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Trimester, secondQuarter.Frequency);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(3).Date, secondQuarter.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(6).AddSeconds(-1).Date, secondQuarter.ThroughDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.Parent, secondQuarter.Parent);
        }

        [Fact]
        public void GivenAccountingPeriod_WhenMonthInNextSemesterIsAdded_ThenPeriodsForMonthForQuarterAndForSemesterAreAdded()
        {
            var belgium = new Countries(this.Session).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.Session)
                .WithDescription("own account")
                .WithBankAccount(new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var organisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Internal")
                .WithDefaultCollectionMethod(ownBankAccount)
                .Build();

            this.Session.Derive();

            organisation.StartNewFiscalYear();

            Assert.Equal(4, this.Session.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(5, this.Session.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(6, this.Session.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(8, this.Session.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(9, this.Session.Extent<AccountingPeriod>().Count);

            organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(10, this.Session.Extent<AccountingPeriod>().Count);

            var seventhMonth = organisation.ActualAccountingPeriod.AddNextMonth();

            Assert.Equal(13, this.Session.Extent<AccountingPeriod>().Count);

            var thirdQuarter = seventhMonth.Parent;

            Assert.Equal(organisation.ActualAccountingPeriod.Parent.PeriodNumber + 2, thirdQuarter.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Trimester, thirdQuarter.Frequency);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(6).Date, thirdQuarter.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(9).AddSeconds(-1).Date, thirdQuarter.ThroughDate);

            var secondSemester = thirdQuarter.Parent;

            Assert.Equal(organisation.ActualAccountingPeriod.Parent.Parent.PeriodNumber + 1, secondSemester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Semester, secondSemester.Frequency);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.Parent.FromDate.AddMonths(6).Date, secondSemester.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.Parent.FromDate.AddMonths(12).AddSeconds(-1).Date, secondSemester.ThroughDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.Parent.Parent, secondSemester.Parent);
        }

        [Fact]
        public void GivenAccountingPeriod_WhenPeriod13IsAdded_ThenOnlyMonthPeriodIsAdded()
        {
            var belgium = new Countries(this.Session).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.Session)
                .WithDescription("own account")
                .WithBankAccount(new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var organisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Internal")
                .WithDefaultCollectionMethod(ownBankAccount)
                .Build();

            this.Session.Derive();

            organisation.StartNewFiscalYear();

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

            Assert.Equal(20, this.Session.Extent<AccountingPeriod>().Count);

            Assert.Equal(13, period13.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Month, period13.Frequency);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(11).Date, period13.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.FromDate.AddMonths(12).AddSeconds(-1).Date, period13.ThroughDate);
            Assert.True(period13.ExistParent);

            var fourthQuarter = period13.Parent;

            Assert.Equal(4, fourthQuarter.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Trimester, fourthQuarter.Frequency);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(9).Date, fourthQuarter.FromDate);
            Assert.Equal(organisation.ActualAccountingPeriod.Parent.FromDate.AddMonths(12).AddSeconds(-1).Date, fourthQuarter.ThroughDate);
        }
    }
}
