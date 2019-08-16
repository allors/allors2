namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("acbe7b46-bcfe-4e8b-b8a7-7b9eeac4d6e2")]
    #endregion
    public partial class EuSalesListType : Enumeration
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
        [Id("81B7AC17-F87C-4E96-AFBF-75ADFFFF4DB9")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}