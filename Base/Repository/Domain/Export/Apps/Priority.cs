namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("caa4814f-85a2-46a8-97a7-82220f8270cb")]
    #endregion
    public partial class Priority : Enumeration 
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
        [Id("37C3DBE8-BB3F-47FC-ABAE-038EE9E725BE")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}