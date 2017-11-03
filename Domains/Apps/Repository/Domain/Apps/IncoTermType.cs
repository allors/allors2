namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("5B218A7B-2C46-40D9-8D23-C045904AC083")]
    #endregion
    public partial class IncoTermType : TermType 
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }


        #endregion
    }
}