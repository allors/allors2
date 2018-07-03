namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("78022da7-d11c-4ab7-96f5-099d6608c4bb")]
    #endregion
    public partial class CustomEngagementItem : EngagementItem 
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
        [Id("71a3ed63-922f-44ae-8e89-6425759b3eb3")]
        [AssociationId("00621849-ee7b-4a7e-b5c3-7ca2e2d40b3a")]
        [RoleId("2b2d9ceb-cce9-4edd-bbaa-2829b3e5e32f")]
        #endregion
        [Size(-1)]

        public string DescriptionOfWork { get; set; }
        #region Allors
        [Id("f0b91526-924e-4f11-b27c-187010e1dff7")]
        [AssociationId("21f41aa4-9417-4822-afba-6e424dd936f2")]
        [RoleId("9f787d7c-663d-4856-a3cb-8d65b4802744")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal AgreedUponPrice { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}