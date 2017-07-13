namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("f15e6b0e-0222-4f9b-8ae2-20c20f3b3673")]
    #endregion
    public partial class ExpenseEntry : ServiceEntry 
    {
        #region inherited properties
        public DateTime ThroughDateTime { get; set; }

        public EngagementItem EngagementItem { get; set; }

        public bool IsBillable { get; set; }

        public DateTime FromDateTime { get; set; }

        public string Description { get; set; }

        public WorkEffort WorkEffort { get; set; }

        public string Comment { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("0bb04781-d5b4-455c-8880-b5bfbc9d69f8")]
        [AssociationId("cc956cd1-4910-4977-afc5-e76f8bb2dc16")]
        [RoleId("821a410a-afa4-4e6e-b505-3732a864554a")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}