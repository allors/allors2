namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("26981f3f-f683-4a59-91ad-7a0e4243aea6")]
    #endregion
    public partial class Dimension : ProductFeature 
    {
        #region inherited properties
        public EstimatedProductCost[] EstimatedProductCosts { get; set; }

        public PriceComponent[] BasePrices { get; set; }

        public string Description { get; set; }

        public ProductFeature[] DependentFeatures { get; set; }

        public ProductFeature[] IncompatibleFeatures { get; set; }

        public VatRate VatRate { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("6901f550-4470-4acf-8234-96c1b1bd0bc6")]
        [AssociationId("094356ad-e8d6-4f6b-b1c6-910a3d9fc518")]
        [RoleId("1863b99e-415e-42a0-acef-613f7b3e3315")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Unit { get; set; }
        #region Allors
        [Id("c4fa3792-9784-43ea-91f1-1533f1d12765")]
        [AssociationId("ea393d05-73c8-4b52-b578-c02cc718f076")]
        [RoleId("fae40aa7-15ea-4b37-8d33-86df26b14b54")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public UnitOfMeasure UnitOfMeasure { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}