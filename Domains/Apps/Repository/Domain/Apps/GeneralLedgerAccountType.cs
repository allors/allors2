namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("ce5c78ee-f892-4ced-9b21-51d84c77127f")]
    #endregion
    public partial class GeneralLedgerAccountType : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("e01a0752-531b-4ee3-a58e-711f377247e1")]
        [AssociationId("dcfb5761-0d99-4a8f-afc9-2c0e64cd1c68")]
        [RoleId("7d579eae-a239-4f55-9719-02f39dbc42d8")]
        #endregion
        [Required]
        [Size(256)]

        public string Description { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}