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
        [Id("859A60D9-EF88-4636-B36E-F91B4C22D206")]
        [AssociationId("230B9BD9-C90A-4C3F-AFD1-59CF6EA2B922")]
        [RoleId("4CA0A8CA-2D8A-40A8-AA68-4EAB60BC097B")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal Price { get; set; }

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