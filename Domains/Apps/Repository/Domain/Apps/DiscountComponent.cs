namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c0b927c4-7197-4295-8edf-057b6b4b3a6a")]
    #endregion
    public partial class DiscountComponent : PriceComponent 
    {
        #region inherited properties
        public GeographicBoundary GeographicBoundary { get; set; }

        public decimal Rate { get; set; }

        public RevenueValueBreak RevenueValueBreak { get; set; }

        public PartyClassification PartyClassification { get; set; }

        public OrderQuantityBreak OrderQuantityBreak { get; set; }

        public PackageQuantityBreak PackageQuantityBreak { get; set; }

        public Product Product { get; set; }

        public RevenueQuantityBreak RevenueQuantityBreak { get; set; }

        public Party SpecifiedFor { get; set; }

        public ProductFeature ProductFeature { get; set; }

        public AgreementPricingProgram AgreementPricingProgram { get; set; }

        public string Description { get; set; }

        public Currency Currency { get; set; }

        public OrderKind OrderKind { get; set; }

        public OrderValue OrderValue { get; set; }

        public decimal Price { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public SalesChannel SalesChannel { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        #endregion

        #region Allors
        [Id("1101cd39-852b-4eac-8649-de1a3f080703")]
        [AssociationId("ff284a40-cfa1-4b5b-90ec-c42b4dc35ef5")]
        [RoleId("88c08616-c1e6-4c53-b1e8-74fa33bc310d")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Percentage { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion

    }
}