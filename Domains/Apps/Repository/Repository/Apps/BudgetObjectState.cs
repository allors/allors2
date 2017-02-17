namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("f8ae512e-bca5-498b-860b-11c06ab04d72")]
    #endregion
    public partial class BudgetObjectState : ObjectState 
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