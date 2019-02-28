namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a73aa458-2293-4578-be67-ad32e36a4991")]
    #endregion
    public partial class FinancialTerm : AgreementTerm 
    {
        #region inherited properties
        public string TermValue { get; set; }

        public TermType TermType { get; set; }

        public string Description { get; set; }

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
        public void Delete() {}

        #endregion

    }
}