namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("F9111BDF-A0B6-40CB-B33A-0A856B357327")]
    #endregion
    public partial class SerialisedInventoryItemVersion : InventoryItemVersion
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public InventoryItemVariance[] InventoryItemVariances { get; set; }
        public Part Part { get; set; }
        public string Name { get; set; }
        public Lot Lot { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public ProductCategory[] DerivedProductCategories { get; set; }
        public Good Good { get; set; }
        public Facility Facility { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("7F30A827-CBFA-4716-B0BD-08641CB66B1B")]
        [AssociationId("854A82D0-4CB3-42A2-8C0D-7DFE999BEE7F")]
        [RoleId("3860102B-4D92-40B8-9CEB-634D5212AFCF")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public SerialisedInventoryItemState SerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("6DD0FA27-1140-4F51-A642-35D8C1126684")]
        [AssociationId("DD8AD770-CCFF-4102-9830-CD6297299936")]
        [RoleId("53709128-15A0-4242-B887-5CA76C409F4D")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string SerialNumber { get; set; }

        #region Allors
        [Id("E9C7EDF0-C3CC-46E7-BD05-E9E39AF9641C")]
        [AssociationId("800DD89D-54F8-4587-B00D-9B3665925A4E")]
        [RoleId("D077E627-E1D8-4BC7-B452-BB9945B2F33B")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public Ownership Ownership { get; set; }

        #region Allors
        [Id("4F1B3DEA-B564-433A-B6E5-BF25B699F5EE")]
        [AssociationId("93C58E16-03AB-43BA-8DA9-014D46C428DA")]
        [RoleId("F218BE19-5EF1-45FC-B545-8DDF16153C57")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public Organisation Owner { get; set; }

        #region Allors
        [Id("839FE92E-18D5-4A05-BBAC-04C1D39E69AA")]
        [AssociationId("5B3C8DC5-2DA3-426E-B73F-88EC120E8234")]
        [RoleId("78F597ED-4990-4BE9-ADBB-2AA8960C7485")]
        #endregion
        [Workspace]
        public int AcquisitionYear { get; set; }

        #region Allors
        [Id("DBA294BA-0291-4F93-8C6F-1FA31ACE667A")]
        [AssociationId("2C51AD2B-1CA9-4081-966B-27222FE3F16C")]
        [RoleId("ED8C89F4-8551-4EA9-A02A-37EC8D97B3F7")]
        #endregion
        [Workspace]
        public int ManufacturingYear { get; set; }

        #region Allors
        [Id("19950E96-99F4-468C-88DD-835ED418AD03")]
        [AssociationId("CCD79283-9FA2-4A4F-8B1A-C5487028E415")]
        [RoleId("4C42C74C-4403-4035-8C5C-157D34986FF7")]
        #endregion
        [Workspace]
        public decimal ReplacementValue { get; set; }

        #region Allors
        [Id("744286AA-CCA9-45AD-BCE8-E9970F23C526")]
        [AssociationId("1CE3391A-DD2D-4A5E-A39E-D440A035E0BD")]
        [RoleId("46AE9828-8D90-423C-BFD1-C020BE8BC8DB")]
        #endregion
        [Workspace]
        public int LifeTime { get; set; }

        #region Allors
        [Id("34A7281B-DD96-4C01-B771-47C22C23A12E")]
        [AssociationId("677978EE-380B-4790-A13F-C115980FC6DF")]
        [RoleId("7BBC34AE-E821-43C9-A78E-C725B67412FD")]
        #endregion
        [Workspace]
        public int DepreciationYears { get; set; }

        #region Allors
        [Id("C8AFC3D8-6AE0-4400-80BC-3D8C729555C5")]
        [AssociationId("179A11D0-1E8C-415C-9B56-67DBFF784239")]
        [RoleId("5B236B92-90B1-46CD-9A30-B0D3865BA096")]
        #endregion
        [Workspace]
        public decimal PurchasePrice { get; set; }

        #region Allors
        [Id("81B4FA27-1D6E-4C7C-B0C2-23F363BAD482")]
        [AssociationId("4A913F3C-6DD0-417B-8123-95ABF1D8DD6E")]
        [RoleId("B27B5DB4-CC50-490A-9161-D50DD435840F")]
        #endregion
        [Workspace]
        public decimal ExpectedSalesPrice { get; set; }

        #region Allors
        [Id("B7E62F3F-F83C-48E0-AAF4-3947DFDCE6E8")]
        [AssociationId("28CA9CE9-F69B-4F42-93B4-F4B78270254E")]
        [RoleId("D72C0D23-CBD6-49EF-9044-2C196026481F")]
        #endregion
        [Workspace]
        public decimal RefurbishCost { get; set; }

        #region Allors
        [Id("B38936D9-5EE6-422F-A4F7-14DC33A12832")]
        [AssociationId("DD60FDAA-7B59-4C79-8D0F-375336212C58")]
        [RoleId("6E9FC065-EC6A-4F56-8FDA-8563A36E0D55")]
        #endregion
        [Workspace]
        public decimal TransportCost { get; set; }
        
        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}