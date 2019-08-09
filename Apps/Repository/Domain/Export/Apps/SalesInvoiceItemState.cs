namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("4babdd0c-52dd-4fb8-bbf5-120aa58eff50")]
    #endregion
    public partial class SalesInvoiceItemState : ObjectState 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        public Permission[] ObjectDeniedPermissions { get; set; }

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