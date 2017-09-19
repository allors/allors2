namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("4a70cbb3-6e23-4118-a07d-d611de9297de")]
    #endregion
    public partial class SerialisedInventoryItem : InventoryItem
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public Guid UniqueId { get; set; }
        public ProductCharacteristicValue[] ProductCharacteristicValues { get; set; }
        public InventoryItemVariance[] InventoryItemVariances { get; set; }
        public Part Part { get; set; }
        public Container Container { get; set; }
        public string Name { get; set; }
        public Lot Lot { get; set; }
        public string Sku { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public ProductCategory[] DerivedProductCategories { get; set; }
        public Good Good { get; set; }
        public ProductType ProductType { get; set; }
        public Facility Facility { get; set; }

        #endregion

        #region Allors
        [Id("de9caf09-6ae7-412e-b9bc-19ece66724da")]
        [AssociationId("ba630eb8-3087-43c6-9082-650094a0226e")]
        [RoleId("c0ada954-d86e-46c3-9a99-09209fb812a5")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        string SerialNumber { get; set; }

        #region Allors
        [Id("D9D4FF13-6D54-4F35-9A81-902E0BB86545")]
        [AssociationId("991A97F7-4277-442D-9DE2-26348B22002C")]
        [RoleId("CD405E7C-D058-4306-8495-54BF3D0974E1")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        Ownership Ownership { get; set; }

        #region Allors
        [Id("753F1209-A92C-4BEA-85FE-7B7C42C5C8A3")]
        [AssociationId("472A51BF-4FC9-4CAC-8118-272D1F2F1D62")]
        [RoleId("5A330D37-3CDE-4F80-BA51-92A77F4FDDFA")]
        #endregion
        [Workspace]
        string Owner { get; set; }

        #region Allors
        [Id("E511EE11-FA2E-4F84-8010-EE1453C609F3")]
        [AssociationId("E6598114-E52B-4343-B0EC-E943262C5380")]
        [RoleId("DAA8C492-9675-4192-8651-4B9BD05C9B70")]
        #endregion
        [Workspace]
        int AcquisitionYear { get; set; }

        #region Allors
        [Id("CCDD8203-F635-4821-876D-A83A925C145D")]
        [AssociationId("CCDD4029-2268-4822-B7FE-B76864B61DBE")]
        [RoleId("223C23A1-5514-47A3-BAFF-5F88D3DC5B59")]
        #endregion
        [Workspace]
        int ManufacturingYear { get; set; }

        #region Allors
        [Id("DDECD426-40C7-4D17-A225-2C46B47F0C89")]
        [AssociationId("B2F5E3CA-E4BF-43B8-B812-D9479A74DF6E")]
        [RoleId("3570A014-1A46-4C61-8D7F-14D07BF1F5AA")]
        #endregion
        [Workspace]
        decimal ReplacementValue { get; set; }

        #region Allors
        [Id("E25F1487-6F08-4DC5-9838-BAE4FF990ADA")]
        [AssociationId("26F70AEA-7F70-48C4-BDCA-41DC58E3BDB3")]
        [RoleId("F386A2DB-5DBE-4BD6-921B-024A9C80105C")]
        #endregion
        [Workspace]
        int LifeTime { get; set; }

        #region Allors
        [Id("D96B2474-B8AE-40F4-9D86-4CA09E2B6965")]
        [AssociationId("AA3AA882-D022-42A1-8E1A-4C7901358EE8")]
        [RoleId("19E428F3-9C68-4C59-BD19-68EA17688F04")]
        #endregion
        [Workspace]
        int DepreciationYears { get; set; }

        #region Allors
        [Id("ECE5838C-6E0B-4889-91DA-4F9277760E9D")]
        [AssociationId("0CCF5035-5E6E-4F06-9921-35B8F922BFA2")]
        [RoleId("4519FC49-C403-4FE2-B85F-BB7F01B6B907")]
        #endregion
        [Workspace]
        decimal PurchasePrice { get; set; }

        #region Allors
        [Id("53E31ACE-5F48-4CBF-9D35-003534E1A1F1")]
        [AssociationId("91414EC6-6C95-4CEF-A816-646BD8795F5A")]
        [RoleId("B30E951F-29E7-4013-BDAA-E3DAF7AE2FE3")]
        #endregion
        [Workspace]
        decimal ExpectedSalesPrice { get; set; }

        #region Allors
        [Id("6EBA501E-5EB8-4B9C-B0E7-2658562D8F44")]
        [AssociationId("3F8D9AFD-B4ED-461B-A829-31745FE8DCB2")]
        [RoleId("D0AD48FF-6D74-4D07-9F92-08A9AB1FB3B7")]
        #endregion
        [Workspace]
        decimal RefurbishCost { get; set; }

        #region Allors
        [Id("901E5D13-4B43-4FE4-9A79-8EFED2CAFE74")]
        [AssociationId("C223E10B-6AFF-4B7B-A8ED-89B7C8A69FF9")]
        [RoleId("D1B62377-F8F5-4713-BBC4-AF47C2A89AF1")]
        #endregion
        [Workspace]
        decimal TransportCost { get; set; }

        #region Allors
        [Id("887163E8-720C-4CFD-83DC-7B70A2B155E3")]
        [AssociationId("08923E88-696D-490F-82BC-C775156023FD")]
        [RoleId("CE39E19B-1BE5-487A-A728-BBE7BFFD9901")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        SerialisedInventoryItemObjectState CurrentObjectState { get; set; }

        #region Versioning
        #region Allors
        [Id("14266ECC-B4FF-4365-9087-0F67946246D2")]
        [AssociationId("3B2D539D-3886-4614-B4D5-170F5A4D77DD")]
        [RoleId("FCDED27A-83F2-4D97-A74A-49ED05F5C212")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SerialisedInventoryItemVersion[] AllVersions { get; set; }

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
        [Id("15DE96E6-4406-4D6F-80DA-581D96A91706")]
        [AssociationId("8798C7DF-D871-4834-B366-E2306E627D1F")]
        [RoleId("84F259F6-90B6-4C55-887C-7E6D97066A2A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SerialisedInventoryItemVersion PreviousVersion { get; set; }

        #region Allors
        [Id("F92C3F40-87A1-49D1-9FB0-F31D3FEAE671")]
        [AssociationId("5720BB2C-31B8-4893-86A9-8739887AF8C7")]
        [RoleId("3D5C935C-90CF-440B-8961-2AF1551ACE18")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SerialisedInventoryItemVersion CurrentStateVersion { get; set; }
        
        #region Allors
        [Id("DECD9EDC-C9CD-4F23-9212-FF23B201AE9F")]
        [AssociationId("2572229F-57F7-4B39-9108-BD21A7180845")]
        [RoleId("17A6072D-96C1-432E-840F-8E041BC18534")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SerialisedInventoryItemVersion[] AllStateVersions { get; set; }
        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion
   }
}