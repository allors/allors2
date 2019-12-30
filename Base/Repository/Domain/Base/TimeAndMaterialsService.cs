// <copyright file="TimeAndMaterialsService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("da504b46-2fd0-4500-ae23-61fa73151077")]
    #endregion
    public partial class TimeAndMaterialsService : Service
    {
        #region inherited properties

        public string InternalComment { get; set; }

        public DateTime SupportDiscontinuationDate { get; set; }

        public DateTime SalesDiscontinuationDate { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public LocalisedText[] LocalisedDescriptions { get; set; }

        public string Description { get; set; }

        public PriceComponent[] VirtualProductPriceComponents { get; set; }

        public string IntrastatCode { get; set; }

        public Product ProductComplement { get; set; }

        public Product[] Variants { get; set; }

        public ProductIdentification[] ProductIdentifications { get; set; }

        public string Name { get; set; }

        public DateTime IntroductionDate { get; set; }

        public Document[] Documents { get; set; }

        public Media[] ElectronicDocuments { get; set; }

        public LocalisedMedia[] LocalisedElectronicDocuments { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public string Keywords { get; set; }
        public LocalisedText[] LocalisedKeywords { get; set; }

        public Media PrimaryPhoto { get; set; }

        public Media[] Photos { get; set; }

        public EstimatedProductCost[] EstimatedProductCosts { get; set; }

        public Product[] ProductObsolescences { get; set; }

        public VatRate VatRate { get; set; }

        public PriceComponent[] BasePrices { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string SearchOptions { get; set; }

        #endregion

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
    }
}
