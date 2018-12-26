namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("9426c214-c85d-491b-a5a6-9f573c3341a0")]
    #endregion
    public partial class EmailCommunication : CommunicationEvent, Versioned
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public DateTime ScheduledStart { get; set; }

        public Party FromParty { get; set; }

        public Party ToParty { get; set; }

        public ContactMechanism[] ContactMechanisms { get; set; }

        public Party[] InvolvedParties { get; set; }

        public DateTime InitialScheduledStart { get; set; }

        public CommunicationEventPurpose[] EventPurposes { get; set; }

        public DateTime ScheduledEnd { get; set; }

        public DateTime ActualEnd { get; set; }

        public WorkEffort[] WorkEfforts { get; set; }

        public string Description { get; set; }

        public DateTime InitialScheduledEnd { get; set; }

        public string Subject { get; set; }

        public Media[] Documents { get; set; }

        public Case Case { get; set; }

        public Priority Priority { get; set; }

        public Person Owner { get; set; }

        public DateTime ActualStart { get; set; }
        public bool SendNotification { get; set; }
        public bool SendReminder { get; set; }
        public DateTime RemindAt { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public CommunicationEventState PreviousCommunicationEventState { get; set; }

        public CommunicationEventState LastCommunicationEventState { get; set; }

        public CommunicationEventState CommunicationEventState { get; set; }

        #endregion

        #region Versioning
        #region Allors
        [Id("B3BC815E-17E9-4722-A421-42E211421693")]
        [AssociationId("ADB26602-A342-490E-A503-33F7B3EE33D2")]
        [RoleId("1414865A-3240-404A-AE19-3D42884DEAB5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public EmailCommunicationVersion CurrentVersion { get; set; }

        #region Allors
        [Id("44420D9F-80FC-4432-85C3-1641A5493765")]
        [AssociationId("62386C79-D5A7-4DE8-B04F-2A4B64237FDB")]
        [RoleId("DAFCBD9B-5A82-4EAA-8197-B187D1B6C507")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToMany)]
        public EmailCommunicationVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("25b8aa5e-e7c5-4689-b1ed-d9a0ba47b8eb")]
        [AssociationId("11649936-a5fa-488e-8d17-e80619c4d634")]
        [RoleId("6219fd3b-4f38-4f8f-8a5a-783f908ef55a")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public EmailAddress FromEmail { get; set; }

        #region Allors
        [Id("4026fcf7-3fc2-494b-9c4a-3e19eed74134")]
        [AssociationId("f2febf7f-7917-4499-8546-cae1e53d6791")]
        [RoleId("50439b5a-2251-469c-8512-f9dc65b0d9f6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public EmailAddress ToEmail { get; set; }

        #region Allors
        [Id("e12818ad-4ffd-4d91-8142-4ac9bfcbc146")]
        [AssociationId("a44a8d84-2510-45fd-add1-646f84be072d")]
        [RoleId("ae354426-6273-4b09-aabf-3f6d25f86e56")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public EmailTemplate EmailTemplate { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Cancel(){}

        public void Close(){}

        public void Reopen(){}




        public void Delete(){}


        #endregion
    }
}