namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("c0e14757-9825-4a86-95d9-b87c68efcb9c")]
    #endregion
    public partial class OrganisationUnit : Enumeration 
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

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void OnPreFinalize(){} public void OnFinalize()
        {
            
        }

        public void OnPostFinalize()
        {
            
        }

        #endregion

        #region Allors
        [Id("CFC86F33-F8CB-4611-9B97-378FC9C407E2")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}