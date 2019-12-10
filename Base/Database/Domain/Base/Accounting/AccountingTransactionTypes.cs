// <copyright file="AccountingTransactionTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;

    public partial class AccountingTransactionTypes
    {
        private static readonly Guid BankStatementId = new Guid("2E2CB1CB-BA50-43FD-8A3C-D5CA23CA5B4F");
        private static readonly Guid PaymentReceiptId = new Guid("BD9396A4-EFD9-444A-9666-0ACF2EE172F6");
        private static readonly Guid PaymentDisbursementId = new Guid("02C1BC8E-49BB-4BBA-8B1C-8A0D525253B9");
        private static readonly Guid AccountsPayableId = new Guid("96D33391-AF81-4ED5-910F-D680555ABC2C");
        private static readonly Guid AccountsReceivableId = new Guid("9598D854-8DBF-43B1-BCA4-26D54AD86945");
        private static readonly Guid BudgettingId = new Guid("FB0E3794-A3E8-4A5A-9B8B-AD66525D3747");
        private static readonly Guid InventoryAdjustmentId = new Guid("2EC6F589-57C8-44C3-A3E2-810E7967C61D");
        private static readonly Guid GeneralId = new Guid("B9600D50-566F-43D2-84E6-65F315EED78D");
        private UniquelyIdentifiableSticky<AccountingTransactionType> cache;

        public AccountingTransactionType BankStatement => this.Cache[BankStatementId];

        public AccountingTransactionType PaymentReceipt => this.Cache[PaymentReceiptId];

        public AccountingTransactionType PaymentDisbursement => this.Cache[PaymentDisbursementId];

        public AccountingTransactionType AccountsPayable => this.Cache[AccountsPayableId];

        public AccountingTransactionType AccountsReceivable => this.Cache[AccountsReceivableId];

        public AccountingTransactionType Budgetting => this.Cache[BudgettingId];

        public AccountingTransactionType InventoryAdjustment => this.Cache[InventoryAdjustmentId];

        public AccountingTransactionType General => this.Cache[GeneralId];

        private UniquelyIdentifiableSticky<AccountingTransactionType> Cache => this.cache ??= new UniquelyIdentifiableSticky<AccountingTransactionType>(this.Session);

        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.Locale);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(BankStatementId, v =>
            {
                v.Name = "Bank statement";
                localisedName.Set(v, dutchLocale, "Bankafschrift");
                v.IsActive = true;
            });

            merge(PaymentReceiptId, v =>
            {
                v.Name = "Payment receipt";
                localisedName.Set(v, dutchLocale, "Binnenkomende betaling");
                v.IsActive = true;
            });

            merge(PaymentDisbursementId, v =>
            {
                v.Name = "Payment disbursement";
                localisedName.Set(v, dutchLocale, "Uitgaande betaling");
                v.IsActive = true;
            });

            merge(AccountsPayableId, v =>
            {
                v.Name = "Accounts payable";
                localisedName.Set(v, dutchLocale, "Debiteuren post");
                v.IsActive = true;
            });

            merge(AccountsReceivableId, v =>
            {
                v.Name = "Accounts receivable";
                localisedName.Set(v, dutchLocale, "Crediteuren post");
                v.IsActive = true;
            });

            merge(BudgettingId, v =>
            {
                v.Name = "Budget posting";
                localisedName.Set(v, dutchLocale, "Budget post");
                v.IsActive = true;
            });

            merge(InventoryAdjustmentId, v =>
            {
                v.Name = "Inventory adjustment";
                localisedName.Set(v, dutchLocale, "Voorraad aanpassing");
                v.IsActive = true;
            });

            merge(GeneralId, v =>
            {
                v.Name = "General";
                localisedName.Set(v, dutchLocale, "Overige");
                v.IsActive = true;
            });
        }
    }
}
