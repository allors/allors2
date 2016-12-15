namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("9a1d67c5-159c-41e0-9b5c-5ffdfe257b8d")]
    #endregion
    public partial class Shelf : Container 
    {
        #region inherited properties
        public Facility Facility { get; set; }

        public string ContainerDescription { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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