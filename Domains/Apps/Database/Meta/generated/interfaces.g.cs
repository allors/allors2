namespace Allors.Meta
{
    public partial class MetaCachable : MetaInterface
	{
	    public static MetaCachable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType CacheId;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaCachable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("B17AFC19-9E91-4631-B6D8-43B32A65E0A0"))
			{
				SingularName = "Cachable",
			};
        }
	}

    public partial class MetaDeletable : MetaInterface
	{
	    public static MetaDeletable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods

		// Defined Roles

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaDeletable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("9279e337-c658-4086-946d-03c75cdb1ad3"))
			{
				SingularName = "Deletable",
			};
        }
	}

    public partial class MetaEnumeration : MetaInterface
	{
	    public static MetaEnumeration Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType LocalisedNames;
        public RoleType Name;
        public RoleType IsActive;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaEnumeration(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("b7bcc22f-03f0-46fd-b738-4e035921d445"))
			{
				SingularName = "Enumeration",
			};
        }
	}

    public partial class MetaObject : MetaInterface
	{
	    public static MetaObject Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Inherited Methods

		// Defined Roles

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaObject(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("12504f04-02c6-4778-98fe-04eba12ef8b2"))
			{
				SingularName = "Object",
			};
        }
	}

    public partial class MetaUniquelyIdentifiable : MetaInterface
	{
	    public static MetaUniquelyIdentifiable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType UniqueId;

		// Inherited Roles

		// Defined Associations
        public AssociationType NotificationsWhereTarget;

		// Inherited Associations

		internal MetaUniquelyIdentifiable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("122ccfe1-f902-44c1-9d6c-6f6a0afa9469"))
			{
				SingularName = "UniquelyIdentifiable",
			};
        }
	}

    public partial class MetaVersion : MetaInterface
	{
	    public static MetaVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("A6A3C79E-150B-4586-96EA-5AC0E2E638C6"))
			{
				SingularName = "Version",
			};
        }
	}

    public partial class MetaVersioned : MetaInterface
	{
	    public static MetaVersioned Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaVersioned(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("39F9CB84-B321-424A-864C-0B128ACAA965"))
			{
				SingularName = "Versioned",
			};
        }
	}

    public partial class MetaLocalised : MetaInterface
	{
	    public static MetaLocalised Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Locale;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaLocalised(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("7979a17c-0829-46df-a0d4-1b01775cfaac"))
			{
				SingularName = "Localised",
			};
        }
	}

    public partial class MetaAccessControlledObject : MetaInterface
	{
	    public static MetaAccessControlledObject Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaAccessControlledObject(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("eb0ff756-3e3d-4cf9-8935-8802a73d2df2"))
			{
				SingularName = "AccessControlledObject",
			};
        }
	}

    public partial class MetaSecurityTokenOwner : MetaInterface
	{
	    public static MetaSecurityTokenOwner Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaSecurityTokenOwner(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("a69cad9c-c2f1-463f-9af1-873ce65aeea6"))
			{
				SingularName = "SecurityTokenOwner",
			};
        }
	}

    public partial class MetaObjectState : MetaInterface
	{
	    public static MetaObjectState Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType DeniedPermissions;
        public RoleType Name;

		// Inherited Roles
        public RoleType UniqueId;

		// Defined Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaObjectState(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("f991813f-3146-4431-96d0-554aa2186887"))
			{
				SingularName = "ObjectState",
			};
        }
	}

    public partial class MetaTask : MetaInterface
	{
	    public static MetaTask Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType WorkItem;
        public RoleType DateCreated;
        public RoleType DateClosed;
        public RoleType Participants;
        public RoleType Performer;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations
        public AssociationType TaskAssignmentsWhereTask;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaTask(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("84eb0e6e-68e1-478c-a35f-6036d45792be"))
			{
				SingularName = "Task",
			};
        }
	}

    public partial class MetaTransitional : MetaInterface
	{
	    public static MetaTransitional Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaTransitional(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("ab2179ad-9eac-4b61-8d84-81cd777c4926"))
			{
				SingularName = "Transitional",
			};
        }
	}

    public partial class MetaTransitionalVersion : MetaInterface
	{
	    public static MetaTransitionalVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaTransitionalVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("A13C9057-8786-40CA-8421-476E55787D73"))
			{
				SingularName = "TransitionalVersion",
			};
        }
	}

    public partial class MetaUser : MetaInterface
	{
	    public static MetaUser Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType UserName;
        public RoleType NormalizedUserName;
        public RoleType UserPasswordHash;
        public RoleType UserEmail;
        public RoleType UserEmailConfirmed;
        public RoleType TaskList;
        public RoleType NotificationList;

		// Inherited Roles
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Locale;

		// Defined Associations
        public AssociationType SingletonWhereGuest;
        public AssociationType AccessControlsWhereSubject;
        public AssociationType AccessControlsWhereEffectiveUser;
        public AssociationType LoginsWhereUser;
        public AssociationType EmailMessagesWhereSender;
        public AssociationType EmailMessagesWhereRecipient;
        public AssociationType TaskAssignmentsWhereUser;
        public AssociationType UserGroupsWhereMember;
        public AssociationType AuditablesWhereCreatedBy;
        public AssociationType AuditablesWhereLastModifiedBy;
        public AssociationType CommunicationEventVersionsWhereCreatedBy;
        public AssociationType CommunicationEventVersionsWhereLastModifiedBy;
        public AssociationType InvoiceVersionsWhereCreatedBy;
        public AssociationType InvoiceVersionsWhereLastModifiedBy;
        public AssociationType OrderVersionsWhereCreatedBy;
        public AssociationType OrderVersionsWhereLastModifiedBy;
        public AssociationType PartyVersionsWhereCreatedBy;
        public AssociationType PartyVersionsWhereLastModifiedBy;

		// Inherited Associations

		internal MetaUser(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("a0309c3b-6f80-4777-983e-6e69800df5be"))
			{
				SingularName = "User",
			};
        }
	}

    public partial class MetaWorkItem : MetaInterface
	{
	    public static MetaWorkItem Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType WorkItemDescription;

		// Inherited Roles

		// Defined Associations
        public AssociationType TasksWhereWorkItem;

		// Inherited Associations

		internal MetaWorkItem(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("fbea29c6-6109-4163-a088-9f0b4deac896"))
			{
				SingularName = "WorkItem",
			};
        }
	}

    public partial class MetaAccountingTransaction : MetaInterface
	{
	    public static MetaAccountingTransaction Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType AccountingTransactionDetails;
        public RoleType Description;
        public RoleType TransactionDate;
        public RoleType DerivedTotalAmount;
        public RoleType AccountingTransactionNumber;
        public RoleType EntryDate;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaAccountingTransaction(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("785a36a9-4710-4f3f-bd26-dbaff5353535"))
			{
				SingularName = "AccountingTransaction",
			};
        }
	}

    public partial class MetaAgreement : MetaInterface
	{
	    public static MetaAgreement Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType AgreementDate;
        public RoleType Addenda;
        public RoleType Description;
        public RoleType AgreementTerms;
        public RoleType Text;
        public RoleType AgreementItems;
        public RoleType AgreementNumber;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;
        public RoleType FromDate;
        public RoleType ThroughDate;

		// Defined Associations
        public AssociationType EngagementsWhereAgreement;
        public AssociationType PartyVersionWhereAgreement;
        public AssociationType PartyWhereAgreement;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaAgreement(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("4deca253-7135-4ceb-b984-6adaf1515630"))
			{
				SingularName = "Agreement",
			};
        }
	}

    public partial class MetaAgreementItem : MetaInterface
	{
	    public static MetaAgreementItem Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Text;
        public RoleType Addenda;
        public RoleType Children;
        public RoleType Description;
        public RoleType AgreementTerms;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType AgreementWhereAgreementItem;
        public AssociationType AgreementItemWhereChild;

		// Inherited Associations

		internal MetaAgreementItem(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("8ba98e1b-1d4d-46b1-bf27-bb2bf53501fd"))
			{
				SingularName = "AgreementItem",
			};
        }
	}

    public partial class MetaAgreementTerm : MetaInterface
	{
	    public static MetaAgreementTerm Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType TermValue;
        public RoleType TermType;
        public RoleType Description;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType AgreementWhereAgreementTerm;
        public AssociationType AgreementItemWhereAgreementTerm;
        public AssociationType InvoiceItemVersionsWhereInvoiceTerm;
        public AssociationType InvoiceItemWhereInvoiceTerm;

		// Inherited Associations

		internal MetaAgreementTerm(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("734be1c9-e6af-49b7-8fe8-331cd7036e2e"))
			{
				SingularName = "AgreementTerm",
			};
        }
	}

    public partial class MetaAuditable : MetaInterface
	{
	    public static MetaAuditable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaAuditable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("6C726DED-C081-46D7-8DCF-F0A376943531"))
			{
				SingularName = "Auditable",
			};
        }
	}

    public partial class MetaBudget : MetaInterface
	{
	    public static MetaBudget Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType Close;
        public MethodType Reopen;

		// Inherited Methods

		// Defined Roles
        public RoleType PreviousBudgetState;
        public RoleType LastBudgetState;
        public RoleType BudgetState;
        public RoleType Description;
        public RoleType BudgetRevisions;
        public RoleType BudgetNumber;
        public RoleType BudgetReviews;
        public RoleType BudgetItems;

		// Inherited Roles
        public RoleType FromDate;
        public RoleType ThroughDate;
        public RoleType Comment;
        public RoleType UniqueId;
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaBudget(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("ebd4da8c-b86a-4317-86b9-e90a02994dcc"))
			{
				SingularName = "Budget",
			};
        }
	}

    public partial class MetaBudgetVersion : MetaInterface
	{
	    public static MetaBudgetVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType BudgetState;
        public RoleType FromDate;
        public RoleType ThroughDate;
        public RoleType Comment;
        public RoleType Description;
        public RoleType BudgetRevisions;
        public RoleType BudgetNumber;
        public RoleType BudgetReviews;
        public RoleType BudgetItems;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaBudgetVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("7FBECB76-27B6-44E3-BD08-FCBB6998B525"))
			{
				SingularName = "BudgetVersion",
			};
        }
	}

    public partial class MetaCityBound : MetaInterface
	{
	    public static MetaCityBound Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Cities;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaCityBound(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("bfdd33dc-5701-41ec-a768-f745155663d3"))
			{
				SingularName = "CityBound",
			};
        }
	}

    public partial class MetaCommentable : MetaInterface
	{
	    public static MetaCommentable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Comment;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaCommentable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("fdd52472-e863-4e91-bb01-1dada2acc8f6"))
			{
				SingularName = "Commentable",
			};
        }
	}

    public partial class MetaCommunicationAttachment : MetaInterface
	{
	    public static MetaCommunicationAttachment Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaCommunicationAttachment(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("452ae775-def1-4e75-b325-2e9184eb8c1f"))
			{
				SingularName = "CommunicationAttachment",
			};
        }
	}

    public partial class MetaCommunicationEventVersion : MetaInterface
	{
	    public static MetaCommunicationEventVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType CommunicationEventState;
        public RoleType Comment;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;
        public RoleType ScheduledStart;
        public RoleType ToParties;
        public RoleType ContactMechanisms;
        public RoleType InvolvedParties;
        public RoleType InitialScheduledStart;
        public RoleType EventPurposes;
        public RoleType ScheduledEnd;
        public RoleType ActualEnd;
        public RoleType WorkEfforts;
        public RoleType Description;
        public RoleType InitialScheduledEnd;
        public RoleType FromParties;
        public RoleType Subject;
        public RoleType Documents;
        public RoleType Case;
        public RoleType Priority;
        public RoleType Owner;
        public RoleType Note;
        public RoleType ActualStart;
        public RoleType SendNotification;
        public RoleType SendReminder;
        public RoleType RemindAt;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaCommunicationEventVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("8407B4DF-88BF-43E5-89DA-999A32B16CF5"))
			{
				SingularName = "CommunicationEventVersion",
			};
        }
	}

    public partial class MetaCommunicationEvent : MetaInterface
	{
	    public static MetaCommunicationEvent Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType Cancel;
        public MethodType Close;
        public MethodType Reopen;

		// Inherited Methods

		// Defined Roles
        public RoleType PreviousCommunicationEventState;
        public RoleType LastCommunicationEventState;
        public RoleType CommunicationEventState;
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;
        public RoleType ScheduledStart;
        public RoleType ToParties;
        public RoleType ContactMechanisms;
        public RoleType InvolvedParties;
        public RoleType InitialScheduledStart;
        public RoleType EventPurposes;
        public RoleType ScheduledEnd;
        public RoleType ActualEnd;
        public RoleType WorkEfforts;
        public RoleType Description;
        public RoleType InitialScheduledEnd;
        public RoleType FromParties;
        public RoleType Subject;
        public RoleType Documents;
        public RoleType Case;
        public RoleType Priority;
        public RoleType Owner;
        public RoleType Note;
        public RoleType ActualStart;
        public RoleType SendNotification;
        public RoleType SendReminder;
        public RoleType RemindAt;

		// Inherited Roles
        public RoleType Comment;
        public RoleType UniqueId;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType RequirementCommunicationsWhereCommunicationEvent;
        public AssociationType PartyVersionsWhereCommunicationEvent;
        public AssociationType PartiesWhereCommunicationEvent;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaCommunicationEvent(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("b05371ff-0c9e-4ee3-b31d-e2edeed8649e"))
			{
				SingularName = "CommunicationEvent",
			};
        }
	}

    public partial class MetaContactMechanism : MetaInterface
	{
	    public static MetaContactMechanism Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Description;
        public RoleType FollowTo;
        public RoleType ContactMechanismType;

		// Inherited Roles
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType BankAccountWhereContactMechanism;
        public AssociationType BillingAccountsWhereContactMechanism;
        public AssociationType EngagementsWherePlacingContactMechanism;
        public AssociationType EngagementsWhereBillToContactMechanism;
        public AssociationType EngagementsWhereTakenViaContactMechanism;
        public AssociationType FacilitiesWhereFacilityContactMechanism;
        public AssociationType InternalOrganisationsWhereBillingAddress;
        public AssociationType InternalOrganisationsWhereOrderAddress;
        public AssociationType InternalOrganisationsWhereBillingInquiriesFax;
        public AssociationType InternalOrganisationsWhereBillingInquiriesPhone;
        public AssociationType InternalOrganisationsWhereCellPhoneNumber;
        public AssociationType InternalOrganisationsWhereGeneralFaxNumber;
        public AssociationType InternalOrganisationsWhereGeneralPhoneNumber;
        public AssociationType InternalOrganisationsWhereHeadQuarter;
        public AssociationType InternalOrganisationsWhereInternetAddress;
        public AssociationType InternalOrganisationsWhereOrderInquiriesFax;
        public AssociationType InternalOrganisationsWhereOrderInquiriesPhone;
        public AssociationType InternalOrganisationsWhereGeneralEmailAddress;
        public AssociationType InternalOrganisationsWhereSalesOffice;
        public AssociationType InternalOrganisationsWhereShippingInquiriesFax;
        public AssociationType InternalOrganisationsWhereShippingInquiriesPhone;
        public AssociationType PurchaseOrderVersionsWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBilledFromContactMechanism;
        public AssociationType SalesOrderVersionsWhereTakenByContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillFromContactMechanism;
        public AssociationType SalesOrderVersionsWherePlacingContactMechanism;
        public AssociationType PartyContactMechanismsWhereContactMechanism;
        public AssociationType PurchaseOrdersWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrdersWhereBillToContactMechanism;
        public AssociationType RespondingPartiesWhereContactMechanism;
        public AssociationType SalesInvoicesWhereBillToContactMechanism;
        public AssociationType SalesInvoicesWhereBilledFromContactMechanism;
        public AssociationType SalesOrdersWhereTakenByContactMechanism;
        public AssociationType SalesOrdersWhereBillToContactMechanism;
        public AssociationType SalesOrdersWhereBillFromContactMechanism;
        public AssociationType SalesOrdersWherePlacingContactMechanism;
        public AssociationType CommunicationEventVersionsWhereContactMechanism;
        public AssociationType CommunicationEventsWhereContactMechanism;
        public AssociationType ContactMechanismsWhereFollowTo;
        public AssociationType PartyVersionsWhereHomeAddress;
        public AssociationType PartyVersionsWhereSalesOffice;
        public AssociationType PartyVersionsWhereBillingAddress;
        public AssociationType PartyVersionsWhereHeadQuarter;
        public AssociationType PartyVersionsWhereOrderAddress;
        public AssociationType QuoteVersionsWhereFullfillContactMechanism;
        public AssociationType RequestVersionsWhereFullfillContactMechanism;
        public AssociationType PartiesWhereHomeAddress;
        public AssociationType PartiesWhereSalesOffice;
        public AssociationType PartiesWhereBillingAddress;
        public AssociationType PartiesWhereHeadQuarter;
        public AssociationType PartiesWhereOrderAddress;
        public AssociationType QuotesWhereFullfillContactMechanism;
        public AssociationType RequestsWhereFullfillContactMechanism;
        public AssociationType ShipmentVersionsWhereBillToContactMechanism;
        public AssociationType ShipmentVersionsWhereReceiverContactMechanism;
        public AssociationType ShipmentVersionsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentVersionsWhereBillFromContactMechanism;
        public AssociationType ShipmentsWhereBillToContactMechanism;
        public AssociationType ShipmentsWhereReceiverContactMechanism;
        public AssociationType ShipmentsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentsWhereBillFromContactMechanism;

		// Inherited Associations

		internal MetaContactMechanism(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("b033f9c9-c799-485c-a199-914a9e9119d9"))
			{
				SingularName = "ContactMechanism",
			};
        }
	}

    public partial class MetaCountryBound : MetaInterface
	{
	    public static MetaCountryBound Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Country;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaCountryBound(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("eaebcfe7-0d65-43ab-857c-b171086a1982"))
			{
				SingularName = "CountryBound",
			};
        }
	}

    public partial class MetaDeploymentUsage : MetaInterface
	{
	    public static MetaDeploymentUsage Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType TimeFrequency;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Comment;
        public RoleType FromDate;
        public RoleType ThroughDate;

		// Defined Associations
        public AssociationType DeploymentsWhereDeploymentUsage;

		// Inherited Associations

		internal MetaDeploymentUsage(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("ca0f0654-3974-4e5e-a57e-593216c05e16"))
			{
				SingularName = "DeploymentUsage",
			};
        }
	}

    public partial class MetaDocument : MetaInterface
	{
	    public static MetaDocument Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Name;
        public RoleType Description;
        public RoleType Text;
        public RoleType DocumentLocation;

		// Inherited Roles
        public RoleType PrintContent;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Comment;

		// Defined Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;

		// Inherited Associations

		internal MetaDocument(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("1d21adf0-6008-459d-9f6a-3a026e7640bc"))
			{
				SingularName = "Document",
			};
        }
	}

    public partial class MetaElectronicAddress : MetaInterface
	{
	    public static MetaElectronicAddress Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType ElectronicAddressString;

		// Inherited Roles
        public RoleType Description;
        public RoleType FollowTo;
        public RoleType ContactMechanismType;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType PartyVersionsWhereGeneralEmail;
        public AssociationType PartyVersionWherePersonalEmailAddress;
        public AssociationType PartyVersionsWhereInternetAddress;
        public AssociationType PartiesWhereInternetAddress;

		// Inherited Associations
        public AssociationType BankAccountWhereContactMechanism;
        public AssociationType BillingAccountsWhereContactMechanism;
        public AssociationType EngagementsWherePlacingContactMechanism;
        public AssociationType EngagementsWhereBillToContactMechanism;
        public AssociationType EngagementsWhereTakenViaContactMechanism;
        public AssociationType FacilitiesWhereFacilityContactMechanism;
        public AssociationType InternalOrganisationsWhereBillingAddress;
        public AssociationType InternalOrganisationsWhereOrderAddress;
        public AssociationType InternalOrganisationsWhereBillingInquiriesFax;
        public AssociationType InternalOrganisationsWhereBillingInquiriesPhone;
        public AssociationType InternalOrganisationsWhereCellPhoneNumber;
        public AssociationType InternalOrganisationsWhereGeneralFaxNumber;
        public AssociationType InternalOrganisationsWhereGeneralPhoneNumber;
        public AssociationType InternalOrganisationsWhereHeadQuarter;
        public AssociationType InternalOrganisationsWhereInternetAddress;
        public AssociationType InternalOrganisationsWhereOrderInquiriesFax;
        public AssociationType InternalOrganisationsWhereOrderInquiriesPhone;
        public AssociationType InternalOrganisationsWhereGeneralEmailAddress;
        public AssociationType InternalOrganisationsWhereSalesOffice;
        public AssociationType InternalOrganisationsWhereShippingInquiriesFax;
        public AssociationType InternalOrganisationsWhereShippingInquiriesPhone;
        public AssociationType PurchaseOrderVersionsWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBilledFromContactMechanism;
        public AssociationType SalesOrderVersionsWhereTakenByContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillFromContactMechanism;
        public AssociationType SalesOrderVersionsWherePlacingContactMechanism;
        public AssociationType PartyContactMechanismsWhereContactMechanism;
        public AssociationType PurchaseOrdersWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrdersWhereBillToContactMechanism;
        public AssociationType RespondingPartiesWhereContactMechanism;
        public AssociationType SalesInvoicesWhereBillToContactMechanism;
        public AssociationType SalesInvoicesWhereBilledFromContactMechanism;
        public AssociationType SalesOrdersWhereTakenByContactMechanism;
        public AssociationType SalesOrdersWhereBillToContactMechanism;
        public AssociationType SalesOrdersWhereBillFromContactMechanism;
        public AssociationType SalesOrdersWherePlacingContactMechanism;
        public AssociationType CommunicationEventVersionsWhereContactMechanism;
        public AssociationType CommunicationEventsWhereContactMechanism;
        public AssociationType ContactMechanismsWhereFollowTo;
        public AssociationType PartyVersionsWhereHomeAddress;
        public AssociationType PartyVersionsWhereSalesOffice;
        public AssociationType PartyVersionsWhereBillingAddress;
        public AssociationType PartyVersionsWhereHeadQuarter;
        public AssociationType PartyVersionsWhereOrderAddress;
        public AssociationType QuoteVersionsWhereFullfillContactMechanism;
        public AssociationType RequestVersionsWhereFullfillContactMechanism;
        public AssociationType PartiesWhereHomeAddress;
        public AssociationType PartiesWhereSalesOffice;
        public AssociationType PartiesWhereBillingAddress;
        public AssociationType PartiesWhereHeadQuarter;
        public AssociationType PartiesWhereOrderAddress;
        public AssociationType QuotesWhereFullfillContactMechanism;
        public AssociationType RequestsWhereFullfillContactMechanism;
        public AssociationType ShipmentVersionsWhereBillToContactMechanism;
        public AssociationType ShipmentVersionsWhereReceiverContactMechanism;
        public AssociationType ShipmentVersionsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentVersionsWhereBillFromContactMechanism;
        public AssociationType ShipmentsWhereBillToContactMechanism;
        public AssociationType ShipmentsWhereReceiverContactMechanism;
        public AssociationType ShipmentsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentsWhereBillFromContactMechanism;

		internal MetaElectronicAddress(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("5cd86f69-e09b-4150-a2a6-2eed4c72b426"))
			{
				SingularName = "ElectronicAddress",
			};
        }
	}

    public partial class MetaEngagementItem : MetaInterface
	{
	    public static MetaEngagementItem Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType QuoteItem;
        public RoleType Description;
        public RoleType ExpectedStartDate;
        public RoleType ExpectedEndDate;
        public RoleType EngagementWorkFulfillment;
        public RoleType EngagementRates;
        public RoleType CurrentEngagementRate;
        public RoleType OrderedWiths;
        public RoleType CurrentAssignedProfessional;
        public RoleType Product;
        public RoleType ProductFeature;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType EngagementWhereEngagementItem;
        public AssociationType ProfessionalAssignmentsWhereEngagementItem;
        public AssociationType EngagementItemWhereOrderedWith;
        public AssociationType ServiceEntriesWhereEngagementItem;

		// Inherited Associations

		internal MetaEngagementItem(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("aa3bf631-5aa5-48ab-a249-ef61f640fb72"))
			{
				SingularName = "EngagementItem",
			};
        }
	}

    public partial class MetaEstimatedProductCost : MetaInterface
	{
	    public static MetaEstimatedProductCost Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Cost;
        public RoleType Currency;
        public RoleType Organisation;
        public RoleType Description;
        public RoleType GeographicBoundary;

		// Inherited Roles
        public RoleType FromDate;
        public RoleType ThroughDate;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType ProductWhereEstimatedProductCost;
        public AssociationType ProductFeatureWhereEstimatedProductCost;

		// Inherited Associations

		internal MetaEstimatedProductCost(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("c8df7ac5-4e6f-4add-981f-f0d9a8c14e24"))
			{
				SingularName = "EstimatedProductCost",
			};
        }
	}

    public partial class MetaExternalAccountingTransaction : MetaInterface
	{
	    public static MetaExternalAccountingTransaction Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType FromParty;
        public RoleType ToParty;

		// Inherited Roles
        public RoleType AccountingTransactionDetails;
        public RoleType Description;
        public RoleType TransactionDate;
        public RoleType DerivedTotalAmount;
        public RoleType AccountingTransactionNumber;
        public RoleType EntryDate;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaExternalAccountingTransaction(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("6bfa631c-80f4-495f-bb9a-0d3351390d64"))
			{
				SingularName = "ExternalAccountingTransaction",
			};
        }
	}

    public partial class MetaFinancialAccount : MetaInterface
	{
	    public static MetaFinancialAccount Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType FinancialAccountTransactions;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaFinancialAccount(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("27b45d45-459a-43cb-87b0-f8842ec56445"))
			{
				SingularName = "FinancialAccount",
			};
        }
	}

    public partial class MetaFinancialAccountTransaction : MetaInterface
	{
	    public static MetaFinancialAccountTransaction Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Description;
        public RoleType EntryDate;
        public RoleType TransactionDate;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType FinancialAccountWhereFinancialAccountTransaction;

		// Inherited Associations

		internal MetaFinancialAccountTransaction(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("5500cb42-1aae-4816-9bc1-d63ff273f144"))
			{
				SingularName = "FinancialAccountTransaction",
			};
        }
	}

    public partial class MetaFixedAsset : MetaInterface
	{
	    public static MetaFixedAsset Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Name;
        public RoleType LastServiceDate;
        public RoleType AcquiredDate;
        public RoleType Description;
        public RoleType ProductionCapacity;
        public RoleType NextServiceDate;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType DepreciationsWhereFixedAsset;
        public AssociationType PartyFixedAssetAssignmentsWhereFixedAsset;
        public AssociationType WorkEffortFixedAssetAssignmentsWhereFixedAsset;
        public AssociationType WorkEffortFixedAssetStandardsWhereFixedAsset;
        public AssociationType WorkEffortTypesWhereFixedAssetToRepair;

		// Inherited Associations

		internal MetaFixedAsset(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("4a3efb9c-1556-4e57-bb59-f09d297e607e"))
			{
				SingularName = "FixedAsset",
			};
        }
	}

    public partial class MetaGeographicBoundary : MetaInterface
	{
	    public static MetaGeographicBoundary Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Abbreviation;

		// Inherited Roles
        public RoleType Latitude;
        public RoleType Longitude;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaGeographicBoundary(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("3453c2e1-77a4-4fe8-b663-02bac689883a"))
			{
				SingularName = "GeographicBoundary",
			};
        }
	}

    public partial class MetaGeographicBoundaryComposite : MetaInterface
	{
	    public static MetaGeographicBoundaryComposite Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Associations;

		// Inherited Roles
        public RoleType Abbreviation;
        public RoleType Latitude;
        public RoleType Longitude;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;

		internal MetaGeographicBoundaryComposite(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("3b7ac95a-fdab-488d-b599-17ef9fcf33b0"))
			{
				SingularName = "GeographicBoundaryComposite",
			};
        }
	}

    public partial class MetaGeoLocatable : MetaInterface
	{
	    public static MetaGeoLocatable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Latitude;
        public RoleType Longitude;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaGeoLocatable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("93960be2-f676-4e7f-9efb-f99c92303059"))
			{
				SingularName = "GeoLocatable",
			};
        }
	}

    public partial class MetaInternalAccountingTransaction : MetaInterface
	{
	    public static MetaInternalAccountingTransaction Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles
        public RoleType AccountingTransactionDetails;
        public RoleType Description;
        public RoleType TransactionDate;
        public RoleType DerivedTotalAmount;
        public RoleType AccountingTransactionNumber;
        public RoleType EntryDate;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaInternalAccountingTransaction(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("5a783d98-845a-4784-9c92-5c75a4af3fb8"))
			{
				SingularName = "InternalAccountingTransaction",
			};
        }
	}

    public partial class MetaInventoryItemVersion : MetaInterface
	{
	    public static MetaInventoryItemVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType ProductCharacteristicValues;
        public RoleType InventoryItemVariances;
        public RoleType Part;
        public RoleType Name;
        public RoleType Lot;
        public RoleType Sku;
        public RoleType UnitOfMeasure;
        public RoleType DerivedProductCategories;
        public RoleType Good;
        public RoleType ProductType;
        public RoleType Facility;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaInventoryItemVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("F3D6AC19-E987-4C01-B582-A4567B7818A9"))
			{
				SingularName = "InventoryItemVersion",
			};
        }
	}

    public partial class MetaInventoryItem : MetaInterface
	{
	    public static MetaInventoryItem Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType ProductCharacteristicValues;
        public RoleType InventoryItemVariances;
        public RoleType Part;
        public RoleType Name;
        public RoleType Lot;
        public RoleType Sku;
        public RoleType UnitOfMeasure;
        public RoleType DerivedProductCategories;
        public RoleType Good;
        public RoleType ProductType;
        public RoleType Facility;

		// Inherited Roles
        public RoleType UniqueId;
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType ItemIssuancesWhereInventoryItem;
        public AssociationType PickListItemsWhereInventoryItem;
        public AssociationType ShipmentItemsWhereInventoryItem;
        public AssociationType WorkEffortInventoryAssignmentsWhereInventoryItem;
        public AssociationType InventoryItemConfigurationsWhereInventoryItem;
        public AssociationType InventoryItemConfigurationsWhereComponentInventoryItem;
        public AssociationType WorkEffortVersionsWhereInventoryItemsProduced;
        public AssociationType WorkEffortsWhereInventoryItemsProduced;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaInventoryItem(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("61af6d19-e8e4-4b5b-97e8-3610fbc82605"))
			{
				SingularName = "InventoryItem",
			};
        }
	}

    public partial class MetaInventoryItemConfiguration : MetaInterface
	{
	    public static MetaInventoryItemConfiguration Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType InventoryItem;
        public RoleType Quantity;
        public RoleType ComponentInventoryItem;

		// Inherited Roles
        public RoleType Comment;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaInventoryItemConfiguration(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("f135770b-7228-4e4b-b7ea-9307b6317fd2"))
			{
				SingularName = "InventoryItemConfiguration",
			};
        }
	}

    public partial class MetaInvoiceItemVersion : MetaInterface
	{
	    public static MetaInvoiceItemVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType InternalComment;
        public RoleType InvoiceTerms;
        public RoleType TotalInvoiceAdjustment;
        public RoleType InvoiceVatRateItems;
        public RoleType AdjustmentFor;
        public RoleType SerializedInventoryItem;
        public RoleType Message;
        public RoleType TotalInvoiceAdjustmentCustomerCurrency;
        public RoleType AmountPaid;
        public RoleType Quantity;
        public RoleType Description;

		// Inherited Roles
        public RoleType TotalDiscountAsPercentage;
        public RoleType DiscountAdjustment;
        public RoleType UnitVat;
        public RoleType TotalVatCustomerCurrency;
        public RoleType VatRegime;
        public RoleType TotalVat;
        public RoleType UnitSurcharge;
        public RoleType UnitDiscount;
        public RoleType TotalExVatCustomerCurrency;
        public RoleType DerivedVatRate;
        public RoleType ActualUnitPrice;
        public RoleType TotalIncVatCustomerCurrency;
        public RoleType UnitBasePrice;
        public RoleType CalculatedUnitPrice;
        public RoleType TotalSurchargeCustomerCurrency;
        public RoleType TotalIncVat;
        public RoleType TotalSurchargeAsPercentage;
        public RoleType TotalDiscountCustomerCurrency;
        public RoleType TotalDiscount;
        public RoleType TotalSurcharge;
        public RoleType AssignedVatRegime;
        public RoleType TotalBasePrice;
        public RoleType TotalExVat;
        public RoleType TotalBasePriceCustomerCurrency;
        public RoleType CurrentPriceComponents;
        public RoleType SurchargeAdjustment;
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaInvoiceItemVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("D8B0BB9D-BD15-426C-A4A4-6AEEFF2FBDB2"))
			{
				SingularName = "InvoiceItemVersion",
			};
        }
	}

    public partial class MetaInvoiceVersion : MetaInterface
	{
	    public static MetaInvoiceVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Comment;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType InternalComment;
        public RoleType TotalShippingAndHandlingCustomerCurrency;
        public RoleType CustomerCurrency;
        public RoleType Description;
        public RoleType ShippingAndHandlingCharge;
        public RoleType TotalFeeCustomerCurrency;
        public RoleType Fee;
        public RoleType TotalExVatCustomerCurrency;
        public RoleType CustomerReference;
        public RoleType DiscountAdjustment;
        public RoleType AmountPaid;
        public RoleType TotalDiscount;
        public RoleType BillingAccount;
        public RoleType TotalIncVat;
        public RoleType TotalSurcharge;
        public RoleType TotalBasePrice;
        public RoleType TotalVatCustomerCurrency;
        public RoleType InvoiceDate;
        public RoleType EntryDate;
        public RoleType TotalIncVatCustomerCurrency;
        public RoleType TotalShippingAndHandling;
        public RoleType TotalBasePriceCustomerCurrency;
        public RoleType SurchargeAdjustment;
        public RoleType TotalExVat;
        public RoleType InvoiceTerms;
        public RoleType TotalSurchargeCustomerCurrency;
        public RoleType InvoiceNumber;
        public RoleType Message;
        public RoleType VatRegime;
        public RoleType TotalDiscountCustomerCurrency;
        public RoleType TotalVat;
        public RoleType TotalFee;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaInvoiceVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("683DF770-70BE-4E31-830A-B9DD9031030D"))
			{
				SingularName = "InvoiceVersion",
			};
        }
	}

    public partial class MetaInvoice : MetaInterface
	{
	    public static MetaInvoice Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType InternalComment;
        public RoleType TotalShippingAndHandlingCustomerCurrency;
        public RoleType CustomerCurrency;
        public RoleType Description;
        public RoleType ShippingAndHandlingCharge;
        public RoleType TotalFeeCustomerCurrency;
        public RoleType Fee;
        public RoleType TotalExVatCustomerCurrency;
        public RoleType CustomerReference;
        public RoleType DiscountAdjustment;
        public RoleType AmountPaid;
        public RoleType TotalDiscount;
        public RoleType BillingAccount;
        public RoleType TotalIncVat;
        public RoleType TotalSurcharge;
        public RoleType TotalBasePrice;
        public RoleType TotalVatCustomerCurrency;
        public RoleType InvoiceDate;
        public RoleType EntryDate;
        public RoleType TotalIncVatCustomerCurrency;
        public RoleType TotalShippingAndHandling;
        public RoleType TotalBasePriceCustomerCurrency;
        public RoleType SurchargeAdjustment;
        public RoleType TotalExVat;
        public RoleType InvoiceTerms;
        public RoleType TotalSurchargeCustomerCurrency;
        public RoleType InvoiceNumber;
        public RoleType Message;
        public RoleType VatRegime;
        public RoleType TotalDiscountCustomerCurrency;
        public RoleType TotalVat;
        public RoleType TotalFee;
        public RoleType ContactPerson;

		// Inherited Roles
        public RoleType Locale;
        public RoleType Comment;
        public RoleType PrintContent;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType PaymentApplicationsWhereInvoice;
        public AssociationType SalesAccountingTransactionWhereInvoice;

		// Inherited Associations

		internal MetaInvoice(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("a6f4eedb-b0b5-491d-bcc0-09d2bc109e86"))
			{
				SingularName = "Invoice",
			};
        }
	}

    public partial class MetaInvoiceItem : MetaInterface
	{
	    public static MetaInvoiceItem Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType InternalComment;
        public RoleType InvoiceTerms;
        public RoleType TotalInvoiceAdjustment;
        public RoleType InvoiceVatRateItems;
        public RoleType AdjustmentFor;
        public RoleType SerializedInventoryItem;
        public RoleType Message;
        public RoleType TotalInvoiceAdjustmentCustomerCurrency;
        public RoleType AmountPaid;
        public RoleType Quantity;
        public RoleType Description;

		// Inherited Roles
        public RoleType TotalDiscountAsPercentage;
        public RoleType DiscountAdjustment;
        public RoleType UnitVat;
        public RoleType TotalVatCustomerCurrency;
        public RoleType VatRegime;
        public RoleType TotalVat;
        public RoleType UnitSurcharge;
        public RoleType UnitDiscount;
        public RoleType TotalExVatCustomerCurrency;
        public RoleType DerivedVatRate;
        public RoleType ActualUnitPrice;
        public RoleType TotalIncVatCustomerCurrency;
        public RoleType UnitBasePrice;
        public RoleType CalculatedUnitPrice;
        public RoleType TotalSurchargeCustomerCurrency;
        public RoleType TotalIncVat;
        public RoleType TotalSurchargeAsPercentage;
        public RoleType TotalDiscountCustomerCurrency;
        public RoleType TotalDiscount;
        public RoleType TotalSurcharge;
        public RoleType AssignedVatRegime;
        public RoleType TotalBasePrice;
        public RoleType TotalExVat;
        public RoleType TotalBasePriceCustomerCurrency;
        public RoleType CurrentPriceComponents;
        public RoleType SurchargeAdjustment;
        public RoleType Comment;
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType PaymentApplicationsWhereInvoiceItem;
        public AssociationType ServiceEntryBillingsWhereInvoiceItem;
        public AssociationType ShipmentItemWhereInvoiceItem;
        public AssociationType WorkEffortBillingsWhereInvoiceItem;
        public AssociationType InvoiceItemVersionsWhereAdjustmentFor;
        public AssociationType InvoiceItemsWhereAdjustmentFor;

		// Inherited Associations

		internal MetaInvoiceItem(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("d79f734d-4434-4710-a7ea-7d6306f3064f"))
			{
				SingularName = "InvoiceItem",
			};
        }
	}

    public partial class MetaIUnitOfMeasure : MetaInterface
	{
	    public static MetaIUnitOfMeasure Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Description;
        public RoleType UnitOfMeasureConversions;
        public RoleType Abbreviation;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations
        public AssociationType UnitOfMeasureConversionsWhereToUnitOfMeasure;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaIUnitOfMeasure(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("b7215af5-97d6-42b0-9f6f-c1fccb2bc695"))
			{
				SingularName = "IUnitOfMeasure",
			};
        }
	}

    public partial class MetaOrderAdjustmentVersion : MetaInterface
	{
	    public static MetaOrderAdjustmentVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Amount;
        public RoleType VatRate;
        public RoleType Percentage;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaOrderAdjustmentVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("A5627D2B-3E75-41B7-86BB-642C12714471"))
			{
				SingularName = "OrderAdjustmentVersion",
			};
        }
	}

    public partial class MetaOrderItemVersion : MetaInterface
	{
	    public static MetaOrderItemVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType InternalComment;
        public RoleType BudgetItem;
        public RoleType PreviousQuantity;
        public RoleType QuantityOrdered;
        public RoleType Description;
        public RoleType CorrespondingPurchaseOrder;
        public RoleType TotalOrderAdjustmentCustomerCurrency;
        public RoleType TotalOrderAdjustment;
        public RoleType QuoteItem;
        public RoleType AssignedDeliveryDate;
        public RoleType DeliveryDate;
        public RoleType OrderTerms;
        public RoleType ShippingInstruction;
        public RoleType Associations;
        public RoleType Message;

		// Inherited Roles
        public RoleType TotalDiscountAsPercentage;
        public RoleType DiscountAdjustment;
        public RoleType UnitVat;
        public RoleType TotalVatCustomerCurrency;
        public RoleType VatRegime;
        public RoleType TotalVat;
        public RoleType UnitSurcharge;
        public RoleType UnitDiscount;
        public RoleType TotalExVatCustomerCurrency;
        public RoleType DerivedVatRate;
        public RoleType ActualUnitPrice;
        public RoleType TotalIncVatCustomerCurrency;
        public RoleType UnitBasePrice;
        public RoleType CalculatedUnitPrice;
        public RoleType TotalSurchargeCustomerCurrency;
        public RoleType TotalIncVat;
        public RoleType TotalSurchargeAsPercentage;
        public RoleType TotalDiscountCustomerCurrency;
        public RoleType TotalDiscount;
        public RoleType TotalSurcharge;
        public RoleType AssignedVatRegime;
        public RoleType TotalBasePrice;
        public RoleType TotalExVat;
        public RoleType TotalBasePriceCustomerCurrency;
        public RoleType CurrentPriceComponents;
        public RoleType SurchargeAdjustment;
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaOrderItemVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("304981F1-CCF3-4946-86E0-5DE1F929BA08"))
			{
				SingularName = "OrderItemVersion",
			};
        }
	}

    public partial class MetaOrderVersion : MetaInterface
	{
	    public static MetaOrderVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Comment;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType InternalComment;
        public RoleType CustomerCurrency;
        public RoleType TotalBasePriceCustomerCurrency;
        public RoleType TotalIncVatCustomerCurrency;
        public RoleType TotalDiscountCustomerCurrency;
        public RoleType CustomerReference;
        public RoleType Fee;
        public RoleType TotalExVat;
        public RoleType OrderTerms;
        public RoleType TotalVat;
        public RoleType TotalSurcharge;
        public RoleType ValidOrderItems;
        public RoleType OrderNumber;
        public RoleType TotalVatCustomerCurrency;
        public RoleType TotalDiscount;
        public RoleType Message;
        public RoleType TotalShippingAndHandlingCustomerCurrency;
        public RoleType EntryDate;
        public RoleType DiscountAdjustment;
        public RoleType OrderKind;
        public RoleType TotalIncVat;
        public RoleType TotalSurchargeCustomerCurrency;
        public RoleType VatRegime;
        public RoleType TotalFeeCustomerCurrency;
        public RoleType TotalShippingAndHandling;
        public RoleType ShippingAndHandlingCharge;
        public RoleType OrderDate;
        public RoleType TotalExVatCustomerCurrency;
        public RoleType DeliveryDate;
        public RoleType TotalBasePrice;
        public RoleType TotalFee;
        public RoleType SurchargeAdjustment;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaOrderVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("2D3761C6-CF53-4CA8-9392-99ED6B01EDE1"))
			{
				SingularName = "OrderVersion",
			};
        }
	}

    public partial class MetaPartyVersion : MetaInterface
	{
	    public static MetaPartyVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Comment;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType PartyName;
        public RoleType GeneralCorrespondence;
        public RoleType YTDRevenue;
        public RoleType LastYearsRevenue;
        public RoleType BillingInquiriesFax;
        public RoleType Qualifications;
        public RoleType HomeAddress;
        public RoleType InactiveOrganisationContactRelationships;
        public RoleType SalesOffice;
        public RoleType InactiveContacts;
        public RoleType InactivePartyContactMechanisms;
        public RoleType OrderInquiriesFax;
        public RoleType CurrentSalesReps;
        public RoleType PartyContactMechanisms;
        public RoleType ShippingInquiriesFax;
        public RoleType ShippingInquiriesPhone;
        public RoleType BillingAccounts;
        public RoleType OrderInquiriesPhone;
        public RoleType PartySkills;
        public RoleType PartyClassifications;
        public RoleType ExcludeFromDunning;
        public RoleType BankAccounts;
        public RoleType CurrentContacts;
        public RoleType BillingAddress;
        public RoleType GeneralEmail;
        public RoleType DefaultShipmentMethod;
        public RoleType Resumes;
        public RoleType HeadQuarter;
        public RoleType PersonalEmailAddress;
        public RoleType CellPhoneNumber;
        public RoleType BillingInquiriesPhone;
        public RoleType OrderAddress;
        public RoleType InternetAddress;
        public RoleType Contents;
        public RoleType CreditCards;
        public RoleType ShippingAddress;
        public RoleType CurrentOrganisationContactRelationships;
        public RoleType OpenOrderAmount;
        public RoleType GeneralFaxNumber;
        public RoleType DefaultPaymentMethod;
        public RoleType CurrentPartyContactMechanisms;
        public RoleType GeneralPhoneNumber;
        public RoleType PreferredCurrency;
        public RoleType VatRegime;
        public RoleType SimpleMovingAverage;
        public RoleType AmountOverDue;
        public RoleType DunningType;
        public RoleType AmountDue;
        public RoleType LastReminderDate;
        public RoleType CreditLimit;
        public RoleType SubAccountNumber;
        public RoleType BlockedForDunning;
        public RoleType Agreements;
        public RoleType CommunicationEvents;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaPartyVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("FA4FC65D-8B26-434D-95C9-06991EAA0B57"))
			{
				SingularName = "PartyVersion",
			};
        }
	}

    public partial class MetaPartyRelationship : MetaInterface
	{
	    public static MetaPartyRelationship Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Parties;

		// Inherited Roles
        public RoleType FromDate;
        public RoleType ThroughDate;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaPartyRelationship(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("084abb92-31fd-46e6-ab85-9a7a88c9d72b"))
			{
				SingularName = "PartyRelationship",
			};
        }
	}

    public partial class MetaPriceableVersion : MetaInterface
	{
	    public static MetaPriceableVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType TotalDiscountAsPercentage;
        public RoleType DiscountAdjustment;
        public RoleType UnitVat;
        public RoleType TotalVatCustomerCurrency;
        public RoleType VatRegime;
        public RoleType TotalVat;
        public RoleType UnitSurcharge;
        public RoleType UnitDiscount;
        public RoleType TotalExVatCustomerCurrency;
        public RoleType DerivedVatRate;
        public RoleType ActualUnitPrice;
        public RoleType TotalIncVatCustomerCurrency;
        public RoleType UnitBasePrice;
        public RoleType CalculatedUnitPrice;
        public RoleType TotalSurchargeCustomerCurrency;
        public RoleType TotalIncVat;
        public RoleType TotalSurchargeAsPercentage;
        public RoleType TotalDiscountCustomerCurrency;
        public RoleType TotalDiscount;
        public RoleType TotalSurcharge;
        public RoleType AssignedVatRegime;
        public RoleType TotalBasePrice;
        public RoleType TotalExVat;
        public RoleType TotalBasePriceCustomerCurrency;
        public RoleType CurrentPriceComponents;
        public RoleType SurchargeAdjustment;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaPriceableVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("7CFD2B40-4C45-489B-9F92-83E7E1641F19"))
			{
				SingularName = "PriceableVersion",
			};
        }
	}

    public partial class MetaQuoteVersion : MetaInterface
	{
	    public static MetaQuoteVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType QuoteState;
        public RoleType InternalComment;
        public RoleType RequiredResponseDate;
        public RoleType ValidFromDate;
        public RoleType QuoteTerms;
        public RoleType ValidThroughDate;
        public RoleType Description;
        public RoleType Receiver;
        public RoleType FullfillContactMechanism;
        public RoleType Price;
        public RoleType Currency;
        public RoleType IssueDate;
        public RoleType QuoteItems;
        public RoleType QuoteNumber;
        public RoleType Request;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaQuoteVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("4C239D5F-85BF-4B8B-A2E6-609A2FE672B8"))
			{
				SingularName = "QuoteVersion",
			};
        }
	}

    public partial class MetaRequestVersion : MetaInterface
	{
	    public static MetaRequestVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType RequestState;
        public RoleType InternalComment;
        public RoleType Description;
        public RoleType RequestDate;
        public RoleType RequiredResponseDate;
        public RoleType RequestItems;
        public RoleType RequestNumber;
        public RoleType RespondingParties;
        public RoleType Originator;
        public RoleType Currency;
        public RoleType FullfillContactMechanism;
        public RoleType EmailAddress;
        public RoleType TelephoneNumber;
        public RoleType TelephoneCountryCode;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaRequestVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("CB830374-2F89-4911-9A33-98CE902741A8"))
			{
				SingularName = "RequestVersion",
			};
        }
	}

    public partial class MetaOrder : MetaInterface
	{
	    public static MetaOrder Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType Approve;
        public MethodType Reject;
        public MethodType Hold;
        public MethodType Continue;
        public MethodType Confirm;
        public MethodType Cancel;
        public MethodType Complete;

		// Inherited Methods

		// Defined Roles
        public RoleType InternalComment;
        public RoleType CustomerCurrency;
        public RoleType TotalBasePriceCustomerCurrency;
        public RoleType TotalIncVatCustomerCurrency;
        public RoleType TotalDiscountCustomerCurrency;
        public RoleType CustomerReference;
        public RoleType Fee;
        public RoleType TotalExVat;
        public RoleType OrderTerms;
        public RoleType TotalVat;
        public RoleType TotalSurcharge;
        public RoleType ValidOrderItems;
        public RoleType OrderNumber;
        public RoleType TotalVatCustomerCurrency;
        public RoleType TotalDiscount;
        public RoleType Message;
        public RoleType Description;
        public RoleType TotalShippingAndHandlingCustomerCurrency;
        public RoleType EntryDate;
        public RoleType DiscountAdjustment;
        public RoleType OrderKind;
        public RoleType TotalIncVat;
        public RoleType TotalSurchargeCustomerCurrency;
        public RoleType VatRegime;
        public RoleType TotalFeeCustomerCurrency;
        public RoleType TotalShippingAndHandling;
        public RoleType ShippingAndHandlingCharge;
        public RoleType OrderDate;
        public RoleType TotalExVatCustomerCurrency;
        public RoleType DeliveryDate;
        public RoleType TotalBasePrice;
        public RoleType TotalFee;
        public RoleType SurchargeAdjustment;
        public RoleType ContactPerson;

		// Inherited Roles
        public RoleType PrintContent;
        public RoleType Comment;
        public RoleType Locale;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaOrder(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("7dde949a-6f54-4ece-92b3-d269f50ef9d9"))
			{
				SingularName = "Order",
			};
        }
	}

    public partial class MetaOrderAdjustment : MetaInterface
	{
	    public static MetaOrderAdjustment Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Amount;
        public RoleType VatRate;
        public RoleType Percentage;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaOrderAdjustment(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("c5578565-c07a-4dc1-8381-41955db364e2"))
			{
				SingularName = "OrderAdjustment",
			};
        }
	}

    public partial class MetaOrderItem : MetaInterface
	{
	    public static MetaOrderItem Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType Cancel;
        public MethodType Reject;
        public MethodType Confirm;
        public MethodType Approve;

		// Inherited Methods

		// Defined Roles
        public RoleType InternalComment;
        public RoleType BudgetItem;
        public RoleType PreviousQuantity;
        public RoleType QuantityOrdered;
        public RoleType Description;
        public RoleType CorrespondingPurchaseOrder;
        public RoleType TotalOrderAdjustmentCustomerCurrency;
        public RoleType TotalOrderAdjustment;
        public RoleType QuoteItem;
        public RoleType AssignedDeliveryDate;
        public RoleType DeliveryDate;
        public RoleType OrderTerms;
        public RoleType ShippingInstruction;
        public RoleType Associations;
        public RoleType Message;

		// Inherited Roles
        public RoleType TotalDiscountAsPercentage;
        public RoleType DiscountAdjustment;
        public RoleType UnitVat;
        public RoleType TotalVatCustomerCurrency;
        public RoleType VatRegime;
        public RoleType TotalVat;
        public RoleType UnitSurcharge;
        public RoleType UnitDiscount;
        public RoleType TotalExVatCustomerCurrency;
        public RoleType DerivedVatRate;
        public RoleType ActualUnitPrice;
        public RoleType TotalIncVatCustomerCurrency;
        public RoleType UnitBasePrice;
        public RoleType CalculatedUnitPrice;
        public RoleType TotalSurchargeCustomerCurrency;
        public RoleType TotalIncVat;
        public RoleType TotalSurchargeAsPercentage;
        public RoleType TotalDiscountCustomerCurrency;
        public RoleType TotalDiscount;
        public RoleType TotalSurcharge;
        public RoleType AssignedVatRegime;
        public RoleType TotalBasePrice;
        public RoleType TotalExVat;
        public RoleType TotalBasePriceCustomerCurrency;
        public RoleType CurrentPriceComponents;
        public RoleType SurchargeAdjustment;
        public RoleType Comment;
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType SalesOrderItemVersionsWhereOrderedWithFeature;
        public AssociationType OrderItemBillingsWhereOrderItem;
        public AssociationType OrderRequirementCommitmentsWhereOrderItem;
        public AssociationType SalesOrderItemWhereOrderedWithFeature;
        public AssociationType ShipmentReceiptsWhereOrderItem;
        public AssociationType OrderItemVersionsWhereAssociation;
        public AssociationType OrderVersionsWhereValidOrderItem;
        public AssociationType OrderWhereValidOrderItem;
        public AssociationType OrderItemsWhereAssociation;
        public AssociationType WorkEffortVersionsWhereOrderItemFulfillment;
        public AssociationType WorkEffortsWhereOrderItemFulfillment;

		// Inherited Associations

		internal MetaOrderItem(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("f3ef0124-e867-4da2-9323-80fbe1f214c2"))
			{
				SingularName = "OrderItem",
			};
        }
	}

    public partial class MetaOrganisationClassification : MetaInterface
	{
	    public static MetaOrganisationClassification Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles
        public RoleType Name;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType OrganisationVersionsWhereOrganisationClassification;
        public AssociationType OrganisationsWhereOrganisationClassification;

		// Inherited Associations
        public AssociationType PartyVersionsWherePartyClassification;
        public AssociationType PartiesWherePartyClassification;
        public AssociationType PriceComponentsWherePartyClassification;

		internal MetaOrganisationClassification(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("0aaee7b0-0a32-4d0b-a3ed-a448608fe935"))
			{
				SingularName = "OrganisationClassification",
			};
        }
	}

    public partial class MetaPart : MetaInterface
	{
	    public static MetaPart Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Name;
        public RoleType PartSpecifications;
        public RoleType UnitOfMeasure;
        public RoleType Documents;
        public RoleType ManufacturerId;
        public RoleType ReorderLevel;
        public RoleType ReorderQuantity;
        public RoleType PriceComponents;
        public RoleType InventoryItemKind;
        public RoleType Sku;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseInvoiceItemVersionsWherePart;
        public AssociationType PurchaseOrderItemVersionsWherePart;
        public AssociationType PartRevisionsWhereSupersededByPart;
        public AssociationType PartRevisionsWherePart;
        public AssociationType PartSubstitutesWhereSubstitutionPart;
        public AssociationType PartSubstitutesWherePart;
        public AssociationType PurchaseInvoiceItemsWherePart;
        public AssociationType PurchaseOrderItemsWherePart;
        public AssociationType ShipmentItemsWherePart;
        public AssociationType SupplierOfferingsWherePart;
        public AssociationType WorkEffortPartStandardsWherePart;
        public AssociationType InventoryItemVersionsWherePart;
        public AssociationType InventoryItemsWherePart;
        public AssociationType PartBillOfMaterialsWherePart;
        public AssociationType PartBillOfMaterialsWhereComponentPart;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaPart(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("75916246-b1b5-48ef-9578-d65980fd2623"))
			{
				SingularName = "Part",
			};
        }
	}

    public partial class MetaPartBillOfMaterial : MetaInterface
	{
	    public static MetaPartBillOfMaterial Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Part;
        public RoleType Instruction;
        public RoleType QuantityUsed;
        public RoleType ComponentPart;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Comment;
        public RoleType FromDate;
        public RoleType ThroughDate;

		// Defined Associations
        public AssociationType EngineeringChangesWherePartBillOfMaterial;
        public AssociationType PartBillOfMaterialSubstitutesWhereSubstitutionPartBillOfMaterial;
        public AssociationType PartBillOfMaterialSubstitutesWherePartBillOfMaterial;

		// Inherited Associations

		internal MetaPartBillOfMaterial(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("d204e616-039c-40c8-81cc-18f3a7345d99"))
			{
				SingularName = "PartBillOfMaterial",
			};
        }
	}

    public partial class MetaParty : MetaInterface
	{
	    public static MetaParty Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType PartyName;
        public RoleType GeneralCorrespondence;
        public RoleType YTDRevenue;
        public RoleType LastYearsRevenue;
        public RoleType BillingInquiriesFax;
        public RoleType Qualifications;
        public RoleType HomeAddress;
        public RoleType InactiveOrganisationContactRelationships;
        public RoleType SalesOffice;
        public RoleType InactiveContacts;
        public RoleType InactivePartyContactMechanisms;
        public RoleType OrderInquiriesFax;
        public RoleType CurrentSalesReps;
        public RoleType PartyContactMechanisms;
        public RoleType ShippingInquiriesFax;
        public RoleType ShippingInquiriesPhone;
        public RoleType BillingAccounts;
        public RoleType OrderInquiriesPhone;
        public RoleType PartySkills;
        public RoleType PartyClassifications;
        public RoleType ExcludeFromDunning;
        public RoleType BankAccounts;
        public RoleType CurrentContacts;
        public RoleType BillingAddress;
        public RoleType GeneralEmail;
        public RoleType DefaultShipmentMethod;
        public RoleType Resumes;
        public RoleType HeadQuarter;
        public RoleType PersonalEmailAddress;
        public RoleType CellPhoneNumber;
        public RoleType BillingInquiriesPhone;
        public RoleType OrderAddress;
        public RoleType InternetAddress;
        public RoleType Contents;
        public RoleType CreditCards;
        public RoleType ShippingAddress;
        public RoleType CurrentOrganisationContactRelationships;
        public RoleType OpenOrderAmount;
        public RoleType GeneralFaxNumber;
        public RoleType DefaultPaymentMethod;
        public RoleType CurrentPartyContactMechanisms;
        public RoleType GeneralPhoneNumber;
        public RoleType PreferredCurrency;
        public RoleType VatRegime;
        public RoleType SimpleMovingAverage;
        public RoleType AmountOverDue;
        public RoleType DunningType;
        public RoleType AmountDue;
        public RoleType LastReminderDate;
        public RoleType CreditLimit;
        public RoleType SubAccountNumber;
        public RoleType BlockedForDunning;
        public RoleType Agreements;
        public RoleType CommunicationEvents;

		// Inherited Roles
        public RoleType Locale;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType UniqueId;
        public RoleType Comment;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType CustomerRelationshipsWhereCustomer;
        public AssociationType EngagementsWhereBillToParty;
        public AssociationType EngagementsWherePlacingParty;
        public AssociationType FaceToFaceCommunicationsWhereParticipant;
        public AssociationType FaceToFaceCommunicationVersionsWhereParticipant;
        public AssociationType FaxCommunicationsWhereOriginator;
        public AssociationType FaxCommunicationsWhereReceiver;
        public AssociationType FaxCommunicationVersionsWhereOriginator;
        public AssociationType FaxCommunicationVersionsWhereReceiver;
        public AssociationType GoodsWhereManufacturedBy;
        public AssociationType GoodsWhereSuppliedBy;
        public AssociationType InternalOrganisationWhereCurrentCustomer;
        public AssociationType InternalOrganisationWhereActiveCustomer;
        public AssociationType InternalOrganisationWhereActiveSupplier;
        public AssociationType LetterCorrespondencesWhereOriginator;
        public AssociationType LetterCorrespondencesWhereReceiver;
        public AssociationType LetterCorrespondenceVersionsWhereOriginator;
        public AssociationType LetterCorrespondenceVersionsWhereReceiver;
        public AssociationType PhoneCommunicationVersionsWhereReceiver;
        public AssociationType PhoneCommunicationVersionsWhereCaller;
        public AssociationType PickListVersionsWhereShipToParty;
        public AssociationType PurchaseInvoiceVersionsWhereBilledFromParty;
        public AssociationType PurchaseOrderVersionsWherePreviousTakenViaSupplier;
        public AssociationType PurchaseOrderVersionsWhereTakenViaSupplier;
        public AssociationType QuoteItemVersionsWhereAuthorizer;
        public AssociationType RequirementVersionsWhereAuthorizer;
        public AssociationType RequirementVersionsWhereNeededFor;
        public AssociationType RequirementVersionsWhereOriginator;
        public AssociationType RequirementVersionsWhereServicedBy;
        public AssociationType SalesInvoiceVersionsWherePreviousBillToCustomer;
        public AssociationType SalesInvoiceVersionsWherePreviousShipToCustomer;
        public AssociationType SalesInvoiceVersionsWhereBillToCustomer;
        public AssociationType SalesInvoiceVersionsWhereShipToCustomer;
        public AssociationType SalesInvoiceVersionsWhereCustomer;
        public AssociationType SalesOrderItemVersionsWhereShipToParty;
        public AssociationType SalesOrderItemVersionsWhereAssignedShipToParty;
        public AssociationType SalesOrderVersionsWhereShipToCustomer;
        public AssociationType SalesOrderVersionsWhereBillToCustomer;
        public AssociationType SalesOrderVersionsWherePreviousShipToCustomer;
        public AssociationType SalesOrderVersionsWhereCustomer;
        public AssociationType SalesOrderVersionsWherePreviousBillToCustomer;
        public AssociationType SalesOrderVersionsWherePlacingCustomer;
        public AssociationType OrganisationGlAccountsWhereParty;
        public AssociationType PartyFixedAssetAssignmentsWhereParty;
        public AssociationType PartyPackageRevenuesWhereParty;
        public AssociationType PartyProductCategoryRevenuesWhereParty;
        public AssociationType PartyProductRevenuesWhereParty;
        public AssociationType PartyRevenuesWhereParty;
        public AssociationType PhoneCommunicationsWhereReceiver;
        public AssociationType PhoneCommunicationsWhereCaller;
        public AssociationType PickListsWhereShipToParty;
        public AssociationType PurchaseInvoicesWhereBilledFromParty;
        public AssociationType PurchaseOrdersWherePreviousTakenViaSupplier;
        public AssociationType PurchaseOrdersWhereTakenViaSupplier;
        public AssociationType QuoteItemsWhereAuthorizer;
        public AssociationType RequirementsWhereAuthorizer;
        public AssociationType RequirementsWhereNeededFor;
        public AssociationType RequirementsWhereOriginator;
        public AssociationType RequirementsWhereServicedBy;
        public AssociationType RespondingPartiesWhereParty;
        public AssociationType SalesInvoicesWherePreviousBillToCustomer;
        public AssociationType SalesInvoicesWherePreviousShipToCustomer;
        public AssociationType SalesInvoicesWhereBillToCustomer;
        public AssociationType SalesInvoicesWhereShipToCustomer;
        public AssociationType SalesInvoicesWhereCustomer;
        public AssociationType SalesOrdersWhereShipToCustomer;
        public AssociationType SalesOrdersWhereBillToCustomer;
        public AssociationType SalesOrdersWherePreviousShipToCustomer;
        public AssociationType SalesOrdersWhereCustomer;
        public AssociationType SalesOrdersWherePreviousBillToCustomer;
        public AssociationType SalesOrdersWherePlacingCustomer;
        public AssociationType SalesOrderItemsWhereShipToParty;
        public AssociationType SalesOrderItemsWhereAssignedShipToParty;
        public AssociationType SalesRepPartyProductCategoryRevenuesWhereParty;
        public AssociationType SalesRepPartyRevenuesWhereParty;
        public AssociationType SalesRepRelationshipsWhereCustomer;
        public AssociationType SubContractorRelationshipsWhereContractor;
        public AssociationType SubContractorRelationshipsWhereSubContractor;
        public AssociationType SupplierOfferingsWhereSupplier;
        public AssociationType WebSiteCommunicationVersionsWhereOriginator;
        public AssociationType WebSiteCommunicationVersionsWhereReceiver;
        public AssociationType WebSiteCommunicationsWhereOriginator;
        public AssociationType WebSiteCommunicationsWhereReceiver;
        public AssociationType WorkEffortPartyAssignmentsWhereParty;
        public AssociationType CommunicationEventVersionsWhereToParty;
        public AssociationType CommunicationEventVersionsWhereInvolvedParty;
        public AssociationType CommunicationEventVersionsWhereFromParty;
        public AssociationType CommunicationEventsWhereToParty;
        public AssociationType CommunicationEventsWhereInvolvedParty;
        public AssociationType CommunicationEventsWhereFromParty;
        public AssociationType ExternalAccountingTransactionsWhereFromParty;
        public AssociationType ExternalAccountingTransactionsWhereToParty;
        public AssociationType PartyRelationshipsWhereParty;
        public AssociationType QuoteVersionsWhereReceiver;
        public AssociationType RequestVersionsWhereOriginator;
        public AssociationType PaymentsWhereSendingParty;
        public AssociationType PaymentsWhereReceivingParty;
        public AssociationType QuotesWhereReceiver;
        public AssociationType RequestsWhereOriginator;
        public AssociationType ShipmentVersionsWhereBillToParty;
        public AssociationType ShipmentVersionsWhereShipToParty;
        public AssociationType ShipmentVersionsWhereShipFromParty;
        public AssociationType ShipmentsWhereBillToParty;
        public AssociationType ShipmentsWhereShipToParty;
        public AssociationType ShipmentsWhereShipFromParty;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaParty(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("3bba6e5a-dc2d-4838-b6c4-881f6c8c3013"))
			{
				SingularName = "Party",
				PluralName = "Parties",
			};
        }
	}

    public partial class MetaPartyClassification : MetaInterface
	{
	    public static MetaPartyClassification Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType PartyVersionsWherePartyClassification;
        public AssociationType PartiesWherePartyClassification;
        public AssociationType PriceComponentsWherePartyClassification;

		// Inherited Associations

		internal MetaPartyClassification(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("3bb83aa5-e58a-4421-bdbc-3c9fa0b2324f"))
			{
				SingularName = "PartyClassification",
			};
        }
	}

    public partial class MetaPayment : MetaInterface
	{
	    public static MetaPayment Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Amount;
        public RoleType PaymentMethod;
        public RoleType EffectiveDate;
        public RoleType SendingParty;
        public RoleType PaymentApplications;
        public RoleType ReferenceNumber;
        public RoleType ReceivingParty;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Comment;
        public RoleType UniqueId;

		// Defined Associations
        public AssociationType PaymentBudgetAllocationsWherePayment;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaPayment(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("9f20a35c-d814-4690-a96f-2bcd25f6c6a2"))
			{
				SingularName = "Payment",
			};
        }
	}

    public partial class MetaPaymentMethod : MetaInterface
	{
	    public static MetaPaymentMethod Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType BalanceLimit;
        public RoleType CurrentBalance;
        public RoleType Journal;
        public RoleType Description;
        public RoleType GlPaymentInTransit;
        public RoleType Remarks;
        public RoleType GeneralLedgerAccount;
        public RoleType IsActive;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations
        public AssociationType AmountDuesWherePaymentMethod;
        public AssociationType CustomerShipmentsWherePaymentMethod;
        public AssociationType CustomerShipmentVersionsWherePaymentMethod;
        public AssociationType InternalOrganisationWhereActivePaymentMethod;
        public AssociationType InternalOrganisationWhereDefaultPaymentMethod;
        public AssociationType SalesInvoiceVersionsWherePaymentMethod;
        public AssociationType SalesOrderVersionsWherePaymentMethod;
        public AssociationType PayrollPreferencesWherePaymentMethod;
        public AssociationType SalesInvoicesWherePaymentMethod;
        public AssociationType SalesOrdersWherePaymentMethod;
        public AssociationType StoresWhereDefaultPaymentMethod;
        public AssociationType StoresWherePaymentMethod;
        public AssociationType PartyVersionsWhereDefaultPaymentMethod;
        public AssociationType PartiesWhereDefaultPaymentMethod;
        public AssociationType PaymentsWherePaymentMethod;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaPaymentMethod(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("f34d5b9b-b940-4885-9744-754dd0eae08d"))
			{
				SingularName = "PaymentMethod",
			};
        }
	}

    public partial class MetaPeriod : MetaInterface
	{
	    public static MetaPeriod Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType FromDate;
        public RoleType ThroughDate;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaPeriod(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("80adbbfd-952e-46f3-a744-78e0ce42bc80"))
			{
				SingularName = "Period",
			};
        }
	}

    public partial class MetaPersonClassification : MetaInterface
	{
	    public static MetaPersonClassification Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles
        public RoleType Name;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType PeopleWherePersonClassification;
        public AssociationType PersonVersionsWherePersonClassification;

		// Inherited Associations
        public AssociationType PartyVersionsWherePartyClassification;
        public AssociationType PartiesWherePartyClassification;
        public AssociationType PriceComponentsWherePartyClassification;

		internal MetaPersonClassification(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("77f63100-054a-459b-8864-e69b646ff307"))
			{
				SingularName = "PersonClassification",
			};
        }
	}

    public partial class MetaPriceable : MetaInterface
	{
	    public static MetaPriceable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType TotalDiscountAsPercentage;
        public RoleType DiscountAdjustment;
        public RoleType UnitVat;
        public RoleType TotalVatCustomerCurrency;
        public RoleType VatRegime;
        public RoleType TotalVat;
        public RoleType UnitSurcharge;
        public RoleType UnitDiscount;
        public RoleType TotalExVatCustomerCurrency;
        public RoleType DerivedVatRate;
        public RoleType ActualUnitPrice;
        public RoleType TotalIncVatCustomerCurrency;
        public RoleType UnitBasePrice;
        public RoleType CalculatedUnitPrice;
        public RoleType TotalSurchargeCustomerCurrency;
        public RoleType TotalIncVat;
        public RoleType TotalSurchargeAsPercentage;
        public RoleType TotalDiscountCustomerCurrency;
        public RoleType TotalDiscount;
        public RoleType TotalSurcharge;
        public RoleType AssignedVatRegime;
        public RoleType TotalBasePrice;
        public RoleType TotalExVat;
        public RoleType TotalBasePriceCustomerCurrency;
        public RoleType CurrentPriceComponents;
        public RoleType SurchargeAdjustment;

		// Inherited Roles
        public RoleType Comment;
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaPriceable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("26e69a5f-0220-4b60-99bf-26e150bcb64c"))
			{
				SingularName = "Priceable",
			};
        }
	}

    public partial class MetaPriceComponent : MetaInterface
	{
	    public static MetaPriceComponent Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType GeographicBoundary;
        public RoleType Rate;
        public RoleType RevenueValueBreak;
        public RoleType PartyClassification;
        public RoleType OrderQuantityBreak;
        public RoleType PackageQuantityBreak;
        public RoleType Product;
        public RoleType RevenueQuantityBreak;
        public RoleType ProductFeature;
        public RoleType AgreementPricingProgram;
        public RoleType Description;
        public RoleType Currency;
        public RoleType OrderKind;
        public RoleType OrderValue;
        public RoleType Price;
        public RoleType ProductCategory;
        public RoleType SalesChannel;

		// Inherited Roles
        public RoleType FromDate;
        public RoleType ThroughDate;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Comment;

		// Defined Associations
        public AssociationType EngagementRateWhereGoverningPriceComponent;
        public AssociationType PriceableVersionsWhereCurrentPriceComponent;
        public AssociationType PartWherePriceComponent;
        public AssociationType PriceablesWhereCurrentPriceComponent;
        public AssociationType ProductWhereVirtualProductPriceComponent;
        public AssociationType ProductsWhereBasePrice;
        public AssociationType ProductFeaturesWhereBasePrice;

		// Inherited Associations

		internal MetaPriceComponent(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("383589fb-f410-4d22-ade6-aa5126fdef18"))
			{
				SingularName = "PriceComponent",
			};
        }
	}

    public partial class MetaPrintable : MetaInterface
	{
	    public static MetaPrintable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType PrintContent;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaPrintable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("61207a42-3199-4249-baa4-9dd11dc0f5b1"))
			{
				SingularName = "Printable",
			};
        }
	}

    public partial class MetaProduct : MetaInterface
	{
	    public static MetaProduct Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType InternalComment;
        public RoleType PrimaryProductCategory;
        public RoleType SupportDiscontinuationDate;
        public RoleType SalesDiscontinuationDate;
        public RoleType LocalisedNames;
        public RoleType LocalisedDescriptions;
        public RoleType LocalisedComments;
        public RoleType Description;
        public RoleType VirtualProductPriceComponents;
        public RoleType IntrastatCode;
        public RoleType ProductCategoriesExpanded;
        public RoleType ProductComplement;
        public RoleType OptionalFeatures;
        public RoleType Variants;
        public RoleType Name;
        public RoleType IntroductionDate;
        public RoleType Documents;
        public RoleType StandardFeatures;
        public RoleType UnitOfMeasure;
        public RoleType EstimatedProductCosts;
        public RoleType ProductObsolescences;
        public RoleType SelectableFeatures;
        public RoleType VatRate;
        public RoleType BasePrices;
        public RoleType ProductCategories;

		// Inherited Roles
        public RoleType Comment;
        public RoleType UniqueId;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType GeneralLedgerAccountsWhereDefaultCostUnit;
        public AssociationType GeneralLedgerAccountsWhereCostUnitsAllowed;
        public AssociationType GoodsWhereProductSubstitution;
        public AssociationType GoodsWhereProductIncompatibility;
        public AssociationType MarketingPackageWhereProductsUsedIn;
        public AssociationType MarketingPackagesWhereProduct;
        public AssociationType PurchaseOrderItemVersionsWhereProduct;
        public AssociationType QuoteItemVersionsWhereProduct;
        public AssociationType RequestItemVersionsWhereProduct;
        public AssociationType SalesInvoiceItemVersionsWhereProduct;
        public AssociationType SalesOrderItemVersionsWherePreviousProduct;
        public AssociationType SalesOrderItemVersionsWhereProduct;
        public AssociationType OrganisationGlAccountsWhereProduct;
        public AssociationType PartyProductRevenuesWhereProduct;
        public AssociationType ProductCategoriesWhereAllProduct;
        public AssociationType ProductConfigurationsWhereProductsUsedIn;
        public AssociationType ProductConfigurationsWhereProduct;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereAvailableFor;
        public AssociationType ProductRevenuesWhereProduct;
        public AssociationType PurchaseOrderItemsWhereProduct;
        public AssociationType QuoteItemsWhereProduct;
        public AssociationType RequestItemsWhereProduct;
        public AssociationType SalesInvoiceItemsWhereProduct;
        public AssociationType SalesOrderItemsWherePreviousProduct;
        public AssociationType SalesOrderItemsWhereProduct;
        public AssociationType SupplierOfferingsWhereProduct;
        public AssociationType WorkEffortTypesWhereProductToProduce;
        public AssociationType EngagementItemsWhereProduct;
        public AssociationType PriceComponentsWhereProduct;
        public AssociationType ProductsWhereProductComplement;
        public AssociationType ProductWhereVariant;
        public AssociationType ProductsWhereProductObsolescence;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaProduct(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("56b79619-d04a-4924-96e8-e3e7be9faa09"))
			{
				SingularName = "Product",
			};
        }
	}

    public partial class MetaProductAssociation : MetaInterface
	{
	    public static MetaProductAssociation Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles
        public RoleType Comment;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType FromDate;
        public RoleType ThroughDate;

		// Defined Associations

		// Inherited Associations

		internal MetaProductAssociation(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("f194d2e1-d246-40eb-9eab-70ee2521703a"))
			{
				SingularName = "ProductAssociation",
			};
        }
	}

    public partial class MetaProductFeature : MetaInterface
	{
	    public static MetaProductFeature Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType EstimatedProductCosts;
        public RoleType BasePrices;
        public RoleType Description;
        public RoleType DependentFeatures;
        public RoleType IncompatibleFeatures;
        public RoleType VatRate;

		// Inherited Roles
        public RoleType UniqueId;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType DesiredProductFeaturesWhereProductFeature;
        public AssociationType QuoteItemVersionsWhereProductFeature;
        public AssociationType RequestItemVersionsWhereProductFeature;
        public AssociationType SalesInvoiceItemVersionsWhereProductFeature;
        public AssociationType SalesOrderItemVersionsWhereProductFeature;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereUsedToDefine;
        public AssociationType QuoteItemsWhereProductFeature;
        public AssociationType RequestItemsWhereProductFeature;
        public AssociationType SalesInvoiceItemsWhereProductFeature;
        public AssociationType SalesOrderItemsWhereProductFeature;
        public AssociationType ShipmentItemsWhereProductFeature;
        public AssociationType EngagementItemsWhereProductFeature;
        public AssociationType PriceComponentsWhereProductFeature;
        public AssociationType ProductsWhereOptionalFeature;
        public AssociationType ProductsWhereStandardFeature;
        public AssociationType ProductsWhereSelectableFeature;
        public AssociationType ProductFeaturesWhereDependentFeature;
        public AssociationType ProductFeaturesWhereIncompatibleFeature;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaProductFeature(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("d3c5a482-e17a-4e37-84eb-55a035e80f2f"))
			{
				SingularName = "ProductFeature",
			};
        }
	}

    public partial class MetaQuote : MetaInterface
	{
	    public static MetaQuote Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType Approve;
        public MethodType Reject;

		// Inherited Methods

		// Defined Roles
        public RoleType PreviousQuoteState;
        public RoleType LastQuoteState;
        public RoleType QuoteState;
        public RoleType InternalComment;
        public RoleType RequiredResponseDate;
        public RoleType ValidFromDate;
        public RoleType QuoteTerms;
        public RoleType ValidThroughDate;
        public RoleType Description;
        public RoleType Receiver;
        public RoleType FullfillContactMechanism;
        public RoleType Price;
        public RoleType Currency;
        public RoleType IssueDate;
        public RoleType QuoteItems;
        public RoleType QuoteNumber;
        public RoleType Request;
        public RoleType ContactPerson;

		// Inherited Roles
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType PrintContent;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType Comment;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaQuote(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("066bf242-2710-4a68-8ff6-ce4d7d88a04a"))
			{
				SingularName = "Quote",
			};
        }
	}

    public partial class MetaRequest : MetaInterface
	{
	    public static MetaRequest Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType Cancel;
        public MethodType Reject;
        public MethodType Submit;
        public MethodType Hold;

		// Inherited Methods

		// Defined Roles
        public RoleType PreviousRequestState;
        public RoleType LastRequestState;
        public RoleType RequestState;
        public RoleType InternalComment;
        public RoleType Description;
        public RoleType RequestDate;
        public RoleType RequiredResponseDate;
        public RoleType RequestItems;
        public RoleType RequestNumber;
        public RoleType RespondingParties;
        public RoleType Originator;
        public RoleType Currency;
        public RoleType FullfillContactMechanism;
        public RoleType EmailAddress;
        public RoleType TelephoneNumber;
        public RoleType TelephoneCountryCode;
        public RoleType ContactPerson;

		// Inherited Roles
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType Comment;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType PrintContent;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType QuoteVersionsWhereRequest;
        public AssociationType QuoteWhereRequest;

		// Inherited Associations

		internal MetaRequest(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("321a6047-2233-4bec-a1b1-9b965c0099e5"))
			{
				SingularName = "Request",
			};
        }
	}

    public partial class MetaService : MetaInterface
	{
	    public static MetaService Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles
        public RoleType InternalComment;
        public RoleType PrimaryProductCategory;
        public RoleType SupportDiscontinuationDate;
        public RoleType SalesDiscontinuationDate;
        public RoleType LocalisedNames;
        public RoleType LocalisedDescriptions;
        public RoleType LocalisedComments;
        public RoleType Description;
        public RoleType VirtualProductPriceComponents;
        public RoleType IntrastatCode;
        public RoleType ProductCategoriesExpanded;
        public RoleType ProductComplement;
        public RoleType OptionalFeatures;
        public RoleType Variants;
        public RoleType Name;
        public RoleType IntroductionDate;
        public RoleType Documents;
        public RoleType StandardFeatures;
        public RoleType UnitOfMeasure;
        public RoleType EstimatedProductCosts;
        public RoleType ProductObsolescences;
        public RoleType SelectableFeatures;
        public RoleType VatRate;
        public RoleType BasePrices;
        public RoleType ProductCategories;
        public RoleType Comment;
        public RoleType UniqueId;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType ProductDeliverySkillRequirementsWhereService;

		// Inherited Associations
        public AssociationType GeneralLedgerAccountsWhereDefaultCostUnit;
        public AssociationType GeneralLedgerAccountsWhereCostUnitsAllowed;
        public AssociationType GoodsWhereProductSubstitution;
        public AssociationType GoodsWhereProductIncompatibility;
        public AssociationType MarketingPackageWhereProductsUsedIn;
        public AssociationType MarketingPackagesWhereProduct;
        public AssociationType PurchaseOrderItemVersionsWhereProduct;
        public AssociationType QuoteItemVersionsWhereProduct;
        public AssociationType RequestItemVersionsWhereProduct;
        public AssociationType SalesInvoiceItemVersionsWhereProduct;
        public AssociationType SalesOrderItemVersionsWherePreviousProduct;
        public AssociationType SalesOrderItemVersionsWhereProduct;
        public AssociationType OrganisationGlAccountsWhereProduct;
        public AssociationType PartyProductRevenuesWhereProduct;
        public AssociationType ProductCategoriesWhereAllProduct;
        public AssociationType ProductConfigurationsWhereProductsUsedIn;
        public AssociationType ProductConfigurationsWhereProduct;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereAvailableFor;
        public AssociationType ProductRevenuesWhereProduct;
        public AssociationType PurchaseOrderItemsWhereProduct;
        public AssociationType QuoteItemsWhereProduct;
        public AssociationType RequestItemsWhereProduct;
        public AssociationType SalesInvoiceItemsWhereProduct;
        public AssociationType SalesOrderItemsWherePreviousProduct;
        public AssociationType SalesOrderItemsWhereProduct;
        public AssociationType SupplierOfferingsWhereProduct;
        public AssociationType WorkEffortTypesWhereProductToProduce;
        public AssociationType EngagementItemsWhereProduct;
        public AssociationType PriceComponentsWhereProduct;
        public AssociationType ProductsWhereProductComplement;
        public AssociationType ProductWhereVariant;
        public AssociationType ProductsWhereProductObsolescence;
        public AssociationType NotificationsWhereTarget;

		internal MetaService(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("13d519ec-468e-4fa7-9803-b95dbab4eb82"))
			{
				SingularName = "Service",
			};
        }
	}

    public partial class MetaServiceEntry : MetaInterface
	{
	    public static MetaServiceEntry Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType ThroughDateTime;
        public RoleType EngagementItem;
        public RoleType IsBillable;
        public RoleType FromDateTime;
        public RoleType Description;
        public RoleType WorkEffort;

		// Inherited Roles
        public RoleType Comment;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType ServiceEntryBillingsWhereServiceEntry;
        public AssociationType ServiceEntryHeaderWhereServiceEntry;

		// Inherited Associations

		internal MetaServiceEntry(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("4a4a0548-b75f-4a79-89aa-f5c242121f11"))
			{
				SingularName = "ServiceEntry",
				PluralName = "ServiceEntries",
			};
        }
	}

    public partial class MetaShipmentVersion : MetaInterface
	{
	    public static MetaShipmentVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType ShipmentMethod;
        public RoleType BillToContactMechanism;
        public RoleType ShipmentPackages;
        public RoleType ShipmentNumber;
        public RoleType Documents;
        public RoleType BillToParty;
        public RoleType ShipToParty;
        public RoleType ShipmentItems;
        public RoleType ReceiverContactMechanism;
        public RoleType ShipToAddress;
        public RoleType EstimatedShipCost;
        public RoleType EstimatedShipDate;
        public RoleType LatestCancelDate;
        public RoleType Carrier;
        public RoleType InquireAboutContactMechanism;
        public RoleType EstimatedReadyDate;
        public RoleType ShipFromAddress;
        public RoleType BillFromContactMechanism;
        public RoleType HandlingInstruction;
        public RoleType Store;
        public RoleType ShipFromParty;
        public RoleType ShipmentRouteSegments;
        public RoleType EstimatedArrivalDate;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaShipmentVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("F76BB372-D433-4479-8324-3A232AC50A25"))
			{
				SingularName = "ShipmentVersion",
			};
        }
	}

    public partial class MetaShipment : MetaInterface
	{
	    public static MetaShipment Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType ShipmentMethod;
        public RoleType BillToContactMechanism;
        public RoleType ShipmentPackages;
        public RoleType ShipmentNumber;
        public RoleType Documents;
        public RoleType BillToParty;
        public RoleType ShipToParty;
        public RoleType ShipmentItems;
        public RoleType ReceiverContactMechanism;
        public RoleType ShipToAddress;
        public RoleType EstimatedShipCost;
        public RoleType EstimatedShipDate;
        public RoleType LatestCancelDate;
        public RoleType Carrier;
        public RoleType InquireAboutContactMechanism;
        public RoleType EstimatedReadyDate;
        public RoleType ShipFromAddress;
        public RoleType BillFromContactMechanism;
        public RoleType HandlingInstruction;
        public RoleType Store;
        public RoleType ShipFromParty;
        public RoleType ShipmentRouteSegments;
        public RoleType EstimatedArrivalDate;

		// Inherited Roles
        public RoleType PrintContent;
        public RoleType Comment;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType SalesInvoiceVersionsWhereShipment;
        public AssociationType SalesInvoicesWhereShipment;

		// Inherited Associations

		internal MetaShipment(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("9c6f4ad8-5a4e-4b6e-96b7-876f7aabcffb"))
			{
				SingularName = "Shipment",
			};
        }
	}

    public partial class MetaWorkEffortVersion : MetaInterface
	{
	    public static MetaWorkEffortVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType WorkEffortState;
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;
        public RoleType Name;
        public RoleType Description;
        public RoleType Priority;
        public RoleType WorkEffortPurposes;
        public RoleType ActualCompletion;
        public RoleType ScheduledStart;
        public RoleType ScheduledCompletion;
        public RoleType ActualHours;
        public RoleType EstimatedHours;
        public RoleType Precendencies;
        public RoleType Facility;
        public RoleType DeliverablesProduced;
        public RoleType ActualStart;
        public RoleType InventoryItemsNeeded;
        public RoleType Children;
        public RoleType OrderItemFulfillment;
        public RoleType WorkEffortType;
        public RoleType InventoryItemsProduced;
        public RoleType RequirementFulfillments;
        public RoleType SpecialTerms;
        public RoleType Concurrencies;

		// Inherited Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaWorkEffortVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("E86A7C06-1376-42C0-B901-2F64C6D0B1A6"))
			{
				SingularName = "WorkEffortVersion",
			};
        }
	}

    public partial class MetaTermType : MetaInterface
	{
	    public static MetaTermType Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles
        public RoleType LocalisedNames;
        public RoleType Name;
        public RoleType IsActive;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations
        public AssociationType OrderTermsWhereTermType;
        public AssociationType QuoteTermsWhereTermType;
        public AssociationType AgreementTermsWhereTermType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaTermType(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("1468c86a-4ac4-4c64-a93b-1b0c5f4b41bc"))
			{
				SingularName = "TermType",
			};
        }
	}

    public partial class MetaWorkEffort : MetaInterface
	{
	    public static MetaWorkEffort Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType Confirm;
        public MethodType Finish;
        public MethodType Cancel;
        public MethodType Reopen;

		// Inherited Methods

		// Defined Roles
        public RoleType PreviousWorkEffortState;
        public RoleType LastWorkEffortState;
        public RoleType WorkEffortState;
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;
        public RoleType Owner;
        public RoleType Name;
        public RoleType Description;
        public RoleType Priority;
        public RoleType WorkEffortPurposes;
        public RoleType ActualCompletion;
        public RoleType ScheduledStart;
        public RoleType ScheduledCompletion;
        public RoleType ActualHours;
        public RoleType EstimatedHours;
        public RoleType Precendencies;
        public RoleType Facility;
        public RoleType DeliverablesProduced;
        public RoleType ActualStart;
        public RoleType InventoryItemsNeeded;
        public RoleType Children;
        public RoleType OrderItemFulfillment;
        public RoleType WorkEffortType;
        public RoleType InventoryItemsProduced;
        public RoleType RequirementFulfillments;
        public RoleType SpecialTerms;
        public RoleType Concurrencies;

		// Inherited Roles
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType UniqueId;
        public RoleType CreatedBy;
        public RoleType LastModifiedBy;
        public RoleType CreationDate;
        public RoleType LastModifiedDate;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations
        public AssociationType QuoteItemVersionsWhereWorkEffort;
        public AssociationType QuoteItemsWhereWorkEffort;
        public AssociationType WorkEffortAssignmentsWhereAssignment;
        public AssociationType WorkEffortBillingsWhereWorkEffort;
        public AssociationType WorkEffortFixedAssetAssignmentsWhereAssignment;
        public AssociationType WorkEffortInventoryAssignmentsWhereAssignment;
        public AssociationType WorkEffortPartyAssignmentsWhereAssignment;
        public AssociationType CommunicationEventVersionsWhereWorkEffort;
        public AssociationType CommunicationEventsWhereWorkEffort;
        public AssociationType EngagementItemsWhereEngagementWorkFulfillment;
        public AssociationType ServiceEntriesWhereWorkEffort;
        public AssociationType WorkEffortVersionsWherePrecendency;
        public AssociationType WorkEffortVersionsWhereChild;
        public AssociationType WorkEffortVersionsWhereConcurrency;
        public AssociationType WorkEffortsWherePrecendency;
        public AssociationType WorkEffortsWhereChild;
        public AssociationType WorkEffortsWhereConcurrency;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaWorkEffort(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("553a5280-a768-4ba1-8b5d-304d7c4bb7f1"))
			{
				SingularName = "WorkEffort",
			};
        }
	}
}