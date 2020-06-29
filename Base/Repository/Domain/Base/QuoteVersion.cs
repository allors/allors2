// <copyright file="QuoteVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("4C239D5F-85BF-4B8B-A2E6-609A2FE672B8")]
    #endregion
    public partial interface QuoteVersion : Version
    {
        #region Allors
        [Id("EC36228F-7688-4C5F-9BF1-A05EE82E1B64")]
        [AssociationId("8E713F6C-CB78-4EA1-A66F-B150755EF6D8")]
        [RoleId("49D8150B-6B41-47D3-BF56-CE68811824FD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        QuoteState QuoteState { get; set; }

        #region Allors
        [Id("26E0C5B6-05F4-4D18-9B2F-FAB56E057C76")]
        [AssociationId("E81EEC72-3D26-4348-A47D-8C78C038F09A")]
        [RoleId("DFAF1C08-F2C6-4758-99FA-C590581C322B")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("93DD5B53-08C6-40A9-AD40-FE547C34FE7A")]
        [AssociationId("7A4CFB1A-8DB2-4475-82AD-14FF8D1C7335")]
        [RoleId("A246A752-66AE-4A87-853D-C3949578BD55")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        DateTime RequiredResponseDate { get; set; }

        #region Allors
        [Id("1D8BE58D-45CE-446D-8508-3A95F500EB78")]
        [AssociationId("9BF8B8D5-0209-4DC7-8D7B-0B5C551ABBF2")]
        [RoleId("643EE4B0-6971-48BA-9189-A30F40661F70")]
        #endregion
        [Workspace]
        DateTime ValidFromDate { get; set; }

        #region Allors
        [Id("62A02B3A-03DE-415F-8083-DEDD9D4BDCFE")]
        [AssociationId("2D209FBE-FEE6-4059-A988-3621F8AD571C")]
        [RoleId("D356DECD-3989-4648-B9CC-EFFF9366931E")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        QuoteTerm[] QuoteTerms { get; set; }

        #region Allors
        [Id("BFB459E5-E28A-4EE7-ABAD-06A06919C4C4")]
        [AssociationId("42614E17-7058-4D90-BF4D-484F7EF81CCC")]
        [RoleId("94BB7391-68E6-4E70-BA49-4E0D46D5A0AB")]
        #endregion
        [Workspace]
        DateTime ValidThroughDate { get; set; }

        #region Allors
        [Id("ACF13AE8-CD1A-48BD-BABF-EA4EB67671D2")]
        [AssociationId("60283575-B357-447B-957C-3484A3DB3CB5")]
        [RoleId("E5DEA19F-C197-4BEC-84A1-3DB65FC4587B")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("89535B62-F3B5-488B-BE3E-B8F200B6F5C9")]
        [AssociationId("12A6EF79-5629-4D7F-918E-9D2CA78E6233")]
        [RoleId("8F343776-B1E7-41E3-90ED-697D6E9252C4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party Receiver { get; set; }

        #region Allors
        [Id("3654A64D-43C6-4703-850D-2DE1B4A1127C")]
        [AssociationId("A073BEE8-097D-46F8-B04E-4584D67C1AB2")]
        [RoleId("9A76ED84-4E1A-41EE-A0B0-59FF45392CAC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        ContactMechanism FullfillContactMechanism { get; set; }

        #region Allors
        [Id("1927FAF4-B595-4F53-8BD4-97DD1865187A")]
        [AssociationId("35F4DBA1-EC8F-4C22-990E-1AF6DDBA76A2")]
        [RoleId("FC1AE676-B321-468F-B44A-0E78E0FE6009")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        VatRegime VatRegime { get; set; }

        #region Allors
        [Id("15421231-909B-4374-8EFB-E8C717F242D7")]
        [AssociationId("674798EC-E896-425B-AAC6-299D5120438B")]
        [RoleId("AE05E9B2-5762-469C-98C4-4DDB2D66C34C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        VatClause AssignedVatClause { get; set; }

        #region Allors
        [Id("21D1EA49-C7FB-4771-A669-E2DE3F533AE2")]
        [AssociationId("E87FE03A-9A76-4CA0-9D26-2D4533194489")]
        [RoleId("473AA2B0-BE99-45D9-BC19-F3FCC56D3445")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        VatClause DerivedVatClause { get; set; }

        #region Allors
        [Id("1306f14d-ad14-4792-ac9a-c458a749181b")]
        [AssociationId("a6e27e2a-e69c-44b3-b8cf-b8858782ed92")]
        [RoleId("c257d5ba-22e9-4e78-98f9-d18c20e5ed24")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        IrpfRegime IrpfRegime { get; set; }

        #region Allors
        [Id("57b6f2e5-2c3e-46f8-b47f-8e0b27fffc2e")]
        [AssociationId("937a1b51-8cc8-461f-86eb-af768b589737")]
        [RoleId("3239fb44-57de-4727-b893-1e2058403e38")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime AssignedIrpfRegime { get; set; }

        #region Allors
        [Id("67c226ab-5526-4101-af56-bd216a50555a")]
        [AssociationId("a91ebe1c-79d6-4f73-88e1-02a685ba36c1")]
        [RoleId("4a1c9747-02de-40f9-8f6f-b5a9cada1301")]
        #endregion
        [Required]
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpf { get; set; }

        #region Allors
        [Id("EC2A3D0E-6733-402C-AD44-3010A960064D")]
        [AssociationId("A330DA68-7637-4882-8C92-CB8B23F1A700")]
        [RoleId("A7D63C6B-5A81-41D6-B4A2-D721F5148A50")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVat { get; set; }

        #region Allors
        [Id("2DD9990F-214C-4810-8F97-506FEC845F3C")]
        [AssociationId("7F267BE2-DA85-4507-841F-3B68957D3182")]
        [RoleId("629C879F-6250-494D-9EFA-DDC0FE75234D")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("6DC1543C-6F15-41A3-8906-2967DE811497")]
        [AssociationId("B3E853CE-E3CD-4D6D-B8C7-4E815C06EEA7")]
        [RoleId("812DA3E8-4C25-4A62-84C6-74B60C925232")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVat { get; set; }

        #region Allors
        [Id("84E463DA-DCCE-4469-A4E2-A66BCAE9CB8E")]
        [AssociationId("DEF45F09-CB22-4A30-A2BB-924D7D612C26")]
        [RoleId("C55A7FFB-88FD-4A1F-95DD-CC6364F39459")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurcharge { get; set; }

        #region Allors
        [Id("EA5BE89C-3CB8-4077-8D7E-29A51DED5225")]
        [AssociationId("0C8F9AFE-18D1-4E8F-BEEC-6DE7AA11F48A")]
        [RoleId("382628F5-6C97-4449-9F03-BC32CB495991")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscount { get; set; }

        #region Allors
        [Id("D1AFA863-7983-459D-828A-4CE90722583A")]
        [AssociationId("2FAB7036-FC68-4B7E-845F-9C69C344A7D2")]
        [RoleId("55AFC542-EEFF-46C8-9BE9-EAAADE5D2D11")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandling { get; set; }

        #region Allors
        [Id("BA3800D3-E37B-4479-AB9A-2DD49FFBF131")]
        [AssociationId("D4E3CFCF-4F78-465A-8C6C-28FFC84D2423")]
        [RoleId("5C0C04EB-B39F-4C39-A991-4251E6E79B25")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFee { get; set; }

        #region Allors
        [Id("dae828b3-da1e-4858-8353-6d5b69f2380c")]
        [AssociationId("459c798a-7bfc-4811-9008-86f7ffc6a6b5")]
        [RoleId("fbe3b824-e49d-4c05-86bc-88d3cc495022")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExtraCharge { get; set; }

        #region Allors
        [Id("2F081F44-9627-4A61-8045-BDA8A9754B2C")]
        [AssociationId("B97DC7EF-23C7-4F9F-A650-066C013E5BF7")]
        [RoleId("8FCF5860-6BB7-4001-A8B0-0D91506F6C82")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("4E9D5109-04B6-4057-B35A-9E1BFA149CEE")]
        [AssociationId("1CAC18E3-09AA-4F05-A58D-E9B514072A2B")]
        [RoleId("A0E7D965-DC07-491B-B4AE-B1F20F463D6D")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalListPrice { get; set; }

        #region Allors
        [Id("4f4fef59-a335-45e3-9aff-b2111aeba9e5")]
        [AssociationId("a3976df9-e968-4214-ba22-6aa97213babe")]
        [RoleId("4c0b3ea5-1653-422f-8afc-b217c2c9d4d3")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Indexed]
        OrderAdjustment[] OrderAdjustments { get; set; }

        #region Allors
        [Id("543adf0a-7a2d-4a6f-b220-4db487062e62")]
        [AssociationId("c3f47a99-50e4-45f7-bbfe-61a54d9ec85c")]
        [RoleId("bbb8f16c-f519-4b7d-995c-a4bbcb70f10a")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotal { get; set; }

        #region Allors
        [Id("7C3916BA-3406-4DF3-88C3-FC8DABC9E888")]
        [AssociationId("B8ECDCBB-EC34-4CA3-9947-B6CD4FB46AC5")]
        [RoleId("2525CAAD-0B1C-42B6-A66B-68DB57DF0D2F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Currency Currency { get; set; }

        #region Allors
        [Id("798B7FF6-2E3A-4ACD-8FF3-98E110FBC1DC")]
        [AssociationId("438664C4-25DD-4043-AADE-E7DF7043CD6E")]
        [RoleId("6CFB1028-E15B-4CCA-9C38-BC52C915E9BF")]
        #endregion
        [Workspace]
        DateTime IssueDate { get; set; }

        #region Allors
        [Id("A0BA1842-CC3D-49C3-A9A2-52DA6AA21CB8")]
        [AssociationId("6F632335-A7E5-4492-B007-A380F2E63ADB")]
        [RoleId("8733B40B-8659-46FC-ADAD-34A53F0C0027")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        QuoteItem[] QuoteItems { get; set; }

        #region Allors
        [Id("5A8F66EF-95AB-45AA-8D0F-2FA441D01937")]
        [AssociationId("4C1F1B88-542E-4AF3-92EC-D32331BD2597")]
        [RoleId("DE20F1B3-614D-4B31-B512-C95082B8DC55")]
        #endregion
        [Size(256)]
        [Workspace]
        string QuoteNumber { get; set; }

        #region Allors
        [Id("92C78C7A-3844-4277-AD94-8A67D8E6C267")]
        [AssociationId("026EB5BA-643E-4D1A-A841-3AF2465F09CF")]
        [RoleId("70F516F9-58AD-45F2-A742-0C6654EB9EE1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Request Request { get; set; }
    }
}
