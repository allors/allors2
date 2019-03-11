namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("2A8FF5AC-F2F2-44F5-918B-365AA2BFD9F2")]
    #endregion
    public partial class SerialisedInventoryItemObjectState : ObjectState 
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