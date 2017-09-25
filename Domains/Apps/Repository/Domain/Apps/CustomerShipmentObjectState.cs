namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f2d5bb8b-b50f-45e5-accb-c752a4445ad2")]
    #endregion
    public partial class CustomerShipmentObjectState : ObjectState 
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