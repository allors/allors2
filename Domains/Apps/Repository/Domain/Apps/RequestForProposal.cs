namespace Allors.Repository
{
  using System;

  using Attributes;

  #region Allors
  [Id("0112ddd0-14de-43e2-97d3-981766dd957e")]
  #endregion
  [Plural("RequestsForProposal")]
  public partial class RequestForProposal : Request, Versioned
  {
    #region inherited properties

    public ObjectState[] PreviousObjectStates { get; set; }

    public ObjectState[] LastObjectStates { get; set; }

    public ObjectState[] ObjectStates { get; set; }

    public RequestState PreviousRequestState { get; set; }

    public RequestState LastRequestState { get; set; }

    public RequestState RequestState { get; set; }

    public string InternalComment { get; set; }
    public string Description { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime RequiredResponseDate { get; set; }
    public RequestItem[] RequestItems { get; set; }
    public string RequestNumber { get; set; }
    public RespondingParty[] RespondingParties { get; set; }
    public Party Originator { get; set; }
    public Currency Currency { get; set; }
    public ContactMechanism FullfillContactMechanism { get; set; }
    public string EmailAddress { get; set; }
    public string TelephoneNumber { get; set; }
    public string TelephoneCountryCode { get; set; }
    public Permission[] DeniedPermissions { get; set; }
    public SecurityToken[] SecurityTokens { get; set; }
    public string Comment { get; set; }
    public User CreatedBy { get; set; }
    public User LastModifiedBy { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public string PrintContent { get; set; }

    #endregion

    #region Versioning
    #region Allors
    [Id("0CDCEB30-489D-4CC7-8C27-B51BCBC3B631")]
    [AssociationId("2706C873-5D24-4B86-826E-9FDAD9605D70")]
    [RoleId("2847C8F5-2EA6-4D80-9518-C264C8E7AB8B")]
    [Indexed]
    #endregion
    [Multiplicity(Multiplicity.OneToOne)]
    [Workspace]
    public RequestForProposalVersion CurrentVersion { get; set; }

    #region Allors
    [Id("2043D481-BB86-4F2E-AE34-506E9F12227F")]
    [AssociationId("00C38CFD-1A6F-474D-8B2E-6087B2FAA1C3")]
    [RoleId("C80A0D2E-A695-4FF2-BACF-D95DA9BC0B91")]
    [Indexed]
    #endregion
    [Multiplicity(Multiplicity.OneToMany)]
    [Workspace]
    public RequestForProposalVersion[] AllVersions { get; set; }
    #endregion

    #region inherited methods
    public void Cancel() { }

    public void Reject() { }

    public void Submit() { }

    public void Hold() { }

    public void OnBuild() { }

    public void OnPostBuild() { }

    public void OnPreDerive() { }

    public void OnDerive() { }

    public void OnPostDerive() { }

    #endregion

    #region Allors
    [Id("30472626-909D-4B7E-A153-B2754D6398E3")]
    #endregion
    public void CreateProposal() { }
  }
}