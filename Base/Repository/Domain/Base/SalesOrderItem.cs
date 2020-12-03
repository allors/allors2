// <copyright file="SalesOrderItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("80de925c-04cc-412c-83a5-60405b0e63e6")]
    #endregion
    public partial class SalesOrderItem : OrderItem, Versioned
    {
        #region inherited properties
        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public string InternalComment { get; set; }

        public BudgetItem BudgetItem { get; set; }

        public decimal PreviousQuantity { get; set; }

        public decimal QuantityOrdered { get; set; }

        public string Description { get; set; }

        public PurchaseOrder CorrespondingPurchaseOrder { get; set; }

        public DiscountAdjustment[] DiscountAdjustments { get; set; }

        public SurchargeAdjustment[] SurchargeAdjustments { get; set; }

        public decimal TotalOrderAdjustment { get; set; }

        public QuoteItem QuoteItem { get; set; }

        public DateTime AssignedDeliveryDate { get; set; }

        public DateTime DerivedDeliveryDate { get; set; }

        public SalesTerm[] SalesTerms { get; set; }

        public string ShippingInstruction { get; set; }

        public OrderItem[] Associations { get; set; }

        public IrpfRegime DerivedIrpfRegime { get; set; }

        public IrpfRegime AssignedIrpfRegime { get; set; }

        public IrpfRate IrpfRate { get; set; }

        public decimal UnitIrpf { get; set; }

        public decimal TotalIrpf { get; set; }

        public string Message { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public decimal TotalDiscountAsPercentage { get; set; }

        public decimal UnitVat { get; set; }

        public VatRegime DerivedVatRegime { get; set; }

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

        public decimal GrandTotal{ get; set; }

        public Order SyncedOrder { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public Guid DerivationTrigger { get; set; }

        #endregion

        #region ObjectStates
        #region SalesOrderItemState
        #region Allors
        [Id("552F50AC-5ACD-4F8F-A6CD-68E0C3426F3B")]
        [AssociationId("3A01771D-E007-4D5B-B9D6-8426BF7FA9FA")]
        [RoleId("FF4AAFCC-26A2-4ADB-9FFC-AA96A76F5F55")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemState PreviousSalesOrderItemState { get; set; }

        #region Allors
        [Id("FEBA7CC1-E449-4542-8159-71840DDE093B")]
        [AssociationId("BAF4EDA9-82B0-4591-8546-746CA111AF35")]
        [RoleId("5FD567A2-0220-471B-8726-6457AD93DB6D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemState LastSalesOrderItemState { get; set; }

        #region Allors
        [Id("0A44ABC2-41C2-444C-A20D-D2B5E387A611")]
        [AssociationId("0CD34735-D410-4B5C-A3CE-3F58FCBE59B0")]
        [RoleId("2B4DDB7B-6B88-4781-8D04-8BBD530E1B23")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderItemState SalesOrderItemState { get; set; }
        #endregion

        #region SalesOrderItemPaymentState
        #region Allors
        [Id("C4C0E26C-C324-4391-B660-9DE9545C41DF")]
        [AssociationId("B3570EAB-9F6D-43FC-A172-30F9012D5BD3")]
        [RoleId("5EF86E54-5FD8-44F5-B91C-BE10A03867D3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemPaymentState PreviousSalesOrderItemPaymentState { get; set; }

        #region Allors
        [Id("5B59E71C-C568-4433-A398-0EAD573FAF92")]
        [AssociationId("4C0185B0-624B-41CB-A072-7847C3F92FD8")]
        [RoleId("159907BE-6D0F-436B-843E-0C5C20D39621")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemPaymentState LastSalesOrderItemPaymentState { get; set; }

        #region Allors
        [Id("4DB38F8C-0903-4E05-9B4F-28EF7C3D9C01")]
        [AssociationId("5E11EFB6-A6C3-4F76-A507-D939D50F59F2")]
        [RoleId("101FF303-80CB-4DF6-BF23-B6FBC7B99BB1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public SalesOrderItemPaymentState SalesOrderItemPaymentState { get; set; }
        #endregion

        #region SalesOrderItemInvoiceState

        #region Allors
        [Id("53D4E265-78FA-47E0-A452-03B55F4B1620")]
        [AssociationId("6F1F2ABD-FAC0-4EB7-B6DA-5E082964763D")]
        [RoleId("FDFF17E5-ADF1-41B3-9679-5FF626E1FC03")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemInvoiceState PreviousSalesOrderItemInvoiceState { get; set; }

        #region Allors
        [Id("8F1D474D-3807-4591-8016-21796AF89184")]
        [AssociationId("86294D4D-A7D6-4395-8D10-EFCD74614078")]
        [RoleId("9F34E256-D162-4C07-BE67-7D864342C99C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemInvoiceState LastSalesOrderItemInvoiceState { get; set; }

        #region Allors
        [Id("219A398F-DDE3-4A50-A76D-34F7A1C086F7")]
        [AssociationId("C27E26FC-ACBD-45AF-ACBE-3842F8C880DE")]
        [RoleId("9EAD8A3B-1B77-4026-914E-E9A88486EBA5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public SalesOrderItemInvoiceState SalesOrderItemInvoiceState { get; set; }
        #endregion

        #region SalesOrderItemShipmentState
        #region Allors
        [Id("16139169-9E1C-4B3A-9635-660CA07F3190")]
        [AssociationId("437E24B6-4EA8-46DF-90CE-25B70F27E9FE")]
        [RoleId("DD01B461-220A-4AA1-AE8A-240B568BEF97")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemShipmentState PreviousSalesOrderItemShipmentState { get; set; }

        #region Allors
        [Id("5F81EA61-A2C2-4E88-99C1-FE7DE0EBAB43")]
        [AssociationId("69C7EF09-49FD-40AC-93CF-AB7EB36B3C32")]
        [RoleId("60CB1A99-F414-4B2C-B737-392CF3662FCB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemShipmentState LastSalesOrderItemShipmentState { get; set; }

        #region Allors
        [Id("6F794926-3A3F-4701-ACA6-3D622BADAED6")]
        [AssociationId("12B40C80-F9D5-4134-B8C4-63A9CBFF86D6")]
        [RoleId("38913CF7-1102-43A5-AC6A-E9F523F27A6E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public SalesOrderItemShipmentState SalesOrderItemShipmentState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("6672E7FD-B5B7-41D6-8AFF-799045EBFC26")]
        [AssociationId("F70356B6-2437-4579-BA36-C92C12F77FDA")]
        [RoleId("EF171C82-7A4B-4F58-801F-78C778B35F92")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("97271467-852D-44E9-8D6E-6C5CC8EAABF0")]
        [AssociationId("4C380563-E4F0-4B9B-A7EC-5C332E6E9D43")]
        [RoleId("80FE4F88-FFC0-4795-BCBF-F7E5A9ED0258")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("C1F15DA3-9609-48C0-B319-C0A36418379B")]
        [AssociationId("C0834C2E-02CC-4ADF-8A2B-B90F2A15BC0A")]
        [RoleId("64205300-30DE-4401-A71A-AD775ABEF3F9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Synced]
        [Workspace]
        public SalesOrderItemInventoryAssignment[] SalesOrderItemInventoryAssignments { get; set; }

        #region Allors
        [Id("1ea02a2c-280a-4a48-9ffb-1517789c56f1")]
        [AssociationId("851f33e4-6c43-468d-ab0d-0f5f83bdb179")]
        [RoleId("213d2b36-dbfd-4e2d-a854-82ba271f0d94")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public OrderItem[] OrderedWithFeatures { get; set; }

        #region Allors
        [Id("C0E36C78-95CD-4842-AFAA-137882E65214")]
        [AssociationId("51C60757-8EAE-4628-BFEE-1EA18C2117BC")]
        [RoleId("928D1562-EB3C-4C18-BF0F-F225B986821D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public SerialisedInventoryItem ReservedFromSerialisedInventoryItem { get; set; }

        #region Allors
        [Id("d7c25b48-d82f-4250-b09d-1e935eab665b")]
        [AssociationId("67e9a9d9-74ff-4b04-9aa1-dd08c5348a3e")]
        [RoleId("4bfc1720-a2f6-4204-974b-42ca42c0d2e1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public NonSerialisedInventoryItem ReservedFromNonSerialisedInventoryItem { get; set; }

        #region Allors
        [Id("3e798309-d5d5-4860-87ec-ba3766e96c9e")]
        [AssociationId("4626b586-07e1-468c-877a-d1a8f1b196c5")]
        [RoleId("b2aef5ac-45f7-41aa-8e1b-f2d79d3d9fad")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public NonSerialisedInventoryItem PreviousReservedFromNonSerialisedInventoryItem { get; set; }

        #region Allors
        [Id("8D14514A-190F-4B9F-ABE4-3E65C4337BB1")]
        [AssociationId("61E581D8-D0F8-4269-9DAD-213D97D9D127")]
        [RoleId("4F7017EB-08A4-4663-9A39-2F0754A6AEB5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItemAvailability NextSerialisedItemAvailability { get; set; }

        #region Allors
        [Id("B9E742C3-F497-4663-9874-EB49DCB45BC0")]
        [AssociationId("1112D307-756E-4E1D-9386-EE77506437DF")]
        [RoleId("64B4748B-FF63-404B-A2A1-B02A74984770")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipFromAddress { get; set; }

        #region Allors
        [Id("c7eab44d-eef8-4f8f-9a93-50be1f5a3e3b")]
        [AssociationId("7d92a696-d940-4913-b020-a216b4e7689c")]
        [RoleId("766962f0-c4a5-455c-8f5e-c74b73c0794f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        public PostalAddress DerivedShipFromAddress { get; set; }

        #region Allors
        [Id("7ae1b939-b387-4e6e-9da2-bc0364e04f7b")]
        [AssociationId("808f88ba-3866-4785-812c-c062c5f268a4")]
        [RoleId("64639736-a7d0-47cb-8afb-fa751a19670d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipToAddress { get; set; }

        #region Allors
        [Id("5cc50f26-361b-46d7-a8e6-a9f53f7d2722")]
        [AssociationId("0d8906e9-3bfd-4d9b-8b24-8526fdfb2e33")]
        [RoleId("000b641f-00be-4b9c-84aa-a8c968024ece")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public PostalAddress DerivedShipToAddress { get; set; }

        #region Allors
        [Id("6826e05e-eb9a-4dc4-a653-0230dec934a9")]
        [AssociationId("aa2b8b0a-672c-423b-9ca8-2fd40f8d1306")]
        [RoleId("793f4946-ed12-49ca-9764-8df534941cca")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Product PreviousProduct { get; set; }

        #region Allors
        [Id("b2d2645e-0d3f-473e-b277-6f890b9b911e")]
        [AssociationId("68281397-74f8-4356-b9fc-014f792ab914")]
        [RoleId("1292e876-1c61-42cb-8f01-8b3eb6cf0fa0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party AssignedShipToParty { get; set; }

        #region Allors
        [Id("7a8255f5-4283-4803-9f96-60a9adc2743b")]
        [AssociationId("2c9b2182-7b93-46c9-86ac-d13add6d52b5")]
        [RoleId("7596f471-e54c-4491-8af6-02f0e8d7d015")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public Party DerivedShipToParty { get; set; }

        #region Allors
        [Id("28104b69-ef65-47f7-96fe-e800c8803384")]
        [AssociationId("3df7e05c-e1ad-494e-ae7c-ac825bb17ff0")]
        [RoleId("75ae96d9-3c2b-4cfe-85ca-81d497d2b124")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal CostOfGoodsSold { get; set; }

        #region Allors
        [Id("545eb094-63d8-4d25-a069-7c3e91f26eb7")]
        [AssociationId("686d5956-c2dc-46d5-b812-52020d392f0f")]
        [RoleId("3a8adaf6-82e6-45a6-bd5f-61860125d77b")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityShipped { get; set; }

        #region Allors
        [Id("8145fbd3-a30f-44a0-9520-6b72ac20a82d")]
        [AssociationId("59383e9d-690e-46aa-9cc0-1dd39db14f60")]
        [RoleId("31087f2f-10e8-4558-9e0a-a5dbceb3204a")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityReturned { get; set; }

        #region Allors
        [Id("85d11891-5ffe-488f-9f23-5b2c7bc1c480")]
        [AssociationId("283cdb9a-e7e3-4486-92da-5e94653505a8")]
        [RoleId("fd06dd18-c1d4-40c7-b62e-273a8522f580")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityReserved { get; set; }

        #region Allors
        [Id("1e1ed439-ae25-4446-83e6-295d8627a7b5")]
        [AssociationId("67bc37d9-0d6f-4227-81c9-8f03a1e0da47")]
        [RoleId("d8ab230a-92d2-44cb-8e45-502285dd9a5e")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityShortFalled { get; set; }

        #region Allors
        [Id("b2f7dabb-8b87-41bc-8903-166d77bba1c5")]
        [AssociationId("ad7dfb12-d00d-4a93-a011-7cb09c1e9aa9")]
        [RoleId("ba9a9c6c-4df0-4488-b5fa-6181e45c6f18")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityPendingShipment { get; set; }

        #region Allors
        [Id("DA03C8C6-84F1-44B1-8007-2011D092D2C2")]
        [AssociationId("E5A80C53-7545-4492-BD6E-3AA47650D271")]
        [RoleId("DE5B3C7B-383F-40D3-ABAC-D7661D257D9D")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityCommittedOut { get; set; }

        #region Allors
        [Id("e8980105-2c4d-41de-bd67-802a8c0720f1")]
        [AssociationId("8b747457-bf7a-4274-b245-d04607b2a5ba")]
        [RoleId("90d69cb4-d485-418f-9608-44063f116ff4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("E65F951A-2719-4010-A622-D781E26BFAD8")]
        [AssociationId("F7D7136F-A5A8-42DE-ACA5-37870FADAEF3")]
        [RoleId("075E6936-BC32-4EAA-A74D-CCC21A2C166A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SerialisedItem SerialisedItem { get; set; }

        #region Allors
        [Id("ed586b2f-c687-4d97-9416-52f7156b7b11")]
        [AssociationId("cb5c31c4-2daa-405b-8dc9-5ea6c87f66b3")]
        [RoleId("c5b07ead-1a71-407e-91f8-4ec39853888a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("f148e660-1e09-4e76-97fb-de62a7ee7482")]
        [AssociationId("0105885d-f722-44bd-9f57-6231c38191b5")]
        [RoleId("9132a260-1b35-4b5a-b14c-8dceb6383581")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityRequestsShipping { get; set; }

        #region Allors
        [Id("9400A92D-DCA3-4F60-BC5C-05D4F32C337D")]
        [AssociationId("02F3103F-D254-40B1-9159-6254F9F8A01C")]
        [RoleId("0164D6CA-F07C-4C6E-9D50-EFE4E7D97464")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InvoiceItemType InvoiceItemType { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Cancel() { }

        public void Reject() { }

        public void Approve() { }

        public void Delete() { }

        public void Reopen() { }

        public void DelegateAccess() { }

        #endregion
    }
}
