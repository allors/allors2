// <copyright file="NonUnifiedPart.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("75916246-b1b5-48ef-9578-d65980fd2623")]
    #endregion
    public partial class NonUnifiedPart : Part
    {
        #region inheritedProperties
        public string Comment { get; set; }

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

        public ProductIdentification[] ProductIdentifications { get; set; }

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

        #endregion inheritedProperties

        #region inheritedMethods
        public void OnBuild() { }

        public void OnDerive() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPostDerive() { }

        public void Delete()
        {
        }

        public void OnPreDerive() { }

        public void SetDisplayName() { }
        #endregion inheritedMethods
    }
}
