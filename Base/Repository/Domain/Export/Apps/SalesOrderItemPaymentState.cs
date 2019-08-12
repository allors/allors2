namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("94F8CC92-8937-4AC9-9787-BD00CBCCC458")]
    #endregion
    public partial class SalesOrderItemPaymentState : ObjectState 
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