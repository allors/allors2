namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("6217b428-4ad0-4f7f-ad4b-e334cf0b3ab1")]
    #endregion
    public partial class From : Object
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("d9a9896d-e175-410a-9916-9261d83aa229")]
        [AssociationId("a963f593-cad0-4fa9-96a3-3853f0f7d7c6")]
        [RoleId("775a29b8-6e21-4545-9881-d52f6eb7db8b")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        public To[] Tos { get; set; }


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
