namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("1BBF6140-F74B-4503-8C9E-8C71F3F670C7")]
    #endregion
    public partial class CapitalBudgetVersion : BudgetVersion
    {
        #region inherited properties

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public BudgetState BudgetState { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public string Comment { get; set; }

        public string Description { get; set; }

        public BudgetRevision[] BudgetRevisions { get; set; }

        public string BudgetNumber { get; set; }

        public BudgetReview[] BudgetReviews { get; set; }

        public BudgetItem[] BudgetItems { get; set; }

        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}
        #endregion
    }
}