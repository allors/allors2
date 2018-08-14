namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("0a7ac589-946b-4d49-b7e0-7e0b9bc90111")]
    #endregion
    public partial class Brand : ProductFeature
    {
        #region inherited properties
        public Guid UniqueId { get; set; }
        public EstimatedProductCost[] EstimatedProductCosts { get; set; }
        public PriceComponent[] BasePrices { get; set; }
        public string Description { get; set; }
        public ProductFeature[] DependentFeatures { get; set; }
        public ProductFeature[] IncompatibleFeatures { get; set; }
        public VatRate VatRate { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("08b0dfc6-2e2f-4e40-96d1-851e26b38e8d")]
        [AssociationId("8b22eb1a-ecc3-4e0d-898c-4a5c651f1d2c")]
        [RoleId("2f85b937-569a-4317-ac4a-aa1e89541a20")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}