namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("5459f555-cf6a-49c1-8015-b43cad74da17")]
    #endregion
    [Plural("StatementsOfWork")]
    public partial class StatementOfWork : Quote 
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
        [Id("11822B78-6C35-42E3-A2FC-56C09D83912E")]
        [AssociationId("5EDCD272-B32B-452C-88C0-43B074A4FB51")]
        [RoleId("14D5DB87-1F9E-4320-BF18-885A00F60BB6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]

        public StatementOfWorkObjectState CurrentObjectState { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}