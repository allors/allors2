namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("fb3dd618-eeb5-4ef6-87ca-abfe91dc603f")]
    #endregion
    public partial class Fee : OrderAdjustment
    {
        #region inherited properties
        public decimal Amount { get; set; }

        public VatRate VatRate { get; set; }

        public decimal Percentage { get; set; }

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

        public void Delete()
        {
        }

        #endregion

    }
}
