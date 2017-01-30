namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("b83205c5-261f-4d9d-9789-55966ae8d61b")]
    #endregion
    public partial class ProfessionalPlacement : EngagementItem 
    {
        #region inherited properties
        public QuoteItem QuoteItem { get; set; }

        public string Description { get; set; }

        public DateTime ExpectedStartDate { get; set; }

        public DateTime ExpectedEndDate { get; set; }

        public WorkEffort EngagementWorkFulfillment { get; set; }

        public EngagementRate[] EngagementRates { get; set; }

        public EngagementRate CurrentEngagementRate { get; set; }

        public EngagementItem[] OrderedWiths { get; set; }

        public Person CurrentAssignedProfessional { get; set; }

        public Product Product { get; set; }

        public ProductFeature ProductFeature { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}