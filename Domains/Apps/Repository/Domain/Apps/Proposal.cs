namespace Allors.Repository
{
  using System;
  using Attributes;

  #region Allors
  [Id("360cf15d-c360-4d68-b693-7d1544388169")]
  #endregion
  public partial class Proposal : Quote, Versioned
  {
    #region inherited properties

    public ObjectState[] PreviousObjectStates { get; set; }

    public ObjectState[] LastObjectStates { get; set; }

    public ObjectState[] ObjectStates { get; set; }

    public QuoteState PreviousQuoteState { get; set; }

    public QuoteState LastQuoteState { get; set; }

    public QuoteState QuoteState { get; set; }

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
    [Id("5CDB3F01-2FE6-45BA-979A-C0ABDB53D571")]
    [AssociationId("F71612B7-E089-4E13-84FE-6EFAA000678D")]
    [RoleId("615EAF5C-4128-4C9B-A281-FC1E9EFC4B28")]
    [Indexed]
    #endregion
    [Multiplicity(Multiplicity.OneToMany)]
    [Workspace]
    public ProposalVersion[] AllVersions { get; set; }
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
  }
}