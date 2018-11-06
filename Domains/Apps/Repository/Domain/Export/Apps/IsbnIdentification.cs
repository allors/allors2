namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("2F65A5A9-14A1-4469-87AD-F29A51B9D2EA")]
    #endregion
    public partial class IsbnIdentification : IGoodIdentification
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Identification { get; set; }

        public GoodIdentificationType GoodIdentificationType { get; set; }

        #endregion

        #region inherited methods

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Delete() { }

        #endregion
    }
}