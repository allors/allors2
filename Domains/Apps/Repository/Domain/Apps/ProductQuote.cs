namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("c2214ff4-d592-4f0d-9215-e431b23dc9c2")]
    #endregion
    public partial class ProductQuote : Quote 
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

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Allors
        [Id("AA4B1340-BE7E-400F-891E-D9FC90974FD9")]
        [AssociationId("FCCD60B9-4DE4-44A3-A359-7F6DF67F8D3B")]
        [RoleId("C9BCD102-13EE-474F-9612-D66FAC929FFD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]

        public ProductQuoteObjectState CurrentObjectState { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}