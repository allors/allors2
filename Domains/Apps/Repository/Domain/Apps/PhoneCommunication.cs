namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("fcdf4f00-d6f4-493f-a430-89789a3cdef6")]
    #endregion
    public partial class PhoneCommunication : CommunicationEvent, IPhoneCommunication 
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

        public bool LeftVoiceMail { get; set; }
        public bool IncomingCall { get; set; }
        public Party[] Receivers { get; set; }
        public Party[] Callers { get; set; }

        #endregion

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
        [Id("39FF962B-AF4C-4403-82E3-0AFEB71DB821")]
        [AssociationId("24905D7A-8F29-4C91-AC56-126C250AF102")]
        [RoleId("A447494C-0CDC-4402-8EED-E8A9CA753554")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PhoneCommunicationVersion PreviousVersion { get; set; }

        #region Allors
        [Id("03ED4C6F-8EA4-4D3C-A91E-BCD191A997C9")]
        [AssociationId("A0B07210-891E-4532-A7F3-7AE15E54A77D")]
        [RoleId("35C1CA8F-3EE3-4F2E-B983-68C11DE9A5B6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PhoneCommunicationVersion[] AllVersions { get; set; }

        #region Allors
        [Id("075CF2FC-803A-40C8-A9BD-A2F1847E61BA")]
        [AssociationId("AA861E75-042D-45D2-B32C-C21C4D238660")]
        [RoleId("D9ECE8D6-5976-4095-A356-84848CE80860")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PhoneCommunicationVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("157DE928-5645-4E13-A41D-921A85E5FBC1")]
        [AssociationId("4414C050-BABF-4061-854F-C9C119F1E0DA")]
        [RoleId("F13950A3-FCA8-410F-A6FB-AFEFDDEA7A4F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PhoneCommunicationVersion[] AllStateVersions { get; set; }

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