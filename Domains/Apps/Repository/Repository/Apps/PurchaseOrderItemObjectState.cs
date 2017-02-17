namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("ad76acee-eccc-42ce-9897-8c3f0252caf4")]
    #endregion
    public partial class PurchaseOrderItemObjectState : ObjectState 
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