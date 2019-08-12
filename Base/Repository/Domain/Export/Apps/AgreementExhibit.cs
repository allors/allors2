namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("2830c388-b002-44d6-91b6-b2b43fa778f3")]
    #endregion
    public partial class AgreementExhibit : AgreementItem 
    {
        #region inherited properties
        public string Text { get; set; }

        public Addendum[] Addenda { get; set; }

        public AgreementItem[] Children { get; set; }

        public string Description { get; set; }

        public AgreementTerm[] AgreementTerms { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}
        public void DelegateAccess() { }

        #endregion
    }
}