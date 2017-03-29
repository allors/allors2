namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("B257CA11-1CE5-49D1-AC1B-B16D8882682F")]
    #endregion
    public partial class ProposalObjectState : ObjectState 
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