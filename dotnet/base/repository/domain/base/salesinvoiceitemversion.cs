// <copyright file="SalesInvoiceItemVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("F838CABD-3769-47D8-8623-66D2723B5D1B")]
    #endregion
    public partial class SalesInvoiceItemVersion : InvoiceItemVersion
    {
        #region inherited properties

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

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

        public decimal GrandTotal { get; set; }

        public decimal TotalSurchargeAsPercentage { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalSurcharge { get; set; }

        public VatRegime AssignedVatRegime { get; set; }

        public decimal TotalBasePrice { get; set; }

        public decimal TotalExVat { get; set; }

        public IrpfRegime DerivedIrpfRegime { get; set; }

        public IrpfRegime AssignedIrpfRegime { get; set; }

        public IrpfRate IrpfRate { get; set; }

        public decimal UnitIrpf { get; set; }

        public decimal TotalIrpf { get; set; }

        #endregion

        #region Allors
        [Id("D5986C17-F56D-4402-BBFA-FE8389E89AEA")]
        [AssociationId("BFC9E9F5-B3A0-48EB-B7EA-4F13E730FEC9")]
        [RoleId("F181E01C-9B88-491C-9B78-50AC5BC7A777")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SalesInvoiceItemState SalesInvoiceItemState { get; set; }

        #region Allors
        [Id("30D936D7-5C26-4E70-B880-B7E125B931E7")]
        [AssociationId("23F6AF1C-2B3A-46EC-9C5F-9421D33DA99A")]
        [RoleId("936534D3-CBB3-464C-838B-764D241008E9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public ProductFeature[] ProductFeatures { get; set; }

        #region Allors
        [Id("D223A126-F816-4628-842D-469CBEBB9CB2")]
        [AssociationId("1CC67F90-460E-4FA9-A6ED-081F04F0D4DC")]
        [RoleId("3A1A4A50-9A57-47AD-B152-5FE0F40E945B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("E8E885EC-5F87-469A-9C5D-1A7F369D4D23")]
        [AssociationId("DC6E6C8E-6D6E-43FB-A228-34F221513A52")]
        [RoleId("6B5EB401-EF03-4824-BA5F-CDB3C162DE6E")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SerialisedItem SerialisedItem { get; set; }

        #region Allors
        [Id("ac302862-d876-43c0-a938-8058a6d3c2e8")]
        [AssociationId("388abc03-79d9-4014-aa95-ab356a0e3429")]
        [RoleId("49b1a593-2a10-43d5-ad25-f7b4b9b8930e")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItemAvailability NextSerialisedItemAvailability { get; set; }

        #region Allors
        [Id("7A952F88-BC4E-4F23-A0D5-44D47E30666E")]
        [AssociationId("9D5B95BE-20AA-4D1A-81A2-6FD69B1A6365")]
        [RoleId("26EC36C3-42D1-4941-90D3-2E1B45132E1B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
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

        #endregion
    }
}
