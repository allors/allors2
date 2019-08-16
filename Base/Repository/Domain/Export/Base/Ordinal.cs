namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("385a2ae6-368c-4c3f-ad34-f8d69d8ca6cd")]
    #endregion
    public partial class Ordinal : Enumeration
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

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

        #region Allors
        [Id("A997DCC7-F99F-4B43-8747-77D1D02537A1")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}
