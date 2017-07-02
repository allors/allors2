namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("498BDEC2-FD25-40C1-B0E7-CE393E2F12D9")]
    #endregion
    public partial class RequestItemObjectState : ObjectState 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}