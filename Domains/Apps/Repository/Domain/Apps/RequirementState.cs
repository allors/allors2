namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("b1ee7191-544e-4cee-bbb1-d64364eb7137")]
    #endregion
    public partial class RequirementState : ObjectState 
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