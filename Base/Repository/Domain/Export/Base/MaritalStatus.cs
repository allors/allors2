namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("ef6ce14d-87e9-4704-be8b-595329a6bf20")]
    #endregion
    public partial class MaritalStatus : Enumeration
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
        [Id("5505C467-AD01-446D-9C0D-29D4502DEC1A")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}
