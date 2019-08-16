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

        public Party FromParty { get; set; }

        public Party ToParty { get; set; }
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
        [Id("1CEE575B-0954-4A05-A337-24D025E0A156")]
        [AssociationId("9EB873B9-DBD4-4658-BA67-997DAB857471")]
        [RoleId("E6F82CC0-AB60-4945-93EB-07B47FD73F4A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public TelecommunicationsNumber PhoneNumber { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}