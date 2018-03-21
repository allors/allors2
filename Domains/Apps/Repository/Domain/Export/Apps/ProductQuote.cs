namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("c2214ff4-d592-4f0d-9215-e431b23dc9c2")]
    #endregion
    [Plural("ProductQuotes")]
    public partial class ProductQuote : Quote, Versioned
    {
        #region inherited properties

        public QuoteState PreviousQuoteState { get; set; }

        public QuoteState LastQuoteState { get; set; }

        public QuoteState QuoteState { get; set; }

        public InternalOrganisation Issuer { get; set; }

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
        public Request Request { get; set; }

        public Person ContactPerson { get; set; }

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string PrintContent { get; set; }

        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Comment { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

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
        [Id("82A8F3CE-9811-4F32-BCE5-D103EFFCBA4B")]
        [AssociationId("1EA14BEF-4C32-4AB8-930C-C287DA96FB0F")]
        [RoleId("39B134C3-37ED-480E-84D6-541F5424747B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ProductQuoteVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Approve() { }

        public void Reject() { }

        #endregion

        #region Allors
        [Id("8D92571B-AABE-45EC-A2BB-93219B3E8C12")]
        #endregion
        [Workspace]
        public void Order() { }
    }
}