namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("592260cc-365c-4769-b067-e95dd49609f5")]
    #endregion
    public partial class Receipt : Payment 
    {
        #region inherited properties
        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime EffectiveDate { get; set; }

        public Party SendingParty { get; set; }

        public PaymentApplication[] PaymentApplications { get; set; }

        public string ReferenceNumber { get; set; }

        public Party ReceivingParty { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public Guid UniqueId { get; set; }

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