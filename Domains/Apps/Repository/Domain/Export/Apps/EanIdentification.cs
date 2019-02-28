namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("7B813840-7AB3-454D-9DDC-19BD229B6751")]
    #endregion
    public partial class EanIdentification : ProductIdentification
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