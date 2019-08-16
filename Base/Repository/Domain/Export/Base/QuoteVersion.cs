namespace Allors.Repository
{
    using System;

    using Attributes;

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
        [Id("3D96372E-3027-439F-AD3D-33B47144D324")]
        [AssociationId("E7D92F71-EE0C-4ABD-8CFD-7EC1E06B94AD")]
        [RoleId("8B0BD36E-ECCC-46DE-81BB-769BA426981F")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }

        #region Allors
        [Id("54920170-8D35-4E68-8C5C-F23C7C6095B1")]
        [AssociationId("A1BF356A-DED2-48F0-BF55-F2ACEFAC51F1")]
        [RoleId("C758F402-0B31-41A1-8FBD-7D1033AFDB33")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        DiscountAdjustment DiscountAdjustment { get; set; }

        #region Allors
        [Id("88C13F5A-94CC-4B76-A05D-9D1DB1F2DCD2")]
        [AssociationId("D6175291-3C29-4AF5-B9C8-5B14D4AEC446")]
        [RoleId("CE17721C-07AA-438E-9BDB-8A54FDBA93CD")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        SurchargeAdjustment SurchargeAdjustment { get; set; }

        #region Allors
        [Id("7BF6AF25-FE00-4570-8625-99E052C12040")]
        [AssociationId("4272E067-8B7E-4447-84E6-73E655053188")]
        [RoleId("B4B4D66F-F2BE-4501-901E-4B49383E1637")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        Fee Fee { get; set; }

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