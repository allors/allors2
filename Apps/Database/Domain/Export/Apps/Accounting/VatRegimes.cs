// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VatRegimes.cs" company="Allors bvba">
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
    using Meta;

    public partial class VatRegimes
    {
        private static readonly Guid CoContractorId = new Guid("49D061B2-A9F9-408b-B781-409ECC3D54FC");
        private static readonly Guid PrivatePersonId = new Guid("001A6A60-CC8A-4e6a-8FC0-BCE9707FA496");
        private static readonly Guid AssessableId = new Guid("5973BE64-C785-480f-AF30-74D32C6D6AF9");
        private static readonly Guid ExportId = new Guid("3268B6E5-995D-4f4b-B94E-AF4BE25F4282");
        private static readonly Guid IntraCommunautairId = new Guid("CFA1860E-DEBA-49a8-9062-E5577CDE0CCC");
        private static readonly Guid NotAssessableId = new Guid("4D57C8ED-1DF4-4db2-9AAA-4552257DC2BF");
        private static readonly Guid ExemptId = new Guid("82986030-5E18-43c1-8CBE-9832ACD4151D");

        private UniquelyIdentifiableSticky<VatRegime> cache;

        public VatRegime CoContractor => this.Cache[CoContractorId];

        public VatRegime PrivatePerson => this.Cache[PrivatePersonId];

        public VatRegime Assessable => this.Cache[AssessableId];

        public VatRegime Export => this.Cache[ExportId];

        public VatRegime IntraCommunautair => this.Cache[IntraCommunautairId];

        public VatRegime NotAssessable => this.Cache[NotAssessableId];

        public VatRegime Exempt => this.Cache[ExemptId];

        private UniquelyIdentifiableSticky<VatRegime> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<VatRegime>(this.Session));

        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.VatRate);
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var vatRate0 = new VatRates(this.Session).FindBy(M.VatRate.Rate, 0);
            var vatRate21 = new VatRates(this.Session).FindBy(M.VatRate.Rate, 21);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new VatRegimeBuilder(this.Session)
                .WithName("Co-Contractor")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Medecontractant").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate0)
                .WithUniqueId(CoContractorId)
                .WithIsActive(true)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("Private Person")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("particulier").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate21)
                .WithUniqueId(PrivatePersonId)
                .WithIsActive(true)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("VAT Assessable 21%")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("BTW-plichtig 21%").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate21)
                .WithUniqueId(AssessableId)
                .WithIsActive(true)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("Export")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Export").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate0)
                .WithUniqueId(ExportId)
                .WithIsActive(true)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("Intracommunautair")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Intracommunautair").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate0)
                .WithUniqueId(IntraCommunautairId)
                .WithIsActive(true)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("Not VAT assessable")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Niet BTW-plichtig").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate0)
                .WithUniqueId(NotAssessableId)
                .WithIsActive(true)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("Exempt")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrijgesteld").WithLocale(dutchLocale).Build())
                .WithUniqueId(ExemptId)
                .WithVatRate(vatRate0)
                .WithIsActive(true)
                .Build();
        }
    }
}