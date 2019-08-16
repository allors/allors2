namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("9ec7e136-815c-4726-9991-e95a3ec9e092")]
    #endregion
    public partial class Two : Object, Shared
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("8930c13c-ad5a-4b0e-b3bf-d7cdf6f5b867")]
        [AssociationId("fd97db6d-d946-47ba-a2a0-88b732457b96")]
        [RoleId("39eda296-4e8d-492b-b0c1-756ffcf9a493")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public Shared Shared { get; set; }

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
