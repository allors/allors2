namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("A1982F12-70E7-4B6B-BA58-1507414BBDB2")]
    #endregion
    public partial class RequestState : ObjectState 
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