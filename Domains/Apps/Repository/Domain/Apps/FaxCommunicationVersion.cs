namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("B64D00D7-03C7-4D7A-B0A2-0825234C2070")]
    #endregion
    public partial class FaxCommunicationVersion : CommunicationEventVersion
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
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
        public DateTime TimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("6E0B9344-5FF7-4B12-8974-92397AFF2543")]
        [AssociationId("593B4954-AEE5-41B4-B541-BDB3C361D39F")]
        [RoleId("7368D5AC-BEBA-4E97-97AE-25F039F754FC")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party Originator { get; set; }

        #region Allors
        [Id("00F53390-FD2C-42E9-B495-64FA56339617")]
        [AssociationId("793B25BF-F734-45D5-A724-A23103BF3E15")]
        [RoleId("FBCBF494-83AA-467C-A8CD-AB76E0F08223")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party Receiver { get; set; }

        #region Allors
        [Id("23DDDD16-BD4F-4CAD-9B45-1B34D17A4D97")]
        [AssociationId("181FF5B8-DFA5-4EC1-87BB-B2AEAD195273")]
        [RoleId("9A130A18-09E4-484A-8687-77AF1684C607")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public TelecommunicationsNumber OutgoingFaxNumber { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}