namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("8f838542-cc98-4e95-8e60-fb3e774ba92e")]
    #endregion
    public partial class WorkEffortPurpose : Enumeration
    {
        #region inherited properties
        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

        #region Allors
        [Id("113A0EB5-FDE8-42B2-A4D6-4416AF749386")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}
