// <copyright file="PurchaseInvoiceItemVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("E431E1B1-9CF0-4BA2-AF57-E2EF8B8AE711")]
    #endregion
    public partial class PurchaseInvoiceItemVersion : InvoiceItemVersion
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string InternalComment { get; set; }

        public SalesTerm[] SalesTerms { get; set; }

        public decimal TotalInvoiceAdjustment { get; set; }

        public InvoiceVatRateItem[] InvoiceVatRateItems { get; set; }

        public InvoiceItem AdjustmentFor { get; set; }

        public string Message { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal Quantity { get; set; }

        public string Description { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        public decimal TotalDiscountAsPercentage { get; set; }

        public DiscountAdjustment DiscountAdjustment { get; set; }

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

        public SurchargeAdjustment SurchargeAdjustment { get; set; }

        #endregion

        #region Allors
        [Id("F9AA8529-25F3-4374-9394-7B1A4868490A")]
        [AssociationId("186771F2-0662-46C2-B7DB-0D80439A3AF4")]
        [RoleId("60032850-5640-4B02-AE10-F063EF38CA4F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public PurchaseInvoiceItemState PurchaseInvoiceItemState { get; set; }

        #region Allors
        [Id("C534F912-F733-46A8-B54B-A4001DBB76FE")]
        [AssociationId("23068D37-8FE7-4D3D-A296-08EAB0F9A846")]
        [RoleId("A8F636FA-FDEA-484F-BC6C-10B784355660")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public InvoiceItemType InvoiceItemType { get; set; }

        #region Allors
        [Id("D88804CF-E48A-4C7F-8916-0FC182BF43CA")]
        [AssociationId("F1CC5B6E-5382-4FAB-83B9-D623DFC415AF")]
        [RoleId("593BA640-C917-469C-AC57-9A64218AE04F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderItem PurchaseOrderItem { get; set; }

        #region Allors
        [Id("BBE7B96D-9B68-46E6-9158-743797C6AA43")]
        [AssociationId("0C49AEAE-A22A-4F26-A7E7-294C8ABE2F40")]
        [RoleId("861B5605-BD49-434F-9136-D16523FFEE43")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Part Part { get; set; }

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
