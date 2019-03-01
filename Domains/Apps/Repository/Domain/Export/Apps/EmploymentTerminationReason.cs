namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f7b039f4-10f4-4939-8059-5f190fd13b09")]
    #endregion
    public partial class EmploymentTerminationReason : Enumeration 
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
        [Id("F1EF8CA3-BDC8-44C7-8B20-C4CEED63AB37")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}