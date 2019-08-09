namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("FFFCC7BD-252E-4EE7-B825-99CCBE2D5F49")]
    #endregion
    public partial class Build : Object
    {
        #region Allors
        [Id("A3DED776-B516-4C38-9B5F-5DEBFAFD15CB")]
        [AssociationId("E23C3438-21C5-4E01-A252-033774CF8EA0")]
        [RoleId("AAE972FE-192D-4356-AA41-743EEFA32589")]
        #endregion
        public Guid Guid { get; set; }

        #region Allors
        [Id("19112701-B610-49FC-82B8-FB786EEBCDB4")]
        [AssociationId("91262C96-A305-40F4-953F-4CDC05FD59F9")]
        [RoleId("F81A23A4-AD8F-470E-9717-E3774411C6AE")]
        #endregion
        public string String { get; set; }

        #region Methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public void OnBuild()
        {

        }

        public void OnPostBuild()
        {
        }

        public void OnInit()
        {
            
        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        #endregion

    }
}