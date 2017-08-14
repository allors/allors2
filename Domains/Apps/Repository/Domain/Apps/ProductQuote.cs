namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("c2214ff4-d592-4f0d-9215-e431b23dc9c2")]
    #endregion
    [Plural("ProductQuotes")]

    public partial class ProductQuote : Quote 
    {
        #region inherited properties

        public DateTime RequiredResponseDate { get; set; }

        public DateTime ValidFromDate { get; set; }

        public QuoteTerm[] QuoteTerms { get; set; }

        public Party Issuer { get; set; }

        public DateTime ValidThroughDate { get; set; }

        public string Description { get; set; }
        public string InternalComment { get; set; }

        public Party Receiver { get; set; }

        public ContactMechanism FullfillContactMechanism { get; set; }

        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        public DateTime IssueDate { get; set; }

        public QuoteItem[] QuoteItems { get; set; }

        public string QuoteNumber { get; set; }

        public QuoteStatus[] QuoteStatuses { get; set; }

        public QuoteObjectState CurrentObjectState { get; set; }

        public QuoteStatus CurrentQuoteStatus { get; set; }

        public Request Request { get; set; }

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

        public string Comment { get; set; }

        #endregion

        #region Allors
        [Id("8D92571B-AABE-45EC-A2BB-93219B3E8C12")]
        #endregion
        [Workspace]
        public void Order() { }

        #region inherited methods

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Approve() { }

        public void Reject() { }

        #endregion
    }
}