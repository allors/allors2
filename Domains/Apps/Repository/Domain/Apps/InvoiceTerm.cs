namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("a917f763-e54a-4693-bf7b-d8e7aead8fe6")]
    #endregion
    public partial class InvoiceTerm : AccessControlledObject, AgreementTerm 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string TermValue { get; set; }

        public TermType TermType { get; set; }

        public string Description { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}