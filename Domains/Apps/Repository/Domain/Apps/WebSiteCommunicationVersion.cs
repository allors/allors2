namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("4A1E0BDA-E1EA-416F-BFC5-9407487F2509")]
    #endregion
    public partial class WebSiteCommunicationVersion : IWebSiteCommunication 
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
        public Party Originator { get; set; }
        public Party Receiver { get; set; }
        #endregion

        #region Allors
        [Id("D32394E5-0C55-4942-97E4-FB23E781CA1A")]
        [AssociationId("F9028616-C1F8-484A-A7B0-F55D1E563CAD")]
        [RoleId("C9600B8A-F7DA-424B-9EA3-D9B7CDF853B3")]
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