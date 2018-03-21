namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("273e69b7-6cda-44d4-b1d6-605b32a6a70d")]
    #endregion
    public partial class Model : ProductFeature, Enumeration 
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

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

        #region Allors
        [Id("DB2BF427-B18E-4DDD-96B0-A777FDC6D321")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}