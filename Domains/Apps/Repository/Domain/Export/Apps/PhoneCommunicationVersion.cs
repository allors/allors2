namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("00022659-5830-4A1F-A463-C135D5B65992")]
    #endregion
    public partial class PhoneCommunicationVersion : CommunicationEventVersion 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public SecurityToken OwnerSecurityToken { get; set; }
        public AccessControl OwnerAccessControl { get; set; }
        public DateTime ScheduledStart { get; set; }
        public Party[] ToParties { get; set; }
        public ContactMechanism[] ContactMechanisms { get; set; }
        public Party[] InvolvedParties { get; set; }
        public DateTime InitialScheduledStart { get; set; }
        public CommunicationEventState CommunicationEventState { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

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
        public DateTime ActualStart { get; set; }
        public bool SendNotification { get; set; }
        public bool SendReminder { get; set; }
        public DateTime RemindAt { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("D691FA00-ADDF-41DF-8586-4FEDE646DDF7")]
        [AssociationId("FF10126C-6B7B-4799-A7B7-AACB7FCABD8A")]
        [RoleId("E56889A9-DC2B-4833-9807-F6C73759FC8A")]
        #endregion
        [Workspace]
        public bool LeftVoiceMail { get; set; }

        #region Allors
        [Id("F42FD1E6-ADBA-4734-9AAA-F3176F91E45F")]
        [AssociationId("08FA136F-2527-40AC-86E5-49FBBBC141C5")]
        [RoleId("E475C664-D369-4A0E-993F-0185A13E5470")]
        #endregion
        [Required]
        [Workspace]
        public bool IncomingCall { get; set; }

        #region Allors
        [Id("1CEE575B-0954-4A05-A337-24D025E0A156")]
        [AssociationId("9EB873B9-DBD4-4658-BA67-997DAB857471")]
        [RoleId("E6F82CC0-AB60-4945-93EB-07B47FD73F4A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public TelecommunicationsNumber PhoneNumber { get; set; }

        #region Allors
        [Id("F6BBFADC-A5F9-4F23-B39E-3A43949B667D")]
        [AssociationId("41AB4F17-F952-4E71-8EF2-878BF6522B18")]
        [RoleId("CD7C8284-9221-4A82-AE22-B99F21060E2B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Party[] Receivers { get; set; }

        #region Allors
        [Id("87995CE4-A46A-4DC9-8A6D-603E9B84A131")]
        [AssociationId("7F8A34C4-86AE-4A9F-93A0-92DD5B41A8CF")]
        [RoleId("EBDD2F26-1AE3-44B5-9647-80AFCFBACD39")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Party[] Callers { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}