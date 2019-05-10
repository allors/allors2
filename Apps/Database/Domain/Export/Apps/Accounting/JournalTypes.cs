// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JournalTypes.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;

    public partial class JournalTypes
    {
        private static readonly Guid CashId = new Guid("B057AB5B-3BFD-46A4-80AA-D5330252E7E8");
        private static readonly Guid BankId = new Guid("D09AAFCB-CD28-447F-BE28-92509B7723CB");
        private static readonly Guid GiroId = new Guid("D364CC2B-9173-454F-A81E-F012859DAE44");
        private static readonly Guid GeneralId = new Guid("B886672A-019A-4D27-BFA0-6C18A8BBD4B8");
        private static readonly Guid SalesId = new Guid("FAE9D5DB-0EAD-46CB-89E8-C32E4001D8D1");
        private static readonly Guid PurchaseId = new Guid("BB8A39C3-9F3E-4D3F-83A2-10100016A78E");

        private UniquelyIdentifiableSticky<JournalType> cache;

        public JournalType Cash => this.Cache[CashId];

        public JournalType Bank => this.Cache[BankId];

        public JournalType Giro => this.Cache[GiroId];

        public JournalType General => this.Cache[GeneralId];

        public JournalType Sales => this.Cache[SalesId];

        public JournalType Purchase => this.Cache[PurchaseId];

        private UniquelyIdentifiableSticky<JournalType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<JournalType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new JournalTypeBuilder(this.Session)
                .WithName("Cash")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kas").WithLocale(dutchLocale).Build())
                .WithUniqueId(CashId)
                .WithIsActive(true)
                .Build();

            new JournalTypeBuilder(this.Session)
                .WithName("Bank")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bank").WithLocale(dutchLocale).Build())
                .WithUniqueId(BankId)
                .WithIsActive(true)
                .Build();

            new JournalTypeBuilder(this.Session)
                .WithName("Giro")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Giro").WithLocale(dutchLocale).Build())
                .WithUniqueId(GiroId)
                .WithIsActive(true)
                .Build();

            new JournalTypeBuilder(this.Session)
                .WithName("General")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Memoriaal").WithLocale(dutchLocale).Build())
                .WithUniqueId(GeneralId)
                .WithIsActive(true)
                .Build();

            new JournalTypeBuilder(this.Session)
                .WithName("Sales")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoop").WithLocale(dutchLocale).Build())
                .WithUniqueId(SalesId)
                .WithIsActive(true)
                .Build();

            new JournalTypeBuilder(this.Session)
                .WithName("Purchase")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoop").WithLocale(dutchLocale).Build())
                .WithUniqueId(PurchaseId)
                .WithIsActive(true)
                .Build();
        }
    }
}