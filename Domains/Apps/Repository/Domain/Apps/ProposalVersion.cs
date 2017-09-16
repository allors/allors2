namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("DEFCB0EE-EF38-4CC5-AF51-F963599BE5DF")]
    #endregion
    public partial class ProposalVersion : IProposal
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public string InternalComment { get; set; }
        public DateTime RequiredResponseDate { get; set; }
        public DateTime ValidFromDate { get; set; }
        public QuoteTerm[] QuoteTerms { get; set; }
        public Party Issuer { get; set; }
        public DateTime ValidThroughDate { get; set; }
        public string Description { get; set; }
        public Party Receiver { get; set; }
        public ContactMechanism FullfillContactMechanism { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public DateTime IssueDate { get; set; }
        public QuoteItem[] QuoteItems { get; set; }
        public string QuoteNumber { get; set; }
        public QuoteObjectState CurrentObjectState { get; set; }
        public Request Request { get; set; }
       #endregion

        #region Allors
        [Id("9C853652-D7FB-418C-9AB6-8B4BCDE46544")]
        [AssociationId("27FB1556-F4E8-473B-8B9B-C2F09C3838D3")]
        [RoleId("AD77E4A8-A590-4920-8B3E-8F983477288E")]
        #endregion
        [Workspace]
        public DateTime TimeStamp { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}