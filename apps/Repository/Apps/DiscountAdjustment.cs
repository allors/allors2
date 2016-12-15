namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("0346a1e2-03c7-4f1e-94ae-35fdf64143a9")]
    #endregion
    public partial class DiscountAdjustment : OrderAdjustment 
    {
        #region inherited properties
        public decimal Amount { get; set; }

        public VatRate VatRate { get; set; }

        public decimal Percentage { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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