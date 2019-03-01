namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("EC1A374B-E1D8-4808-A767-8E7910117C08")]
    #endregion
    public partial class ProposalVersion : QuoteVersion
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string InternalComment { get; set; }
        public DateTime RequiredResponseDate { get; set; }
        public DateTime ValidFromDate { get; set; }
        public QuoteTerm[] QuoteTerms { get; set; }
        public DateTime ValidThroughDate { get; set; }
        public string Description { get; set; }
        public Party Receiver { get; set; }
        public ContactMechanism FullfillContactMechanism { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public DateTime IssueDate { get; set; }
        public QuoteItem[] QuoteItems { get; set; }
        public string QuoteNumber { get; set; }
        public QuoteState QuoteState { get; set; }
        public Request Request { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

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

        public void OnPreFinalize(){} public void OnFinalize()
        {
            
        }

        public void OnPostFinalize()
        {
            
        }

        #endregion
    }
}