namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("555882ea-d25a-4da2-a8ea-330469c8cd41")]
    #endregion
    public partial class SkillLevel : Enumeration 
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


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

        #region Allors
        [Id("28AF3DA4-6323-4570-BBBE-D8C8F62C3EF4")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}