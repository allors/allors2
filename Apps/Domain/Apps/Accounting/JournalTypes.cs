// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JournalTypes.cs" company="Allors bvba">
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

    public partial class JournalTypes
    {
        public static readonly Guid CashId = new Guid("B057AB5B-3BFD-46A4-80AA-D5330252E7E8");
        public static readonly Guid BankId = new Guid("D09AAFCB-CD28-447F-BE28-92509B7723CB");
        public static readonly Guid GiroId = new Guid("D364CC2B-9173-454F-A81E-F012859DAE44");
        public static readonly Guid GeneralId = new Guid("B886672A-019A-4D27-BFA0-6C18A8BBD4B8");
        public static readonly Guid SalesId = new Guid("FAE9D5DB-0EAD-46CB-89E8-C32E4001D8D1");
        public static readonly Guid PurchaseId = new Guid("BB8A39C3-9F3E-4D3F-83A2-10100016A78E");

        private UniquelyIdentifiableCache<JournalType> cache;

        public JournalType Cash
        {
            get { return this.Cache.Get(CashId); }
        }

        public JournalType Bank
        {
            get { return this.Cache.Get(BankId); }
        }

        public JournalType Giro
        {
            get { return this.Cache.Get(GiroId); }
        }

        public JournalType General
        {
            get { return this.Cache.Get(GeneralId); }
        }

        public JournalType Sales
        {
            get { return this.Cache.Get(SalesId); }
        }

        public JournalType Purchase
        {
            get { return this.Cache.Get(PurchaseId); }
        }

        private UniquelyIdentifiableCache<JournalType> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<JournalType>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new JournalTypeBuilder(this.Session)
                .WithName("Cash")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Cash").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kas").WithLocale(dutchLocale).Build())
                .WithUniqueId(CashId)
                .Build();

            new JournalTypeBuilder(this.Session)
                .WithName("Bank")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bank").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bank").WithLocale(dutchLocale).Build())
                .WithUniqueId(BankId)
                .Build();

            new JournalTypeBuilder(this.Session)
                .WithName("Giro")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Giro").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Giro").WithLocale(dutchLocale).Build())
                .WithUniqueId(GiroId)
                .Build();

            new JournalTypeBuilder(this.Session)
                .WithName("General")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("General").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Memoriaal").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralId)
                .Build();

            new JournalTypeBuilder(this.Session)
                .WithName("Sales")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Sales").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoop").WithLocale(dutchLocale).Build())
                .WithUniqueId(SalesId)
                .Build();

            new JournalTypeBuilder(this.Session)
                .WithName("Purchase")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Purchase").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoop").WithLocale(dutchLocale).Build())
                .WithUniqueId(PurchaseId)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}