namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("d4cfdb68-9128-4afc-8670-192e55115499")]
    #endregion
    public partial class ManufacturerSuggestedRetailPrice : PriceComponent 
    {
        #region inherited properties

        public Party PricedBy { get; set; }

        public GeographicBoundary GeographicBoundary { get; set; }

        public decimal Rate { get; set; }

        public RevenueValueBreak RevenueValueBreak { get; set; }

        public PartyClassification PartyClassification { get; set; }

        public OrderQuantityBreak OrderQuantityBreak { get; set; }

        public PackageQuantityBreak PackageQuantityBreak { get; set; }

        public Product Product { get; set; }

        public Part Part { get; set; }

        public RevenueQuantityBreak RevenueQuantityBreak { get; set; }

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

        public LocalisedText[] LocalisedComments { get; set; }

        #endregion

        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Delete() { }
        #endregion
    }
}