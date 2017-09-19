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
        [Id("B25F19D3-00DB-40AF-8B1A-95C101C0A357")]
        [AssociationId("D637E43E-2743-4701-9BC1-702019FE1058")]
        [RoleId("65EA0B52-6E84-4C90-95AE-325D891B0828")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProposalVersion CurrentVersion { get; set; }

        #region Allors
        [Id("F5306FDB-0C7F-4609-BA96-C40A56F2F4FE")]
        [AssociationId("56FF9EF0-8F8E-4E91-89B3-8A855AE50EDF")]
        [RoleId("BC36A515-5B96-4A12-8896-0AD69A5D82CD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProposalVersion PreviousVersion { get; set; }

        #region Allors
        [Id("5CDB3F01-2FE6-45BA-979A-C0ABDB53D571")]
        [AssociationId("F71612B7-E089-4E13-84FE-6EFAA000678D")]
        [RoleId("615EAF5C-4128-4C9B-A281-FC1E9EFC4B28")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ProposalVersion[] AllVersions { get; set; }

        #region Allors
        [Id("29C3803B-8739-45CD-BFC1-A2708476C5B8")]
        [AssociationId("13984EF2-ADE0-4417-9E3C-5731B17B10D1")]
        [RoleId("00E890F0-11AE-480C-BA1B-C5AEFB198A41")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProposalVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("8E04F603-75A4-4768-9B57-E6235881FCEF")]
        [AssociationId("8993542F-3C7F-453B-8C52-7AA4C618232C")]
        [RoleId("238D965B-C23F-4A34-9FCB-B4DF5203AA3F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ProposalVersion[] AllStateVersions { get; set; }
        #endregion

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