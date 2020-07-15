// <copyright file="SerialisedItemVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

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
        [Id("60cf90a6-7049-4692-ac73-1394478b0fb6")]
        [AssociationId("2dfc6983-7972-4dd8-a582-bcf328b65d23")]
        [RoleId("439129c1-1904-4077-9871-8810da440e72")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItemAvailability SerialisedItemAvailability { get; set; }

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
        [Id("8AA2ED2E-BB4D-489A-81BD-9B5075AFC7CA")]
        [AssociationId("E8F50F76-62D3-4531-9036-6138450DCAFB")]
        [RoleId("D5A41DB7-7062-4CE7-9D18-3AE9ECA67DB4")]
        #endregion
        [Workspace]
        [Size(-1)]
        public string InternalComment { get; set; }

        #region Allors
        [Id("53857cc0-5fcb-43ee-960d-a9d0c2189b18")]
        [AssociationId("b2dd2602-23eb-408b-991b-63e0d14d9f5d")]
        [RoleId("5d1fc961-e30e-4ae2-88b7-8a17c23a4e40")]
        #endregion
        [Required]
        [Workspace]
        public bool AvailableForSale { get; set; }

        #region Allors
        [Id("c8217953-ad82-4db3-b70f-231bae89c298")]
        [AssociationId("ddab0c06-19fe-4aa0-a897-d0dafdd9ebde")]
        [RoleId("e5614e7c-1915-4b40-9e2f-06b369fe5a48")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation Seller { get; set; }

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

        #region Allors
        [Id("b128557f-63b3-4626-b6a4-e53dce6ddf67")]
        [AssociationId("c8df3cd3-6fb7-498d-afc0-cf97271c6259")]
        [RoleId("171b3432-2c6c-40a7-9707-c90a11d46145")]
        #endregion
        [Workspace]
        public bool OnQuote { get; set; }

        #region Allors
        [Id("c470b360-d7aa-4ce6-bc69-43d8829e5405")]
        [AssociationId("0ecceed7-45c5-4c5e-a00b-3c23875db588")]
        [RoleId("f47e2eec-3863-445c-aa6c-699e0009b506")]
        #endregion
        [Workspace]
        public bool OnSalesOrder { get; set; }

        #region Allors
        [Id("98ff5b83-6cae-4c5e-9137-f8ef9545b189")]
        [AssociationId("be222c46-6f7f-4ea7-9616-fa38858f7b7e")]
        [RoleId("eaa51998-cf1c-4354-8af2-8beec160da34")]
        #endregion
        [Workspace]
        public bool OnWorkEffort { get; set; }

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
