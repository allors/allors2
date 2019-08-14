// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VatClauses.cs" company="Allors bvba">
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

    public partial class VatClauses
    {
        private static readonly Guid ServiceB2BId = new Guid("E4D1A217-2E99-4C6E-8D96-8050F99AABB4");
        private static readonly Guid IntracommunautairId = new Guid("0FDBA90F-7E00-4E75-96A5-824CF2400ABE");
        private static readonly Guid BeArt15Par2Id = new Guid("FE7D4204-B328-42F2-8C65-DE8A17AE3F5A");
        private static readonly Guid BeArt39Par1Item1Id = new Guid("5743B957-3C30-4EDB-B09F-D01D58D01F2E");
        private static readonly Guid BeArt39Par1Item2Id = new Guid("103F00B9-C6F9-4717-992B-26ADA4894912");
        private static readonly Guid BeArt14Par2Id = new Guid("CFF6D5E4-C183-4B93-BCA5-B8C81AEE8DCC");

        private UniquelyIdentifiableSticky<VatClause> cache;

        public VatClause ServiceB2B => this.Cache[ServiceB2BId];

        public VatClause Intracommunautair => this.Cache[IntracommunautairId];

        public VatClause BeArt39Par1Item1 => this.Cache[BeArt39Par1Item1Id];

        public VatClause BeArt15Par2 => this.Cache[BeArt15Par2Id];

        public VatClause BeArt39Par1Item2 => this.Cache[BeArt39Par1Item2Id];

        public VatClause BeArt14Par2 => this.Cache[BeArt14Par2Id];

        private UniquelyIdentifiableSticky<VatClause> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<VatClause>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new VatClauseBuilder(this.Session)
                .WithName("Rent goods")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verhuur goederen").WithLocale(dutchLocale).Build())
                .WithLocalisedClause(new LocalisedTextBuilder(this.Session)
                    .WithText(
                        @"Dienstverrichting niet onderworpen aan Belgische btw art. 21, § 2 van het Wbtw / Art. 44  /EG.")
                    .WithLocale(dutchLocale)
                    .Build())
                .WithUniqueId(ServiceB2BId)
                .WithIsActive(true)
                .Build();

            new VatClauseBuilder(this.Session)
                .WithName("Intracommunautair")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Intracommunautair").WithLocale(dutchLocale).Build())
                .WithLocalisedClause(new LocalisedTextBuilder(this.Session)
                    .WithText(
                        @"Vrijstelling van BTW art. 39bis, eerste lid, 1° van het WBTW / Vrijgesteld || Artikel 138, lid 1 - Richtlijn 2006/112.")
                    .WithLocale(dutchLocale)
                    .Build())
                .WithUniqueId(IntracommunautairId)
                .WithIsActive(true)
                .Build();

            new VatClauseBuilder(this.Session)
                .WithName("Sell goods outside EU, transport responsible is you, destination customer")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoop buiten EU, transport verantwoordelijk bent u, bestemming klant").WithLocale(dutchLocale).Build())
                .WithLocalisedClause(new LocalisedTextBuilder(this.Session)
                    .WithText(
                        @"Vrijstelling van BTW art. 39 § 1, 1° van het WBTW / Vrijgesteld  || artikel 146, lid 1, onder a) – richtlijn 2006/112.")
                    .WithLocale(dutchLocale)
                    .Build())
                .WithUniqueId(BeArt39Par1Item1Id)
                .WithIsActive(true)
                .Build();

            new VatClauseBuilder(this.Session)
                .WithName("Sell goods outside EU, transport responsible is you, destination internal organisation")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoop buiten EU, transport verantwoordelijk bent u, bestemming interne organisatie").WithLocale(dutchLocale).Build())
                .WithLocalisedClause(new LocalisedTextBuilder(this.Session)
                    .WithText(
                        @"Levering van goederen niet onderworpen aan Belgische BTW. art. 15, § 2, van het WBTW / Artikel 33, lid 1, onder a) en b) - Richtlijn 2006/112.")
                    .WithLocale(dutchLocale)
                    .Build())
                .WithUniqueId(BeArt15Par2Id)
                .WithIsActive(true)
                .Build();

            new VatClauseBuilder(this.Session)
                .WithName("Sell goods outside EU, transport responsible is customer, destination customer")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoop buiten EU, transport verantwoordelijk is klant, bestemming klant").WithLocale(dutchLocale).Build())
                .WithLocalisedClause(new LocalisedTextBuilder(this.Session)
                    .WithText(
                        @"Vrijstelling van BTW art. 39 § 1, 2° van het WBTW / Vrijgesteld  || artikel 146, lid 1, onder a) – richtlijn 2006/112.")
                    .WithLocale(dutchLocale)
                    .Build())
                .WithUniqueId(BeArt39Par1Item2Id)
                .WithIsActive(true)
                .Build();

            new VatClauseBuilder(this.Session)
                .WithName("Sell goods outside EU, transport responsible is customer, destination internal organisation")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoop buiten EU, transport verantwoordelijk is klant, bestemming internal organisation").WithLocale(dutchLocale).Build())
                .WithLocalisedClause(new LocalisedTextBuilder(this.Session)
                    .WithText(
                        @"
Levering van goederen niet onderworpen aan Belgische BTW. art. 14, § 2 van het WBTW / Artikel 32, eerste alinea - Richtlijn 2006/112 Extra vermeldingen
De goederen worden niet geïnstalleerd. / The goods are not installed.
Koper vervoert van België naar {{country}}. / The buyer transports from Belgium to {{country}}"
)
                    .WithLocale(dutchLocale)
                    .Build())
                .WithUniqueId(BeArt14Par2Id)
                .WithIsActive(true)
                .Build();
        }
    }
}