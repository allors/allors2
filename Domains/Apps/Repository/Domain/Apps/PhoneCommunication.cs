namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("fcdf4f00-d6f4-493f-a430-89789a3cdef6")]
    #endregion
    public partial class PhoneCommunication : CommunicationEvent 
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public DateTime ScheduledStart { get; set; }

        public Party[] ToParties { get; set; }

        public ContactMechanism[] ToContactMechanisms { get; set; }

        public CommunicationEventStatus[] CommunicationEventStatuses { get; set; }

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

        public ContactMechanism[] FromContactMechanisms { get; set; }

        public Person Owner { get; set; }

        public CommunicationEventStatus CurrentCommunicationEventStatus { get; set; }

        public string Note { get; set; }

        public DateTime ActualStart { get; set; }

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

        #endregion

        #region Allors
        [Id("50a8225e-7094-4572-8074-a5df4a50b0bd")]
        [AssociationId("5fb6405b-2d06-425d-9e42-cb6638a2e308")]
        [RoleId("209e3d12-b5cf-49c9-a39c-15f14690ec69")]
        #endregion
        [Workspace]
        public bool LeftVoiceMail { get; set; }

        #region Allors
        [Id("53df1269-a6f0-49a4-bd2f-af4aff75565a")]
        [AssociationId("32e719bd-39c7-4fc3-bff2-e0091cebd0b7")]
        [RoleId("5bbb6e8a-7c82-492e-b497-3579007f9294")]
        #endregion
        [Required]
        [Workspace]
        public bool IncomingCall { get; set; }

        #region Allors
        [Id("5e3c675b-b329-47a4-9d53-b0e95837a23b")]
        [AssociationId("16fa813c-15d6-4bfb-a7b3-c295efe47a1c")]
        [RoleId("f9320b55-230d-4f10-9a1b-6960137326b7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Required]
        [Workspace]
        public Party[] Receivers { get; set; }

        #region Allors
        [Id("7a37ab85-222a-4d13-b832-b222faefcf39")]
        [AssociationId("79c04646-6f62-4867-9f89-f2ce1876e981")]
        [RoleId("507e6ff3-3baa-4c77-b41b-4d1893443dc2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Required]
        [Workspace]
        public Party[] Callers { get; set; }


        #region inherited methods


        public void OnBuild(){}

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