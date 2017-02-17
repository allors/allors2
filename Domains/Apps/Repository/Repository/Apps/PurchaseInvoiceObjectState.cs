namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("6c485526-bf9e-42e0-b47e-84552a72589a")]
    #endregion
    public partial class PurchaseInvoiceObjectState : ObjectState 
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