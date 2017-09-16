namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("9426c214-c85d-491b-a5a6-9f573c3341a0")]
    #endregion
    public partial class EmailCommunication : CommunicationEvent, IEmailCommunication
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

        public EmailAddress Originator { get; set; }
        public EmailAddress[] Addressees { get; set; }
        public EmailAddress[] CarbonCopies { get; set; }
        public EmailAddress[] BlindCopies { get; set; }
        public EmailTemplate EmailTemplate { get; set; }
        public bool IncomingMail { get; set; }

        #endregion

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
        [Id("D13C6C76-50FB-4F58-A675-0EDEFC06F5B8")]
        [AssociationId("F42D1A6F-7CCB-4526-BB91-7028F62C4FE0")]
        [RoleId("6DDD89EB-EF59-4141-AB9F-29F9F412225A")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        public EmailCommunicationVersion PreviousVersion { get; set; }

        #region Allors
        [Id("44420D9F-80FC-4432-85C3-1641A5493765")]
        [AssociationId("62386C79-D5A7-4DE8-B04F-2A4B64237FDB")]
        [RoleId("DAFCBD9B-5A82-4EAA-8197-B187D1B6C507")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToMany)]
        public EmailCommunicationVersion[] AllVersions { get; set; }

        #region Allors
        [Id("75E2DCFA-4AC2-4609-BE48-37DEA5D59034")]
        [AssociationId("4E2380C8-D94E-49BA-B67A-805346EC92DD")]
        [RoleId("A6F1A50C-09D7-419E-9CF0-F6D7F946B052")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        public EmailCommunicationVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("30A7022D-8320-4417-99EF-C99A6A029EBD")]
        [AssociationId("3B76B7DB-E4FF-49DC-848D-233BFA639E35")]
        [RoleId("2C2DEC99-B389-4F40-AF26-428DD5AC6AA7")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToMany)]
        public EmailCommunicationVersion[] AllStateVersions { get; set; }

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