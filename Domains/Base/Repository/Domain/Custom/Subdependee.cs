namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("46a437d1-455b-4ddd-b83c-068938c352bd")]
    #endregion
    public partial class Subdependee : Object 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("194930f9-9c3f-458d-93ec-3d7bea4cd538")]
        [AssociationId("63ed21ba-b310-43fc-afed-a3eeea918204")]
        [RoleId("6765f2b5-bf55-4713-a693-946fc0846b27")]
        #endregion
        public int Subcounter { get; set; }

        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            throw new System.NotImplementedException();
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}
        #endregion
    }
}