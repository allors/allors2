namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("e3f78cf6-6367-4b0f-9ac0-b887e7187c5e")]
    #endregion
    public partial class EngineeringChangeObjectState : ObjectState 
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