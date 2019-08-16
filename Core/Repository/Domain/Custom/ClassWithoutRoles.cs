namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("e1008840-6d7c-4d44-b2ad-1545d23f90d8")]
    #endregion
    [Plural("ClassWithourRoleses")]
    public partial class ClassWithoutRoles : Object
    {
        #region inherited properties
        #endregion


        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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
