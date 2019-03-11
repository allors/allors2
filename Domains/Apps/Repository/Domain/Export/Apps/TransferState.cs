namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("9f3d9ae6-cbbf-4cfb-900d-bc66edccbf95")]
    #endregion
    public partial class TransferState : ObjectState 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion


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