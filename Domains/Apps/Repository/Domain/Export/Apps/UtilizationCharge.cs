namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("96a64894-e444-4df4-9289-1b121842ac73")]
    #endregion
    public partial class UtilizationCharge : PriceComponent 
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

        #endregion

        #region Allors
        [Id("3a371680-fc37-44dc-b3be-cdd76b6dd1e4")]
        [AssociationId("15d9f938-5a5c-472c-92a6-6769a37f652c")]
        [RoleId("a1e57ec7-561d-4c8e-8652-aea06598fb1b")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("4f933f12-1337-453c-9cfd-6babaf9189d5")]
        [AssociationId("b49286b4-db2a-4025-8fb2-9390514b69dc")]
        [RoleId("037bba17-d291-40ea-920b-f09995ef04fb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public UnitOfMeasure UnitOfMeasure { get; set; }

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