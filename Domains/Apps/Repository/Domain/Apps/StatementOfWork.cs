namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("5459f555-cf6a-49c1-8015-b43cad74da17")]
    #endregion
    [Plural("StatementsOfWork")]
    public partial class StatementOfWork : Quote, IStatementOfWork 
    {
        #region inherited properties
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
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public Guid UniqueId { get; set; }
        public string PrintContent { get; set; }
        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Comment { get; set; }

        #endregion

        #region Allors
        [Id("9AEF9F40-E043-4FEE-AFE4-49E991114286")]
        [AssociationId("B32718B2-3EFB-4942-A33E-CFB7CF5DB2FA")]
        [RoleId("C54D2A3F-7CC7-4313-ADF4-B6AD952DAE01")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public StatementOfWorkVersion CurrentVersion { get; set; }

        #region Allors
        [Id("E5233ED5-209C-4C36-B279-ABEA6307F66F")]
        [AssociationId("EB42FD17-E487-46EC-B0DA-983DBF1ECB54")]
        [RoleId("B805F33F-1771-4237-A405-8B5BB58BB55C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public StatementOfWorkVersion PreviousVersion { get; set; }

        #region Allors
        [Id("01BCD729-E71D-41A1-B996-9FC9A808C0ED")]
        [AssociationId("661B7791-8604-48A4-8034-1A8ECE394B4D")]
        [RoleId("3D5460FF-8BEE-425A-8E53-655444E1BC10")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public StatementOfWorkVersion[] AllVersions { get; set; }

        #region Allors
        [Id("1E65DD56-157E-420C-B084-DE16C919AC90")]
        [AssociationId("786FCBC9-51BA-4030-80B1-4F8E90DF19BF")]
        [RoleId("6E8FA65B-AD0A-4A21-9E15-D696D383DA57")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public StatementOfWorkVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("09853CF8-F654-458C-8C31-06BE5D0567E9")]
        [AssociationId("A8756A80-7648-4E0A-B9D4-66D5746F12C4")]
        [RoleId("8D5279E6-BB76-4F4C-A6F9-BFBE6F638D4D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public StatementOfWorkVersion[] AllStateVersions { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Approve() { }

        public void Reject() { }
        public void AddNewQuoteItem() { }

        #endregion
    }
}