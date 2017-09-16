namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("1666332A-A550-4AE5-B8EB-88E3786676D1")]
    #endregion
    public partial class EmailCommunicationVersion : IEmailCommunication
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }

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
        public EmailAddress Originator { get; set; }
        public EmailAddress[] Addressees { get; set; }
        public EmailAddress[] CarbonCopies { get; set; }
        public EmailAddress[] BlindCopies { get; set; }
        public EmailTemplate EmailTemplate { get; set; }
        public bool IncomingMail { get; set; }
        #endregion

        #region Allors
        [Id("605A66C9-628A-416C-84DA-87699D4564CA")]
        [AssociationId("386C0C30-24AC-40D4-AFFF-D1B769D298A0")]
        [RoleId("6BCD0F6E-ABC5-4837-A5B8-D967F3C01908")]
        #endregion
        [Workspace]
        public DateTime TimeStamp { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}