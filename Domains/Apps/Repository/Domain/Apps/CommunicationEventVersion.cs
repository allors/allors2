namespace Allors.Repository
{
    using System;
    using Attributes;
    
    #region Allors
    [Id("")]
    #endregion
    public partial interface CommunicationEventVersion : Version
    {
        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Derived]
        SecurityToken OwnerSecurityToken { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Indexed]
        [Derived]
        AccessControl OwnerAccessControl { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        DateTime ScheduledStart { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Party[] ToParties { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        ContactMechanism[] ContactMechanisms { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Party[] InvolvedParties { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        DateTime InitialScheduledStart { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        [Workspace]
        CommunicationEventObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        CommunicationEventPurpose[] EventPurposes { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        DateTime ScheduledEnd { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        DateTime ActualEnd { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        WorkEffort[] WorkEfforts { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        DateTime InitialScheduledEnd { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Party[] FromParties { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        string Subject { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Media[] Documents { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Case Case { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Priority Priority { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Person Owner { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Note { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        DateTime ActualStart { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        bool SendNotification { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        bool SendReminder { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        DateTime RemindAt { get; set; }
    }
}