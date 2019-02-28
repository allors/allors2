namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("BB280499-092B-4929-BF9A-D6A55419B8C0")]
    #endregion
    public partial class InvoiceTerm : AccessControlledObject, SalesTerm
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
        public void Delete() { }

        #endregion
    }
}