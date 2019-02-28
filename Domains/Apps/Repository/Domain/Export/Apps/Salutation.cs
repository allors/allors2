namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("91d1ad08-2eae-4d9e-8a2e-223eeae138af")]
    #endregion
    public partial class Salutation : AccessControlledObject, Enumeration, UniquelyIdentifiable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Guid UniqueId { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

        #region Allors
        [Id("758DB9B1-1064-42D7-9853-2BEC3CAA9427")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}