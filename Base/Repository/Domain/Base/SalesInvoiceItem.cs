// <copyright file="SalesInvoiceItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("a98f8aca-d711-47e8-ac9c-25b607cbaef1")]
    #endregion
    public partial class SalesInvoiceItem : InvoiceItem, Versioned
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public string InternalComment { get; set; }

        public SalesTerm[] SalesTerms { get; set; }

        public DiscountAdjustment[] DiscountAdjustments { get; set; }

        public SurchargeAdjustment[] SurchargeAdjustments { get; set; }

        public decimal TotalInvoiceAdjustment { get; set; }

        public InvoiceVatRateItem[] InvoiceVatRateItems { get; set; }

        public InvoiceItem AdjustmentFor { get; set; }

        public string Message { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal Quantity { get; set; }

        public string Description { get; set; }

        public Invoice SyncedInvoice { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public decimal TotalDiscountAsPercentage { get; set; }

        public decimal UnitVat { get; set; }

        public VatRegime VatRegime { get; set; }

        public decimal TotalVat { get; set; }

        public decimal UnitSurcharge { get; set; }

        public decimal UnitDiscount { get; set; }

        public VatRate VatRate { get; set; }

        public decimal AssignedUnitPrice { get; set; }

        public decimal UnitBasePrice { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalIncVat { get; set; }

        public decimal TotalSurchargeAsPercentage { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalSurcharge { get; set; }

        public VatRegime AssignedVatRegime { get; set; }

        public decimal TotalBasePrice { get; set; }

        public decimal TotalExVat { get; set; }

        public decimal GrandTotal { get; set; }

        public IrpfRegime IrpfRegime { get; set; }

        public IrpfRegime AssignedIrpfRegime { get; set; }

        public IrpfRate IrpfRate { get; set; }

        public decimal UnitIrpf { get; set; }

        public decimal TotalIrpf { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region ObjectStates
        #region SalesInvoiceItemState
        #region Allors
        [Id("6033F4A9-9ABA-457C-9A44-218415E01B79")]
        [AssociationId("A71CB471-C8CB-42F1-A1AF-E32F71BEC61F")]
        [RoleId("D6C62ED7-07B7-44FC-A774-1BD6B54554D9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesInvoiceItemState PreviousSalesInvoiceItemState { get; set; }

        #region Allors
        [Id("7623707D-C0F7-47D8-9B39-E34A55FC087B")]
        [AssociationId("E80895A6-EAAE-46B3-8823-3B2CD4DA8324")]
        [RoleId("48627D83-BEEB-42B1-B299-BE11451AF90C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesInvoiceItemState LastSalesInvoiceItemState { get; set; }

        #region Allors
        [Id("AC0B80C8-84C6-4A2D-8CE1-B94994537998")]
        [AssociationId("81BF99E7-5831-42BE-B7D8-64FB11D3C626")]
        [RoleId("06151951-E93B-44B6-8152-84FBAB29057C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesInvoiceItemState SalesInvoiceItemState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("61008480-5266-42B1-BD09-477C514F5FC5")]
        [AssociationId("EC5E9E80-DFED-4EFB-9923-CC066FA6975A")]
        [RoleId("8FCEDD24-9BF7-4CC9-8387-C661BEE650C5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesInvoiceItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("3409316D-FD54-408C-BC1C-9468CCE4B72E")]
        [AssociationId("DC9E9D4F-27AC-4022-ACB4-F4916BF010BF")]
        [RoleId("C646543C-2FF4-4DA5-99D6-EACAE7A28FDA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesInvoiceItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("D3D47236-8B21-420B-883A-C035EB0DBAE0")]
        [AssociationId("64108AA6-8478-49D1-ACB2-34CCB7F790DB")]
        [RoleId("490E7A59-C0CA-47FF-881B-5D2F4474BD5F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("44103C15-D438-433A-B157-3ACAA4544D29")]
        [AssociationId("FE3A79EF-9EC6-4929-B740-771C23806F93")]
        [RoleId("1E576337-FFB0-4B9F-96F6-81DFBD19FB64")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Part Part { get; set; }

        #region Allors
        [Id("6df95cf4-115f-4f43-aaea-52313c47d824")]
        [AssociationId("93ba1265-4050-41c1-aaf8-d09786889245")]
        [RoleId("0abd9811-a8ac-42bf-9113-4f9760cfe9eb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SerialisedItem SerialisedItem { get; set; }

        #region Allors
        [Id("f3516e3b-4bb6-45a2-b7e8-089f20d35648")]
        [AssociationId("639a53df-23b2-4629-ae6b-e1ed3716ff60")]
        [RoleId("05977b32-3111-4023-b059-95b15ca6648e")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public SerialisedItemVersion SerialisedItemVersionBeforeSale { get; set; }

        #region Allors
        [Id("d1931e89-bbe7-4c90-8d22-564c4a8604d0")]
        [AssociationId("35dbb954-be71-466c-b2d0-bbccfb4563eb")]
        [RoleId("ca519df2-7172-4a50-ba2e-514d4b5cae1f")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItemAvailability NextSerialisedItemAvailability { get; set; }

        #region Allors
        [Id("0854aece-6ca1-4b8d-99a9-6d424de8dfd4")]
        [AssociationId("cebb5430-809a-4d46-bc7b-563ee72f0848")]
        [RoleId("f1f68b89-b95f-43c9-82d5-cb9eec635869")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public ProductFeature[] ProductFeatures { get; set; }

        #region Allors
        [Id("6dd4e8ee-48ed-400d-a129-99a3a651586a")]
        [AssociationId("f99e5e01-943c-4de9-862c-c472d2d873f2")]
        [RoleId("6cb182c2-b481-4e26-869e-609990ea68b3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public InvoiceItemType InvoiceItemType { get; set; }

        #region Allors
        [Id("BB115D9A-53F8-4A3C-95F0-403A883C84FE")]
        [AssociationId("563F27E6-37AD-486A-9F90-85751C6458EE")]
        [RoleId("DDB0F028-B9D6-4D8D-88D4-245ADA2B90EB")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Facility Facility { get; set; }

        #region Allors
        [Id("5e268182-f7f2-44aa-bb7c-7da8702bf5d2")]
        [AssociationId("d2fb57e2-0d2d-4bdc-bea1-7ddc78b2d4da")]
        [RoleId("577af065-dc14-4c1c-8ba5-32f795b72589")]
        #endregion
        [Required]
        [Workspace]
        public decimal CostOfGoodsSold { get; set; }

        #region Allors

        [Id("5EFBB240-3B6B-47C4-8696-C7063ACBE074")]

        #endregion
        public void IsSubTotalItem()
        {
        }

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

        public void DelegateAccess() { }

        #endregion
    }
}
