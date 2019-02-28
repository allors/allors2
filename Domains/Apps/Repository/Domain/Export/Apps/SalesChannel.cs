namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("db1678af-6541-4a35-a3b9-cffd0f821bd2")]
    #endregion
    public partial class SalesChannel : Enumeration 
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

        #endregion

        #region Allors
        [Id("0A6385BD-3E85-4074-BF9E-CE53D342894A")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}