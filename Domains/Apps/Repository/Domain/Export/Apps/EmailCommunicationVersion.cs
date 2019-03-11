namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("EB4F71AE-3BA1-4421-82AB-11F6F3E8C4D5")]
    #endregion
    public partial class EmailCommunicationVersion : CommunicationEventVersion
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
        [Id("24A22ECB-E303-436D-B9B8-C0FFEA4388E8")]
        [AssociationId("B744ECF8-9249-4C27-89CB-0A30098B29ED")]
        [RoleId("9A011509-3702-4762-836A-02272DC4CE2C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public EmailAddress FromEmail { get; set; }

        #region Allors
        [Id("DE61E1FA-401E-4C45-B92C-908D7F6693F9")]
        [AssociationId("A0A4717B-E7F0-43C4-B7C2-41D7E2959B8F")]
        [RoleId("60228ABA-E68B-4ECE-9759-226197A5D696")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public EmailAddress ToEmail { get; set; }

        #region Allors
        [Id("44414FC2-28DA-43E2-85DF-B70FF0E26D0C")]
        [AssociationId("3C57343A-2532-472C-95FF-D2396AC25926")]
        [RoleId("E13C9361-134D-4587-BA96-80A37A199E9F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public EmailTemplate EmailTemplate { get; set; }

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