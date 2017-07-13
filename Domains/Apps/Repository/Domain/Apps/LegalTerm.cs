namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("14a2576c-3ea7-4016-aba2-44172fb7a952")]
    #endregion
    public partial class LegalTerm : AgreementTerm 
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

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}