namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("41D19E80-8ABB-4515-AA44-3E0AF1146AE7")]
    #endregion
    public partial class NonSerialisedInventoryItemObjectState : ObjectState 
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