namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("1a4166b3-9d9c-427b-a0d8-da53b0e601a2")]
    #endregion
    public partial class PersonalTitle : Enumeration 
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
        [Id("B762F1DF-4D01-4D41-9593-A732CEC61E89")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}