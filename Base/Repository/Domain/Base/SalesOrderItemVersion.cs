// <copyright file="SalesOrderItemVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("CD97F8F9-C0E8-4E5F-8516-3F9FE6A4F0FC")]
    #endregion
    public partial class SalesOrderItemVersion : OrderItemVersion
    {
        #region inherited properties
        public string InternalComment { get; set; }

        public BudgetItem BudgetItem { get; set; }

        public decimal PreviousQuantity { get; set; }

        public decimal QuantityOrdered { get; set; }

        public string Description { get; set; }

        public PurchaseOrder CorrespondingPurchaseOrder { get; set; }

        public decimal TotalOrderAdjustment { get; set; }

        public QuoteItem QuoteItem { get; set; }

        public DateTime AssignedDeliveryDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public SalesTerm[] SalesTerms { get; set; }

        public string ShippingInstruction { get; set; }

        public OrderItem[] Associations { get; set; }

        public string Message { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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
        [Id("E9D53F26-6B36-4278-A3CE-CA9730458109")]
        [AssociationId("F114BF9A-B2D6-42B6-8EF5-0EDEE5D10BFE")]
        [RoleId("15DFF22E-5A71-417A-AA6E-1023E807E757")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public SalesOrderItemState SalesOrderItemState { get; set; }

        #region Allors
        [Id("054A3EB3-CC49-4377-B5FE-8361165F219B")]
        [AssociationId("991BA75B-46D3-428E-945C-C3F2E6132E49")]
        [RoleId("D6473D76-EB93-4A24-844E-B6B251CD26C5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderItemShipmentState SalesOrderItemShipmentState { get; set; }

        #region Allors
        [Id("5B1EB657-791B-43F6-9106-2484B863082A")]
        [AssociationId("4D1D98A9-29F0-40FE-BB6B-3927369E458E")]
        [RoleId("9D779356-F067-4F4D-9D13-2D07A5074B76")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderItemInvoiceState SalesOrderItemInvoiceState { get; set; }

        #region Allors
        [Id("0BA7F287-96FD-488C-9AED-493F90574CA5")]
        [AssociationId("7386B85E-5A55-45D0-8736-9C4FC14BCE2B")]
        [RoleId("1D28C40E-1883-431A-8C8C-27D040B7B7D2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderItemPaymentState SalesOrderItemPaymentState { get; set; }

        #region Allors
        [Id("519B172B-C966-411A-852F-7486667975CB")]
        [AssociationId("2C14F1C1-A058-447B-83FD-340C9E607F61")]
        [RoleId("6BBD6102-58FF-403C-8E6E-9C93D65543BA")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityShortFalled { get; set; }

        #region Allors
        [Id("AB660021-A95B-47D7-9627-D83DC4A053A4")]
        [AssociationId("840CB200-BD0B-4678-9FD3-61FC23287CE2")]
        [RoleId("998F8A4C-DFBA-4777-9EDE-A4F6F73C0392")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public OrderItem[] OrderedWithFeatures { get; set; }

        #region Allors
        [Id("A442312C-71F5-4C96-ABF7-7C303A6E17F4")]
        [AssociationId("C046A01B-2478-4242-AADD-C1B955BC42DE")]
        [RoleId("82F2571B-D270-4028-A3B7-B8841158F561")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedInventoryItem ReservedFromSerialisedInventoryItem { get; set; }

        #region Allors
        [Id("219FE50D-5CEE-4266-93F7-496668668A00")]
        [AssociationId("EA882CE2-E738-4B52-962B-767BAED08B5B")]
        [RoleId("587BD4A0-805F-4D0B-9FB5-1C236E52C87D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public NonSerialisedInventoryItem ReservedFromNonSerialisedInventoryItem { get; set; }

        #region Allors
        [Id("C90C49BE-C67A-4B66-B110-A7CF09D8235A")]
        [AssociationId("879107B2-49BE-4252-88CC-9ACF931156B5")]
        [RoleId("A793FAAD-60E1-4069-99BD-2B24561539F1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public NonSerialisedInventoryItem PreviousReservedFromNonSerialisedInventoryItem { get; set; }

        #region Allors
        [Id("BBD58CD0-83BD-47B4-955F-95CC4A66E880")]
        [AssociationId("CF7704F9-16EE-4064-94AD-A94C53FFD6CB")]
        [RoleId("3B71B8E3-17B8-4A33-9FE3-CC151DBF1B35")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItemState NewSerialisedItemState { get; set; }

        #region Allors
        [Id("17BB3EF6-6368-436D-A7C4-5AD55889E2B7")]
        [AssociationId("2FAA1278-84B1-4391-A90D-12CA1E335F77")]
        [RoleId("821314C5-EFE3-4183-BE65-D1733AACA58C")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityShipped { get; set; }

        #region Allors
        [Id("6F3996FE-E7E2-44F0-90B5-BD6E59B108A7")]
        [AssociationId("A4BBB841-1D26-498A-9D0F-3B064A6B90CF")]
        [RoleId("8F553EB9-6755-428D-A149-87B3335E44E3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress ShipFromAddress { get; set; }

        #region Allors
        [Id("38F38BF8-B735-4578-A2B1-2FD997A1FE3C")]
        [AssociationId("F615FF3B-23BD-4743-9557-0C355DDA3A18")]
        [RoleId("10709F20-3D42-4A28-9EA6-B1F8F7D9C0B8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("2BE18BDC-27ED-4E1B-8F7C-58CBF8E58ED3")]
        [AssociationId("51EBB5B8-A0DD-4C6A-B3BD-68FAA2330EDF")]
        [RoleId("532D622D-6C55-410C-A4FC-A38160BD7486")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Product PreviousProduct { get; set; }

        #region Allors
        [Id("5589BC3C-DD00-429A-92F5-7981228964DE")]
        [AssociationId("AD18BDBB-01FB-43F3-9914-8D6EF2E01C27")]
        [RoleId("6BD40C20-5E05-43FC-9FA7-00CA92EB7D87")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public Party ShipToParty { get; set; }

        #region Allors
        [Id("0FF4C90C-EBD7-491D-B265-EF5083E4BB95")]
        [AssociationId("17921EF9-05DB-4123-94C5-8876ABE28E29")]
        [RoleId("81D226AB-C9C5-4D9E-AD53-C9F277C4E912")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipToAddress { get; set; }

        #region Allors
        [Id("7A3ED514-0136-45BB-835B-05AA446052B7")]
        [AssociationId("5D6D995F-43B4-4C17-B3F6-9FAB61943300")]
        [RoleId("F0035777-2F1E-4CB9-BC86-04A275C5E94D")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityReturned { get; set; }

        #region Allors
        [Id("4C235BA5-958D-4EA0-B08E-DD98000C2433")]
        [AssociationId("EB4F77C6-FC65-4EB3-84C6-A23CE62AB149")]
        [RoleId("D4D7703C-106D-4F46-AFFF-DDA39C7404D7")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityReserved { get; set; }

        #region Allors
        [Id("1837DB18-F0D5-4A84-88B9-09EF35D98A24")]
        [AssociationId("40EDCBE4-D311-4AFA-8907-44758B146FD0")]
        [RoleId("037CD53E-5814-47DE-85B4-62973723ADB2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party AssignedShipToParty { get; set; }

        #region Allors
        [Id("72C8B608-F504-4A0B-BBFF-144DED827625")]
        [AssociationId("07AD0AC2-616A-409F-A911-953831036EFD")]
        [RoleId("E45DF0A4-4038-4696-9A42-07B29E10825A")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityPendingShipment { get; set; }

        #region Allors
        [Id("F79B3A47-46AE-4098-A4C4-21D098621F52")]
        [AssociationId("CC858C83-8DE5-4ED8-B151-65E2E1D81DB0")]
        [RoleId("E4F3B2B9-162E-4F55-B455-FE55480C943B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("303E6727-E0E8-4D7E-B3F1-FF751B6BF285")]
        [AssociationId("43499012-2C82-40BF-8034-40DA7A4ABD70")]
        [RoleId("2FCF4BEB-9962-4B51-9076-394C23BCF39F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("ED8039CE-8212-43FA-8FD8-9CBD90970E49")]
        [AssociationId("FF18CFE8-63CF-4060-880B-88EA4B3952D2")]
        [RoleId("947395C2-CC69-4650-837B-3B12E771A6A2")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityRequestsShipping { get; set; }

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
