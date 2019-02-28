namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("64DBA13D-BA9B-45C3-8D7D-00DA8CBA8815")]
    #endregion
    public partial class ManufacturerIdentification : ProductIdentification
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Identification { get; set; }

        public ProductIdentificationType ProductIdentificationType { get; set; }

        #endregion

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

        public void Delete() { }

        #endregion
    }
}