namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("123bfcba-0548-4637-8dfc-267d6c0ac262")]
    #endregion
    public partial class Skill : Enumeration 
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
        [Id("020C1A12-4608-4D65-B671-2FDB1D695D68")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}