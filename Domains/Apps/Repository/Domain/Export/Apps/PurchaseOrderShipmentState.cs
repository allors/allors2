namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("B7F6899B-2CA6-4A49-B9BA-B7AD3D9077F1")]
    #endregion
    public partial class PurchaseOrderShipmentState : ObjectState 
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