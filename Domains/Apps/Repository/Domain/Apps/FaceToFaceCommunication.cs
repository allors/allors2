namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("d0f9fc0d-a3c5-46cc-ab00-4c724995fc14")]
    #endregion
    public partial class FaceToFaceCommunication : CommunicationEvent, IFaceToFaceCommunication
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }
        public DateTime ScheduledStart { get; set; }

        public Party[] ToParties { get; set; }

        public ContactMechanism[] ContactMechanisms { get; set; }

        public Party[] InvolvedParties { get; set; }

        public DateTime InitialScheduledStart { get; set; }

        public CommunicationEventObjectState CurrentObjectState { get; set; }

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

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public Party[] Participants { get; set; }
        public string Location { get; set; }

        #endregion

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
        [Id("B3C0A4F9-0C0D-4107-8E4D-3D7342D76A72")]
        [AssociationId("7734CD16-5302-4C8D-AC80-819240CC96F0")]
        [RoleId("208B8780-E4D6-45A0-97D8-F8A3A5F39915")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public FaceToFaceCommunicationVersion PreviousVersion { get; set; }

        #region Allors
        [Id("B97DEBD2-482A-47A7-A7A2-FBC3FD20E7B4")]
        [AssociationId("428849DB-9000-4107-A68E-27FA4828344E")]
        [RoleId("4F9C3CE5-3418-484C-A770-48D2EDEB6E6A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public FaceToFaceCommunicationVersion[] AllVersions { get; set; }

        #region Allors
        [Id("A376BA63-B50B-43AF-97AD-9EC8CB83D572")]
        [AssociationId("3E9C0B31-E21D-49C9-B2E3-6703F868C938")]
        [RoleId("8EFCBE1F-0852-44E5-BF03-FABF04AB5F1F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public FaceToFaceCommunicationVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("C8ADA2DE-B7DB-4673-B7E2-26D46C744295")]
        [AssociationId("B49701D5-F229-4D7C-8A94-C13E87385717")]
        [RoleId("E05E6DFE-041E-4563-BF4D-1A443E2965F2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public FaceToFaceCommunicationVersion[] AllStateVersions { get; set; }

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