namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("c8077ff8-f443-44b5-93f5-15ad7f4a258d")]
    #endregion
    public partial class Qualification : Enumeration
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
        [Id("563A7449-0B8F-4E88-B42F-7739818C8999")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}
