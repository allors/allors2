namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("b5d151c7-0b18-4280-80d1-77b46162dba8")]
    #endregion
    public partial class OperatingBudget : Budget 
    {
        #region inherited properties
        public string Description { get; set; }

        public BudgetRevision[] BudgetRevisions { get; set; }

        public BudgetStatus[] BudgetStatuses { get; set; }

        public string BudgetNumber { get; set; }

        public BudgetObjectState CurrentObjectState { get; set; }

        public BudgetReview[] BudgetReviews { get; set; }

        public BudgetStatus CurrentBudgetStatus { get; set; }

        public BudgetItem[] BudgetItems { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public string Comment { get; set; }

        public Guid UniqueId { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Close(){}

        public void Reopen(){}





        #endregion

    }
}