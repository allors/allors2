namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("cd60cf6d-65ba-4e31-b85d-16c19fc0978b")]
    #endregion
    public partial class QuoteTerm : AccessControlledObject, AgreementTerm 
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

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}
        public void Delete() { }

        #endregion
    }
}