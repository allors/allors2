namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f7d24734-88d3-47e7-b718-8815914c9ad4")]
    #endregion
    public partial class WorkEffortState : ObjectState 
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