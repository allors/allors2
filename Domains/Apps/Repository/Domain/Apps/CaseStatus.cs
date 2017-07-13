namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c0b015e0-57a4-4fe3-984b-12e8bda25db7")]
    #endregion
    public partial class CaseStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("28ef5fa2-7e2a-4ebb-b5a2-fe8cf7f18d04")]
        [AssociationId("9a1d40d3-0c58-4088-9a62-d7a35b787bf6")]
        [RoleId("1acf2d5a-2631-48da-8e9a-222ff1293e83")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }
        #region Allors
        [Id("332b3322-ef2e-4457-8503-045aa99061c9")]
        [AssociationId("76b6d6d5-e406-43aa-be3f-90a685a3f8dc")]
        [RoleId("8fbd70e2-fc8c-4584-ac6c-82bc432f9326")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public CaseObjectState CaseObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}