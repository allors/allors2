namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("3b233161-d2a8-4d8f-a293-09d8a2bea3e2")]
    #endregion
    public partial class VatReturnBoxType : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("95935a8e-fac5-4798-ba2d-1408d231f97b")]
        [AssociationId("d40e1048-b97b-4bae-9319-f4c05ec40484")]
        [RoleId("44678a1f-9af2-404f-8eec-b50fb62737cb")]
        #endregion
        [Size(256)]

        public string Type { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}