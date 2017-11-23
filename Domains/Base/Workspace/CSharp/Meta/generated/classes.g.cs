namespace Allors.Workspace.Meta
{
	using Allors.Meta;

    public partial class MetaLocalisedText : MetaClass
	{
	    public static MetaLocalisedText Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Text;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Locale;

		// Associations
        public AssociationType LocalisedNameCountry;
        public AssociationType LocalisedNameCurrency;
        public AssociationType LocalisedNameLanguage;
        public AssociationType LocalisedNameEnumeration;

		internal MetaLocalisedText(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("020f5d4d-4a59-4d7b-865a-d72fc70e4d97"))
			{
				SingularName = "LocalisedText",
				PluralName = "LocalisedTexts",
			};
        }

	}

    public partial class MetaAccessControl : MetaClass
	{
	    public static MetaAccessControl Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// RoleTypes
        public RoleType SubjectGroups;
        public RoleType Subjects;
        public RoleType Role;
        public RoleType EffectivePermissions;
        public RoleType EffectiveUsers;
        public RoleType CacheId;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType AccessControlSecurityTokens;
        public AssociationType CreatorsAccessControlSingleton;
        public AssociationType GuestAccessControlSingleton;
        public AssociationType AdministratorsAccessControlSingleton;
        public AssociationType SalesAccessControlSingleton;
        public AssociationType OperationsAccessControlSingleton;
        public AssociationType ProcurementAccessControlSingleton;
        public AssociationType OwnerAccessControlSecurityTokenOwner;

		internal MetaAccessControl(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c4d93d5e-34c3-4731-9d37-47a8e801d9a8"))
			{
				SingularName = "AccessControl",
				PluralName = "AccessControls",
			};
        }

	}

    public partial class MetaCounter : MetaClass
	{
	    public static MetaCounter Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Value;
        public RoleType UniqueId;

		// Associations
        public AssociationType TargetNotifications;

		internal MetaCounter(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0568354f-e3d9-439e-baac-b7dce31b956a"))
			{
				SingularName = "Counter",
				PluralName = "Counters",
			};
        }

	}

    public partial class MetaCountry : MetaClass
	{
	    public static MetaCountry Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Currency;
        public RoleType IsoCode;
        public RoleType Name;
        public RoleType LocalisedNames;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType CountryLocales;
        public AssociationType CountryPlaces;

		internal MetaCountry(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c22bf60e-6428-4d10-8194-94f7be396f28"))
			{
				SingularName = "Country",
				PluralName = "Countries",
			};
        }

	}

    public partial class MetaCurrency : MetaClass
	{
	    public static MetaCurrency Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType IsoCode;
        public RoleType Name;
        public RoleType LocalisedNames;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType CurrencyCountries;

		internal MetaCurrency(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("fd397adf-40b4-4ef8-b449-dd5a24273df3"))
			{
				SingularName = "Currency",
				PluralName = "Currencies",
			};
        }

	}

    public partial class MetaLanguage : MetaClass
	{
	    public static MetaLanguage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType IsoCode;
        public RoleType Name;
        public RoleType NativeName;
        public RoleType LocalisedNames;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType LanguageLocales;

		internal MetaLanguage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4a0eca4b-281f-488d-9c7e-497de882c044"))
			{
				SingularName = "Language",
				PluralName = "Languages",
			};
        }

	}

    public partial class MetaLocale : MetaClass
	{
	    public static MetaLocale Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Name;
        public RoleType Language;
        public RoleType Country;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType DefaultLocaleSingletons;
        public AssociationType LocaleSingleton;
        public AssociationType LocaleLocaliseds;

		internal MetaLocale(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("45033ae6-85b5-4ced-87ce-02518e6c27fd"))
			{
				SingularName = "Locale",
				PluralName = "Locales",
			};
        }

	}

    public partial class MetaLogin : MetaClass
	{
	    public static MetaLogin Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType Delete;
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Key;
        public RoleType Provider;
        public RoleType User;

		// Associations

		internal MetaLogin(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ad7277a8-eda4-4128-a990-b47fe43d120a"))
			{
				SingularName = "Login",
				PluralName = "Logins",
			};
        }

	}

    public partial class MetaMedia : MetaClass
	{
	    public static MetaMedia Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// RoleTypes
        public RoleType Revision;
        public RoleType MediaContent;
        public RoleType InData;
        public RoleType InDataUri;
        public RoleType FileName;
        public RoleType Type;
        public RoleType UniqueId;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType PhotoPeople;
        public AssociationType ImageOrganisation;
        public AssociationType LogoOrganisations;
        public AssociationType TargetNotifications;

		internal MetaMedia(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("da5b86a3-4f33-4c0d-965d-f4fbc1179374"))
			{
				SingularName = "Media",
				PluralName = "Medias",
			};
        }

	}

    public partial class MetaMediaContent : MetaClass
	{
	    public static MetaMediaContent Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// RoleTypes
        public RoleType Type;
        public RoleType Data;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType MediaContentMedia;

		internal MetaMediaContent(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6c20422e-cb3e-4402-bb40-dacaf584405e"))
			{
				SingularName = "MediaContent",
				PluralName = "MediaContents",
			};
        }

	}

    public partial class MetaAutomatedAgent : MetaClass
	{
	    public static MetaAutomatedAgent Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Name;
        public RoleType Description;
        public RoleType UserName;
        public RoleType NormalizedUserName;
        public RoleType UserPasswordHash;
        public RoleType UserEmail;
        public RoleType UserEmailConfirmed;
        public RoleType TaskList;
        public RoleType NotificationList;
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Locale;

		// Associations
        public AssociationType SubjectAccessControls;
        public AssociationType EffectiveUserAccessControls;
        public AssociationType UserLogins;
        public AssociationType GuestSingleton;
        public AssociationType MemberUserGroups;
        public AssociationType SenderEmailMessages;
        public AssociationType RecipientEmailMessages;
        public AssociationType UserTaskAssignments;

		internal MetaAutomatedAgent(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3587d2e1-c3f6-4c55-a96c-016e0501d99c"))
			{
				SingularName = "AutomatedAgent",
				PluralName = "AutomatedAgents",
			};
        }

	}

    public partial class MetaPermission : MetaClass
	{
	    public static MetaPermission Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType Delete;
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType OperandTypePointer;
        public RoleType ConcreteClassPointer;
        public RoleType OperationEnum;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType EffectivePermissionAccessControls;
        public AssociationType PermissionRoles;
        public AssociationType DeniedPermissionAccessControlledObjects;
        public AssociationType DeniedPermissionObjectStates;

		internal MetaPermission(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7fded183-3337-4196-afb0-3266377944bc"))
			{
				SingularName = "Permission",
				PluralName = "Permissions",
			};
        }

	}

    public partial class MetaPerson : MetaClass
	{
	    public static MetaPerson Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Method;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// RoleTypes
        public RoleType FirstName;
        public RoleType LastName;
        public RoleType MiddleName;
        public RoleType Addresses;
        public RoleType Age;
        public RoleType BirthDate;
        public RoleType FullName;
        public RoleType Gender;
        public RoleType IsMarried;
        public RoleType IsStudent;
        public RoleType MailboxAddress;
        public RoleType MainAddress;
        public RoleType Photo;
        public RoleType ShirtSize;
        public RoleType Text;
        public RoleType TinyMCEText;
        public RoleType Weight;
        public RoleType CycleOne;
        public RoleType CycleMany;
        public RoleType UserName;
        public RoleType NormalizedUserName;
        public RoleType UserPasswordHash;
        public RoleType UserEmail;
        public RoleType UserEmailConfirmed;
        public RoleType TaskList;
        public RoleType NotificationList;
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Locale;
        public RoleType UniqueId;

		// Associations
        public AssociationType PersonsManyBadUI;
        public AssociationType PersonOneBadUIs;
        public AssociationType EmployeeOrganisation;
        public AssociationType ManagerOrganisation;
        public AssociationType OwnerOrganisations;
        public AssociationType ShareholderOrganisations;
        public AssociationType CycleOneOrganisations;
        public AssociationType CycleManyOrganisations;
        public AssociationType EmployeeStatefulCompanies;
        public AssociationType ManagerStatefulCompanies;
        public AssociationType ParticipantTasks;
        public AssociationType PerformerTasks;
        public AssociationType SubjectAccessControls;
        public AssociationType EffectiveUserAccessControls;
        public AssociationType UserLogins;
        public AssociationType GuestSingleton;
        public AssociationType MemberUserGroups;
        public AssociationType SenderEmailMessages;
        public AssociationType RecipientEmailMessages;
        public AssociationType UserTaskAssignments;
        public AssociationType TargetNotifications;

		internal MetaPerson(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c799ca62-a554-467d-9aa2-1663293bb37f"))
			{
				SingularName = "Person",
				PluralName = "People",
			};
        }

	}

    public partial class MetaRole : MetaClass
	{
	    public static MetaRole Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Permissions;
        public RoleType Name;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Associations
        public AssociationType RoleAccessControls;
        public AssociationType TargetNotifications;

		internal MetaRole(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("af6fe5f4-e5bc-4099-bcd1-97528af6505d"))
			{
				SingularName = "Role",
				PluralName = "Roles",
			};
        }

	}

    public partial class MetaSecurityToken : MetaClass
	{
	    public static MetaSecurityToken Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType Delete;
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType AccessControls;

		// Associations
        public AssociationType InitialSecurityTokenSingletons;
        public AssociationType DefaultSecurityTokenSingletons;
        public AssociationType SecurityTokenAccessControlledObjects;
        public AssociationType OwnerSecurityTokenSecurityTokenOwner;

		internal MetaSecurityToken(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a53f1aed-0e3f-4c3c-9600-dc579cccf893"))
			{
				SingularName = "SecurityToken",
				PluralName = "SecurityTokens",
			};
        }

	}

    public partial class MetaSingleton : MetaClass
	{
	    public static MetaSingleton Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType DefaultLocale;
        public RoleType Locales;
        public RoleType Guest;
        public RoleType InitialSecurityToken;
        public RoleType DefaultSecurityToken;
        public RoleType CreatorsAccessControl;
        public RoleType GuestAccessControl;
        public RoleType AdministratorsAccessControl;
        public RoleType SalesAccessControl;
        public RoleType OperationsAccessControl;
        public RoleType ProcurementAccessControl;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations

		internal MetaSingleton(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("313b97a5-328c-4600-9dd2-b5bc146fb13b"))
			{
				SingularName = "Singleton",
				PluralName = "Singletons",
			};
        }

	}

    public partial class MetaUserGroup : MetaClass
	{
	    public static MetaUserGroup Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Members;
        public RoleType Name;
        public RoleType UniqueId;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType SubjectGroupAccessControls;
        public AssociationType TargetNotifications;

		internal MetaUserGroup(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("60065f5d-a3c2-4418-880d-1026ab607319"))
			{
				SingularName = "UserGroup",
				PluralName = "UserGroups",
			};
        }

	}

    public partial class MetaEmailMessage : MetaClass
	{
	    public static MetaEmailMessage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType DateCreated;
        public RoleType DateSending;
        public RoleType DateSent;
        public RoleType Sender;
        public RoleType Recipients;
        public RoleType RecipientEmailAddress;
        public RoleType Subject;
        public RoleType Body;

		// Associations

		internal MetaEmailMessage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ab20998b-62b1-4064-a7b9-cc9416edf77a"))
			{
				SingularName = "EmailMessage",
				PluralName = "EmailMessages",
			};
        }

	}

    public partial class MetaNotification : MetaClass
	{
	    public static MetaNotification Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Target;
        public RoleType Confirmed;
        public RoleType Title;
        public RoleType Description;
        public RoleType DateCreated;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType NotificationNotificationList;
        public AssociationType UnconfirmedNotificationNotificationList;
        public AssociationType ConfirmedNotificationNotificationList;
        public AssociationType NotificationTaskAssignment;

		internal MetaNotification(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("73dcdc68-7571-4ed1-86db-77c914fe2f62"))
			{
				SingularName = "Notification",
				PluralName = "Notifications",
			};
        }

	}

    public partial class MetaNotificationList : MetaClass
	{
	    public static MetaNotificationList Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// RoleTypes
        public RoleType Notifications;
        public RoleType UnconfirmedNotifications;
        public RoleType ConfirmedNotifications;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType NotificationListUser;

		internal MetaNotificationList(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b6579993-4ff1-4853-b048-1f8e67419c00"))
			{
				SingularName = "NotificationList",
				PluralName = "NotificationLists",
			};
        }

	}

    public partial class MetaTaskAssignment : MetaClass
	{
	    public static MetaTaskAssignment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// RoleTypes
        public RoleType User;
        public RoleType Notification;
        public RoleType Task;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType TaskAssignmentTaskList;
        public AssociationType OpenTaskAssignmentTaskList;

		internal MetaTaskAssignment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4092d0b4-c6f4-4b81-b023-66be3f4c90bd"))
			{
				SingularName = "TaskAssignment",
				PluralName = "TaskAssignments",
			};
        }

	}

    public partial class MetaTaskList : MetaClass
	{
	    public static MetaTaskList Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType Delete;
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType TaskAssignments;
        public RoleType OpenTaskAssignments;
        public RoleType Count;

		// Associations
        public AssociationType TaskListUser;

		internal MetaTaskList(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1c2303a1-f3ce-4084-a1ad-fc25156ac542"))
			{
				SingularName = "TaskList",
				PluralName = "TaskLists",
			};
        }

	}

    public partial class MetaBuild : MetaClass
	{
	    public static MetaBuild Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Guid;
        public RoleType String;

		// Associations

		internal MetaBuild(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("fffcc7bd-252e-4ee7-b825-99ccbe2d5f49"))
			{
				SingularName = "Build",
				PluralName = "Builds",
			};
        }

	}

    public partial class MetaBadUI : MetaClass
	{
	    public static MetaBadUI Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType PersonsMany;
        public RoleType CompanyOne;
        public RoleType PersonOne;
        public RoleType CompanyMany;
        public RoleType AllorsString;

		// Associations

		internal MetaBadUI(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("bb1b0a2e-66d1-4e09-860f-52dc7145029e"))
			{
				SingularName = "BadUI",
				PluralName = "BadUIs",
			};
        }

	}

    public partial class MetaC1 : MetaClass
	{
	    public static MetaC1 Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType ClassMethod;
        public MethodType Sum;

		// Inherited Methods
        public MethodType InterfaceMethod;
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType SuperinterfaceMethod;

		// RoleTypes
        public RoleType C1AllorsBinary;
        public RoleType C1AllorsBoolean;
        public RoleType C1AllorsDateTime;
        public RoleType C1AllorsDecimal;
        public RoleType C1AllorsDouble;
        public RoleType C1AllorsInteger;
        public RoleType C1AllorsString;
        public RoleType AllorsStringMax;
        public RoleType C1AllorsUnique;
        public RoleType C1C1Many2Manies;
        public RoleType C1C1Many2One;
        public RoleType C1C1One2Manies;
        public RoleType C1C1One2One;
        public RoleType C1C2Many2Manies;
        public RoleType C1C2Many2One;
        public RoleType C1C2One2Manies;
        public RoleType C1C2One2One;
        public RoleType C1I12Many2Manies;
        public RoleType C1I12Many2One;
        public RoleType C1I12One2Manies;
        public RoleType C1I12One2One;
        public RoleType C1I1Many2Manies;
        public RoleType C1I1Many2One;
        public RoleType C1I1One2Manies;
        public RoleType C1I1One2One;
        public RoleType C1I2Many2Manies;
        public RoleType C1I2Many2One;
        public RoleType C1I2One2Manies;
        public RoleType C1I2One2One;
        public RoleType I1I1Many2One;
        public RoleType I1I12Many2Manies;
        public RoleType I1I2Many2Manies;
        public RoleType I1I2Many2One;
        public RoleType I1AllorsString;
        public RoleType I1I12Many2One;
        public RoleType I1AllorsDateTime;
        public RoleType I1I2One2Manies;
        public RoleType I1C2One2Manies;
        public RoleType I1C1One2One;
        public RoleType I1AllorsInteger;
        public RoleType I1C2Many2Manies;
        public RoleType I1I1One2Manies;
        public RoleType I1I1Many2Manies;
        public RoleType I1AllorsBoolean;
        public RoleType I1AllorsDecimal;
        public RoleType I1I12One2One;
        public RoleType I1I2One2One;
        public RoleType I1C2One2One;
        public RoleType I1C1One2Manies;
        public RoleType I1AllorsBinary;
        public RoleType I1C1Many2Manies;
        public RoleType I1AllorsDouble;
        public RoleType I1I1One2One;
        public RoleType I1C1Many2One;
        public RoleType I1I12One2Manies;
        public RoleType I1C2Many2One;
        public RoleType I1AllorsUnique;
        public RoleType I12AllorsBinary;
        public RoleType I12C2One2One;
        public RoleType I12AllorsDouble;
        public RoleType I12I1Many2One;
        public RoleType I12AllorsString;
        public RoleType I12I12Many2Manies;
        public RoleType I12AllorsDecimal;
        public RoleType I12I2Many2Manies;
        public RoleType I12C2Many2Manies;
        public RoleType I12I1Many2Manies;
        public RoleType I12I12One2Manies;
        public RoleType Name;
        public RoleType I12C1Many2Manies;
        public RoleType I12I2Many2One;
        public RoleType I12AllorsUnique;
        public RoleType I12AllorsInteger;
        public RoleType I12I1One2Manies;
        public RoleType I12C1One2One;
        public RoleType I12I12One2One;
        public RoleType I12I2One2One;
        public RoleType Dependencies;
        public RoleType I12I2One2Manies;
        public RoleType I12C2Many2One;
        public RoleType I12I12Many2One;
        public RoleType I12AllorsBoolean;
        public RoleType I12I1One2One;
        public RoleType I12C1One2Manies;
        public RoleType I12C1Many2One;
        public RoleType I12AllorsDateTime;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType C1C1Many2ManyC1s;
        public AssociationType C1C1Many2OneC1s;
        public AssociationType C1C1One2ManyC1;
        public AssociationType C1C1One2OneC1;
        public AssociationType C2C1One2OneC2;
        public AssociationType C2C1Many2ManyC2s;
        public AssociationType C2C1Many2OneC2s;
        public AssociationType C2C1One2ManyC2;
        public AssociationType I1C1One2OneI1;
        public AssociationType I1C1One2ManyI1;
        public AssociationType I1C1Many2ManyI1s;
        public AssociationType I1C1Many2OneI1s;
        public AssociationType I12C1Many2ManyI12s;
        public AssociationType I12C1One2OneI12;
        public AssociationType I12C1One2ManyI12;
        public AssociationType I12C1Many2OneI12s;
        public AssociationType I2C1Many2OneI2s;
        public AssociationType I2C1One2ManyI2;
        public AssociationType I2C1One2OneI2;
        public AssociationType I2C1Many2ManyI2s;
        public AssociationType C1I1Many2ManyC1s;
        public AssociationType C1I1Many2OneC1s;
        public AssociationType C1I1One2ManyC1;
        public AssociationType C1I1One2OneC1;
        public AssociationType C2I1Many2ManyC2s;
        public AssociationType C2I1One2ManyC2;
        public AssociationType C2I1Many2OneC2s;
        public AssociationType C2I1One2OneC2;
        public AssociationType I1I1Many2OneI1s;
        public AssociationType I1I1One2ManyI1;
        public AssociationType I1I1Many2ManyI1s;
        public AssociationType I1I1One2OneI1;
        public AssociationType I12I1Many2OneI12s;
        public AssociationType I12I1Many2ManyI12s;
        public AssociationType I12I1One2ManyI12;
        public AssociationType I12I1One2OneI12;
        public AssociationType I2I1Many2OneI2s;
        public AssociationType I2I1Many2ManyI2s;
        public AssociationType I2I1One2OneI2;
        public AssociationType I2I1One2ManyI2;
        public AssociationType C1I12Many2ManyC1s;
        public AssociationType C1I12Many2OneC1s;
        public AssociationType C1I12One2ManyC1;
        public AssociationType C1I12One2OneC1;
        public AssociationType C2I12Many2OneC2s;
        public AssociationType C2I12One2OneC2;
        public AssociationType C2I12Many2ManyC2s;
        public AssociationType C2I12One2ManyC2;
        public AssociationType I1I12Many2ManyI1s;
        public AssociationType I1I12Many2OneI1s;
        public AssociationType I1I12One2OneI1;
        public AssociationType I1I12One2ManyI1;
        public AssociationType I12I12Many2ManyI12s;
        public AssociationType I12I12One2ManyI12;
        public AssociationType I12I12One2OneI12;
        public AssociationType DependencyI12s;
        public AssociationType I12I12Many2OneI12s;
        public AssociationType I2I12Many2OneI2s;
        public AssociationType I2I12One2ManyI2;
        public AssociationType I2I12One2OneI2;
        public AssociationType I2I12Many2ManyI2s;

		internal MetaC1(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7041c691-d896-4628-8f50-1c24f5d03414"))
			{
				SingularName = "C1",
				PluralName = "C1s",
			};
        }

	}

    public partial class MetaC2 : MetaClass
	{
	    public static MetaC2 Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType C2AllorsDecimal;
        public RoleType C2C1One2One;
        public RoleType C2C2Many2One;
        public RoleType C2AllorsUnique;
        public RoleType C2I12Many2One;
        public RoleType C2I12One2One;
        public RoleType C2I1Many2Manies;
        public RoleType C2AllorsDouble;
        public RoleType C2I1One2Manies;
        public RoleType C2I2One2One;
        public RoleType C2AllorsInteger;
        public RoleType C2I2Many2Manies;
        public RoleType C2I12Many2Manies;
        public RoleType C2C2One2Manies;
        public RoleType C2AllorsBoolean;
        public RoleType C2I1Many2One;
        public RoleType C2I1One2One;
        public RoleType C2C1Many2Manies;
        public RoleType C2I12One2Manies;
        public RoleType C2I2One2Manies;
        public RoleType C2C2One2One;
        public RoleType C2AllorsString;
        public RoleType C2C1Many2One;
        public RoleType C2C2Many2Manies;
        public RoleType C2AllorsDateTime;
        public RoleType C2I2Many2One;
        public RoleType C2C1One2Manies;
        public RoleType C2AllorsBinary;
        public RoleType I2I2Many2One;
        public RoleType I2C1Many2One;
        public RoleType I2I12Many2One;
        public RoleType I2AllorsBoolean;
        public RoleType I2C1One2Manies;
        public RoleType I2C1One2One;
        public RoleType I2AllorsDecimal;
        public RoleType I2I2Many2Manies;
        public RoleType I2AllorsBinary;
        public RoleType I2AllorsUnique;
        public RoleType I2I1Many2One;
        public RoleType I2AllorsDateTime;
        public RoleType I2I12One2Manies;
        public RoleType I2I12One2One;
        public RoleType I2C2Many2Manies;
        public RoleType I2I1Many2Manies;
        public RoleType I2C2Many2One;
        public RoleType I2AllorsString;
        public RoleType I2C2One2Manies;
        public RoleType I2I1One2One;
        public RoleType I2I1One2Manies;
        public RoleType I2I12Many2Manies;
        public RoleType I2I2One2One;
        public RoleType I2AllorsInteger;
        public RoleType I2I2One2Manies;
        public RoleType I2C1Many2Manies;
        public RoleType I2C2One2One;
        public RoleType I2AllorsDouble;
        public RoleType I12AllorsBinary;
        public RoleType I12C2One2One;
        public RoleType I12AllorsDouble;
        public RoleType I12I1Many2One;
        public RoleType I12AllorsString;
        public RoleType I12I12Many2Manies;
        public RoleType I12AllorsDecimal;
        public RoleType I12I2Many2Manies;
        public RoleType I12C2Many2Manies;
        public RoleType I12I1Many2Manies;
        public RoleType I12I12One2Manies;
        public RoleType Name;
        public RoleType I12C1Many2Manies;
        public RoleType I12I2Many2One;
        public RoleType I12AllorsUnique;
        public RoleType I12AllorsInteger;
        public RoleType I12I1One2Manies;
        public RoleType I12C1One2One;
        public RoleType I12I12One2One;
        public RoleType I12I2One2One;
        public RoleType Dependencies;
        public RoleType I12I2One2Manies;
        public RoleType I12C2Many2One;
        public RoleType I12I12Many2One;
        public RoleType I12AllorsBoolean;
        public RoleType I12I1One2One;
        public RoleType I12C1One2Manies;
        public RoleType I12C1Many2One;
        public RoleType I12AllorsDateTime;

		// Associations
        public AssociationType C1C2Many2ManyC1s;
        public AssociationType C1C2Many2OneC1s;
        public AssociationType C1C2One2ManyC1;
        public AssociationType C1C2One2OneC1;
        public AssociationType C2C2Many2OneC2s;
        public AssociationType C2C2One2ManyC2;
        public AssociationType C2C2One2OneC2;
        public AssociationType C2C2Many2ManyC2s;
        public AssociationType I1C2One2ManyI1;
        public AssociationType I1C2Many2ManyI1s;
        public AssociationType I1C2One2OneI1;
        public AssociationType I1C2Many2OneI1s;
        public AssociationType I12C2One2OneI12;
        public AssociationType I12C2Many2ManyI12s;
        public AssociationType I12C2Many2OneI12s;
        public AssociationType I2C2Many2ManyI2s;
        public AssociationType I2C2Many2OneI2s;
        public AssociationType I2C2One2ManyI2;
        public AssociationType I2C2One2OneI2;
        public AssociationType C1I2Many2ManyC1s;
        public AssociationType C1I2Many2OneC1s;
        public AssociationType C1I2One2ManyC1;
        public AssociationType C1I2One2OneC1;
        public AssociationType C2I2One2OneC2;
        public AssociationType C2I2Many2ManyC2s;
        public AssociationType C2I2One2ManyC2;
        public AssociationType C2I2Many2OneC2s;
        public AssociationType I1I2Many2ManyI1s;
        public AssociationType I1I2Many2OneI1s;
        public AssociationType I1I2One2ManyI1;
        public AssociationType I1I2One2OneI1;
        public AssociationType I12I2Many2ManyI12s;
        public AssociationType I12I2Many2OneI12s;
        public AssociationType I12I2One2OneI12;
        public AssociationType I12I2One2ManyI12;
        public AssociationType I2I2Many2OneI2s;
        public AssociationType I2I2Many2ManyI2s;
        public AssociationType I2I2One2OneI2;
        public AssociationType I2I2One2ManyI2;
        public AssociationType C1I12Many2ManyC1s;
        public AssociationType C1I12Many2OneC1s;
        public AssociationType C1I12One2ManyC1;
        public AssociationType C1I12One2OneC1;
        public AssociationType C2I12Many2OneC2s;
        public AssociationType C2I12One2OneC2;
        public AssociationType C2I12Many2ManyC2s;
        public AssociationType C2I12One2ManyC2;
        public AssociationType I1I12Many2ManyI1s;
        public AssociationType I1I12Many2OneI1s;
        public AssociationType I1I12One2OneI1;
        public AssociationType I1I12One2ManyI1;
        public AssociationType I12I12Many2ManyI12s;
        public AssociationType I12I12One2ManyI12;
        public AssociationType I12I12One2OneI12;
        public AssociationType DependencyI12s;
        public AssociationType I12I12Many2OneI12s;
        public AssociationType I2I12Many2OneI2s;
        public AssociationType I2I12One2ManyI2;
        public AssociationType I2I12One2OneI2;
        public AssociationType I2I12Many2ManyI2s;

		internal MetaC2(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("72c07e8a-03f5-4da8-ab37-236333d4f74e"))
			{
				SingularName = "C2",
				PluralName = "C2s",
			};
        }

	}

    public partial class MetaClassWithoutRoles : MetaClass
	{
	    public static MetaClassWithoutRoles Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes

		// Associations

		internal MetaClassWithoutRoles(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("e1008840-6d7c-4d44-b2ad-1545d23f90d8"))
			{
				SingularName = "ClassWithoutRoles",
				PluralName = "ClassWithourRoleses",
			};
        }

	}

    public partial class MetaDependee : MetaClass
	{
	    public static MetaDependee Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Subdependee;
        public RoleType Subcounter;
        public RoleType Counter;
        public RoleType DeleteDependent;

		// Associations
        public AssociationType DependeeDependent;

		internal MetaDependee(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2cc9bde1-80da-4159-bb20-219074266101"))
			{
				SingularName = "Dependee",
				PluralName = "Dependees",
			};
        }

	}

    public partial class MetaDependent : MetaClass
	{
	    public static MetaDependent Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType Delete;
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Dependee;
        public RoleType Counter;
        public RoleType Subcounter;

		// Associations

		internal MetaDependent(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0cb8d2a7-4566-432f-9882-893b05a77f44"))
			{
				SingularName = "Dependent",
				PluralName = "Dependents",
			};
        }

	}

    public partial class MetaExtender : MetaClass
	{
	    public static MetaExtender Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType AllorsString;

		// Associations

		internal MetaExtender(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("830cdcb1-31f1-4481-8399-00c034661450"))
			{
				SingularName = "Extender",
				PluralName = "Extenders",
			};
        }

	}

    public partial class MetaFirst : MetaClass
	{
	    public static MetaFirst Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Second;
        public RoleType CreateCycle;
        public RoleType IsDerived;

		// Associations

		internal MetaFirst(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1937b42e-954b-4ef9-bc63-5b8ae7903e9d"))
			{
				SingularName = "First",
				PluralName = "Firsts",
			};
        }

	}

    public partial class MetaFour : MetaClass
	{
	    public static MetaFour Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes

		// Associations
        public AssociationType FourThrees;
        public AssociationType SharedTwos;

		internal MetaFour(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1248e212-ca71-44aa-9e87-6e83dae9d4fd"))
			{
				SingularName = "Four",
				PluralName = "Fours",
			};
        }

	}

    public partial class MetaFrom : MetaClass
	{
	    public static MetaFrom Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Tos;

		// Associations

		internal MetaFrom(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6217b428-4ad0-4f7f-ad4b-e334cf0b3ab1"))
			{
				SingularName = "From",
				PluralName = "Froms",
			};
        }

	}

    public partial class MetaGender : MetaClass
	{
	    public static MetaGender Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType LocalisedNames;
        public RoleType Name;
        public RoleType IsActive;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Associations
        public AssociationType GenderPeople;
        public AssociationType TargetNotifications;

		internal MetaGender(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("270f0dc8-1bc2-4a42-9617-45e93d5403c8"))
			{
				SingularName = "Gender",
				PluralName = "Genders",
			};
        }

	}

    public partial class MetaHomeAddress : MetaClass
	{
	    public static MetaHomeAddress Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Street;
        public RoleType HouseNumber;
        public RoleType Place;

		// Associations
        public AssociationType AddressPeople;
        public AssociationType MainAddressPeople;
        public AssociationType AddressOrganisation;
        public AssociationType MainAddressOrganisations;

		internal MetaHomeAddress(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2561e93c-5b85-44fb-a924-a1c0d1f78846"))
			{
				SingularName = "HomeAddress",
				PluralName = "HomeAddresses",
			};
        }

	}

    public partial class MetaMailboxAddress : MetaClass
	{
	    public static MetaMailboxAddress Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType PoBox;
        public RoleType Place;

		// Associations
        public AssociationType MailboxAddressPeople;
        public AssociationType AddressPeople;
        public AssociationType MainAddressPeople;
        public AssociationType AddressOrganisation;
        public AssociationType MainAddressOrganisations;

		internal MetaMailboxAddress(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7ee3b00b-4e63-4774-b744-3add2c6035ab"))
			{
				SingularName = "MailboxAddress",
				PluralName = "MailboxAddresses",
			};
        }

	}

    public partial class MetaOne : MetaClass
	{
	    public static MetaOne Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Two;

		// Associations
        public AssociationType SharedTwos;

		internal MetaOne(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5d9b9cad-3720-47c3-9693-289698bf3dd0"))
			{
				SingularName = "One",
				PluralName = "Ones",
			};
        }

	}

    public partial class MetaOrder : MetaClass
	{
	    public static MetaOrder Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType PreviousOrderState;
        public RoleType LastOrderState;
        public RoleType OrderState;
        public RoleType PreviousShipmentState;
        public RoleType LastShipmentState;
        public RoleType ShipmentState;
        public RoleType PreviousPaymentState;
        public RoleType LastPaymentState;
        public RoleType PaymentState;
        public RoleType OrderLines;
        public RoleType Amount;
        public RoleType NonVersionedCurrentObjectState;
        public RoleType NonVersionedOrderLines;
        public RoleType NonVersionedAmount;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations

		internal MetaOrder(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("94be4938-77c1-488f-b116-6d4daeffcc8d"))
			{
				SingularName = "Order",
				PluralName = "Orders",
			};
        }

	}

    public partial class MetaOrderLine : MetaClass
	{
	    public static MetaOrderLine Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Amount;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType OrderLineOrder;
        public AssociationType NonVersionedOrderLineOrder;
        public AssociationType OrderLineOrderVersions;

		internal MetaOrderLine(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("721008c3-c87c-40ab-966b-094e1271ed5f"))
			{
				SingularName = "OrderLine",
				PluralName = "OrderLines",
			};
        }

	}

    public partial class MetaOrderLineVersion : MetaClass
	{
	    public static MetaOrderLineVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Amount;
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType CurrentVersionOrderLine;
        public AssociationType AllVersionOrderLine;

		internal MetaOrderLineVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ba589be8-049b-4107-9e20-fbfec19477c4"))
			{
				SingularName = "OrderLineVersion",
				PluralName = "OrderLineVersions",
			};
        }

	}

    public partial class MetaPaymentState : MetaClass
	{
	    public static MetaPaymentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType DeniedPermissions;
        public RoleType Name;
        public RoleType UniqueId;

		// Associations
        public AssociationType PreviousPaymentStateOrders;
        public AssociationType LastPaymentStateOrders;
        public AssociationType PaymentStateOrders;
        public AssociationType PreviousObjectStateTransitionalVersions;
        public AssociationType LastObjectStateTransitionalVersions;
        public AssociationType ObjectStateTransitionalVersions;
        public AssociationType PreviousObjectStateTransitionals;
        public AssociationType LastObjectStateTransitionals;
        public AssociationType ObjectStateTransitionals;
        public AssociationType TargetNotifications;

		internal MetaPaymentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("07e8f845-5ecc-4b42-83ef-bb86e6b10a69"))
			{
				SingularName = "PaymentState",
				PluralName = "PaymentStates",
			};
        }

	}

    public partial class MetaShipmentState : MetaClass
	{
	    public static MetaShipmentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType DeniedPermissions;
        public RoleType Name;
        public RoleType UniqueId;

		// Associations
        public AssociationType PreviousShipmentStateOrders;
        public AssociationType LastShipmentStateOrders;
        public AssociationType ShipmentStateOrders;
        public AssociationType PreviousObjectStateTransitionalVersions;
        public AssociationType LastObjectStateTransitionalVersions;
        public AssociationType ObjectStateTransitionalVersions;
        public AssociationType PreviousObjectStateTransitionals;
        public AssociationType LastObjectStateTransitionals;
        public AssociationType ObjectStateTransitionals;
        public AssociationType TargetNotifications;

		internal MetaShipmentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ce56a6e9-8e4b-4f40-8676-180f4b0513e2"))
			{
				SingularName = "ShipmentState",
				PluralName = "ShipmentStates",
			};
        }

	}

    public partial class MetaOrderState : MetaClass
	{
	    public static MetaOrderState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType DeniedPermissions;
        public RoleType Name;
        public RoleType UniqueId;

		// Associations
        public AssociationType PreviousOrderStateOrders;
        public AssociationType LastOrderStateOrders;
        public AssociationType OrderStateOrders;
        public AssociationType NonVersionedCurrentObjectStateOrders;
        public AssociationType OrderStateOrderVersions;
        public AssociationType PreviousObjectStateTransitionalVersions;
        public AssociationType LastObjectStateTransitionalVersions;
        public AssociationType ObjectStateTransitionalVersions;
        public AssociationType PreviousObjectStateTransitionals;
        public AssociationType LastObjectStateTransitionals;
        public AssociationType ObjectStateTransitionals;
        public AssociationType TargetNotifications;

		internal MetaOrderState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("849393ed-cff6-40da-9b4d-483f045f2e99"))
			{
				SingularName = "OrderState",
				PluralName = "OrderStates",
			};
        }

	}

    public partial class MetaOrderVersion : MetaClass
	{
	    public static MetaOrderVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType OrderState;
        public RoleType OrderLines;
        public RoleType Amount;
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType CurrentVersionOrder;
        public AssociationType AllVersionOrder;

		internal MetaOrderVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6a3a9167-9a77-491e-a1c8-ccfe4572afb4"))
			{
				SingularName = "OrderVersion",
				PluralName = "OrderVersions",
			};
        }

	}

    public partial class MetaOrganisation : MetaClass
	{
	    public static MetaOrganisation Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType JustDoIt;

		// Inherited Methods
        public MethodType Delete;
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Addresses;
        public RoleType Description;
        public RoleType Employees;
        public RoleType Images;
        public RoleType Incorporated;
        public RoleType IncorporationDate;
        public RoleType Information;
        public RoleType IsSupplier;
        public RoleType Logo;
        public RoleType MainAddress;
        public RoleType Manager;
        public RoleType Name;
        public RoleType Owner;
        public RoleType Shareholders;
        public RoleType Size;
        public RoleType CycleOne;
        public RoleType CycleMany;
        public RoleType UniqueId;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations
        public AssociationType CycleOnePeople;
        public AssociationType CycleManyPeople;
        public AssociationType CompanyOneBadUIs;
        public AssociationType CompanyManyBadUIs;
        public AssociationType TargetNotifications;

		internal MetaOrganisation(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3a5dcec7-308f-48c7-afee-35d38415aa0b"))
			{
				SingularName = "Organisation",
				PluralName = "Organisations",
			};
        }

	}

    public partial class MetaPlace : MetaClass
	{
	    public static MetaPlace Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Country;
        public RoleType City;
        public RoleType PostalCode;

		// Associations
        public AssociationType PlaceAddresses;

		internal MetaPlace(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("268f63d2-17da-4f29-b0d0-76db611598c6"))
			{
				SingularName = "Place",
				PluralName = "Places",
			};
        }

	}

    public partial class MetaSecond : MetaClass
	{
	    public static MetaSecond Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Third;
        public RoleType IsDerived;

		// Associations
        public AssociationType SecondFirst;

		internal MetaSecond(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c1f169a1-553b-4a24-aba7-01e0b7102fe5"))
			{
				SingularName = "Second",
				PluralName = "Seconds",
			};
        }

	}

    public partial class MetaSimpleJob : MetaClass
	{
	    public static MetaSimpleJob Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Index;

		// Associations

		internal MetaSimpleJob(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("320985b6-d571-4b6c-b940-e02c04ad37d3"))
			{
				SingularName = "SimpleJob",
				PluralName = "SimpleJobs",
			};
        }

	}

    public partial class MetaStatefulCompany : MetaClass
	{
	    public static MetaStatefulCompany Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Employee;
        public RoleType Name;
        public RoleType Manager;

		// Associations

		internal MetaStatefulCompany(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("62859bfb-7949-4f7f-a428-658447576d0a"))
			{
				SingularName = "StatefulCompany",
				PluralName = "StatefulCompanies",
			};
        }

	}

    public partial class MetaSubdependee : MetaClass
	{
	    public static MetaSubdependee Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Subcounter;

		// Associations
        public AssociationType SubdependeeDependee;

		internal MetaSubdependee(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("46a437d1-455b-4ddd-b83c-068938c352bd"))
			{
				SingularName = "Subdependee",
				PluralName = "Subdependees",
			};
        }

	}

    public partial class MetaThird : MetaClass
	{
	    public static MetaThird Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType IsDerived;

		// Associations
        public AssociationType ThirdSecond;

		internal MetaThird(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("39116edf-34cf-45a6-ac09-2e4f98f28e14"))
			{
				SingularName = "Third",
				PluralName = "Thirds",
			};
        }

	}

    public partial class MetaThree : MetaClass
	{
	    public static MetaThree Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Four;
        public RoleType AllorsString;

		// Associations
        public AssociationType SharedTwos;

		internal MetaThree(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("bdaed62e-6369-46c0-a379-a1eef81b1c3d"))
			{
				SingularName = "Three",
				PluralName = "Threes",
			};
        }

	}

    public partial class MetaTo : MetaClass
	{
	    public static MetaTo Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Name;

		// Associations
        public AssociationType ToFrom;

		internal MetaTo(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7eb25112-4b81-4e8d-9f75-90950c40c65f"))
			{
				SingularName = "To",
				PluralName = "Tos",
			};
        }

	}

    public partial class MetaTwo : MetaClass
	{
	    public static MetaTwo Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType Shared;

		// Associations
        public AssociationType TwoOnes;
        public AssociationType SharedTwos;

		internal MetaTwo(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9ec7e136-815c-4726-9991-e95a3ec9e092"))
			{
				SingularName = "Two",
				PluralName = "Twos",
			};
        }

	}

    public partial class MetaUnitSample : MetaClass
	{
	    public static MetaUnitSample Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType AllorsBinary;
        public RoleType AllorsDateTime;
        public RoleType AllorsBoolean;
        public RoleType AllorsDouble;
        public RoleType AllorsInteger;
        public RoleType AllorsString;
        public RoleType AllorsUnique;
        public RoleType AllorsDecimal;
        public RoleType RequiredBinary;
        public RoleType RequiredDateTime;
        public RoleType RequiredBoolean;
        public RoleType RequiredDouble;
        public RoleType RequiredInteger;
        public RoleType RequiredString;
        public RoleType RequiredUnique;
        public RoleType RequiredDecimal;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations

		internal MetaUnitSample(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4e501cd6-807c-4f10-b60b-acd1d80042cd"))
			{
				SingularName = "UnitSample",
				PluralName = "UnitSamples",
			};
        }

	}

    public partial class MetaValidationC1 : MetaClass
	{
	    public static MetaValidationC1 Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType UniqueId;

		// Associations

		internal MetaValidationC1(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2361c456-b624-493a-8377-2dd1e697e17a"))
			{
				SingularName = "ValidationC1",
				PluralName = "ValidationC1s",
			};
        }

	}

    public partial class MetaValidationC2 : MetaClass
	{
	    public static MetaValidationC2 Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// RoleTypes
        public RoleType UniqueId;

		// Associations

		internal MetaValidationC2(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c7563dd3-77b2-43ff-92f9-a4f98db36acf"))
			{
				SingularName = "ValidationC2",
				PluralName = "ValidationC2s",
			};
        }

	}
}