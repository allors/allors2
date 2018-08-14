namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("273e69b7-6cda-44d4-b1d6-605b32a6a70d")]
    #endregion
    public partial class Model : ProductFeature
    {
        #region inherited properties

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public EstimatedProductCost[] EstimatedProductCosts { get; set; }

        public PriceComponent[] BasePrices { get; set; }

        public string Description { get; set; }

        public ProductFeature[] DependentFeatures { get; set; }

        public ProductFeature[] IncompatibleFeatures { get; set; }

        public VatRate VatRate { get; set; }

        #endregion

        #region Allors
        [Id("EAA58A96-AEC8-4D67-92E4-A0A61651F84D")]
        [AssociationId("920C2CFA-EE8A-4EE0-B189-6654B10232AD")]
        [RoleId("9BA11A0C-1F22-4783-B709-E033A78FAA8F")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}