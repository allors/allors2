namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("084829bc-d347-489a-9557-9ff1ac7fb5a0")]
    #endregion
    public partial class GlBudgetAllocation : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("b09babba-1379-44fe-9e5f-89ec75c65a9c")]
        [AssociationId("9531b256-5424-4c34-9b3c-4348fb1e1672")]
        [RoleId("8908be29-0ab1-446a-b4e0-e251fcb546d2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public GeneralLedgerAccount GeneralLedgerAccount { get; set; }
        #region Allors
        [Id("dddccd24-864c-48bb-b1ac-35b8a201cd65")]
        [AssociationId("babbcdc0-7d4b-4679-a937-cbf6f5632c8b")]
        [RoleId("2ee2162b-6936-4322-8d5b-1175d29f1308")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public BudgetItem BudgetItem { get; set; }
        #region Allors
        [Id("eb1e7e03-8b88-4a69-b1cc-46dc77b44a8b")]
        [AssociationId("cd06b83b-3b19-4b6d-bed8-90c5ece3c600")]
        [RoleId("fb5e417d-b1b1-4e23-90d4-01b0464e3a1b")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal AllocationPercentage { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}