namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("320985b6-d571-4b6c-b940-e02c04ad37d3")]
    #endregion
    public partial class SimpleJob : Object
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("7cd27660-13c6-4a15-8fd8-5775920cfd28")]
        [AssociationId("da384d02-5d30-4df5-acb5-ca36c895ef53")]
        [RoleId("44b9e3cc-e584-48c0-bfec-916ab14e5f03")]
        #endregion
        public int Index { get; set; }

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
