namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("fcdf4f00-d6f4-493f-a430-89789a3cdef6")]
    #endregion
    public partial class PhoneCommunication : CommunicationEvent, Versioned
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

        #endregion

        #region Allors
        [Id("50a8225e-7094-4572-8074-a5df4a50b0bd")]
        [AssociationId("5fb6405b-2d06-425d-9e42-cb6638a2e308")]
        [RoleId("209e3d12-b5cf-49c9-a39c-15f14690ec69")]
        #endregion
        [Workspace]
        public bool LeftVoiceMail { get; set; }

        #region Allors
        [Id("0001D33E-C20F-4674-8EB2-B5DCCAA7E556")]
        [AssociationId("8CB849EA-F0B6-4837-B0D8-75B8AA53C639")]
        [RoleId("EAFB1350-4A2F-450B-82C5-8D3AA12194E0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public TelecommunicationsNumber PhoneNumber { get; set; }
        
        #region Versioning
        #region Allors
        [Id("89C3C1EC-F87F-4417-A0B3-0699FAF0BA53")]
        [AssociationId("DC0305FD-A268-4E24-AEBB-A361BC9150C7")]
        [RoleId("E3D4E553-1A45-4895-98D5-9B0FA96456FC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PhoneCommunicationVersion CurrentVersion { get; set; }

        #region Allors
        [Id("03ED4C6F-8EA4-4D3C-A91E-BCD191A997C9")]
        [AssociationId("A0B07210-891E-4532-A7F3-7AE15E54A77D")]
        [RoleId("35C1CA8F-3EE3-4F2E-B983-68C11DE9A5B6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PhoneCommunicationVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void OnPreFinalize(){} public void OnFinalize()
        {
            
        }

        public void OnPostFinalize()
        {
            
        }

        public void Cancel(){}

        public void Close(){}

        public void Reopen(){}




        public void Delete(){}


        #endregion
    }
}