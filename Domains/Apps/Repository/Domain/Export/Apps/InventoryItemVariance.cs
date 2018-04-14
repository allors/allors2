namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("b00e2650-283f-4326-bdd3-46a2890e2037")]
    #endregion
    public partial class InventoryItemVariance : AccessControlledObject, Commentable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        #endregion

        #region Allors
        [Id("57bdf1d7-84b8-4c7c-a470-396f6facd3bd")]
        [AssociationId("6f8706cd-f005-4ab1-8deb-db5d00b72403")]
        [RoleId("1a45c449-2b0d-4f64-be40-0858018b9cf6")]
        #endregion
        [Required]
        [Workspace]
        public int Quantity { get; set; }

        #region Allors
        [Id("58ead8d2-c9c3-4092-b5d1-79af4811f43c")]
        [AssociationId("82f2636f-738d-45b8-bdc0-5136ad8d8382")]
        [RoleId("eee9f92b-5649-467f-8a99-4318c24cc002")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        public ItemVarianceAccountingTransaction ItemVarianceAccountingTransaction { get; set; }

        #region Allors
        [Id("af9fa5bc-a392-473d-b077-7f06ee24390b")]
        [AssociationId("9a0f9ecd-9954-4c2f-bb0e-e94f9cc3c19a")]
        [RoleId("5665d533-cd9c-4328-b422-66a94d77b19b")]
        #endregion
        [Workspace]
        public DateTime InventoryDate { get; set; }

        #region Allors
        [Id("e422efc4-4d17-46d8-bba4-6e78e7761f93")]
        [AssociationId("468307f7-5033-4e77-9482-5df34ca9a4f1")]
        [RoleId("7e0b8650-0d19-4ecc-b6e6-3c78dfe8c2aa")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public VarianceReason Reason { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}