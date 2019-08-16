namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("9b6bf786-1c6c-4c4e-b940-7314d9c4ba71")]
    #endregion
    public partial class BudgetRevision : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("5124634a-dc8b-477a-8ae2-d4ad577a13bb")]
        [AssociationId("fa00944b-f6a3-4c61-9739-6a8a109d32d5")]
        [RoleId("a1230395-837b-4021-8075-642fdf1d7d2c")]
        #endregion
        [Required]

        public DateTime RevisionDate { get; set; }

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
