// <copyright file="VatRegimes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Allors.Meta;

    public partial class VatRegimes
    {
        private static readonly Guid PrivatePersonId = new Guid("001A6A60-CC8A-4e6a-8FC0-BCE9707FA496");
        private static readonly Guid AssessableId = new Guid("5973BE64-C785-480f-AF30-74D32C6D6AF9");
        private static readonly Guid ExportId = new Guid("3268B6E5-995D-4f4b-B94E-AF4BE25F4282");
        private static readonly Guid IntraCommunautairId = new Guid("CFA1860E-DEBA-49a8-9062-E5577CDE0CCC");
        private static readonly Guid ServiceB2BId = new Guid("4D57C8ED-1DF4-4db2-9AAA-4552257DC2BF");
        private static readonly Guid ExemptId = new Guid("82986030-5E18-43c1-8CBE-9832ACD4151D");

        private UniquelyIdentifiableSticky<VatRegime> cache;

        public VatRegime PrivatePerson => this.Cache[PrivatePersonId];

        public VatRegime Assessable => this.Cache[AssessableId];

        public VatRegime Export => this.Cache[ExportId];

        public VatRegime IntraCommunautair => this.Cache[IntraCommunautairId];

        public VatRegime ServiceB2B => this.Cache[ServiceB2BId];

        public VatRegime Exempt => this.Cache[ExemptId];

        private UniquelyIdentifiableSticky<VatRegime> Cache => this.cache ??= new UniquelyIdentifiableSticky<VatRegime>(this.Session);

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.VatRate);
            setup.AddDependency(this.ObjectType, M.VatClause);
        }

        protected override void BaseSetup(Setup setup)
        {
            var vatRate0 = new VatRates(this.Session).Zero;
            var vatRate21 = new VatRates(this.Session).TwentyOne;

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(PrivatePersonId, v =>
            {
                v.Name = "Private Person";
                localisedName.Set(v, dutchLocale, "particulier");
                v.VatRate = vatRate21;
                v.IsActive = true;
            });

            merge(AssessableId, v =>
            {
                v.Name = "VAT Assessable 21%";
                localisedName.Set(v, dutchLocale, "BTW-plichtig 21%");
                v.VatRate = vatRate21;
                v.IsActive = true;
            });

            merge(ExportId, v =>
            {
                v.Name = "Export";
                localisedName.Set(v, dutchLocale, "Export");
                v.VatRate = vatRate0;
                v.IsActive = true;
            });

            merge(IntraCommunautairId, v =>
            {
                v.Name = "Intracommunautair";
                localisedName.Set(v, dutchLocale, "Intracommunautair");
                v.VatRate = vatRate0;
                v.VatClause = new VatClauses(this.Session).Intracommunautair;
                v.IsActive = true;
            });

            merge(ServiceB2BId, v =>
            {
                v.Name = "Service B2B: Not VAT assessable";
                localisedName.Set(v, dutchLocale, "Service B2B: Niet BTW-plichtig");
                v.VatRate = vatRate0;
                v.VatClause = new VatClauses(this.Session).ServiceB2B;
                v.IsActive = true;
            });

            merge(ExemptId, v =>
            {
                v.Name = "Exempt";
                localisedName.Set(v, dutchLocale, "Vrijgesteld");
                v.VatRate = vatRate0;
                v.IsActive = true;
            });
        }
    }
}
