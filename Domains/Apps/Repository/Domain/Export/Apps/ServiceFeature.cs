namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("fdbea721-61f8-4e75-b1dd-e3880636ee78")]
    #endregion
    public partial class ServiceFeature : Enumeration, ProductFeature 
    {
        #region inherited properties
        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public EstimatedProductCost[] EstimatedProductCosts { get; set; }

        public PriceComponent[] BasePrices { get; set; }

        public string Description { get; set; }

        public ProductFeature[] DependentFeatures { get; set; }

        public ProductFeature[] IncompatibleFeatures { get; set; }

        public VatRate VatRate { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

        #region Allors
        [Id("DC64EB61-DF8C-4D79-B7E9-D56C91D17A8C")]
        #endregion
        [Workspace]
        public void Delete(){}
    }
}