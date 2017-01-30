namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("89d2037a-4bc2-4929-b333-5358ac4a14e5")]
    #endregion
    public partial class DropShipmentObjectState : ObjectState 
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