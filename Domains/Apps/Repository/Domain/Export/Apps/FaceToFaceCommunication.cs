namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("d0f9fc0d-a3c5-46cc-ab00-4c724995fc14")]
    #endregion
    public partial class FaceToFaceCommunication : CommunicationEvent, Versioned
    {
        #region inherited properties

        public CommunicationEventState PreviousCommunicationEventState { get; set; }

        public CommunicationEventState LastCommunicationEventState { get; set; }

        public CommunicationEventState CommunicationEventState { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }
        public DateTime ScheduledStart { get; set; }

        public Party[] ToParties { get; set; }

        public ContactMechanism[] ContactMechanisms { get; set; }

        public Party[] InvolvedParties { get; set; }

        public DateTime InitialScheduledStart { get; set; }

        public CommunicationEventPurpose[] EventPurposes { get; set; }

        public DateTime ScheduledEnd { get; set; }

        public DateTime ActualEnd { get; set; }

        public WorkEffort[] WorkEfforts { get; set; }

        public string Description { get; set; }

        public DateTime InitialScheduledEnd { get; set; }

        public Party[] FromParties { get; set; }

        public string Subject { get; set; }

        public Media[] Documents { get; set; }

        public Case Case { get; set; }

        public Priority Priority { get; set; }

        public Person Owner { get; set; }

        public string Note { get; set; }

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

        #endregion

        #region Allors
        [Id("52b8614b-799e-4aea-a012-ea8dbc23f8dd")]
        [AssociationId("ac424847-d426-4614-99a2-37c70841c454")]
        [RoleId("bcf4a8df-8b57-4b3c-a6e5-f9b56c71a13b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Party[] Participants { get; set; }

        #region Allors
        [Id("95ae979f-d549-4ea1-87f0-46aa55e4b14a")]
        [AssociationId("d34e4203-0bd2-4fe4-a2ef-9f9f52b49cf9")]
        [RoleId("9f67b296-953d-4e04-b94d-6ffece87ceef")]
        #endregion
        [Size(256)]
        [Workspace]
        public string Location { get; set; }
        
        #region Versioning
        #region Allors
        [Id("4339C173-EEAA-4B11-8E54-D96C98B2AF01")]
        [AssociationId("41DDA0ED-160D-41B7-A347-CD167A957555")]
        [RoleId("DE4BD7DC-B219-4CE6-9F03-4E7F4681BFEC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public FaceToFaceCommunicationVersion CurrentVersion { get; set; }

        #region Allors
        [Id("B97DEBD2-482A-47A7-A7A2-FBC3FD20E7B4")]
        [AssociationId("428849DB-9000-4107-A68E-27FA4828344E")]
        [RoleId("4F9C3CE5-3418-484C-A770-48D2EDEB6E6A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public FaceToFaceCommunicationVersion[] AllVersions { get; set; }
        #endregion

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