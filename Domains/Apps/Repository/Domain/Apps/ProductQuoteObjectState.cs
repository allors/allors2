namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("E4C73B17-6805-482A-93E5-8F22E8BFFD9D")]
    #endregion
    public partial class ProductQuoteObjectState : ObjectState 
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