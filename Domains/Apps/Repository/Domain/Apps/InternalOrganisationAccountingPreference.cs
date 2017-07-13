namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("20f97398-6614-44ec-8e33-3a0b3f113e11")]
    #endregion
    public partial class InternalOrganisationAccountingPreference : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("0ac44c21-6a2c-4162-9d77-fe1b16b60b73")]
        [AssociationId("4d61b711-7aab-4162-bb31-74db09f666fe")]
        [RoleId("0da9c561-a6fc-4fea-aee3-5c24a2b08aea")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public GeneralLedgerAccount GeneralLedgerAccount { get; set; }
        #region Allors
        [Id("7279a067-a219-478a-8573-4a212448328b")]
        [AssociationId("c86e149d-dba6-4928-ac01-66f74cb7f102")]
        [RoleId("0dd6f18e-4ec3-491e-b6d0-fbfb7fceb37d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public InventoryItemKind InventoryItemKind { get; set; }
        #region Allors
        [Id("bd24005a-4391-417b-83f3-da7fa0324cf1")]
        [AssociationId("588aff14-7523-47be-bf96-5a81630754a7")]
        [RoleId("f4799bf2-32bc-4f3e-b337-dfca87a58b21")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public PaymentMethod PaymentMethod { get; set; }
        #region Allors
        [Id("bdd72700-8be8-4db4-8d1c-3fa3fdb8548f")]
        [AssociationId("d27bfc28-e617-438c-a8d6-c36ce7cd22b6")]
        [RoleId("e9b60088-c318-4755-a3ac-d25737e0a21b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Receipt Receipt { get; set; }
        #region Allors
        [Id("bf3732f2-2c6e-4931-9f35-ab46b8b26e63")]
        [AssociationId("28945e7e-6cce-43e5-afb6-13c224a3bf34")]
        [RoleId("f94c3b98-53ec-4622-96f3-d8d7d8baa383")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]

        public InternalOrganisation InternalOrganisation { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}