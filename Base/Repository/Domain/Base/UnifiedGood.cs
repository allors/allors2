// <copyright file="UnifiedGood.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("76C7C629-AA80-48F7-8D66-0A4F3BB5AE38")]
    #endregion
    public partial class UnifiedGood : Good, Part
    {
        #region inherited properties
        public string Comment { get; set; }

        public string ProductNumber { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public string Description { get; set; }

        public LocalisedText[] LocalisedDescriptions { get; set; }

        public string InternalComment { get; set; }

        public Document[] Documents { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public string Keywords { get; set; }

        public LocalisedText[] LocalisedKeywords { get; set; }

        public Media PrimaryPhoto { get; set; }

        public Media[] Photos { get; set; }

        public DateTime SupportDiscontinuationDate { get; set; }

        public DateTime SalesDiscontinuationDate { get; set; }

        public PriceComponent[] VirtualProductPriceComponents { get; set; }

        public string IntrastatCode { get; set; }

        public Product ProductComplement { get; set; }

        public Product[] Variants { get; set; }

        public DateTime IntroductionDate { get; set; }

        public EstimatedProductCost[] EstimatedProductCosts { get; set; }

        public Product[] ProductObsolescences { get; set; }

        public VatRate VatRate { get; set; }

        public PriceComponent[] BasePrices { get; set; }

        public ProductIdentification[] ProductIdentifications { get; set; }

        public string BarCode { get; set; }
        public decimal ReplacementValue { get; set; }
        public int LifeTime { get; set; }
        public int DepreciationYears { get; set; }

        public Product[] ProductSubstitutions { get; set; }

        public Product[] ProductIncompatibilities { get; set; }

        public Facility DefaultFacility { get; set; }

        public PartSpecification[] PartSpecifications { get; set; }

        public Party ManufacturedBy { get; set; }

        public Party[] SuppliedBy { get; set; }

        public Brand Brand { get; set; }

        public string HsCode { get; set; }

        public Model Model { get; set; }

        public int ReorderLevel { get; set; }

        public int ReorderQuantity { get; set; }

        public InventoryItemKind InventoryItemKind { get; set; }

        public ProductType ProductType { get; set; }

        public SerialisedItem[] SerialisedItems { get; set; }

        public SerialisedItemCharacteristic[] SerialisedItemCharacteristics { get; set; }

        public decimal QuantityOnHand { get; set; }

        public decimal AvailableToPromise { get; set; }

        public decimal QuantityCommittedOut { get; set; }

        public decimal QuantityExpectedIn { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string SearchString { get; set; }

        public Media[] PublicElectronicDocuments { get; set; }

        public LocalisedMedia[] PublicLocalisedElectronicDocuments { get; set; }

        public Media[] PrivateElectronicDocuments { get; set; }

        public LocalisedMedia[] PrivateLocalisedElectronicDocuments { get; set; }

        public PartWeightedAverage PartWeightedAverage { get; set; }
        #endregion

        #region Allors
        [Id("f1509899-4ca0-43db-ba4d-a003d823ced7")]
        [AssociationId("dba4dc87-ed2b-4820-9f52-0c1523dd93ab")]
        [RoleId("bb1b74ae-9065-45c7-98ea-e5f0e89a78eb")]
        #endregion
        [Required]
        [Workspace]
        public Guid DerivationTrigger { get; set; }

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

        public void SetDisplayName() { }

        #endregion
    }
}
