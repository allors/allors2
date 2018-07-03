namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("0eaa8719-bbf4-408a-8226-851580556024")]
    #endregion
    public partial class Training : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("7d2e7956-fb60-4a1b-8e5f-ee88b1b8e3b7")]
        [AssociationId("ff4c2753-ce42-4aa8-b1b1-3486e6aa11d9")]
        [RoleId("ee47ec51-a1d0-4d12-97cc-5a089869caa6")]
        #endregion
        [Required]
        [Size(-1)]

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