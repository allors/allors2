namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("db24e487-daf7-4625-9073-8fd083f653dc")]
    #endregion
    public partial class CustomerRequirement : Requirement 
    {
        #region inherited properties
        public DateTime RequiredByDate { get; set; }

        public Party Authorizer { get; set; }

        public RequirementStatus[] RequirementStatuses { get; set; }

        public string Reason { get; set; }

        public Requirement[] Children { get; set; }

        public Party NeededFor { get; set; }

        public Party Originator { get; set; }

        public RequirementObjectState CurrentObjectState { get; set; }

        public RequirementStatus CurrentRequirementStatus { get; set; }

        public Facility Facility { get; set; }

        public Party ServicedBy { get; set; }

        public decimal EstimatedBudget { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Reopen(){}

        public void Cancel(){}

        public void Hold(){}

        public void Close(){}



        #endregion

    }
}