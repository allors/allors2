namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("FE3B3C65-5CD2-4E23-956C-7BFA752172BE")]
    #endregion
    public partial class SalesOrderItemInvoiceState : ObjectState 
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