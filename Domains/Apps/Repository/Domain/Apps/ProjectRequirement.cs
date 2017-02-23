namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("51d0b6f6-221b-44d5-9a0b-9a880620b1ad")]
    #endregion
    public partial class ProjectRequirement : Requirement 
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

        #region Allors
        [Id("75d89129-9aa9-491c-894b-feb86b33bf52")]
        [AssociationId("e83f19ff-1441-4d6e-912f-ca56301e3621")]
        [RoleId("80b74f53-e962-4988-a1c1-2860a08ca6b3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        public Deliverable[] NeededDeliverables { get; set; }


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