namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("3b330b42-b359-4de7-a084-cc96ce1e6420")]
    #endregion
    public partial class DebitCreditConstant : UniquelyIdentifiable, Enumeration
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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

    }
}
