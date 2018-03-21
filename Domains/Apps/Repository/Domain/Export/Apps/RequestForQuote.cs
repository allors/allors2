namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("874dfe70-2e50-4861-b26d-dc55bc8fa0d0")]
    #endregion
    [Plural("RequestsForQuote")]
    public partial class RequestForQuote : Request, Versioned
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public RequestState PreviousRequestState { get; set; }

        public RequestState LastRequestState { get; set; }

        public RequestState RequestState { get; set; }

        public InternalOrganisation Recipient { get; set; }

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

        public Person ContactPerson { get; set; }

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
        [Id("4C48ECD7-C684-4103-89B2-F5CFC0675124")]
        [AssociationId("200351D5-D66F-423E-8DDE-61FBA6F4D950")]
        [RoleId("94D40D97-7FAA-4C47-9199-7C693C89F34B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestForQuoteVersion CurrentVersion { get; set; }

        #region Allors
        [Id("A14F99B1-A53B-4ABD-B5AC-810FF7CBAC6D")]
        [AssociationId("CA69625D-6D3E-435F-8520-843D26783033")]
        [RoleId("9B5447F2-A591-44CA-8414-2406C7BEB14D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestForQuoteVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("A57F9C84-A17D-4F5E-91EE-C0AD38EF6985")]
        #endregion
        [Workspace]
        public void CreateQuote() { }

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
    }
}