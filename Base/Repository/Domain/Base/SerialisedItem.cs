// <copyright file="SerialisedItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("5E594A00-15A4-4871-84E9-B8010A78FD21")]
    #endregion
    public partial class SerialisedItem : Deletable, FixedAsset, Versioned
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

        public string SearchString { get; set; }

        public LocalisedText[] LocalisedKeywords { get; set; }

        public Media[] PublicElectronicDocuments { get; set; }

        public LocalisedMedia[] PublicLocalisedElectronicDocuments { get; set; }

        public Media[] PrivateElectronicDocuments { get; set; }

        public LocalisedMedia[] PrivateLocalisedElectronicDocuments { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion InheritedProperties

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
        [Id("9C9A7694-4E41-46D7-B33C-14A703370A5B")]
        [AssociationId("FBF63B46-AD14-43EA-AD29-31652901BE89")]
        [RoleId("106E5048-AC33-427C-8B9E-462A9A998879")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItemState SerialisedItemState { get; set; }

        #region Allors
        [Id("330381e1-f1de-4f44-9c08-0417c2df3c0d")]
        [AssociationId("ce2cab1a-e89c-4a00-8647-1dcf74ed22e8")]
        [RoleId("c543b283-aac2-4d50-b604-89bbc491fbaa")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItemAvailability SerialisedItemAvailability { get; set; }

        #region Allors
        [Id("B6DD4F80-EE97-446E-9779-610FF07F13B2")]
        [AssociationId("3CC4D71C-3CBF-4F6B-997A-C1FD113FD25B")]
        [RoleId("0CC2B6F1-69F7-404A-9620-57152FE2782C")]
        #endregion
        [Derived]
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
        public Media[] SecondaryPhotos { get; set; }

        #region Allors
        [Id("65BBB01F-66A1-47E2-B206-2F1BE6C91398")]
        [AssociationId("6437F990-28F7-4A70-BF04-D27F022C35FE")]
        [RoleId("1D1102AF-F8B1-46D4-A2E1-3C9700D54767")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToMany)]
        public Media[] AdditionalPhotos { get; set; }

        #region Allors
        [Id("2A6D6DA0-A106-400E-9F2F-BA19D3F9EC77")]
        [AssociationId("47426B6B-6F51-4D46-89E5-F48B01B3203E")]
        [RoleId("ED9FF9B5-0000-4FD4-9586-9E6CEABE1F0C")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToMany)]
        public Media[] PrivatePhotos { get; set; }

        #region Allors
        [Id("18A320F1-2F65-4E49-A615-D88EDD15AC5C")]
        [AssociationId("7909A953-6F94-4CCE-B214-F6BE9272DFB1")]
        [RoleId("304C12C1-91F6-42DD-BB65-3DBF87A77F17")]
        #endregion
        [Workspace]
        [Size(-1)]
        [MediaType("text/markdown")]
        public string InternalComment { get; set; }

        #region Allors
        [Id("7A2A878B-1428-4C75-9A52-8725606FAA41")]
        [AssociationId("98B173A0-51DA-48A6-9556-4B8F2CFDC72B")]
        [RoleId("D86152FD-1D45-463E-B5FC-481F6E0D4CAE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public Party SuppliedBy { get; set; }

        #region Allors
        [Id("C16A8A73-84D3-4889-8B95-B8B05CB561DE")]
        [AssociationId("D46271CB-6AA1-419B-8AAB-2C547FACFD29")]
        [RoleId("2305CB0E-4280-41B6-B058-8A2DFC4DD7CC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party AssignedSuppliedBy { get; set; }

        #region Allors
        [Id("66CEB3A4-C1AD-4CAD-BBB9-F29FB12669DA")]
        [AssociationId("B67B1EA5-1CC1-4958-B8A5-13D9C60B407D")]
        [RoleId("AFC60632-E929-40E9-8139-CE1B4B2FFE1B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation Buyer { get; set; }

        #region Allors
        [Id("3a7c5038-bd54-4caa-8f61-7d8a5336f24b")]
        [AssociationId("f8f573c9-6313-4862-9abb-5dca2592c48d")]
        [RoleId("4c01f67c-5ca8-4262-8264-9e864b6c696b")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation Seller { get; set; }

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
        public PurchaseOrder PurchaseOrder { get; set; }

        #region Allors
        [Id("b2188137-dfd4-4f0a-a76d-a2266f87e352")]
        [AssociationId("e7562ac0-66fd-445b-84bb-8c6a93f4e5d5")]
        [RoleId("38b120a4-9d93-4f5c-8b0d-ef0949ed70e5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public PurchaseInvoice PurchaseInvoice { get; set; }

        #region Allors
        [Id("56FBFE00-2480-476C-86C0-140D419C33DE")]
        [AssociationId("6D33AAA1-8F48-4454-9583-E250B9B5B6BD")]
        [RoleId("6ED98788-BD73-495F-B2DE-871299372165")]
        #endregion
        [Required]
        [Workspace]
        public bool AvailableForSale { get; set; }

        #region Allors
        [Id("D5E98D57-6DAC-46E6-A30A-E70044EC5C40")]
        [AssociationId("340B7FCA-2052-424E-9F18-48F5D4789C9E")]
        [RoleId("971EDD90-1F7B-41C0-836A-A52550CE81AE")]
        #endregion
        [Required]
        [Workspace]
        public bool ShowOnFrontPage { get; set; }

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
        public DateTime ExpectedReturnDate { get; set; }

        // TODO: Don't use WHERE in role name
        #region Allors
        [Id("E927291E-21A1-4289-B5AF-4A2CA2996DA2")]
        [AssociationId("2955FFAE-0058-432B-801B-3E440A9308B8")]
        [RoleId("71F28692-AD46-4B35-9424-DB9C8269C82A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Part PartWhereItem { get; set; }

        #region Allors
        [Id("2b8a24f3-ce26-4b53-94b2-d7d0cef3f6b1")]
        [AssociationId("357c0a38-9888-40f8-b22c-7236f69ef646")]
        [RoleId("0dd73bc0-5030-4c2e-b4b5-57d227f56d76")]
        #endregion
        [Required]
        public Guid DerivationTrigger { get; set; }

        #region Allors
        [Id("50db0036-a15c-418d-b354-ad3b5b1c4bd6")]
        [AssociationId("1eb73e6c-e4b6-474f-b338-51bd8dc6ce8c")]
        [RoleId("e0ca04f1-1a86-40c0-82ba-12d8197d4f8e")]
        #endregion
        [Indexed]
        [Workspace]
        public string DisplayProductCategories { get; set; }

        #region Allors
        [Id("23572f9e-9423-49ac-baf7-c0ecb039c823")]
        [AssociationId("89bda606-88e3-4900-a1d0-86f11d36a550")]
        [RoleId("cf15f073-b214-4320-bb79-04584cd70175")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        public string SerialisedItemAvailabilityName { get; set; }

        #region Allors
        [Id("70442e8b-9965-4c5a-a2a7-10b11ee8620a")]
        [AssociationId("c0eda4a2-cff9-420b-828a-396beb7b47a2")]
        [RoleId("92e615a3-782b-4daf-84c2-832f360168da")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        public string SuppliedByPartyName { get; set; }

        #region Allors
        [Id("ea0a8474-490b-4ae3-8c0d-546b9167b552")]
        [AssociationId("6429cb2c-2c8a-4ad6-842c-cbf35b829ede")]
        [RoleId("a5b2c7cd-a64f-454a-aefd-0f4630fcc715")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        public string OwnedByPartyName { get; set; }

        #region Allors
        [Id("0e2a38d0-c550-4cd7-8fc1-e2f93c546b5d")]
        [AssociationId("7c905f64-5f21-4316-8fc7-eb758cbfee92")]
        [RoleId("df0424ed-ac64-4c0c-b0ed-57a04270b2d8")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        public string RentedByPartyName { get; set; }

        #region Allors
        [Id("148487bd-4561-400a-8540-ae1e57fa2268")]
        [AssociationId("ae0a240d-442a-49ab-be1f-057b59ff7645")]
        [RoleId("cd419e8d-7431-4c2e-9baa-7b8d6c781a31")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        public string OwnershipByOwnershipName { get; set; }

        #region Allors
        [Id("80c6e34f-aadd-4ef6-b8cf-da532833ac03")]
        [AssociationId("d668026d-14b7-4071-84bf-f8e28c7dfbc9")]
        [RoleId("63799217-89c0-41f7-bf35-3ec9f962ecf2")]
        #endregion
        [Required]
        [Derived]
        [Workspace]
        public bool OnQuote { get; set; }

        #region Allors
        [Id("85daec66-1768-40ce-a91b-f987256ee0ed")]
        [AssociationId("0abfa6f0-3a5a-46eb-9070-10110d9ee25d")]
        [RoleId("9ff0c909-630c-4b04-b58e-77d9479d9871")]
        #endregion
        [Required]
        [Derived]
        [Workspace]
        public bool OnSalesOrder { get; set; }

        #region Allors
        [Id("7885e0a2-514d-4eb9-b654-f047eda00574")]
        [AssociationId("2fad91dd-f819-409a-adcd-de341ba8568b")]
        [RoleId("d65d7c59-abfc-4a76-944f-3e5b734501d0")]
        #endregion
        [Required]
        [Derived]
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

        #region Allors
        [Id("3a008524-c74c-48e7-8aa8-a8f9743bd32f")]
        #endregion
        [Workspace]
        public void DeriveDisplayProductCategories() { }

        #region Allors
        [Id("c67862f1-32dc-4c73-993c-8d6d8afacfc8")]
        #endregion
        [Workspace]
        public void DerivePurchaseInvoice() { }

        #region Allors
        [Id("ce43d614-bdf6-4c27-864b-92fed0cfeb53")]
        #endregion
        [Workspace]
        public void DerivePurchaseOrder() { }

        #region Allors
        [Id("df08a96b-bada-4e8a-bb22-0385c406bdde")]
        #endregion
        [Workspace]
        public void DerivePurchasePrice() { }
    }
}
