namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("12DB12F5-88CC-46D2-A664-28797C143D92")]
    #endregion
    public partial class FaceToFaceCommunicationVersion : CommunicationEventVersion
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
        [Id("5E7F5ACC-9486-403C-8541-EE28D402BB3F")]
        [AssociationId("8202F898-EFD5-4CEA-82F2-6C64479A34E2")]
        [RoleId("0D30F956-401B-4144-9266-8B2A9087C69A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Party[] Participants { get; set; }

        #region Allors
        [Id("2B614B23-A909-423F-A80F-7D13F062E3F7")]
        [AssociationId("FB51535D-D322-4BA1-A3B2-C65989447898")]
        [RoleId("28C9E924-2B87-4E72-B74E-62BC717D6611")]
        #endregion
        [Size(256)]
        [Workspace]
        public string Location { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}