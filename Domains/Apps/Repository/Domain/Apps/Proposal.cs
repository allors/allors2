namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("360cf15d-c360-4d68-b693-7d1544388169")]
    #endregion
    public partial class Proposal : Quote 
    {
        #region inherited properties
        public DateTime ValidFromDate { get; set; }

        public QuoteTerm[] QuoteTerms { get; set; }

        public Party Issuer { get; set; }

        public DateTime ValidThroughDate { get; set; }

        public string Description { get; set; }

        public Party Receiver { get; set; }

        public decimal Amount { get; set; }

        public DateTime IssueDate { get; set; }

        public QuoteItem[] QuoteItems { get; set; }

        public string QuoteNumber { get; set; }

        public QuoteStatus[] QuotesStatuses { get; set; }

        public QuoteObjectState CurrentObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public Guid UniqueId { get; set; }

        public string PrintContent { get; set; }

        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}