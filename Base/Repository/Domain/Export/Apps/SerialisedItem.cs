namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("5E594A00-15A4-4871-84E9-B8010A78FD21")]
    #endregion
    public partial class SerialisedItem : Deletable, FixedAsset
    {
        #region InheritedProperties

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Name { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public DateTime LastServiceDate { get; set; }
        public DateTime AcquiredDate { get; set; }
        public string Description { get; set; }

        public LocalisedText[] LocalisedDescriptions { get; set; }

        public decimal ProductionCapacity { get; set; }
        public DateTime NextServiceDate { get; set; }

        public string Keywords { get; set; }

        #endregion InheritedProperties

        #region SerialisedItemState
        #region Allors
        [Id("B611C1C0-6AB4-4082-B464-0494F3DE2051")]
        [AssociationId("C6DBB08B-9547-4235-9649-55411E6D0196")]
        [RoleId("2829D2FE-1A27-490C-9F0B-5BA476C21FDC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SerialisedItemState PreviousSerialisedItemState { get; set; }

        #region Allors
        [Id("424D5D46-A253-4E32-BFED-8160D20E3BBE")]
        [AssociationId("F35DABA1-8726-4E25-BA1D-95D775B1E3F4")]
        [RoleId("903C3573-8176-4C8F-83FE-4456DF42928D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SerialisedItemState LastSerialisedItemState { get; set; }

        #region Allors
        [Id("9C9A7694-4E41-46D7-B33C-14A703370A5B")]
        [AssociationId("FBF63B46-AD14-43EA-AD29-31652901BE89")]
        [RoleId("106E5048-AC33-427C-8B9E-462A9A998879")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItemState SerialisedItemState { get; set; }
        #endregion

        #region Versioning
        #region Allors
        [Id("414BDA46-B49A-4AB4-A9E2-02842414D572")]
        [AssociationId("ED1EFFA3-BDE5-4F96-B286-2CC4D007D0D7")]
        [RoleId("935BBA47-3AB3-4423-876F-8769855892C0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SerialisedItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("0318F8DE-D3D1-497D-870D-34E3A8F55ACC")]
        [AssociationId("2359CBB0-AE07-48BC-A65A-E8E5DC194CEF")]
        [RoleId("30DC64DC-6272-4A1A-B0D9-800D6971DCDB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SerialisedItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("B6DD4F80-EE97-446E-9779-610FF07F13B2")]
        [AssociationId("3CC4D71C-3CBF-4F6B-997A-C1FD113FD25B")]
        [RoleId("0CC2B6F1-69F7-404A-9620-57152FE2782C")]
        #endregion
        [Derived]
        [Required]
        [Size(256)]
        [Workspace]
        public string ItemNumber { get; set; }

        #region Allors
        [Id("de9caf09-6ae7-412e-b9bc-19ece66724da")]
        [AssociationId("ba630eb8-3087-43c6-9082-650094a0226e")]
        [RoleId("c0ada954-d86e-46c3-9a99-09209fb812a5")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string SerialNumber { get; set; }

        #region Allors
        [Id("91D1A28D-AE04-4445-B4AC-2053559DCFB7")]
        [AssociationId("2FBE6AA9-9E34-4A9A-9972-88E729AAEFBC")]
        [RoleId("6FE84CF4-959C-48AE-9923-C91D77E1C439")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public SerialisedItemCharacteristic[] SerialisedItemCharacteristics { get; set; }

        #region Allors
        [Id("D9D4FF13-6D54-4F35-9A81-902E0BB86545")]
        [AssociationId("991A97F7-4277-442D-9DE2-26348B22002C")]
        [RoleId("CD405E7C-D058-4306-8495-54BF3D0974E1")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public Ownership Ownership { get; set; }

        #region Allors
        [Id("E511EE11-FA2E-4F84-8010-EE1453C609F3")]
        [AssociationId("E6598114-E52B-4343-B0EC-E943262C5380")]
        [RoleId("DAA8C492-9675-4192-8651-4B9BD05C9B70")]
        #endregion
        [Workspace]
        public int AcquisitionYear { get; set; }

        #region Allors
        [Id("CCDD8203-F635-4821-876D-A83A925C145D")]
        [AssociationId("CCDD4029-2268-4822-B7FE-B76864B61DBE")]
        [RoleId("223C23A1-5514-47A3-BAFF-5F88D3DC5B59")]
        #endregion
        [Workspace]
        public int ManufacturingYear { get; set; }

        #region Allors
        [Id("DDECD426-40C7-4D17-A225-2C46B47F0C89")]
        [AssociationId("B2F5E3CA-E4BF-43B8-B812-D9479A74DF6E")]
        [RoleId("3570A014-1A46-4C61-8D7F-14D07BF1F5AA")]
        #endregion
        [Workspace]
        public decimal ReplacementValue { get; set; }

        #region Allors
        [Id("E25F1487-6F08-4DC5-9838-BAE4FF990ADA")]
        [AssociationId("26F70AEA-7F70-48C4-BDCA-41DC58E3BDB3")]
        [RoleId("F386A2DB-5DBE-4BD6-921B-024A9C80105C")]
        #endregion
        [Workspace]
        public int LifeTime { get; set; }

        #region Allors
        [Id("D96B2474-B8AE-40F4-9D86-4CA09E2B6965")]
        [AssociationId("AA3AA882-D022-42A1-8E1A-4C7901358EE8")]
        [RoleId("19E428F3-9C68-4C59-BD19-68EA17688F04")]
        #endregion
        [Workspace]
        public int DepreciationYears { get; set; }

        #region Allors
        [Id("ECE5838C-6E0B-4889-91DA-4F9277760E9D")]
        [AssociationId("0CCF5035-5E6E-4F06-9921-35B8F922BFA2")]
        [RoleId("4519FC49-C403-4FE2-B85F-BB7F01B6B907")]
        #endregion
        [Derived]
        [Workspace]
        public decimal PurchasePrice { get; set; }

        #region Allors
        [Id("D7BA117D-6C14-4A26-BAD2-F418E472A1A1")]
        [AssociationId("EBDE86D2-3DC2-4960-A465-216A935627B3")]
        [RoleId("862AC0F3-0DF4-419C-8558-D8C042C5045B")]
        #endregion
        [Workspace]
        public decimal AssignedPurchasePrice { get; set; }

        #region Allors
        [Id("53E31ACE-5F48-4CBF-9D35-003534E1A1F1")]
        [AssociationId("91414EC6-6C95-4CEF-A816-646BD8795F5A")]
        [RoleId("B30E951F-29E7-4013-BDAA-E3DAF7AE2FE3")]
        #endregion
        [Workspace]
        public decimal ExpectedSalesPrice { get; set; }

        #region Allors
        [Id("6EBA501E-5EB8-4B9C-B0E7-2658562D8F44")]
        [AssociationId("3F8D9AFD-B4ED-461B-A829-31745FE8DCB2")]
        [RoleId("D0AD48FF-6D74-4D07-9F92-08A9AB1FB3B7")]
        #endregion
        [Workspace]
        public decimal RefurbishCost { get; set; }

        #region Allors
        [Id("901E5D13-4B43-4FE4-9A79-8EFED2CAFE74")]
        [AssociationId("C223E10B-6AFF-4B7B-A8ED-89B7C8A69FF9")]
        [RoleId("D1B62377-F8F5-4713-BBC4-AF47C2A89AF1")]
        #endregion
        [Workspace]
        public decimal TransportCost { get; set; }

        #region Allors
        [Id("1A2285C0-9DE8-4BC4-B5F8-225C357A149C")]
        [AssociationId("94EF2F74-D2E2-42A0-B7C8-7BCD931C1567")]
        [RoleId("D7300671-1F91-437E-A8A8-730609FD9A16")]
        #endregion
        [Workspace]
        public decimal ExpectedRentalPriceFullService { get; set; }

        #region Allors
        [Id("FEC7C97D-1505-48F0-838D-9FFD8B9BB033")]
        [AssociationId("CA75ECD2-F7E1-42CE-9E94-EC5176622AF3")]
        [RoleId("D90F9123-FCD9-46D0-B9FE-CEAA1D7934C2")]
        #endregion
        [Workspace]
        public decimal ExpectedRentalPriceDryLease { get; set; }

        #region Allors
        [Id("A616AE10-EA83-4878-BCBA-377396B4357A")]
        [AssociationId("AA15AAF5-26E7-48F8-B15F-B5B11AF516F5")]
        [RoleId("0E159138-B2D2-429F-8DE5-ACCC5BB02C32")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public Media PrimaryPhoto { get; set; }

        #region Allors
        [Id("2F5FF954-C9E2-463F-8DD6-BBC0701DD3EA")]
        [AssociationId("C5A31199-527C-4AC5-A7DA-2FC72BA4C7B8")]
        [RoleId("9D8AECED-A967-4100-BF7A-CF081E5A6002")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToMany)]
        public Media[] Photos { get; set; }

        #region Allors
        [Id("18A320F1-2F65-4E49-A615-D88EDD15AC5C")]
        [AssociationId("7909A953-6F94-4CCE-B214-F6BE9272DFB1")]
        [RoleId("304C12C1-91F6-42DD-BB65-3DBF87A77F17")]
        #endregion
        [Workspace]
        [Size(-1)]
        public string InternalComment { get; set; }

        #region Allors
        [Id("B5D9E50B-3004-47C0-9C8D-62935DB15ECC")]
        [AssociationId("609D27BC-96C9-48EF-9516-6B45AECBEC20")]
        [RoleId("DAEF2150-D69C-4255-B78E-70ACFA63EA06")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Details { get; set; }

        #region Allors
        [Id("7A2A878B-1428-4C75-9A52-8725606FAA41")]
        [AssociationId("98B173A0-51DA-48A6-9556-4B8F2CFDC72B")]
        [RoleId("D86152FD-1D45-463E-B5FC-481F6E0D4CAE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public Organisation SuppliedBy { get; set; }

        #region Allors
        [Id("C16A8A73-84D3-4889-8B95-B8B05CB561DE")]
        [AssociationId("D46271CB-6AA1-419B-8AAB-2C547FACFD29")]
        [RoleId("2305CB0E-4280-41B6-B058-8A2DFC4DD7CC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Organisation AssignedSuppliedBy { get; set; }

        #region Allors
        [Id("E9ACD0EE-693C-4459-9F40-D478F538659F")]
        [AssociationId("0BA8139B-6910-441D-82B5-5318D074AC21")]
        [RoleId("DA88A4DD-CB8D-48DC-BFFA-772CA75A1379")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party OwnedBy { get; set; }

        #region Allors
        [Id("18F5FCB0-E48B-4DD2-8871-45540E040B80")]
        [AssociationId("30ED486E-8142-4EBF-AAF4-377E9181FA55")]
        [RoleId("03DF0077-B8E8-468D-8017-F11EB74F1A26")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party RentedBy { get; set; }

        #region Allors
        [Id("5E13E62A-FD8F-49D9-9BFA-6701892FC243")]
        [AssociationId("81FC3487-07BD-48A1-BB67-22C82E9AD67A")]
        [RoleId("CCB5A314-0ADD-4535-9622-B34D6D1E0A6E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public PurchaseOrder PurchaseOrder{ get; set; }

        #region Allors
        [Id("56FBFE00-2480-476C-86C0-140D419C33DE")]
        [AssociationId("6D33AAA1-8F48-4454-9583-E250B9B5B6BD")]
        [RoleId("6ED98788-BD73-495F-B2DE-871299372165")]
        #endregion
        [Required]
        [Workspace]
        public bool AvailableForSale { get; set; }

        #region Allors
        [Id("BB954677-BEB7-4092-96C6-44D36503174D")]
        [AssociationId("9EB5189B-3F6F-423A-A48C-05B1EB337169")]
        [RoleId("E6382C24-8AC5-4E3E-B6E3-14AE7B48241E")]
        #endregion
        [Workspace]
        public string CustomerReferenceNumber { get; set; }

        #region Allors
        [Id("15179D87-D6D8-438A-AB36-E30418DAE2AE")]
        [AssociationId("103485C1-7BB8-4238-872C-BC83BBE450B8")]
        [RoleId("55AA0337-3E3F-4A8C-8455-13E82D665692")]
        #endregion
        [Workspace]
        public DateTime RentalFromDate { get; set; }

        #region Allors
        [Id("83220BB7-AB7D-4CE4-A3FA-1EF13720E167")]
        [AssociationId("58C01E0A-6FD3-43F3-B885-1F69F47AD531")]
        [RoleId("A11A996B-C61E-4970-82D9-758F8426254F")]
        #endregion
        [Workspace]
        public DateTime RentalThroughDate { get; set; }

        #region Allors
        [Id("D5ABF25F-31BB-4406-AC4A-4171E42EF0D7")]
        [AssociationId("36A67ACC-3BE1-4037-AA67-498A86B9F6C1")]
        [RoleId("FCC0A100-7E70-4A5C-B763-EEC6916F189B")]
        #endregion
        [Workspace]
        public DateTime ExpectedReturnDate{ get; set; }

        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Delete() { }
        #endregion
    }
}