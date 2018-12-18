namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a18de27f-54fe-4160-b149-475bebeaf716")]
    #endregion
    public partial class SurchargeComponent : PriceComponent 
    {
        #region inherited properties

        public Party PricedBy { get; set; }

        public GeographicBoundary GeographicBoundary { get; set; }

        public decimal Rate { get; set; }

        public PartyClassification PartyClassification { get; set; }

        public OrderQuantityBreak OrderQuantityBreak { get; set; }

        public PackageQuantityBreak PackageQuantityBreak { get; set; }

        public Product Product { get; set; }

        public Part Part { get; set; }

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

        #region Allors
        [Id("0e9d10cd-6905-42ca-9db3-aed9b123eb2a")]
        [AssociationId("79b7473b-d65d-469b-9061-bb344da42c7e")]
        [RoleId("f5d669b0-89d3-4605-ae6e-dcee6c673c50")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Percentage { get; set; }

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