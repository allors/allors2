namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("59f3100c-da48-4b4c-a302-1a75e37216a6")]
    #endregion
    public partial class OrganisationGlAccount : AccessControlledObject, Period 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("8e20ce74-a772-45c8-a76a-a8ca0d4d7ebd")]
        [AssociationId("948a2115-8780-46c6-83cf-dd4d27a1771b")]
        [RoleId("a3d68e22-e492-4d1e-8386-ea45ad67ee3a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("9337f791-56aa-4086-b661-2043cf02c662")]
        [AssociationId("59fb9b8f-4d0a-4f97-b4d6-b3a5708de269")]
        [RoleId("f264e9da-aa7f-4d81-aa1b-741f020c2bef")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public OrganisationGlAccount Parent { get; set; }

        #region Allors
        [Id("9af20c76-200c-4aed-8154-99fd88907a15")]
        [AssociationId("7d9f9cad-0685-4b7d-b12d-770f046465f3")]
        [RoleId("817a7322-724b-4239-8261-a9c683f1ea4a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party Party { get; set; }

        #region Allors
        [Id("a1608d47-9fa7-4dc4-9736-c59f28221842")]
        [AssociationId("61d6a380-171a-41c2-bda9-6cd8638ba442")]
        [RoleId("de2ad2c4-bf0e-4092-9611-b23c3e613429")]
        #endregion
        [Derived]
        [Required]
        [Workspace]
        public bool HasBankStatementTransactions { get; set; }

        #region Allors
        [Id("c0de2fbb-9e70-4094-8279-fb46734e920e")]
        [AssociationId("92c29de0-8454-4ae1-8bf9-ed4c5ec0d313")]
        [RoleId("8b65fb70-4905-4c1b-a1b1-51470ce58599")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductCategory ProductCategory { get; set; }

        #region Allors
        [Id("f1d3e642-2844-4c5a-a053-4dcfce461902")]
        [AssociationId("b0706892-9a04-4e5a-8caa-bd015f3d81f9")]
        [RoleId("6cb69b76-2852-43eb-bff6-a10bc44503a3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public GeneralLedgerAccount GeneralLedgerAccount { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}