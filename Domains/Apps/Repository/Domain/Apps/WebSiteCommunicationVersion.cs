namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("B1F9AAA7-188E-4C03-9AC1-EA4373DAA21A")]
    #endregion
    public partial class WebSiteCommunicationVersion : CommunicationEventVersion 
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

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("59451355-2635-47AA-8B0F-06F7F21D9451")]
        [AssociationId("DA3173B2-BCA3-4A91-9D98-8093800F4189")]
        [RoleId("D615BB76-F50F-4E0B-91A6-8959C7E55647")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party Originator { get; set; }

        #region Allors
        [Id("6CCEE4B3-38BB-43DA-B076-1609DC4DD009")]
        [AssociationId("DED77D19-757A-489F-AF3D-E0724A574F87")]
        [RoleId("3CF32BA0-B876-4F9A-9531-462AED54FCA7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party Receiver { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}