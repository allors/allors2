namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("05964e28-2c1d-4837-a887-2255f157e889")]
    #endregion
    public partial class LetterCorrespondence : CommunicationEvent, ILetterCorrespondence 
    {
        #region inherited properties

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

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public PostalAddress[] PostalAddresses { get; set; }
        public Party[] Originators { get; set; }
        public Party[] Receivers { get; set; }
        public bool IncomingLetter { get; set; }

        #endregion

        #region Allors
        [Id("9A42BAF7-D2CA-46D6-95E2-28198DA719FA")]
        [AssociationId("B168D640-F4B7-4142-9A0B-77FC405C9512")]
        [RoleId("0700BA1C-CB54-4D1C-8B00-8DE505E503C0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public LetterCorrespondenceVersion CurrentVersion { get; set; }

        #region Allors
        [Id("E9EB1E16-BC1F-4677-9B67-2CDE3035D010")]
        [AssociationId("1EBC915D-C786-412C-A3C4-3D00F62CD620")]
        [RoleId("906C0103-86E3-45D4-9A9D-DB9C726D3B3D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public LetterCorrespondenceVersion PreviousVersion { get; set; }

        #region Allors
        [Id("1BD17B22-88CB-4F44-8B27-8EB8BCD9C963")]
        [AssociationId("FD6F1CE4-D23F-4B99-9CF8-71C3A7D50599")]
        [RoleId("3ED25C01-401F-42D4-ABAA-C24EA95E0BAA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public LetterCorrespondenceVersion[] AllVersions { get; set; }

        #region Allors
        [Id("4F31F156-39D9-4F6B-A8E7-CBA53F89CC58")]
        [AssociationId("D7EA86BC-ACBB-442E-8A47-23BB7301B9BA")]
        [RoleId("50847AC8-5968-4C53-8B58-1F37B9803361")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public LetterCorrespondenceVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("6066BF91-C495-408C-BA2E-A05984C1F648")]
        [AssociationId("3F46EAC2-7EE3-4AE0-83A8-2ED60E814C4F")]
        [RoleId("EEF160C6-66B0-40C3-A343-0F8A58ECE260")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public LetterCorrespondenceVersion[] AllStateVersions { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Cancel(){}

        public void Close(){}

        public void Reopen(){}




        public void Delete(){}


        #endregion
    }
}