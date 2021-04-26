// <copyright file="PurchaseOrderVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("5B8C17C9-17BF-4B80-9246-AF7125EAE976")]
    #endregion
    public partial class PurchaseOrderVersion : OrderVersion
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string InternalComment { get; set; }

        public Currency AssignedCurrency { get; set; }

        public string CustomerReference { get; set; }

        public OrderAdjustment[] OrderAdjustments { get; set; }

        public decimal TotalExVat { get; set; }

        public SalesTerm[] SalesTerms { get; set; }

        public decimal TotalVat { get; set; }

        public decimal TotalSurcharge { get; set; }

        public OrderItem[] ValidOrderItems { get; set; }

        public string OrderNumber { get; set; }

        public decimal TotalDiscount { get; set; }

        public string Message { get; set; }

        public DateTime EntryDate { get; set; }

        public OrderKind OrderKind { get; set; }

        public decimal TotalIncVat { get; set; }

        public decimal GrandTotal { get; set; }

        public VatRegime AssignedVatRegime { get; set; }

        public VatRegime DerivedVatRegime { get; set; }

        public decimal TotalShippingAndHandling { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public decimal TotalBasePrice { get; set; }

        public decimal TotalFee { get; set; }

        public decimal TotalExtraCharge { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public IrpfRegime AssignedIrpfRegime { get; set; }

        public IrpfRegime DerivedIrpfRegime { get; set; }

        public decimal TotalIrpf { get; set; }

        public decimal TotalIrpfInPreferredCurrency { get; set; }

        public decimal TotalExVatInPreferredCurrency { get; set; }

        public decimal TotalVatInPreferredCurrency { get; set; }

        public decimal TotalIncVatInPreferredCurrency { get; set; }

        public decimal GrandTotalInPreferredCurrency { get; set; }

        public decimal TotalSurchargeInPreferredCurrency { get; set; }

        public decimal TotalDiscountInPreferredCurrency { get; set; }

        public decimal TotalShippingAndHandlingInPreferredCurrency { get; set; }

        public decimal TotalFeeInPreferredCurrency { get; set; }

        public decimal TotalExtraChargeInPreferredCurrency { get; set; }

        public decimal TotalBasePriceInPreferredCurrency { get; set; }

        public decimal TotalListPriceInPreferredCurrency { get; set; }
        #endregion

        #region Allors
        [Id("A6D9BF95-9717-4336-A261-4773FBD93CA8")]
        [AssociationId("3184D552-1468-4F10-B734-336BEABA1D39")]
        [RoleId("E7F10255-1913-4361-8F2A-4C1AAA176CB4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public PurchaseOrderState PurchaseOrderState { get; set; }

        #region Allors
        [Id("C14D2C09-056E-4D36-B0CB-5F5471C28CA1")]
        [AssociationId("223428A0-6A20-4D3F-A3AA-8490E325330A")]
        [RoleId("FEC45864-A3A4-41CD-B8BB-F603AE6F3732")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public PurchaseOrderShipmentState PurchaseOrderShipmentState { get; set; }

        #region Allors
        [Id("EABEB07B-871B-4FCC-A38C-51CBAEBD4F35")]
        [AssociationId("925F905A-CFE4-4272-A4E0-31BD3BE3D2E9")]
        [RoleId("9EE86C5B-43E5-45E1-9453-BF3FD548771C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public PurchaseOrderPaymentState PurchaseOrderPaymentState { get; set; }

        #region Allors
        [Id("CAE880EC-B266-4CB2-9FD4-2A0F8B0ACBF8")]
        [AssociationId("A1AB9BBA-921A-4CA7-B1A0-A3500BBF769C")]
        [RoleId("5E33CAE0-4522-4A44-8085-3353E0BABB21")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        public PurchaseOrderItem[] PurchaseOrderItems { get; set; }

        #region Allors
        [Id("774CEA12-501D-4C7A-885B-A198079CF74E")]
        [AssociationId("498F1CC3-1097-4216-BB18-DD2892CDEF15")]
        [RoleId("6E35415E-33B4-4914-A14A-AF16E641412E")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public Party TakenViaSupplier { get; set; }

        #region Allors
        [Id("34274ec5-7e51-431c-bbe5-91a5131fe85c")]
        [AssociationId("294a45f0-4a4b-4c7d-b206-99128999dc08")]
        [RoleId("96dd8ebf-66e4-4bd9-b52c-d9224fc9e9ff")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public Party TakenViaSubcontractor { get; set; }

        #region Allors
        [Id("8AC728F2-F766-47C4-93B7-15B5D5DC2FF6")]
        [AssociationId("7824A2B4-F57A-4D5A-942D-EBAB64D674C1")]
        [RoleId("7A484CAE-17FB-46D0-A49E-44A5EA970FAB")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public ContactMechanism AssignedTakenViaContactMechanism { get; set; }

        #region Allors
        [Id("b0b71810-c486-4c8d-9f6a-568ff181e0ab")]
        [AssociationId("486c60e6-cf5b-4e17-b145-73d257cf27d5")]
        [RoleId("ff024cb7-8022-4883-9080-8d8986a81962")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism DerivedTakenViaContactMechanism { get; set; }

        #region Allors
        [Id("C7B99EF4-7DE8-4214-A598-E6E46608E166")]
        [AssociationId("3789439A-992A-4947-8EE0-A4E40440F113")]
        [RoleId("B55B7BCC-386E-47A2-B328-17B6F6A3EF21")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public Person TakenViaContactPerson { get; set; }

        #region Allors
        [Id("A368FB1C-8467-40E9-BC33-47BA5AEA9A0B")]
        [AssociationId("155B4626-22C8-4FE8-AC24-E75F91ECF52E")]
        [RoleId("90426B16-51F2-4D93-AEF2-4A109348831A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism AssignedBillToContactMechanism { get; set; }

        #region Allors
        [Id("0115fdfc-3192-4b34-aeae-7c5fb392d57a")]
        [AssociationId("256da529-c66c-4a1c-91bc-e3cffd28f2cf")]
        [RoleId("75a54087-b529-4ed0-8ef6-973c8acea604")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism DerivedBillToContactMechanism { get; set; }

        #region Allors
        [Id("555F06E3-2C07-4A62-A4D5-E52E64A92362")]
        [AssociationId("DF0499B7-1BAA-42FB-BECD-F023D8EC43BA")]
        [RoleId("D431167C-CA76-42BF-A11F-6D8E79EEFDDC")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public Person BillToContactPerson { get; set; }

        #region Allors
        [Id("69DDEF12-B6AA-4040-991D-CF1D20A0D5EC")]
        [AssociationId("10D5F3CB-64ED-4010-92FD-13539CAD3F78")]
        [RoleId("D2C6D6DB-F38B-49E4-9AC5-187CD41D3863")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public Facility StoredInFacility { get; set; }

        #region Allors
        [Id("57C5DCE6-ACA0-4D03-89B2-4D7CC3AE6E45")]
        [AssociationId("8DBE214D-F4B6-445A-838A-F04B02839F46")]
        [RoleId("41AAB6C9-FFA9-4AA3-8867-27101B8D3D08")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public PostalAddress AssignedShipToAddress { get; set; }

        #region Allors
        [Id("ab0c504f-7706-48fb-a77c-c777f6cef1b3")]
        [AssociationId("a04c6d4b-a4fa-4fbc-b682-0ee303fa53ff")]
        [RoleId("6e46521e-114a-4f56-86c4-3d381c83ba8d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress DerivedShipToAddress { get; set; }

        #region Allors
        [Id("88691341-493F-4F23-8329-32AC6FC7682E")]
        [AssociationId("C689156A-7A51-4838-A790-360DB50F7C69")]
        [RoleId("3862640B-4ADE-4C52-9FF4-E108FC889736")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public Person ShipToContactPerson { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
