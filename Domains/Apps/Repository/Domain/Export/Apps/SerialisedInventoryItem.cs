namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("4a70cbb3-6e23-4118-a07d-d611de9297de")]
    #endregion
    public partial class SerialisedInventoryItem : InventoryItem, Versioned
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public Permission[] DeniedPermissions { get; set; }
        
        public SecurityToken[] SecurityTokens { get; set; }
        
        public Guid UniqueId { get; set; }

        public InventoryItemTransaction[] InventoryItemTransactions { get; set; }
        
        public Part Part { get; set; }
        
        public string Name { get; set; }
        
        public Lot Lot { get; set; }
        
        public UnitOfMeasure UnitOfMeasure { get; set; }
        
        public Facility Facility { get; set; }

        #endregion

        #region ObjectStates
        #region SerialisedInventoryItemState
        #region Allors
        [Id("CCB71B4F-1A3F-4D08-B3E4-380FB2D513FF")]
        [AssociationId("D35F6D66-DAA2-4044-B4E9-FBCFBC7D2CD9")]
        [RoleId("35C5FABD-1F83-4D6C-8268-F027CC9F7B51")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SerialisedInventoryItemState PreviousSerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("72A268C1-4A32-48C1-BB2D-837AC1DF361E")]
        [AssociationId("0ED35F86-9400-4F89-8F9D-A8D6A7408A78")]
        [RoleId("DF809B37-E9DA-463C-B532-02E44BC0394F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SerialisedInventoryItemState LastSerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("7E757767-61AC-49E9-97CF-DE929C015D5B")]
        [AssociationId("60B25B4C-B160-498C-A3CF-EBB057EACACC")]
        [RoleId("87B18D10-A205-40E7-8403-733791AF3FD9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedInventoryItemState SerialisedInventoryItemState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("235F117A-3288-4729-8348-92BCEBCDB3B6")]
        [AssociationId("63D481C8-C311-4789-9145-A0AA9F8648FD")]
        [RoleId("F5D5D294-C53E-4174-BE73-687400481205")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SerialisedInventoryItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("14266ECC-B4FF-4365-9087-0F67946246D2")]
        [AssociationId("3B2D539D-3886-4614-B4D5-170F5A4D77DD")]
        [RoleId("FCDED27A-83F2-4D97-A74A-49ED05F5C212")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SerialisedInventoryItemVersion[] AllVersions { get; set; }
        #endregion

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
        public SerialisedInventoryItemCharacteristic[] SerialisedInventoryItemCharacteristics { get; set; }

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
        [Workspace]
        public decimal PurchasePrice { get; set; }

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
        [Id("B5D9E50B-3004-47C0-9C8D-62935DB15ECC")]
        [AssociationId("609D27BC-96C9-48EF-9516-6B45AECBEC20")]
        [RoleId("DAEF2150-D69C-4255-B78E-70ACFA63EA06")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Details { get; set; }

        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Delete() { }
        #endregion
    }
}