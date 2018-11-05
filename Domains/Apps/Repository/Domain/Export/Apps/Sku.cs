namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("D7FC7378-8A6D-44F6-ABE2-3DAD0F2FBE6D")]
    #endregion
    public partial class Sku : IGoodIdentification
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