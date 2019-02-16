namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("ac18c87b-683c-4529-9171-d23e73c583d4")]
    #endregion
    public partial class WorkEffortAssignmentRate : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("738EFE42-075D-46B6-974C-CD57FFAC7401")]
        [AssociationId("F43638F0-B550-456C-8FA3-1A1C6EC55A09")]
        [RoleId("728ECA9A-D473-4454-A85E-0B0C452E489B")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Rate { get; set; }

        #region Allors
        [Id("74A2B45F-4873-434F-900F-D1663B170172")]
        [AssociationId("DA2A0123-D5CC-49DE-9FA5-166710F1F1AC")]
        [RoleId("C4A299E5-8BF8-4155-8BB8-B3C11AABF017")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public RateType RateType { get; set; }

        #region Allors
        [Id("A140FB3E-76E8-411E-835A-FE9EB8E84F19")]
        [AssociationId("325BC434-6E19-46B1-A854-52D313E357FA")]
        [RoleId("A47F37E0-15F7-439B-8FAF-423A270D9927")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Cost { get; set; }

        #region Allors
        [Id("627da684-d501-4221-97c2-81329e2b5e8c")]
        [AssociationId("4b9c1fd3-acf0-4e5b-8cb5-d32f94bff10b")]
        [RoleId("e6409680-f8e1-4c61-8bd3-b9ec42435741")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public WorkEffortPartyAssignment WorkEffortPartyAssignment { get; set; }

        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}
        #endregion

    }
}