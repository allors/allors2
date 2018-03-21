namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("21f09e4c-7b3f-4152-8822-8c485011759c")]
    #endregion
    public partial class SalesOrderItemState : ObjectState 
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