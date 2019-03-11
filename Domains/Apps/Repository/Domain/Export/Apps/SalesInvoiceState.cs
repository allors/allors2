namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a4092f59-2baf-4041-83e6-5d50c8338a5c")]
    #endregion
    public partial class SalesInvoiceState : ObjectState 
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