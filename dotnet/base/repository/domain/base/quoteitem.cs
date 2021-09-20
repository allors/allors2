// <copyright file="QuoteItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("01fc58a0-89b8-4dc0-97f9-5f628b9c9577")]
    #endregion
    public partial class QuoteItem : Priceable, Versioned, Deletable
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public decimal AssignedUnitPrice { get; set; }

        public decimal UnitBasePrice { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal UnitSurcharge { get; set; }

        public decimal UnitDiscount { get; set; }

        public decimal UnitVat { get; set; }

        public VatRegime AssignedVatRegime { get; set; }

        public VatRegime DerivedVatRegime { get; set; }

        public VatRate VatRate { get; set; }

        public decimal TotalBasePrice { get; set; }

        public decimal TotalVat { get; set; }

        public decimal TotalIncVat { get; set; }

        public decimal TotalExVat { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalDiscountAsPercentage { get; set; }

        public decimal TotalSurcharge { get; set; }

        public decimal TotalSurchargeAsPercentage { get; set; }

        public decimal GrandTotal { get; set; }
        public DiscountAdjustment[] DiscountAdjustments { get; set; }

        public SurchargeAdjustment[] SurchargeAdjustments { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region ObjectStates
        #region QuoteItemState
        #region Allors
        [Id("72B768E0-1F06-4409-A3A0-9F2AE622CB0E")]
        [AssociationId("65CE6D55-2BC0-46AF-861A-C44743EB099F")]
        [RoleId("CF72E1F4-C856-406C-8642-61181B628857")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public QuoteItemState PreviousQuoteItemState { get; set; }

        #region Allors
        [Id("9BA3DB20-4F1C-472F-958F-D1D506ECB019")]
        [AssociationId("21CF4617-6175-40EE-A1BE-5BCFA2D296E2")]
        [RoleId("6CEC1FDD-ED7E-4458-A08D-A4866A7B01F7")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public QuoteItemState LastQuoteItemState { get; set; }

        #region Allors
        [Id("D4272795-F320-4DAA-9009-E1150197F890")]
        [AssociationId("8A555F65-2C11-4E7D-B329-DC76433DA3B2")]
        [RoleId("0601C352-3CFF-4D9E-A872-A2A78C4EB635")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public QuoteItemState QuoteItemState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("AD9C5BEA-6D7D-4417-8859-18D7D46DF8CC")]
        [AssociationId("4C91B89F-9207-4521-86EE-015A17DDB4B2")]
        [RoleId("0D558E2F-7D52-4AB8-99F6-484591B48EF0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public QuoteItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("DA5C696C-3496-49F4-B380-3D78851AC064")]
        [AssociationId("7FD9D35F-2F12-4E72-95F3-F6C56FAF495C")]
        [RoleId("1F87E6FA-379A-47EA-9FA9-71AFD7BF762B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public QuoteItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("77096A96-3445-441B-86EE-A2B60E9EAD91")]
        [AssociationId("F8807C79-2D86-4F59-8CD7-DBE241D21D7F")]
        [RoleId("EBB3E68F-1960-45B0-A03A-A365AEFAACA3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InvoiceItemType InvoiceItemType { get; set; }

        #region Allors
        [Id("E534ABCB-FB3B-4722-B740-7EE4C2DE7EF7")]
        [AssociationId("E9002889-294A-4834-A2C3-45E47F10D828")]
        [RoleId("61883933-D482-442C-AF73-D3310F24C096")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public QuoteItem[] QuotedWithFeatures { get; set; }

        #region Allors
        [Id("F8F42AF0-193A-4427-96AC-B20FAC637ADD")]
        [AssociationId("BA4AE4E9-33E3-4E18-BB37-96139761B579")]
        [RoleId("46B10647-7A09-4128-9437-12E3D87E7C82")]
        #endregion
        [Workspace]
        [Size(-1)]
        public string InternalComment { get; set; }

        #region Allors
        [Id("05c69ae6-e671-4520-87c7-5fa24a92c44d")]
        [AssociationId("3f668e84-81dc-479a-a26f-b4fbc1cd79ee")]
        [RoleId("e47f270a-f3d9-4c7b-968f-395bbf8e7e68")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party Authorizer { get; set; }

        #region Allors
        [Id("1214acee-1b91-4c16-b6d0-84f865b6a43a")]
        [AssociationId("b9120662-ebae-4f52-a913-4a3f9a91398e")]
        [RoleId("d008f8e2-a378-4e50-a9dd-32ffa427708c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Deliverable Deliverable { get; set; }

        #region Allors
        [Id("20a5f3d3-8b12-4717-874f-eb62ad0a1654")]
        [AssociationId("10c5839d-c046-4b43-919b-d647c70bd94f")]
        [RoleId("56e57558-988c-4b1a-a6f8-7f93f621bd06")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("28f5767e-16fa-40aa-89d9-c23ee29572d1")]
        [AssociationId("4d7a3080-b3f9-47e8-8363-474a94699772")]
        [RoleId("1da894ac-53bb-4414-b582-9bc6717f369a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("7A5BFCE5-D7FB-483C-AEE0-E05427EAAF2E")]
        [AssociationId("4551FA99-0633-466B-88D0-4BE1F343756C")]
        [RoleId("4E23BDA6-6A3A-4769-B641-17A48E12100A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SerialisedItem SerialisedItem { get; set; }

        #region Allors
        [Id("262a458d-0b38-4123-b210-576633297f44")]
        [AssociationId("e252b457-9fac-429d-a337-0c48a46c2bf0")]
        [RoleId("a7ae793d-d315-4ac1-93c7-783391b2d294")]
        #endregion
        [Workspace]
        public DateTime EstimatedDeliveryDate { get; set; }

        #region Allors
        [Id("D071BBFA-8960-4F02-8F55-702112A0F608")]
        [AssociationId("F4EA603F-AE0D-425A-AFA9-1162D9BB46CB")]
        [RoleId("EAA89947-2651-470E-8172-581B30929E12")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Workspace]
        public DateTime RequiredByDate { get; set; }

        #region Allors
        [Id("28c0e280-16ce-48fc-8bc4-734e1ea0cd36")]
        [AssociationId("49bd248e-a34f-43ce-b2fd-9db0d5b01db4")]
        [RoleId("6eb4000d-559d-42b2-b02b-452370fa15b4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Indexed]
        [Workspace]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("8b1280eb-0fef-450e-afc8-dbdc6fc65abb")]
        [AssociationId("8a93a23b-6be9-44db-8c92-4ad4c2cc405b")]
        [RoleId("1961e2a8-ecf5-4c7b-8815-8ee4b2461820")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Skill Skill { get; set; }

        #region Allors
        [Id("8be8dc07-a358-4b8d-a84c-01bd3efea6fb")]
        [AssociationId("803fc0c9-ad84-4679-8906-4f9536c7ff6d")]
        [RoleId("a997bb36-f534-4d90-9a90-947cc2a56a64")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public WorkEffort WorkEffort { get; set; }

        #region Allors
        [Id("d1f7f2cb-cbc8-42b4-a3f0-198ff35957de")]
        [AssociationId("0f429c19-5cb8-459a-b95a-9e3ec1e045f3")]
        [RoleId("0750e77a-40bd-4a0b-89a6-6e6fbb797cc4")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public QuoteTerm[] QuoteTerms { get; set; }

        #region Allors
        [Id("d7805656-dd9c-4144-a11f-efbb32e6ecb3")]
        [AssociationId("a1d818f2-8e1a-4984-b2d7-4b1f34558568")]
        [RoleId("3a3442f4-26af-407d-90c6-38c4d5d40bae")]
        #endregion
        [Required]
        [Workspace]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("F68BA38D-5F7B-49F8-B019-522E2E5463EC")]
        [AssociationId("AF9A870F-64F3-4772-8A9C-6E525294D12E")]
        [RoleId("149A11E3-2ED4-4581-BF64-19B1F7DCD574")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal TotalPriceAdjustment { get; set; }

        #region Allors
        [Id("dc00905b-bb4f-4a47-88d6-1ae6ce0855f7")]
        [AssociationId("f9a2cdde-485c-46a0-8f06-9f9687328737")]
        [RoleId("e3308741-e48e-4b91-81ef-de38dcb5d80d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public RequestItem RequestItem { get; set; }

        #region Allors
        [Id("06C27EDA-0DF1-4318-BC57-D62F8BF32B0C")]
        [AssociationId("48040485-66EC-4599-96B0-6685783245FF")]
        [RoleId("E5A7A323-B3F2-449E-A276-C28656EE6F0D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Synced]
        public Quote SyncedQuote { get; set; }

        #region Allors
        [Id("F8746889-097A-4C4E-BB55-511F0A8E3B41")]
        [AssociationId("9704CE4F-2FC9-4F11-A612-90A65D2E93C4")]
        [RoleId("F5A70A81-ADBE-4BFB-A3E0-045E1F7EB4E2")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Details { get; set; }

        #region Allors
        [Id("ecd72f14-a7f3-43d9-bc95-46f0339ab920")]
        [AssociationId("572c07ae-1636-4e47-8e9d-1011f7e8209d")]
        [RoleId("36d36820-0c0e-42c7-a05b-e105f57bd6c1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public IrpfRegime AssignedIrpfRegime { get; set; }

        #region Allors
        [Id("5ccce98c-eeec-439f-adf5-d472aa00eecd")]
        [AssociationId("35703d63-c4c0-4ea7-a70c-9e9e9ab358ae")]
        [RoleId("ddb00591-5cb3-47be-9aa3-8b6068389e5b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public IrpfRegime DerivedIrpfRegime { get; set; }

        #region Allors
        [Id("da79fdb4-2488-4b7c-926f-209d62d901fc")]
        [AssociationId("2ee018ef-54d8-4b06-b28f-d39c7c592968")]
        [RoleId("eb2e0ec8-cfcc-40ec-9af5-538f7d035cdf")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public IrpfRate IrpfRate { get; set; }

        #region Allors
        [Id("e646b243-674a-42c5-804a-596bfcc80d33")]
        [AssociationId("657746b3-4411-4e34-ad80-7b1f029e25c7")]
        [RoleId("a80887e2-641e-4edd-9c9c-f11a6edd971d")]
        #endregion
        [Required]
        [Derived]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        public decimal UnitIrpf { get; set; }

        #region Allors
        [Id("135abd04-f017-435b-b7db-b0a3dc730674")]
        [AssociationId("44ccd48b-ae20-4d5b-8f57-b13847f3d29b")]
        [RoleId("a4f6e5fb-f74e-465c-81fb-39f98880709b")]
        #endregion
        [Required]
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalIrpf { get; set; }

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
        [Id("57B07865-B4CA-4443-8877-0DDAC1EA106B")]
        #endregion
        [Workspace]
        public void Cancel() { }

        #region Allors
        [Id("BE5F50FE-4524-486C-8D3D-66AB32B27C7B")]
        #endregion
        [Workspace]
        public void Reject() { }

        #region Allors
        [Id("4388A6A1-43C7-4B15-AB36-C587D2997A34")]
        #endregion
        [Workspace]
        public void Order() { }

        #region Allors
        [Id("C6494A74-92B0-4C9F-9931-8D5C97647DCA")]
        #endregion
        [Workspace]
        public void Submit() { }

        #region Allors
        [Id("DC016E6B-24FE-415C-8C50-2B9E643734BE")]
        #endregion
        [Workspace]
        public void Send() { }

        #region Allors
        [Id("81f4a9c9-d170-4590-8b1e-c42900093645")]
        #endregion
        [Workspace]
        public void DeriveDetails() { }

    }
}
