namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("9A5F97F0-8E2B-4F4F-A31E-C3599F5EAE1D")]
    #endregion
    public partial class LetterCorrespondenceVersion : ILetterCorrespondence 
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
        public PostalAddress[] PostalAddresses { get; set; }
        public Party[] Originators { get; set; }
        public Party[] Receivers { get; set; }
        public bool IncomingLetter { get; set; }
        #endregion

        #region Allors
        [Id("BC3015EC-0D29-467A-BA47-E6D434DB87B5")]
        [AssociationId("4E6C1599-6C00-44DA-9542-CF2B38E75873")]
        [RoleId("BFBE5779-5053-4522-ACB9-D164623D14EF")]
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