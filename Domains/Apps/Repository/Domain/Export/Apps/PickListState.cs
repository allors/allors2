namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f7108ec0-3203-4e62-b323-2e3a6a527d66")]
    #endregion
    public partial class PickListState : ObjectState 
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