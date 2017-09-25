namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("671951f1-78fd-4b05-ac15-eafb2a35a6f8")]
    #endregion
    public partial class CustomerReturnObjectState : ObjectState 
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