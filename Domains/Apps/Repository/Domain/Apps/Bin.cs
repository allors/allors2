namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("b5d29ed3-b850-4607-9f49-9a920a2bffa1")]
    #endregion
    public partial class Bin : Container 
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