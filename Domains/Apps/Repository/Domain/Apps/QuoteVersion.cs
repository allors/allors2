namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("62DB5CD0-9CF4-4086-9538-7B5A3E612DA2")]
    #endregion
    public partial class QuoteVersion : QuoteVersioned
    {
        #region inherited properties

        public QuoteStatus[] QuotesStatuses { get; set; }
        public QuoteStatus CurrentQuoteStatus { get; set; }
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
        [Id("F199F8A3-EB94-482C-AFA4-84EB227E2800")]
        [AssociationId("0A61F07B-23F2-4E6E-AA33-7855A709E2ED")]
        [RoleId("2D0B586B-54F2-4E6E-B817-6DFECA5AEE64")]
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