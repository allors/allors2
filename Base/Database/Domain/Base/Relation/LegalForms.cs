// <copyright file="LegalForms.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class LegalForms
    {
        private static readonly Guid UKPublicLimitedCompanyId = new Guid("2C8413C4-C9A0-4193-ACF5-6F8EC6BCD5CB");
        private static readonly Guid UKLimitedLiabilityCompanyId = new Guid("91462406-14D0-41DB-AA80-32DADA7E9A12");
        private static readonly Guid UKOnePersonPrivateLimitedCompanyId = new Guid("B003323F-18AA-4378-B342-CA041289EAE1");
        private static readonly Guid UKCooperativeCompanyWithLimitedLiabilityId = new Guid("9B298557-F8DE-4014-A2FA-32E2A693C1FC");
        private static readonly Guid UKCooperativeCompanyWithUnlimitedLiabilityId = new Guid("A5BEC3D4-0A7A-47C8-AA79-2078582B770D");
        private static readonly Guid UKGeneralPartnershipId = new Guid("2380C77F-9FDD-4F75-AE29-69A555FBF285");
        private static readonly Guid UKLimitedPartnershipId = new Guid("B49E8E41-FED7-4023-A9AB-224B2DB57E0C");
        private static readonly Guid UKNonStockCorporationId = new Guid("91369994-A3C4-433F-890D-FAD793AF3E6F");
        private static readonly Guid UKCompanyEstablishedForSocialPurposesId = new Guid("C18249DF-1ED2-4E93-965A-A860C4040348");
        private static readonly Guid UKSelfEmployedId = new Guid("CFF2D0C0-46F7-4A84-A22B-5F49F79AF32B");

        private static readonly Guid FrSaId = new Guid("952ECF13-9D91-47D9-B494-BE96824C583B");
        private static readonly Guid FrSasId = new Guid("D1A96B44-2EBD-4B67-938D-732E105970D6");
        private static readonly Guid FrSasuId = new Guid("8CDC528A-CC3E-457C-9956-0C3C37054B40");
        private static readonly Guid FrSarlId = new Guid("AD0B8768-1578-4F09-8ABA-25EF2D083E25");
        private static readonly Guid FrSncId = new Guid("28A74719-0D47-4C13-9D3B-226C7D49A013");
        private static readonly Guid FrScsId = new Guid("ABDD4495-3658-44F5-AB34-227B09D16192");
        private static readonly Guid FrScaId = new Guid("D8715B49-DD75-433D-AEC3-177C747C9B0C");
        private static readonly Guid FrEurlId = new Guid("C6FA919E-EFE5-4460-9D76-C0B228640FEA");
        private static readonly Guid FrIndependentId = new Guid("82EF0849-AD84-4A5E-B1AC-AA792030E3CB");

        private static readonly Guid BeNvSaId = new Guid("20E59E4D-1344-4C86-9F26-9B99AD93CE93");
        private static readonly Guid BeBvbaSprlId = new Guid("B9B02314-C844-418F-AEF0-177282530CFE");
        private static readonly Guid BeEbvbaSprluId = new Guid("C9A5021D-873C-4231-BAD0-DA70BCED8F6C");
        private static readonly Guid BeCbvbaScrlId = new Guid("30DDDC13-2766-47B4-ACBE-F32D425D6F1A");
        private static readonly Guid BeCvoaScriId = new Guid("277EC37A-1175-48BB-BB2E-8DA120B2CF82");
        private static readonly Guid BeCommVaSncId = new Guid("31716613-7451-4E4A-8149-5FB18022C75C");
        private static readonly Guid BeCommVScsId = new Guid("9C702C27-24B0-45A9-80D1-DF7ED35F88BB");
        private static readonly Guid BeMaatschapSocDroitCommunId = new Guid("02B1AD9D-3374-4D8F-A84D-55725AA2959A");
        private static readonly Guid BeVzwAsblId = new Guid("B24CB7F3-F904-4890-B95E-6FD6EA94F20B");
        private static readonly Guid BeZelfstandigeIndependentId = new Guid("07621297-135C-4D4D-BE5F-6978025573E2");

        private static readonly Guid NlEenmanszaakId = new Guid("C7B4B466-4BEB-431D-8F86-14201F657A3F");
        private static readonly Guid NlVofId = new Guid("AED1356A-2C87-4258-88CD-DC65413B6E9E");
        private static readonly Guid NlCvId = new Guid("641049DD-86D0-4E4F-B728-35993D3CCA21");
        private static readonly Guid NlMaatschapId = new Guid("9353416E-87FA-420A-8216-103DFCA7E0D7");
        private static readonly Guid NlBvId = new Guid("97B90814-475C-4192-9F5B-CE55E24BE109");
        private static readonly Guid NlNvId = new Guid("D08877A6-AC24-465F-815D-AF35965D686F");
        private static readonly Guid NlVerenigingId = new Guid("C39C03F9-99F1-4542-AFF4-ACC24B6BCF91");
        private static readonly Guid NlCowId = new Guid("75F2F089-B9D0-4E5E-A4FF-78567E6144AD");
        private static readonly Guid NlStichtingId = new Guid("7ABC91E5-27C1-4319-8AC4-23EA86DFE7A2");

        private UniquelyIdentifiableSticky<LegalForm> cache;

        public LegalForm UKPublicLimitedCompany => this.Cache[UKPublicLimitedCompanyId];
        public LegalForm UKLimitedLiabilityCompany => this.Cache[UKLimitedLiabilityCompanyId];
        public LegalForm UKOnePersonPrivateLimitedCompany => this.Cache[UKOnePersonPrivateLimitedCompanyId];
        public LegalForm UKCooperativeCompanyWithLimitedLiability => this.Cache[UKCooperativeCompanyWithLimitedLiabilityId];
        public LegalForm UKCooperativeCompanyWithUnlimitedLiability => this.Cache[UKCooperativeCompanyWithUnlimitedLiabilityId];
        public LegalForm UKGeneralPartnership => this.Cache[UKGeneralPartnershipId];
        public LegalForm UKLimitedPartnership => this.Cache[UKLimitedPartnershipId];
        public LegalForm UKNonStockCorporation => this.Cache[UKNonStockCorporationId];
        public LegalForm UKCompanyEstablishedForSocialPurposes => this.Cache[UKCompanyEstablishedForSocialPurposesId];
        public LegalForm UKSelfEmployed => this.Cache[UKSelfEmployedId];

        public LegalForm FrSa => this.Cache[FrSaId];
        public LegalForm FrSas => this.Cache[FrSasId];
        public LegalForm FrSasu => this.Cache[FrSasuId];
        public LegalForm FrSarl => this.Cache[FrSarlId];
        public LegalForm FrSca => this.Cache[FrScaId];
        public LegalForm FrEurl => this.Cache[FrEurlId];
        public LegalForm FrIndependent => this.Cache[FrIndependentId];

        public LegalForm BeNvSa => this.Cache[BeNvSaId];
        public LegalForm BeBvbaSprl => this.Cache[BeBvbaSprlId];
        public LegalForm BeCbvbaScrl => this.Cache[BeCbvbaScrlId];
        public LegalForm BeCvoaScri => this.Cache[BeCvoaScriId];
        public LegalForm BeCommVaSnc => this.Cache[BeCommVaSncId];
        public LegalForm BeCommVScs => this.Cache[BeCommVScsId];
        public LegalForm BeMaatschapSocDroitCommun => this.Cache[BeMaatschapSocDroitCommunId];
        public LegalForm BeVzwAsbl => this.Cache[BeVzwAsblId];
        public LegalForm BeZelfstandigeIndependent => this.Cache[BeZelfstandigeIndependentId];

        public LegalForm NlEenmanszaak => this.Cache[NlEenmanszaakId];
        public LegalForm NlVof => this.Cache[NlVofId];
        public LegalForm NlCv => this.Cache[NlCvId];
        public LegalForm NlMaatschap => this.Cache[NlMaatschapId];
        public LegalForm NlBv => this.Cache[NlBvId];
        public LegalForm NlNv => this.Cache[NlNvId];
        public LegalForm NlVereniging => this.Cache[NlVerenigingId];
        public LegalForm NlCow => this.Cache[NlCowId];
        public LegalForm NlStichting => this.Cache[NlStichtingId];

        private UniquelyIdentifiableSticky<LegalForm> Cache => this.cache ??= new UniquelyIdentifiableSticky<LegalForm>(this.Session);

        protected override void BaseSetup(Setup setup)
        {

            var merge = this.Cache.Merger().Action();

            merge(UKPublicLimitedCompanyId, v => v.Description = "UK - Public Limited Company");
            merge(UKLimitedLiabilityCompanyId, v => v.Description = "UK - Limited Liability Company");
            merge(UKOnePersonPrivateLimitedCompanyId, v => v.Description = "UK - One Person Private Limited Company");
            merge(UKCooperativeCompanyWithLimitedLiabilityId, v => v.Description = "UK - Cooperative Company With Limited Liability");
            merge(UKCooperativeCompanyWithUnlimitedLiabilityId, v => v.Description = "UK - Cooperative Company With Unlimited Liability");
            merge(UKGeneralPartnershipId, v => v.Description = "UK - General Partnership");
            merge(UKLimitedPartnershipId, v => v.Description = "UK - Limited Partnership");
            merge(UKNonStockCorporationId, v => v.Description = "UK - Non Stock Corporation");
            merge(UKCompanyEstablishedForSocialPurposesId, v => v.Description = "UK - Company Established For Social Purposes");
            merge(UKSelfEmployedId, v => v.Description = "UK - Self Employed");

            merge(FrSaId, v => v.Description = "FR - SA");
            merge(FrSasId, v => v.Description = "FR - SAS");
            merge(FrSasId, v => v.Description = "FR - SASU");
            merge(FrSarlId, v => v.Description = "FR - SARL");
            merge(FrSncId, v => v.Description = "FR - SNC");
            merge(FrScsId, v => v.Description = "FR - SCS");
            merge(FrScaId, v => v.Description = "FR - SCA");
            merge(FrEurlId, v => v.Description = "FR - EURL");
            merge(FrIndependentId, v => v.Description = "FR - Indépendent");

            merge(BeNvSaId, v => v.Description = "BE - NV / SA");
            merge(BeBvbaSprlId, v => v.Description = "BE - BVBA / SPRL");
            merge(BeEbvbaSprluId, v => v.Description = "BE - EBVBA / SPRLU");
            merge(BeCbvbaScrlId, v => v.Description = "BE - CBVBA / SCRL");
            merge(BeCvoaScriId, v => v.Description = "BE - CVOA / SCRI");
            merge(BeCommVaSncId, v => v.Description = "BE - Comm VA / SNC");
            merge(BeCommVScsId, v => v.Description = "BE - Comm V / SCS");
            merge(BeMaatschapSocDroitCommunId, v => v.Description = "BE - Maatschap / Société de Droit Commun");
            merge(BeVzwAsblId, v => v.Description = "BE - VZW / ASBL");
            merge(BeZelfstandigeIndependentId, v => v.Description = "BE - Zelfstandige / Indépendent");

            merge(NlEenmanszaakId, v => v.Description = "NL - Eenmanszaak");
            merge(NlVofId, v => v.Description = "NL - Vennotschap onder firma (vof)");
            merge(NlCvId, v => v.Description = "NL - Commanditaire vennootschap (cv)");
            merge(NlMaatschapId, v => v.Description = "NL - Maatschap");
            merge(NlBvId, v => v.Description = "NL - Besloten vennootschap (bv)");
            merge(NlNvId, v => v.Description = "NL - Naamloze vennootschap (nv)");
            merge(NlVerenigingId, v => v.Description = "NL - Vereniging");
            merge(NlCowId, v => v.Description = "NL - Coörperatie en onderlinge waarborgmaatschappij");
            merge(NlStichtingId, v => v.Description = "NL - Stichting");
        }
    }
}
