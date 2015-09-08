// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VatRegimes.cs" company="Allors bvba">
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

    public partial class VatRegimes
    {
        public static readonly Guid CoContractorId = new Guid("49D061B2-A9F9-408b-B781-409ECC3D54FC");
        public static readonly Guid PrivatePersonId = new Guid("001A6A60-CC8A-4e6a-8FC0-BCE9707FA496");
        public static readonly Guid AssessableId = new Guid("5973BE64-C785-480f-AF30-74D32C6D6AF9");
        public static readonly Guid ExportId = new Guid("3268B6E5-995D-4f4b-B94E-AF4BE25F4282");
        public static readonly Guid IntraCommunautairId = new Guid("CFA1860E-DEBA-49a8-9062-E5577CDE0CCC");
        public static readonly Guid NotAssessableId = new Guid("4D57C8ED-1DF4-4db2-9AAA-4552257DC2BF");
        public static readonly Guid ExemptId = new Guid("82986030-5E18-43c1-8CBE-9832ACD4151D");

        private UniquelyIdentifiableCache<VatRegime> cache;

        public VatRegime CoContractor
        {
            get { return this.Cache.Get(CoContractorId); }
        }

        public VatRegime PrivatePerson
        {
            get { return this.Cache.Get(PrivatePersonId); }
        }

        public VatRegime Assessable
        {
            get { return this.Cache.Get(AssessableId); }
        }

        public VatRegime Export
        {
            get { return this.Cache.Get(ExportId); }
        }

        public VatRegime IntraCommunautair
        {
            get { return this.Cache.Get(IntraCommunautairId); }
        }

        public VatRegime NotAssessable
        {
            get { return this.Cache.Get(NotAssessableId); }
        }

        public VatRegime Exempt
        {
            get { return this.Cache.Get(ExemptId); }
        }

        private UniquelyIdentifiableCache<VatRegime> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<VatRegime>(this.Session));
            }
        }

        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, VatRates.Meta.ObjectType);
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var vatRate0 = new VatRates(this.Session).FindBy(VatRates.Meta.Rate, 0);
        
            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new VatRegimeBuilder(this.Session)
                .WithName("Co-Contractor")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Co-Contractor").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Medecontractant").WithLocale(dutchLocale).Build())
                .WithUniqueId(CoContractorId)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("Private Person")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Private Person").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("particulier").WithLocale(dutchLocale).Build())
                .WithUniqueId(PrivatePersonId)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("VAT Assessable")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("VAT Assessable").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("BTW-plichtig").WithLocale(dutchLocale).Build())
                .WithUniqueId(AssessableId)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("Export")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Export").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Export").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate0)
                .WithUniqueId(ExportId)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("Intracommunautair")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Intracommunautair").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Intracommunautair").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate0)
                .WithUniqueId(IntraCommunautairId)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("Not VAT assessable")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Not VAT assessable").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Niet BTW-plichtig").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate0)
                .WithUniqueId(NotAssessableId)
                .Build();
            
            new VatRegimeBuilder(this.Session)
                .WithName("Exempt")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Exempt").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrijgesteld").WithLocale(dutchLocale).Build())
                .WithUniqueId(ExemptId)
                .WithVatRate(vatRate0)
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