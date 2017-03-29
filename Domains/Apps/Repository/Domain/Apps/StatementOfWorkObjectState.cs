namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("0A3399DE-C3E2-4BE4-8E80-941DD78D25A0")]
    #endregion
    public partial class StatementOfWorkObjectState : ObjectState 
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