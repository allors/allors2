namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("485C8073-22B6-402B-B0F0-479764CFB67A")]
    #endregion
    public partial class SerialisedItemVersion : Version, Deletable
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("4EE6B72D-B1EC-4586-8666-1FE8006F147A")]
        [AssociationId("4C57DA52-A994-4BFC-8169-68B5C1F520A2")]
        [RoleId("E1CE647A-860A-4782-BF1F-8229DD2FA7F8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItemState SerialisedItemState { get; set; }

        #region Allors
        [Id("76B16EB6-4526-4024-B29A-F51AAB49F20E")]
        [AssociationId("87FCF20A-EF42-4360-BC86-7926C1EC05B7")]
        [RoleId("4A973109-35EB-44A5-AD09-21622F5134A8")]
        #endregion
        [Workspace]
        public string SerialNumber { get; set; }

        #region Allors
        [Id("94F10411-FDDA-4A7D-8617-AF7BFE36BE9F")]
        [AssociationId("66BC35F8-195A-4FC5-AEF8-D7AD7CF1BC52")]
        [RoleId("EBAA814C-23CA-48FA-A7E3-2887AC5E5997")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SerialisedItemCharacteristic[] SerialisedItemCharacteristics { get; set; }

        #region Allors
        [Id("7E46E5D7-FBFB-4D7A-9EC6-522FBE37826D")]
        [AssociationId("50D535CB-A1C0-4E75-B2F3-48EF6FC4CC0F")]
        [RoleId("10B3EC07-A3DA-4E50-8858-533D04B56E6B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Ownership Ownership { get; set; }

        #region Allors
        [Id("25178972-F921-47CA-B32D-D63CCF9A4AC8")]
        [AssociationId("88BD2180-F2DF-440A-AB46-6E330BAA1DF1")]
        [RoleId("052D5C67-8859-4C7C-9FDB-AD02D6966687")]
        #endregion
        [Workspace]
        public int AcquisitionYear { get; set; }

        #region Allors
        [Id("59266D15-C7B2-4BFD-8470-0517B634AA50")]
        [AssociationId("44833E77-7091-4EFA-A454-938716828DDA")]
        [RoleId("D3203DDB-03EF-4AA1-8638-61FCBFE1F3F7")]
        #endregion
        [Workspace]
        public int ManufacturingYear { get; set; }

        #region Allors
        [Id("9C1372D7-8FDD-4673-9457-E38B1EBBDC0B")]
        [AssociationId("F0B7AA69-4F63-4410-845A-0112460E0B19")]
        [RoleId("892964C1-2BD6-4409-B684-7016D5741FF9")]
        #endregion
        [Workspace]
        public decimal ReplacementValue { get; set; }

        #region Allors
        [Id("F1EBE4D1-AFFE-4A33-83D4-8C8D0DF09C49")]
        [AssociationId("109E634C-DB76-4F7C-A00C-0431646D8D30")]
        [RoleId("488E9A3E-1647-4D2B-8199-8068EFFA2F91")]
        #endregion
        [Workspace]
        public int LifeTime { get; set; }

        #region Allors
        [Id("880D7E08-AB6D-4EBB-8743-815935C452C3")]
        [AssociationId("2AE21241-3AEC-4364-9610-83D45CD033A0")]
        [RoleId("DE506840-AF2A-42E9-AC41-2DABCECC24DB")]
        #endregion
        [Workspace]
        public int DepreciationYears { get; set; }

        #region Allors
        [Id("03D549E9-0DCD-4674-A789-8D9CB6CF0377")]
        [AssociationId("5E1D6798-2006-4163-9CE6-9AE9F625B47D")]
        [RoleId("54A334B2-D965-4F3E-B0A7-3CFAEF2A3315")]
        #endregion
        [Workspace]
        public decimal PurchasePrice { get; set; }

        #region Allors
        [Id("D7B6361C-2387-4838-BBB1-B6F001D9E2B4")]
        [AssociationId("5A5FCF42-4CF6-4BF3-96E1-03D94853A205")]
        [RoleId("478A9F0F-1211-4EBB-9308-7FFEA934470A")]
        #endregion
        [Workspace]
        public decimal ExpectedSalesPrice { get; set; }

        #region Allors
        [Id("628362EA-82A6-4E17-A9AF-6E490BE81F18")]
        [AssociationId("F2773425-D379-48FD-8E38-7F6C01D97427")]
        [RoleId("20DC666A-C2C0-4A02-9BD3-FBCD0AED7AA0")]
        #endregion
        [Workspace]
        public decimal RefurbishCost { get; set; }

        #region Allors
        [Id("1D0FB264-4951-4387-B2E5-FA38C61ACF82")]
        [AssociationId("226BC0BD-A348-4046-B4A2-CB7218D5F8E6")]
        [RoleId("57F30811-FF1E-40A5-B677-9FFBCD866122")]
        #endregion
        [Workspace]
        public decimal TransportCost { get; set; }

        #region Allors
        [Id("8AA2ED2E-BB4D-489A-81BD-9B5075AFC7CA")]
        [AssociationId("E8F50F76-62D3-4531-9036-6138450DCAFB")]
        [RoleId("D5A41DB7-7062-4CE7-9D18-3AE9ECA67DB4")]
        #endregion
        [Workspace]
        [Size(-1)]
        public string InternalComment { get; set; }

        #region Allors
        [Id("15F649C2-B482-43B1-B9D5-168CEE7BED4D")]
        [AssociationId("57B07164-788A-4354-B88A-0B3CB1ACF172")]
        [RoleId("31DB637E-F166-449A-B81D-18F71A93D3C5")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Details { get; set; }

        #region Allors
        [Id("34F61A40-3794-4195-A269-749C68CBC8A4")]
        [AssociationId("747929AE-9654-45F5-A450-97ADFF3813F8")]
        [RoleId("248B523D-D654-480F-BA7F-DB446E7D5CEB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party SuppliedBy { get; set; }

        #region Allors
        [Id("92A371AC-A079-403F-9219-829F217B3EB6")]
        [AssociationId("9BF64D86-3570-443D-96A4-FEDDC47E11F7")]
        [RoleId("1DFA663E-DADB-40FC-823E-70C65F11117D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party OwnedBy { get; set; }

        #region Allors
        [Id("46F8C336-584F-4B18-AA4C-71A576EE2136")]
        [AssociationId("59C4E4B9-7217-4EB7-886C-3E5AC4966F6D")]
        [RoleId("74CDC970-E4A5-4D27-A3B9-CD9045774A63")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party RentedBy { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
            
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}