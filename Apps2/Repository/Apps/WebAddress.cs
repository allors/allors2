namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("5138c0e3-1b28-4297-bf45-697624ee5c19")]
    #endregion
    public partial class WebAddress : ElectronicAddress 
    {
        #region inherited properties
        public string ElectronicAddressString { get; set; }

        public string Description { get; set; }

        public ContactMechanism[] FollowTo { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        public void Delete(){}
        #endregion

    }
}