namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("F2C51AE4-CD1E-489C-946F-539F24744C52")]
    #endregion
    public partial class SalesTermType : TermType 
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