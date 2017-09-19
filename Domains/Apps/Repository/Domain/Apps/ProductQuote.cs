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

        #region Versioning
        #region Allors
        [Id("A3400F09-19A8-494A-B50A-4081B9E5D174")]
        [AssociationId("C9CEB7B2-53A7-4074-BFCF-C0068ACE28DE")]
        [RoleId("FE7CE5B8-BC53-4968-B675-E9597B5309A2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProductQuoteVersion CurrentVersion { get; set; }

        #region Allors
        [Id("1992D2C7-C624-475B-B082-5590160C71B0")]
        [AssociationId("D65C568F-5581-4D29-BBA2-F816577124F5")]
        [RoleId("1A19A1DC-6ACE-4B34-8F5B-43D1527BB495")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProductQuoteVersion PreviousVersion { get; set; }

        #region Allors
        [Id("82A8F3CE-9811-4F32-BCE5-D103EFFCBA4B")]
        [AssociationId("1EA14BEF-4C32-4AB8-930C-C287DA96FB0F")]
        [RoleId("39B134C3-37ED-480E-84D6-541F5424747B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ProductQuoteVersion[] AllVersions { get; set; }

        #region Allors
        [Id("9C4A08B9-7BFF-46EE-B55D-30CF49E1347E")]
        [AssociationId("455D6EEB-5679-4E63-9EA4-77B95EB3C5C3")]
        [RoleId("8B1A4571-233E-4C82-BE2F-7F58049F0E52")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProductQuoteVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("FDA63BA8-7A05-4227-98C0-989D11F6283D")]
        [AssociationId("CB64EFC6-DB64-42A5-8015-3FCFBF03E79E")]
        [RoleId("936401DD-0343-40FE-BB90-DC023836FBD0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ProductQuoteVersion[] AllStateVersions { get; set; }
        #endregion

        #region inherited methods

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Approve() { }

        public void Reject() { }
        public void AddNewQuoteItem() { }

        #endregion

        #region Allors
        [Id("8D92571B-AABE-45EC-A2BB-93219B3E8C12")]
        #endregion
        [Workspace]
        public void Order() { }
    }
}