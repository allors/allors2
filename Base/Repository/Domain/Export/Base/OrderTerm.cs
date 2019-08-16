namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("86cf6a28-baeb-479d-ac9e-fabc7fe1994d")]
    #endregion
    public partial class OrderTerm : SalesTerm, Object
    {
        #region inherited properties

        public string TermValue { get; set; }
        public TermType TermType { get; set; }
        public string Description { get; set; }
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}