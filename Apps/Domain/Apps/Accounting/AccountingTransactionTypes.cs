// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountingTransactionTypes.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class AccountingTransactionTypes
    {
        public static readonly Guid BankStatementId = new Guid("2E2CB1CB-BA50-43FD-8A3C-D5CA23CA5B4F");
        public static readonly Guid PaymentReceiptId = new Guid("BD9396A4-EFD9-444A-9666-0ACF2EE172F6");
        public static readonly Guid PaymentDisbursementId = new Guid("02C1BC8E-49BB-4BBA-8B1C-8A0D525253B9");
        public static readonly Guid AccountsPayableId = new Guid("96D33391-AF81-4ED5-910F-D680555ABC2C");
        public static readonly Guid AccountsReceivableId = new Guid("9598D854-8DBF-43B1-BCA4-26D54AD86945");
        public static readonly Guid BudgettingId = new Guid("FB0E3794-A3E8-4A5A-9B8B-AD66525D3747");
        public static readonly Guid InventoryAdjustmentId = new Guid("2EC6F589-57C8-44C3-A3E2-810E7967C61D");
        public static readonly Guid GeneralId = new Guid("B9600D50-566F-43D2-84E6-65F315EED78D");

        private UniquelyIdentifiableCache<AccountingTransactionType> cache;

        public AccountingTransactionType BankStatement
        {
            get { return this.Cache.Get(BankStatementId); }
        }

        public AccountingTransactionType PaymentReceipt
        {
            get { return this.Cache.Get(PaymentReceiptId); }
        }

        public AccountingTransactionType PaymentDisbursement
        {
            get { return this.Cache.Get(PaymentDisbursementId); }
        }

        public AccountingTransactionType AccountsPayable
        {
            get { return this.Cache.Get(AccountsPayableId); }
        }

        public AccountingTransactionType AccountsReceivable
        {
            get { return this.Cache.Get(AccountsReceivableId); }
        }

        public AccountingTransactionType Budgetting
        {
            get { return this.Cache.Get(BudgettingId); }
        }

        public AccountingTransactionType InventoryAdjustment
        {
            get { return this.Cache.Get(InventoryAdjustmentId); }
        }

        public AccountingTransactionType General
        {
            get { return this.Cache.Get(GeneralId); }
        }

        private UniquelyIdentifiableCache<AccountingTransactionType> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<AccountingTransactionType>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new AccountingTransactionTypeBuilder(this.Session)
                .WithName("Bank statement")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bank statement").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bankafschrift").WithLocale(dutchLocale).Build())
                .WithUniqueId(BankStatementId).Build();

            new AccountingTransactionTypeBuilder(this.Session)
                .WithName("Payment receipt")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Payment receipt").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Binnenkomende betaling").WithLocale(dutchLocale).Build())
                .WithUniqueId(PaymentReceiptId).Build();

            new AccountingTransactionTypeBuilder(this.Session)
                .WithName("Payment disbursement")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Payment disbursement").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Uitgaande betaling").WithLocale(dutchLocale).Build())
                .WithUniqueId(PaymentDisbursementId).Build();

            new AccountingTransactionTypeBuilder(this.Session)
                .WithName("Accounts payable")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Accounts payable").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Debiteuren post").WithLocale(dutchLocale).Build())
                .WithUniqueId(AccountsPayableId).Build();

            new AccountingTransactionTypeBuilder(this.Session)
                .WithName("Accounts receivable")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Accounts receivable").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Crediteuren post").WithLocale(dutchLocale).Build())
                .WithUniqueId(AccountsReceivableId).Build();

            new AccountingTransactionTypeBuilder(this.Session)
                .WithName("Budget posting")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Budget posting").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Budget post").WithLocale(dutchLocale).Build())
                .WithUniqueId(BudgettingId).Build();

            new AccountingTransactionTypeBuilder(this.Session)
                .WithName("Inventory adjustment")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Inventory adjustment").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Voorraad aanpassing").WithLocale(dutchLocale).Build())
                .WithUniqueId(InventoryAdjustmentId).Build();

            new AccountingTransactionTypeBuilder(this.Session)
                .WithName("General")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("General").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overige").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralId).Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}