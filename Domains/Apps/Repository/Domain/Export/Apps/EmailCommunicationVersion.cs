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
        public string Note { get; set; }
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
        public EmailAddress Originator { get; set; }

        #region Allors
        [Id("DE61E1FA-401E-4C45-B92C-908D7F6693F9")]
        [AssociationId("A0A4717B-E7F0-43C4-B7C2-41D7E2959B8F")]
        [RoleId("60228ABA-E68B-4ECE-9759-226197A5D696")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Required]
        [Workspace]
        public EmailAddress[] Addressees { get; set; }

        #region Allors
        [Id("644BD5F8-AB23-44BB-90B8-098C9F0223BA")]
        [AssociationId("69D92237-1A7B-4AD4-80A3-55A7F20077A8")]
        [RoleId("7BE43008-2BCA-478C-8234-7606D90293ED")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public EmailAddress[] CarbonCopies { get; set; }

        #region Allors
        [Id("A668BBB8-30F7-4997-B778-74590E46FE59")]
        [AssociationId("BA872A44-57A6-4746-B686-9E175C39E78A")]
        [RoleId("3A6DC9CC-306B-4FBC-997E-C44BD41E7F61")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public EmailAddress[] BlindCopies { get; set; }

        #region Allors
        [Id("44414FC2-28DA-43E2-85DF-B70FF0E26D0C")]
        [AssociationId("3C57343A-2532-472C-95FF-D2396AC25926")]
        [RoleId("E13C9361-134D-4587-BA96-80A37A199E9F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public EmailTemplate EmailTemplate { get; set; }

        #region Allors
        [Id("9D21C03F-E810-4296-8CEE-47EDAFB39441")]
        [AssociationId("9BED13F5-973D-4ED8-B57C-E1D367ABC1C0")]
        [RoleId("08B1C365-EA8D-4320-B9AF-6E4DD20D2E19")]
        #endregion
        [Workspace]
        [Required]
        public bool IncomingMail { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}