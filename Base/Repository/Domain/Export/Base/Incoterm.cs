namespace Allors.Repository
{
    using Attributes;

    #region Allors

    [Id("BAF23654-7A2D-49A6-81F2-309A61363447")]

    #endregion

    public partial class IncoTerm : SalesTerm, Object
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string TermValue { get; set; }

        public TermType TermType { get; set; }

        public string Description { get; set; }

        #endregion

        #region inherited methods


        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnInit()
        {

        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        public void Delete()
        {
        }

        #endregion
    }
}
