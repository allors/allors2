namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c1b6fac9-8e69-4c07-8cec-e9b52c690e72")]
    #endregion
    public partial class GoodOrderItem : EngagementItem 
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

        #region Allors
        [Id("de65b7a6-b2b3-4d77-9cb4-94720adb43f0")]
        [AssociationId("3ed4dffc-09eb-4285-a31c-ba3af0666451")]
        [RoleId("2f1173ef-1723-4ee5-9ff3-a01b6216584a")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Price { get; set; }
        #region Allors
        [Id("f7399ebd-64f0-4bfa-a063-e75389d6a7cc")]
        [AssociationId("30b12a84-e2cc-4d24-aca3-71568961f9ee")]
        [RoleId("bf1eeede-db39-4996-a2da-b3da503c2415")]
        #endregion

        public int Quantity { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}