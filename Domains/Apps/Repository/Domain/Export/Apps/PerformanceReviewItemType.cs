namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("e80a9fe3-027b-4abd-acfb-99e3db9da70c")]
    #endregion
    public partial class PerformanceReviewItemType : Enumeration 
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

    }
}