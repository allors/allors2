namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("9FEE8DC0-E418-4E10-AD5A-2E2A33AFC0E9")]
    #endregion
    public partial class SalesOrderShipmentState : ObjectState 
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