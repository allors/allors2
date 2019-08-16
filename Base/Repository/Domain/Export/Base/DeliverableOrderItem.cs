namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("66bd584c-37c4-4969-874b-7a459195fd25")]
    #endregion
    public partial class DeliverableOrderItem : EngagementItem
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
        [Id("f9e13dab-0081-4d25-8021-f5ed5bef5f0e")]
        [AssociationId("86376834-b792-425e-a21d-30065dca6dd4")]
        [RoleId("fb6ba6e4-2f9f-4230-b536-df8e305797f9")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal AgreedUponPrice { get; set; }

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        public void DelegateAccess() { }

        #endregion
    }
}
