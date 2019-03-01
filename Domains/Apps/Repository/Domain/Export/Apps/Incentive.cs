namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("150d21f7-20dd-4951-848f-f74a69dadb5b")]
    #endregion
    public partial class Incentive : AgreementTerm 
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

        public void OnPreFinalize(){} public void OnFinalize()
        {
            
        }

        public void OnPostFinalize()
        {
            
        }

        public void Delete() { }


        #endregion

    }
}