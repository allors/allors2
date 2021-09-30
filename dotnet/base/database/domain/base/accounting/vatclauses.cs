// <copyright file="VatClauses.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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

        private UniquelyIdentifiableSticky<VatClause> Cache => this.cache ??= new UniquelyIdentifiableSticky<VatClause>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;
            var dutchBelgium = new Locales(this.Session).DutchBelgium;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);
            var localisedClause = new LocalisedTextAccessor(this.Meta.LocalisedClauses);

            merge(ServiceB2BId, v =>
            {
                v.Name = "Rent goods";
                localisedName.Set(v, dutchBelgium, "Verhuur goederen");
                localisedClause.Set(v, dutchBelgium, "Dienstverrichting niet onderworpen aan Belgische btw art. 21, § 2 van het Wbtw / Art. 44  /EG.");
                v.IsActive = true;
            });

            merge(IntracommunautairId, v =>
            {
                v.Name = "Intracommunautair";
                localisedName.Set(v, dutchBelgium, "Intracommunautair");
                localisedClause.Set(v, dutchBelgium, "Vrijstelling van BTW art. 39bis, eerste lid, 1° van het WBTW / Vrijgesteld || Artikel 138, lid 1 - Richtlijn 2006/112.");
                v.IsActive = true;
            });

            merge(BeArt39Par1Item1Id, v =>
            {
                v.Name = "Export goods, transport responsible is you, destination customer";
                localisedName.Set(v, dutchBelgium, "Verkoop buiten EU, transport verantwoordelijk bent u, bestemming klant");
                localisedClause.Set(v, dutchBelgium, "Vrijstelling van BTW art. 39 § 1, 1° van het WBTW / Vrijgesteld  || artikel 146, lid 1, onder a) – richtlijn 2006/112.");
                v.IsActive = true;
            });

            merge(BeArt15Par2Id, v =>
            {
                v.Name = "Export goods, transport responsible is you, destination internal organisation";
                localisedName.Set(v, dutchBelgium, "Verkoop buiten EU, transport verantwoordelijk bent u, bestemming interne organisatie");
                localisedClause.Set(v, dutchBelgium, "Levering van goederen niet onderworpen aan Belgische BTW. art. 15, § 2, van het WBTW / Artikel 33, lid 1, onder a) en b) - Richtlijn 2006/112.");
                v.IsActive = true;
            });

            merge(BeArt39Par1Item2Id, v =>
            {
                v.Name = "Export goods, transport responsible is customer, destination customer";
                localisedName.Set(v, dutchBelgium, "Verkoop buiten EU, transport verantwoordelijk is klant, bestemming klant");
                localisedClause.Set(v, dutchBelgium, "Vrijstelling van BTW art. 39 § 1, 2° van het WBTW / Vrijgesteld  || artikel 146, lid 1, onder a) – richtlijn 2006/112.");
                v.IsActive = true;
            });

            merge(BeArt14Par2Id, v =>
            {
                v.Name = "Export goods, transport responsible is customer, destination internal organisation";
                localisedName.Set(v, dutchBelgium, "Verkoop buiten EU, transport verantwoordelijk is klant, bestemming internal organisation");
                localisedClause.Set(v, dutchBelgium, @"
Levering van goederen niet onderworpen aan Belgische BTW. art. 14, § 2 van het WBTW / Artikel 32, eerste alinea - Richtlijn 2006/112 Extra vermeldingen:
De goederen worden niet geïnstalleerd. / The goods are not installed.
Koper vervoert van België naar {shipToCountry}. / The buyer transports from Belgium to {shipToCountry}");
                v.IsActive = true;
            });
        }
    }
}
