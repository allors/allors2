namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("A298A2F8-4D4A-4CBA-B929-75DC5AA9E3D4")]
    #endregion
    public partial class LetterCorrespondenceVersion : CommunicationEventVersion
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
        [Id("14536536-13D9-4340-9F2E-6ECA15D337F2")]
        [AssociationId("E728A709-525D-49A2-BDBE-36FA4E281118")]
        [RoleId("F4043DF9-FACB-4C37-97EB-753409FBA012")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress PostalAddress { get; set; }

        #region Allors
        [Id("AD7AE076-7849-43ED-857B-DA3DA959CD8D")]
        [AssociationId("35A98C84-9AD8-4E47-A906-4254C092AD1F")]
        [RoleId("1683B25F-4988-408A-8B9D-1B98ACED0891")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Party[] Originators { get; set; }

        #region Allors
        [Id("98B54F39-032C-476A-910C-AE48D7F1ACC5")]
        [AssociationId("BB8F93C5-683E-4315-A442-6245519A7D70")]
        [RoleId("2035BEF1-4F44-4E14-9267-7ECD228C4B2A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Party[] Receivers { get; set; }

        #region Allors
        [Id("7488ECF0-D220-4CDF-86E4-EF768BA17B7F")]
        [AssociationId("3E69607E-1EEB-415D-8491-D60191D141C2")]
        [RoleId("A80748A2-B93B-45C4-BBF3-4BE618F90C34")]
        #endregion
        [Required]
        [Workspace]
        public bool IncomingLetter { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}