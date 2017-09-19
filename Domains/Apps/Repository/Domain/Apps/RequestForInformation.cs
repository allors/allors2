namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("eab85f26-c3f4-4f47-97dc-8f9429856c00")]
    #endregion
    [Plural("RequestsForInformation")]
    public partial class RequestForInformation : Request
    {
        #region inherited properties
        public string InternalComment { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredResponseDate { get; set; }
        public RequestItem[] RequestItems { get; set; }
        public string RequestNumber { get; set; }
        public RespondingParty[] RespondingParties { get; set; }
        public Party Originator { get; set; }
        public Currency Currency { get; set; }
        public RequestObjectState CurrentObjectState { get; set; }
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
        public Guid UniqueId { get; set; }
        public string PrintContent { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        #endregion

        #region Versioning
        #region Allors
        [Id("07D23C22-963B-485D-8E5C-F7962AE050A8")]
        [AssociationId("148E8D4D-FB66-45AA-A996-BF286E247B38")]
        [RoleId("B8632272-190A-43A0-8B6D-912A4524C82D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestForInformationVersion CurrentVersion { get; set; }

        #region Allors
        [Id("D224FF86-4420-4D77-88F1-B734AF5AD5AA")]
        [AssociationId("CAB5EE0B-04D4-4052-B377-51CEC57DA936")]
        [RoleId("7D082E78-C813-4814-B601-4867DF3F604E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestForInformationVersion PreviousVersion { get; set; }

        #region Allors
        [Id("1E80AB51-8E49-4655-A982-C78B6ABEE202")]
        [AssociationId("8C62A675-C1E8-4788-BE3C-2A01FB465E24")]
        [RoleId("C8D212BF-3770-46C2-9BBA-BC081457AAB8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestForInformationVersion[] AllVersions { get; set; }

        #region Allors
        [Id("9985BF85-A336-44DA-B931-785D9172961A")]
        [AssociationId("7D943E8E-F68A-43F9-9499-EF668B7E6DA5")]
        [RoleId("A1919EA7-4616-4FB3-89B3-FC0E3459D6BD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestForInformationVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("7676A3EE-2AA1-44BD-8C3B-F41DE6CBFF1C")]
        [AssociationId("C24C0096-7CC7-4E6B-A86B-3A9EF695C4E2")]
        [RoleId("023DA0FE-9642-4FAB-849C-F926FBCB5FCB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestForInformationVersion[] AllStateVersions { get; set; }
        #endregion

        #region inherited methods
        public void Cancel() { }

        public void Reject() { }

        public void Submit() { }

        public void Hold() { }

        public void AddNewRequestItem() { }

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}