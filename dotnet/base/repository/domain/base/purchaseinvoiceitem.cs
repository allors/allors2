// <copyright file="PurchaseInvoiceItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("1ee19062-e36d-4836-b0e6-928a3957bd57")]
    #endregion
    public partial class PurchaseInvoiceItem : InvoiceItem, Versioned
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

        public Restriction[] Restrictions { get; set; }

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

        public decimal GrandTotal { get; set; }

        public User CreatedBy { get; set; }

        public IrpfRegime DerivedIrpfRegime { get; set; }

        public IrpfRegime AssignedIrpfRegime { get; set; }

        public IrpfRate IrpfRate { get; set; }

        public decimal UnitIrpf { get; set; }

        public decimal TotalIrpf { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public Guid DerivationTrigger { get; set; }


        #endregion

        #region ObjectStates
        #region PurchaseInvoiceItemState
        #region Allors
        [Id("7DDF09F9-1F6F-4863-9A6D-83172DDB20FE")]
        [AssociationId("2F16E176-27CE-4F12-8FDF-71F941E2788D")]
        [RoleId("3F37D76B-C8C9-4FB8-9C85-FA2352AF453C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseInvoiceItemState PreviousPurchaseInvoiceItemState { get; set; }

        #region Allors
        [Id("4422FA5F-8DE7-4329-B0C0-5938CBB559A8")]
        [AssociationId("5685E503-AAD5-4202-8268-ABDAA485FEB9")]
        [RoleId("ABABF851-D89D-4C36-BC6A-82FCEB779F4C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseInvoiceItemState LastPurchaseInvoiceItemState { get; set; }

        #region Allors
        [Id("C0467550-8CB5-4A65-BCBF-7A51D048529C")]
        [AssociationId("C70919C3-1317-4DC9-AC1C-A86DF3C5AC15")]
        [RoleId("194C2E3C-EE49-4F83-82A5-6BE9626FF67E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseInvoiceItemState PurchaseInvoiceItemState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("F41279D9-A0F9-44EB-857D-3C76D9CBE634")]
        [AssociationId("46FD1730-7AED-4B5F-8858-279FFE7F30CC")]
        [RoleId("5D1A28E5-0694-417C-B508-7AE40FA60BBC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseInvoiceItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("E17BF428-BA56-451B-90D9-371CDA61E0E6")]
        [AssociationId("EDCC5A53-993F-40B6-81B1-F9070E04D584")]
        [RoleId("0CC8F3F3-A774-41E2-9242-FCEBE88D93B7")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseInvoiceItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("3DC3728F-748D-454F-8D5C-0F1BD5AE3855")]
        [AssociationId("E853ECED-1EF7-40A4-BEA5-FC6A8C6CFFB3")]
        [RoleId("640663C5-0053-4339-834E-B67F1D3CE110")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public PurchaseOrderItem PurchaseOrderItem { get; set; }

        #region Allors
        [Id("56e47122-faaa-4211-806c-1c19695fe434")]
        [AssociationId("826db2b1-3048-4237-8e83-0c472a166d49")]
        [RoleId("893de8bc-93eb-4864-89ba-efdb66b32fd5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public InvoiceItemType InvoiceItemType { get; set; }

        #region Allors
        [Id("65eebcc4-d5ef-4933-8640-973b67c65127")]
        [AssociationId("40703e06-25f8-425d-aa95-3c73fafbfa81")]
        [RoleId("05f86785-08d8-4282-9734-6230e807181b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Part Part { get; set; }

        #region Allors
        [Id("2E3887BF-A895-4D25-9519-9B0ADA1DE6C4")]
        [AssociationId("D05D0646-8E5C-4853-A8D6-A857ABD91221")]
        [RoleId("4CEEFC88-A01E-462F-A52F-D9B17D0DBF96")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product ObsoleteProduct { get; set; }

        #region Allors
        [Id("B036079F-0B4A-4F7A-87A2-F3854A01F73A")]
        [AssociationId("F160C06B-8ED6-44F1-A2C4-27CEDB2041E4")]
        [RoleId("FCE3F19E-C4A3-4484-88EE-7CF3B7BE449F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SerialisedItem SerialisedItem { get; set; }

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

        #region Allors
        [Id("EE82F184-2FFD-4B9C-A0C5-556A76737591")]
        #endregion
        [Workspace]
        public void Reject() { }

        #region Allors
        [Id("f75e58d0-af6e-41d5-a606-d28ae53f63c8")]
        #endregion
        [Workspace]
        public void Revise() { }

        #region Allors
        [Id("eedc34ce-d944-435d-bc67-c57d695a6102")]
        #endregion
        [Workspace]
        public void FinishRevising() { }
    }
}
