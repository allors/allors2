namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("3b73eea7-6455-4fe5-87c0-99c852f57e6b")]
    #endregion
    public partial class VatCalculationMethod : Enumeration 
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

    }
}