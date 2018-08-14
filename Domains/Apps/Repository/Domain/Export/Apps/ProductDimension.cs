namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("C3A647C2-1073-4D8B-99EB-AE5293AADB6B")]
    #endregion
    public partial class ProductDimension : ProductFeature, Deletable
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
        [Id("8E02D67A-1033-4F19-B67B-6B58A000494C")]
        [AssociationId("9AE4BC79-233F-415F-B5F1-54F5D0706669")]
        [RoleId("B2AB0723-9859-48A9-AF00-81EC231D507C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public Dimension2 Dimension2{ get; set; }

        #region Allors
        [Id("21E9C310-4C2F-4B51-8D8B-24FFAA00FE0F")]
        [AssociationId("8518E6A0-6D68-45F1-996F-785AE2605061")]
        [RoleId("2B58FD01-1307-40DA-9FCF-812E337F8765")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Size(-1)]
        [Workspace]
        public string Value { get; set; }

        #region inherited methods

        public void OnBuild()
        {
            throw new NotImplementedException();
        }

        public void OnPostBuild()
        {
            throw new NotImplementedException();
        }

        public void OnPreDerive()
        {
            throw new NotImplementedException();
        }

        public void OnDerive()
        {
            throw new NotImplementedException();
        }

        public void OnPostDerive()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}