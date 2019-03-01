namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("129e6fe8-01d0-40ad-bc6a-e5449c19274f")]
    #endregion
    public partial class EmploymentTermination : Enumeration 
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
        [Id("7E49FA93-F363-45DC-96F8-FFA8A043ED76")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}