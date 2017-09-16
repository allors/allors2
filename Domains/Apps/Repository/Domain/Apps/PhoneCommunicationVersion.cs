namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("BFEADA01-5679-4C63-9FB8-FE897CAEC9DD")]
    #endregion
    public partial class PhoneCommunicationVersion : IPhoneCommunication 
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
        public bool LeftVoiceMail { get; set; }
        public bool IncomingCall { get; set; }
        public Party[] Receivers { get; set; }
        public Party[] Callers { get; set; }
        #endregion

        #region Allors
        [Id("08BD4203-B7CC-4B77-9AF2-8C65F9A08000")]
        [AssociationId("FD135CA1-A716-4193-B2C9-0EFE11B916F1")]
        [RoleId("8A7E6C19-24F3-4A14-9AC1-1BCA1162C1EB")]
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